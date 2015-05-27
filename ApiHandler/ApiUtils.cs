using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;
using Action = ParatureSDK.ParaObjects.Action;

namespace ParatureSDK.ApiHandler
{
    internal static class ApiUtils
    {
        // This Optimize Page Size should be enhanced to get the Exact best pagesize
        // this will take more time, and as such should be the kind of thing that the application runs once,
        // then stores the results and reuses them from then on.
        // Because it will be used once then reused, there should be several calls of each page size to ensure accuracy.
        // The optimizeObjectCalls method should also be built out to determine the best case number of 
        // calls depending on current server load.
        public static OptimizationResult OptimizeObjectPageSize(PagedData.PagedData objectList, ParaQuery query, ParaCredentials paraCredentials, int requestdepth, ParaEnums.ParatureModule module)
        {
            var rtn = new OptimizationResult
            {
                Query = query, 
                objectList = objectList
            };

            //Two distinct sets of logic are required for optimization.  The two methods are as described:
            //OptimizeCalls requires us to compensate for 5 parallel asynchronous threads.  This is best 
            //  approached by making a series of test calls and calculating the required pages size to achieve 
            //  call duration equal to the thread spawn interval multiplied by the thread limit.  Obviously this
            //  requires revision, but it’s good enough for now.
            //NonOptimizeCalls requires us to calculate the minimum total call time by taking the current 
            //  call time multiplied by the calls required.  The calls required is calculated by dividing the 
            //  total records by the page size then rounded up. Currently we do this by stepping through a 
            //  fixed set of test calls, but this should be refactored to a more dynamic calculation.
            ApiCallResponse tempAr;
            var testTimePerPage = 0.0;    //units are milliseconds
            var callStopWatch = new Stopwatch();
            var masterTimePerPage = 0.0;  //units are milliseconds
            var masterPagesRequired = 0.0;

            rtn.apiResponse = new ApiCallResponse();

            if (rtn.Query.OptimizeCalls)
            {
                var pageSizeSampleSet = new List<int>
                {
                    rtn.Query.OptimizePageSizeSeed
                };

                for (var i = 0; i < rtn.Query.OptimizePageSizeTestCalls; i++)
                {
                    rtn.Query.PageSize = pageSizeSampleSet[i];

                    callStopWatch.Reset();
                    callStopWatch.Start();
                    tempAr = ApiCallFactory.ObjectGetList(paraCredentials, module, rtn.Query.BuildQueryArguments());
                    callStopWatch.Stop();

                    ParaEntityParser.ObjectFillList(tempAr.XmlReceived, rtn.Query.MinimalisticLoad, requestdepth, paraCredentials, module);

                    testTimePerPage = (int)(callStopWatch.ElapsedMilliseconds);

                    if (i == (rtn.Query.OptimizePageSizeTestCalls - 1) && tempAr.HasException == false)
                    {
                        var total = 0;
                        for (var j = 1; j < pageSizeSampleSet.Count; j++)
                        {
                            total = total + pageSizeSampleSet[j];
                        }
                        rtn.Query.PageSize = (total / (pageSizeSampleSet.Count - 1));
                        break;
                    }
                    else
                    {   // ((5 * 500) == Golden Number
                        pageSizeSampleSet.Add((int)((5 * 500) / (testTimePerPage / pageSizeSampleSet[i])));
                    }
                }

                //first page call
                rtn.apiResponse = ApiCallFactory.ObjectGetList(paraCredentials, module, rtn.Query.BuildQueryArguments());
                rtn.objectList = ParaEntityParser.ObjectFillList(rtn.apiResponse.XmlReceived, rtn.Query.MinimalisticLoad, requestdepth, paraCredentials, module);
            }
            else
            {
                int[] pageSizeSampleSet = { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500 };

                for (var i = 0; i < pageSizeSampleSet.Length; i++)
                {
                    rtn.Query.PageSize = pageSizeSampleSet[i];

                    callStopWatch.Reset();
                    callStopWatch.Start();
                    tempAr = ApiCallFactory.ObjectGetList(paraCredentials, module, rtn.Query.BuildQueryArguments());
                    callStopWatch.Stop();

                    var tempObjectList = ParaEntityParser.ObjectFillList(tempAr.XmlReceived, rtn.Query.MinimalisticLoad, requestdepth, paraCredentials, module);

                    testTimePerPage = (int)(callStopWatch.ElapsedMilliseconds);
                    double testTimePagesRequired = (int)Math.Ceiling((double)tempObjectList.TotalItems / (double)pageSizeSampleSet[i]);
                    // if the improvment is less than 25 percent lets just take it
                    if ((((masterPagesRequired * masterTimePerPage) / (testTimePagesRequired * testTimePerPage)) < 1.25) && i != 0 && tempAr.HasException == false)
                    {
                        rtn.Query.PageSize = pageSizeSampleSet[(i - 1)];
                        break;
                    }
                    else
                    {
                        masterTimePerPage = testTimePerPage;
                        masterPagesRequired = testTimePagesRequired;
                        rtn.objectList = tempObjectList;
                        rtn.apiResponse = tempAr;
                    }
                }
            }

            return rtn;
        }

