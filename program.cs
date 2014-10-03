using System;
using System.Collections.Generic;
using System.Text;

namespace ParatureAPI
{
    class program
    {
        static void Main(string[] args)
        {
            var creds =
                new ParaCredentials(
                    "NIH6wZi6gR7G34/2O2@r/7MZFhNh3aMl7aYv23dlMEoBxvpHV7qN6NX6a9DFV/XLCgXcm9apKqK9uKxCnJSmqw==",
                    "https://demo.parature.com", Paraenums.ApiVersion.v1, 7636, 7789, false);

            var tickets = ApiHandler.Ticket.TicketSchema(creds);

        }
    }
}