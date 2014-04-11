using System;
using System.Collections.Generic;
using System.Text;

namespace ParaConnect
{
    class program
    {
        static void Main(string[] args)
        {

          //  ParaCredentials pc = new ParaCredentials("", Paraenums.ServerFarm.S5, Paraenums.ApiVersion.v1, 0, 0);








































            // ParaCredentials pc = new ParaCredentials("A86B9AapxIW7qcthQ/PSeojqVmFm@DgiCTXHX9e57pliqywmCX7H7SBXGIEzmca5aSYr995rtN2LJOsFtxhwHA==", Paraenums.ServerFarm.SandboxD17, Paraenums.ApiVersion.v1, 5377, 5383, false);
           


            ParaCredentials pc = new ParaCredentials("pfawUquX9qFEV9jDN2yledTDFsEnoS/NTWBzjYHsdgnj3LM1n2i9J9EH7vvZ60xiiz/4mmCo4vTF347/TghIEg==", Paraenums.ServerFarm.D17SB1, Paraenums.ApiVersion.v1, 5377, 5383, false);
            ParaObjects.Ticket t = new ParaObjects.Ticket();
            t.id = 10379361;
            t.Email_Notification = true;
            t.Email_Notification_Additional_Contact = true;
            t.Hide_From_Customer = false;
            t.Ticket_Customer.customerid = 4858598;

            t.CustomFieldSetSelectedFieldOption(96427, 194919);
            t.CustomFieldSetValue(127456, "3");
            t.CustomFieldSetValue(127437, "75667,999,000");
            t.CustomFieldSetValue(127436, "OP1L1");
            t.CustomFieldSetSelectedFieldOption(127440, 290576);
            t.CustomFieldSetSelectedFieldOption(127432, 290564);
            t.CustomFieldSetSelectedFieldOption(127212, 288039);

            ParaObjects.ApiCallResponse tapr = ApiHandler.Ticket.TicketUpdate(t, pc);

            Console.WriteLine("Finished");

            return;
           // ParaCredentials pc = new ParaCredentials("aqUzqIgWcLvBFqWispbtl9GL0dfEFq/teTjdxdmkphauCAPnLCTkonXbT1VCp/6V8qPI4eFRPBWEhEmIPLzhXA==", Paraenums.ServerFarm.S6, Paraenums.ApiVersion.v1, 16035, 16038, false);

            ParaObjects.Ticket tick = ApiHandler.Ticket.TicketGetDetails(53449, false, pc);
            Console.WriteLine(tick.CustomFieldGetValue(27));

            tick.CustomFieldSetValue(27, "My Schedules -  Holiday icon does not display when day after holiday is an approved TAFW");

            ParaObjects.ApiCallResponse apr = ApiHandler.Ticket.TicketUpdate(tick, pc);

            //ParaObjects.Account account = ApiHandler.Account.AccountGetDetails(1012243, pc);

            ////account.Accountid = 0;
            ////account.Account_Name = "Malek - Parature Test Account";

            //account.CustomFieldSetValue(127373, "Value 1");
            //account.CustomFieldSetValue(127374, "Value 2");
            //account.CustomFieldSetValue(127375, "Value 3");

            //ParaObjects.ApiCallResponse apr = ApiHandler.Account.AccountUpdate(account, pc);

            // ParaObjects.ApiCallResponse apr = ApiHandler.Account.AccountInsert(account, pc);
            
            Console.WriteLine("finished");

        }
    }
}
