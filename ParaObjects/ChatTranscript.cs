using System;

namespace ParatureAPI.ParaObjects
{
    public partial class ChatTranscript:ObjectBaseProperties
    {
        public Boolean isInternal =false;
        public ParaEnums.ActionHistoryPerformerType performer;
        public String csrName="";
        public String customerName="";
        public String Text="";
        public DateTime Timestamp= new DateTime();
    }
}