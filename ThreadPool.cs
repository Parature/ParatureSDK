using System;
using System.Collections.Generic;
using System.Text;
using ParatureAPI.PagedData;
using ParatureAPI.ParaObjects;
using ParatureAPI.XmlToObjectParser;

namespace ParatureAPI
{
    /// <summary>
    /// Main Thread controller class
    /// </summary>
    public class ThreadPool
    {
        /// <summary>
        /// Main Thread object, limit of 5 parallel threads
        /// </summary>
        public class ObjectList
        {
            private static System.Threading.Semaphore sem = new System.Threading.Semaphore(5, 5);
            private ParaCredentials _paracredentials = null;
            private ParaEnums.ParatureModule _module;
            private System.Collections.ArrayList _Arguments = null;
            private int _requestdepth;
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="paracredentials"></param>
            /// <param name="module"></param>
            /// <param name="Arguments"></param>
            /// <param name="requestdepth"></param>
            public ObjectList(ParaCredentials paracredentials, ParaEnums.ParatureModule module, System.Collections.ArrayList Arguments, int requestdepth)
            {
                sem.WaitOne();
                _paracredentials = paracredentials;
                _module = module;
                _Arguments = new System.Collections.ArrayList(Arguments);
                _requestdepth = requestdepth;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="customerList"></param>
            public void Go(ParaEntityList<ParaObjects.Customer> customerList)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = CustomerParser.CustomersFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                customerList.Data.AddRange(objectlist.Data);
                customerList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chatList"></param>
            public void Go(ParaEntityList<ParaObjects.Chat> chatList, Boolean IncludeTranscripts)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = ChatParser.ChatsFillList(ar.xmlReceived, true, IncludeTranscripts, _requestdepth, _paracredentials);
                chatList.Data.AddRange(objectlist.Data);
                chatList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaEntityList<ParaObjects.Account> accountList)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = AccountParser.AccountsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                accountList.Data.AddRange(objectlist.Data);
                accountList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaEntityList<ParaObjects.Ticket> ticketList)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = TicketParser.TicketsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                ticketList.Data.AddRange(objectlist.Data);
                ticketList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaEntityList<ParaObjects.Product> productList)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = ProductParser.ProductsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                productList.Data.AddRange(objectlist.Data);
                productList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaEntityList<ParaObjects.Asset> assetList)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = AssetParser.AssetsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                assetList.Data.AddRange(objectlist.Data);
                assetList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaEntityList<ParaObjects.Download> downloadList)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = DownloadParser.DownloadsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                downloadList.Data.AddRange(objectlist.Data);
                downloadList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaEntityList<ParaObjects.Article> articleList)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = ArticleParser.ArticlesFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                articleList.Data.AddRange(objectlist.Data);
                articleList.ApiCallResponse = ar;
                sem.Release();
            }
        }
    }
}