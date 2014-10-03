using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ParatureAPI
{
    /// <summary>
    /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
    /// </summary>
    public class ParaCredentials
    {
        #region Properties
        //Specify private properties
        string m_token;
        string m_ServerfarmAddress;
        Paraenums.ApiVersion m_apiversion;
        Paraenums.AutoRetryMode m_AutoretryMode = Paraenums.AutoRetryMode.None;
        int m_accountid;
        int m_departmentid;
        bool m_enforceRequiredFields;

        Int16 m_retriesWaitTime = 260;

        bool m_prevalidateCalls=false;

        /// <summary>
        /// Indicates whether to pre-validate an API call before it occues or not.
        /// Will only check that required static fields are provided.
        /// </summary>
        public bool PrevalidateCalls
        {
            get { return m_prevalidateCalls; }

            set
            {
                m_prevalidateCalls = value;
            }
        }

        /// <summary>
        /// The number of milliseconds to wait for, between API calls.
        /// </summary>
        public Int16 retriesWaitTime
        {
            get { return m_retriesWaitTime; }

            set
            {
                m_retriesWaitTime = value;
            }
        }

        /// <summary>
        /// Indicates the retry behaviour in case the Parature API servers are not available (unhandled exception returned). 
        /// </summary>
        public Paraenums.AutoRetryMode AutoretryMode
        {
            get { return m_AutoretryMode; }

            set
            {
                m_AutoretryMode = value;
            }
        }

        /// <summary>
        /// A valid security token for the CSR account to use to perform the API operations
        /// </summary>
        public string Token
        {
            get { return m_token; }

            set { m_token = value; }
        }

        /// <summary>
        /// A read only value of the server farm's physical address.
        /// </summary>
        public string ServerfarmAddress
        {
            get { return m_ServerfarmAddress; }
            set { m_ServerfarmAddress = value; }
        }

        /// <summary>
        /// The version of the API being used.
        /// </summary>
        public Paraenums.ApiVersion Apiversion
        {
            get { return m_apiversion; }

            set { m_apiversion = value; }
        }

        /// <summary>
        /// Numeric value of the Account being used.
        /// </summary>
        public int Accountid
        {
            get { return m_accountid; }

            set { m_accountid = value; }
        }

        /// <summary>
        /// Numeric value of the department to be used.
        /// </summary>
        public int Departmentid
        {
            get { return m_departmentid; }

            set { m_departmentid = value; }
        }

        /// <summary>
        /// Enables/Disables bypass custom field validation (Dependencies and Required)
        /// </summary>
        public bool EnforceRequiredFields
        {
            get { return m_enforceRequiredFields; }

            set { m_enforceRequiredFields = value; }
        }

        private Dictionary<string, ParaObjects.objectBaseProperties> SchemaCache= new Dictionary<string,ParaObjects.objectBaseProperties>();

        public bool logRetries = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate a new credential object to be passed to the different methods that need it to manage the API Calls with the proper login information.
        /// </summary>
        public ParaCredentials(string token, string serverfarm, Paraenums.ApiVersion apiversion, int accountid, int departmentid, bool enforceRequiredFields)
        {            
            setCredentials(token, serverfarm, apiversion, accountid, departmentid, enforceRequiredFields);
        }
        #endregion
        
        #region Internal Methods
        /// <summary>
        /// Internal method to set required credential
        /// </summary>
        private void setCredentials(string token, String serverfarm, Paraenums.ApiVersion apiversion, int accountid, int departmentid, bool enforceRequiredFields)
        {
            Token = token;
            ServerfarmAddress = serverfarm;
            Apiversion = apiversion;
            Accountid = accountid;
            Departmentid = departmentid;
            EnforceRequiredFields = enforceRequiredFields;
            if (AutoretryMode == Paraenums.AutoRetryMode.Auto)
            {
                if (System.Web.HttpContext.Current != null)
                {
                    // this is a web application context
                    AutoretryMode = Paraenums.AutoRetryMode.WebApplication;
                }
                else
                {
                    // We are not in a web app environment
                    AutoretryMode = Paraenums.AutoRetryMode.ConsoleApp;
                }
            }

        }
        #endregion

        #region Pre-Validation Feature

        /// <summary>
        /// Insert the Schema of an object to the ParaCredentials cache, so that we are able to prevalidated the call before it is placed.
        /// </summary>
        public void PreValidationSchemaAdd(Paraenums.ParatureModule module, ParaObjects.objectBaseProperties moduleSchema)
        {
            if (moduleSchema != null)
            {
                string CacheKey = PreValidationGetCacheKey(module);
                
                if (SchemaCache.ContainsKey(CacheKey))
                {
                    SchemaCache[CacheKey] = moduleSchema;
                }
                else
                {
                    SchemaCache.Add(CacheKey, moduleSchema);
                }
            }
        }

        /// <summary>
        /// Returns the cache key of the schema stored in ParaCredentials.
        /// </summary>
        private string PreValidationGetCacheKey(Paraenums.ParatureModule module)
        {
            string CacheKey = "";
            if (module == Paraenums.ParatureModule.Account || module == Paraenums.ParatureModule.Customer)
            {
                // The module is independant from the department
                CacheKey = Accountid + "-" + module.ToString();
            }

            else
            {
                CacheKey = Accountid + "-" + Departmentid + "-" + module.ToString();
            }
            return CacheKey;
        }

        #endregion
    }
}