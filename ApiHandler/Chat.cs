using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Contains all the methods that allow you to interact with the Parature Chat module.
    /// </summary>
    public class Chat : FirstLevelApiHandler<ParaObjects.Chat>
    {
        private static ParaEnums.ParatureModule _module = ParaEnums.ParatureModule.Chat;

        /// <summary>
        /// Returns a Chat object with all the properties of a chat.
        /// </summary>
        /// <param name="chatid">
        ///The chat number that you would like to get the details of. 
        ///Value Type: <see cref="Int64" />   (System.Int64)
        ///</param>
        /// <param name="ParaCredentials">
        /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
        /// </param>
        /// <param name="IncludeHistory">
        /// Whether to include the chat history (action history) for this particular chat
        /// </param>
        /// <param name="IncludeTranscripts">
        /// Whether to include the chat transcript (chat discussion) for this particular chat 
        /// </param>
        public static ParaObjects.Chat GetDetails(Int64 chatid, ParaCredentials ParaCredentials, Boolean IncludeHistory, Boolean IncludeTranscripts)
        {

            ParaObjects.Chat chat = new ParaObjects.Chat();
            chat = FillDetails(chatid, ParaCredentials, IncludeHistory);

            return chat;

        }

        public static ParaEntityList<ParaObjects.Chat> GetList(ParaCredentials ParaCredentials, Boolean IncludeTranscripts, ChatQuery Query)
        {
            return FillList(ParaCredentials, IncludeTranscripts, Query, ParaEnums.RequestDepth.Standard);
        }

        public static ParaEntityList<ParaObjects.Chat> GetList(ParaCredentials ParaCredentials, Boolean IncludeTranscripts, Boolean IncludeHistory)
        {
            return FillList(ParaCredentials, IncludeTranscripts, null, ParaEnums.RequestDepth.Standard);
        }

        private static ParaEntityList<ParaObjects.Chat> FillList(ParaCredentials ParaCredentials, Boolean IncludeTranscripts, ChatQuery Query, ParaEnums.RequestDepth RequestDepth)
        {
            int requestdepth = (int)RequestDepth;
            if (Query == null)
            {
                Query = new ChatQuery();
            }
            ApiCallResponse ar = new ApiCallResponse();
            var ChatList = new ParaEntityList<ParaObjects.Chat>();

            if (Query.RetrieveAllRecords && Query.OptimizePageSize)
            {
                OptimizationResult rslt = ApiUtils.OptimizeObjectPageSize(ChatList, Query, ParaCredentials, ParaEnums.ParatureModule.Customer);
                ar = rslt.apiResponse;
                Query = (ChatQuery)rslt.Query;
                ChatList = ((ParaEntityList<ParaObjects.Chat>)rslt.objectList);
                rslt = null;
            }
            else
            {
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Chat, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ChatList = ParaEntityParser.FillList<ParaObjects.Chat>(ar.XmlReceived);
                }
                ChatList.ApiCallResponse = ar;
            }

            // Checking if the system needs to recursively call all of the data returned.
            if (Query.RetrieveAllRecords && !ar.HasException)
            {
                // A flag variable to check if we need to make more calls
                if (Query.OptimizeCalls)
                {
                    System.Threading.Thread t;
                    ThreadPool.ObjectList instance = null;
                    int callsRequired = (int)Math.Ceiling((double)(ChatList.TotalItems / (double)ChatList.PageSize));
                    for (int i = 2; i <= callsRequired; i++)
                    {
                        //ApiCallFactory.waitCheck(ParaCredentials.Accountid);
                        Query.PageNumber = i;
                        //implement semaphore right here (in the thread pool instance to control the generation of threads
                        instance = new ThreadPool.ObjectList(ParaCredentials, ParaEnums.ParatureModule.Customer, Query.BuildQueryArguments());
                        t = new System.Threading.Thread(delegate() { instance.Go(ChatList, IncludeTranscripts); });
                        t.Start();
                    }

                    while (ChatList.TotalItems > ChatList.Data.Count)
                    {
                        Thread.Sleep(500);
                    }

                    ChatList.ResultsReturned = ChatList.Data.Count;
                    ChatList.PageNumber = callsRequired;
                }
                else
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {

                        if (ChatList.TotalItems > ChatList.Data.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureModule.Customer, Query.BuildQueryArguments());
                            if (ar.HasException == false)
                            {
                                ChatList.Data.AddRange(ParaEntityParser.FillList<ParaObjects.Chat>(ar.XmlReceived).Data);
                                ChatList.ResultsReturned = ChatList.Data.Count;
                                ChatList.PageNumber = Query.PageNumber;
                            }
                            else
                            {
                                continueCalling = false;
                                ChatList.ApiCallResponse = ar;
                                break;
                            }
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            ChatList.ApiCallResponse = ar;
                        }
                    }
                }
            }

            return ChatList;
        }

        static ParaObjects.Chat FillTranscriptDetails(Int64 chatid, ParaCredentials paraCredentials)
        {
            //Because chat transcripts return a Chat object with just a list messages, we'll deserialize the transcript into a chat object
            var chat = new ParaObjects.Chat();

            var ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.ChatTranscript, chatid);
            if (ar.HasException == false)
            {
                chat = ParaEntityParser.EntityFill<ParaObjects.Chat>(ar.XmlReceived);
                chat.FullyLoaded = true;
            }
            else
            {
                chat.FullyLoaded = false;
                chat.Id = 0;
            }
            chat.ApiCallResponse = ar;
            chat.IsDirty = false;
            return chat;
        }    

        static ParaObjects.Chat FillDetails(Int64 chatid, ParaCredentials ParaCredentials, Boolean IncludeHistory)
        {
            ParaObjects.Chat chat = new ParaObjects.Chat();
            //Customer = null;
            ApiCallResponse ar = new ApiCallResponse();
            ArrayList arl = new ArrayList();
            if (IncludeHistory)
            {
                arl.Add("_history_=true");
            }
            ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureModule.Chat, chatid, arl);
            if (ar.HasException == false)
            {
                chat = ParaEntityParser.EntityFill<ParaObjects.Chat>(ar.XmlReceived);
                chat.FullyLoaded = true;
            }
            else
            {
                chat.FullyLoaded = false;
                chat.Id = 0;
            }
            chat.ApiCallResponse = ar;
            chat.IsDirty = false;
            return chat;
        }

        /// <summary>
        /// Retrieve the transcript for a particualr chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="paraCredentials"></param>
        /// <returns>A list of chat messages</returns>
        static public List<ChatMessage> GetTranscript(Int64 chatId, ParaCredentials paraCredentials)
        {
            var chat = FillTranscriptDetails(chatId, paraCredentials);
            return chat.Transcript;
        }
    }
}