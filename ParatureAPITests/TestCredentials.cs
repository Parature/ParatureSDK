using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ParatureAPI;

namespace ParatureAPITests
{
    static class TestCredentials
    {
        public static ParaCredentials Credentials
        {
            get
            {
                return new ParaCredentials("1uYL8hcXDySYSXhkQJDwqnjTzrDC2ZxdMG8JkCASeNYmn@GLw@Q2Iotdp12cvvAGq8o7l2FADj2GjNR8JHpLug==", "https://demo.parature.com", ParaEnums.ApiVersion.v1, 45001, 45021, false);
            }
        }
    }
}