        /// <summary>
        /// Internal Method to run an Action, independently from the module.
        /// </summary>
        public static ApiCallResponse ActionRun(Int64 objectId, Action action, ParaCredentials pc, ParaEnums.ParatureModule module)
        {
            var doc = XmlGenerator.GenerateXml(action, module);
            var ar = ApiCallFactory.ObjectCreateUpdate(pc, module, doc, objectId);
            return ar;
        }

        public static Attachment UploadFile(ParaEnums.ParatureModule module, ParaCredentials pc, System.Net.Mail.Attachment attachment)
        {
            var postUrlR = ApiCallFactory.FileUploadGetUrl(pc, module);
            var uploadUrlDoc = postUrlR.XmlReceived;
            var postUrl = AttachmentParser.AttachmentGetUrlToPost(uploadUrlDoc);

            var upresp = ApiCallFactory.FilePerformUpload(postUrl, attachment, pc.Instanceid, pc);

            var attaDoc = upresp.XmlReceived;

            var attach = ParaEntityParser.EntityFill<Attachment>(attaDoc);
            return attach;
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        public static Attachment UploadFile(ParaEnums.ParatureModule module, ParaCredentials pc, string text, string contentType, string fileName)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            return UploadFile(module, pc, bytes, contentType, fileName);
        }

        /// <summary>
        /// Internal method to handle the upload of a file to Parature.
        /// </summary>
        public static Attachment UploadFile(ParaEnums.ParatureModule module, ParaCredentials pc, Byte[] attachment, String contentType, String fileName)
        {
            Attachment attach;
            var postUrl = "";
            postUrl = AttachmentParser.AttachmentGetUrlToPost(ApiCallFactory.FileUploadGetUrl(pc, module).XmlReceived);

            if (String.IsNullOrEmpty(postUrl) == false)
            {
                var attaDoc = ApiCallFactory.FilePerformUpload(postUrl, attachment, contentType, fileName, pc.Instanceid, pc).XmlReceived;
                attach = ParaEntityParser.EntityFill<Attachment>(attaDoc);
            }
            else
            {
                attach = new Attachment();
            }
            return attach;
        }

        /// <summary>
        /// Remove the static fields, which screw up the deserializer when fetching the schema
        /// </summary>
        /// <param name="xmlReceived"></param>
        /// <returns></returns>
        public static XmlDocument RemoveStaticFieldsNodes(XmlDocument xmlReceived)
        {
            var xmlPurged = XDocument.Parse(xmlReceived.OuterXml);
            xmlPurged.Root.Elements()
                .Where(node => node.Name.LocalName != "Custom_Field")
                .Remove();

            var xmlDocument = new XmlDocument();
            using (var xmlReader = xmlPurged.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        /// <summary>
        /// Fills a Role list object
        /// </summary>
        /// <param name="paraCredentials"></param>
        /// <param name="query"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        internal static ParaEntityList<T> FillList<T>(ParaCredentials paraCredentials, ParaQuery query, ParaEnums.ParatureEntity entity, ParaEnums.ParatureModule module)
        {
            var rolesList = new ParaEntityList<T>();
            var ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, module, entity, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                rolesList = ParaEntityParser.FillList<T>(ar.XmlReceived);
            }
            rolesList.ApiCallResponse = ar;

            // Checking if the system needs to recursively call all of the data returned.
            if (query.RetrieveAllRecords)
            {
                var continueCalling = true;
                while (continueCalling)
                {
                    if (rolesList.TotalItems > rolesList.Data.Count)
                    {
                        // We still need to pull data
                        // Getting next page's data
                        query.PageNumber = query.PageNumber + 1;

                        ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, module, entity, query.BuildQueryArguments());

                        var objectlist = ParaEntityParser.FillList<T>(ar.XmlReceived);

                        if (objectlist.Data.Count == 0)
                        {
                            continueCalling = false;
                        }

                        rolesList.Data.AddRange(objectlist.Data);
                        rolesList.ResultsReturned = rolesList.Data.Count;
                        rolesList.PageNumber = query.PageNumber;
                    }
                    else
                    {
                        // That is it, pulled all the items.
                        continueCalling = false;
                        rolesList.ApiCallResponse = ar;
                    }
                }
            }

            return rolesList;
        }

        internal static T FillDetails<T>(Int64 entityId, ParaCredentials paraCredentials, ParaEnums.ParatureEntity entityType) where T: ParaEntityBaseProperties, new()
        {
            var role = new T();
            var ar = ApiCallFactory.ObjectGetDetail(paraCredentials, entityType, entityId);
            if (ar.HasException == false)
            {
                role = ParaEntityParser.EntityFill<T>(ar.XmlReceived);
            }
            else
            {
                role.Id = 0;
            }
            return role;
        }
    }
}
