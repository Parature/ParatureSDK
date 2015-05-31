using System;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK
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

            /// <summary>
            /// 
            /// </summary>
            /// <param name="paracredentials"></param>
            /// <param name="module"></param>
            /// <param name="arguments"></param>
            public ObjectList(ParaCredentials paracredentials, ParaEnums.ParatureModule module, System.Collections.ArrayList arguments)
            {
                sem.WaitOne();
                _paracredentials = paracredentials;
                _module = module;
                _Arguments = new System.Collections.ArrayList(arguments);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chatList"></param>
            /// <param name="includeTranscripts"></param>
            public void Go(ParaEntityList<ParaObjects.Chat> chatList, Boolean includeTranscripts)
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = ParaEntityParser.FillList<ParaObjects.Chat>(ar.XmlReceived);

                if (includeTranscripts)
                {
                    //Fetch transcripts for each chat
                    foreach (var chat in chatList)
                    {
                        chat.Transcript = ApiHandler.Chat.GetTranscript(chat.Id, _paracredentials);
                    }
                }

                chatList.Data.AddRange(objectlist.Data);
                chatList.ApiCallResponse = ar;
                sem.Release();
            }

            public void Go<T>(ParaEntityList<T> entityList) where T: ParaEntity
            {
                var ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                var objectlist = ParaEntityParser.FillList<T>(ar.XmlReceived);
                objectlist.Data.AddRange(objectlist.Data);
                objectlist.ApiCallResponse = ar;
                sem.Release();
            }
        }
    }
}