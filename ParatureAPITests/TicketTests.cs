using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParatureAPI;
using ParatureAPI.ApiHandler;

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
            var ticket = Ticket.TicketGetDetails(1016, false, creds);

            Assert.IsTrue(ticket.Ticket_Sla.SlaID != 0);
        }

        [TestMethod]
        public void CustomFieldOptionParsing()
        {
            var creds = TestCredentials.Credentials;
            creds.Departmentid = 45001;
            var ticket = Ticket.TicketGetDetails(1016, false, creds);
            var ticketOriginOptions = ticket.CustomFieldGetOptions(24);
            Assert.IsTrue(ticketOriginOptions.Count > 1);
        }
    }
}
