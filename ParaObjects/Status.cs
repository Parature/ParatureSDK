using System;

namespace ParatureSDK.ParaObjects
{
    public class Status
    {
        // Specific properties for this module

        /// <summary>
        /// Contains all the information regarding the API Call that was made.
        /// </summary>
        public ApiCallResponse ApiCallResponse = new ApiCallResponse();
        private Int64 _StatusID = 0;

        /// <summary>
        /// 1 = Active, -1 = Deactivated
        /// </summary>
        public Int64 StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }
        private string _Name = "";

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public Status()
        {
        }

        public Status(Status status)
        {
            StatusID = status.StatusID;
            Name = status.Name;
        }

        public Status(Int64 ID, string Name)
        {
            StatusID = ID;
            this.Name = Name;
        }
    }
}