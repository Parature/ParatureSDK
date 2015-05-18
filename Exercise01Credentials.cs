using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ParatureSDK;

namespace Exercises
{
    class CredentialProvider
    {
        public static int AcctID
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["AcctID"]);
            }
        }

        public static int DeptID
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["DeptID"]);
            }
        }

        public static string Token
        {
            get
            {
                return ConfigurationManager.AppSettings["APIToken"];
            }
        }

        public static string ServerDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerDomain"];
            }
        }

        public static ParaCredentials Creds
        {
            get
            {
                return new ParaCredentials(Token, ServerDomain, ParaEnums.ApiVersion.v1, AcctID, DeptID, false);
            }
        }
    }
}
