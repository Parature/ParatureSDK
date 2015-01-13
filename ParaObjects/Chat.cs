using System;
using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// Holds all the properties of the Chat module.
    /// </summary>
    public class Chat : ParaEntity
    {
        public Int64 Chat_Number = 0;
        public String Browser_Language = "";
        public String Browser_Type = "";
        public String Browser_Version = "";
        public Customer customer= new Customer();
        public DateTime Date_Created;
        public DateTime Date_Ended;

        public List<Ticket> Related_Tickets;
        public string Email = "";
            
        public Csr Initial_Csr= new Csr();

        public Role Customer_Role = new Role();
        public String Ip_Address = "";
        public Boolean Is_Anonymous;
        public String Referrer_Url = "";
        public Status Status= new Status();
        public String Summary="";
        public String User_Agent="";
        public Int32 Sla_Violation = 0;

        public List<ChatTranscript> ChatTranscripts= new List<ChatTranscript>();

        public Chat()
            : base()
        {
        }

        public override string GetReadableName()
        {
            return "Chat #" + uniqueIdentifier.ToString();
        }
    }
}