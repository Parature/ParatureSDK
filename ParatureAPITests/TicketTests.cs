using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParatureAPI;

namespace ParatureAPITests
{
    [TestClass]
    public class TicketTests
    {
        [TestMethod]
        public void TicketSlaParsing()
        {
            var creds = TestCredentials.Credentials;
            creds.Departmentid = 45001;
            var ticket = ApiHandler.Ticket.TicketGetDetails(1016, false, creds);

            Assert.IsTrue(ticket.Ticket_Sla.SlaID != 0);
        }
    }
}
