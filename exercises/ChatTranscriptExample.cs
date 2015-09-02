using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.ModuleQuery;

namespace Exercises
{
    public static class ChatTranscriptExample
    {
        static ParaService Service { get; set; }

        static ChatTranscriptExample()
        {
            Service = new ParaService(CredentialProvider.Creds);
        }

        public static ParaEntityList<Chat> GetChats(DateTime dateFilter, bool retrieveTranscripts)
        {
            var chatQuery = new ChatQuery();
            chatQuery.RetrieveAllRecords = true;
            chatQuery.AddStaticFieldFilter(ChatQuery.ChatStaticFields.Date_Ended, ParaEnums.QueryCriteria.MoreThan, dateFilter);

            var chats = Service.GetList<Chat>(chatQuery);

            if (retrieveTranscripts)
            {
                foreach (var chat in chats)
                {
                    chat.Transcript = Service.GetChatTranscript(chat.Id);
                }
            }

            //Best practice is to check for an API exception
            if(chats.ApiCallResponse.HasException)
                throw new Exception(chats.ApiCallResponse.ExceptionDetails);

            return chats;
        }

        public static void ProcessTranscripts(ParaEntityList<Chat> chats)
        {
            foreach (var chat in chats)
            {
                var transcripts = Service.GetChatTranscript(chat.Id);
                if (transcripts != null) foreach (var message in transcripts)
                {
                    //Do something

                    //The message from the Customer/CSR
                    var text = message.Text;

                    //The name of the CSR if there is one
                    var csr = message.Csr;

                    //The name of the Customer if there is one, only one Customer or Csr will be populated
                    var customer = message.Customer;

                    //Note: The actual Customer ID can be retrieved from the Chat object

                    //The timestamp
                    var timestamp = message.Timestamp;

                    //If the chat is internal, IE only Csrs can view the message, the message will be falgged as internal
                    var visbleToCsrsOnly = message.Internal;
                }
            }
        }
    }
}
