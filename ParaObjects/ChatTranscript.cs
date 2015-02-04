using System;

namespace ParatureSDK.ParaObjects
{
    public class ChatTranscript : ParaEntityBaseProperties
    {
        public Boolean IsInternal = false;
        public ParaEnums.ActionHistoryPerformerType Performer;
        public String CsrName = "";
        public String CustomerName = "";
        public String Text = "";
        public DateTime Timestamp = DateTime.MinValue;
    }
}