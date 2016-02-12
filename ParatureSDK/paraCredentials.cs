namespace ParatureSDK
{
    /// <summary>
    /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
    /// </summary>
    public class ParaCredentials
    {
        #region Properties
        /// <summary>
        /// A valid security token for the CSR account to use to perform the API operations
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// A read only value of the server farm's physical address.
        /// </summary>
        public string ServerfarmAddress { get; set; }

        /// <summary>
        /// The version of the API being used.
        /// </summary>
        public ParaEnums.ApiVersion Apiversion { get; set; }

        /// <summary>
        /// Numeric value of the Instance being used.
        /// </summary>
        public int Instanceid { get; set; }

        /// <summary>
        /// Numeric value of the department to be used.
        /// </summary>
        public int Departmentid { get; set; }

        /// <summary>
        /// Enables/Disables bypass custom field validation (Dependencies and Required)
        /// </summary>
        public bool EnforceRequiredFields { get; set; }
        #endregion

        /// <summary>
        /// Object used to connect to a Parature Instance
        /// </summary>
        /// <param name="token">API Token</param>
        /// <param name="serverfarmaddress">Address of your severfarm. Include https://. Ex: https://demo.parature.com</param>
        /// <param name="apiversion">Version of the API</param>
        /// <param name="instanceid">Instance ID</param>
        /// <param name="departmentid">Department ID</param>
        /// <param name="enforceRequiredFields">Whether to enforce required custom fields or not</param>
        public ParaCredentials(string token, string serverfarmaddress, ParaEnums.ApiVersion apiversion, int instanceid, int departmentid, bool enforceRequiredFields)
        {
            Token = token;
            ServerfarmAddress = serverfarmaddress;
            Apiversion = apiversion;
            Instanceid = instanceid;
            Departmentid = departmentid;
            EnforceRequiredFields = enforceRequiredFields;
        }
    }
}