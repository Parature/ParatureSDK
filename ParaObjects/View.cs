using System;

namespace ParatureAPI.ParaObjects
{
    public abstract class View
    {
        // Specific properties for this module
        //private Int64 id
        private Int64 _ID = 0;

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _Name = "";

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        /// Indicates whether this object is fully loaded or not. An object that is not fully loaded means 
        /// that only the id and name are available.
        /// </summary>
        public bool FullyLoaded = false;

        /// <summary>
        /// Contains all the information regarding the API Call that was made.
        /// </summary>
        public ApiCallResponse ApiCallResponse = new ApiCallResponse();


        public View()
        {
        }

        public View(View view)
        {
            ID = view.ID;
            Name = view.Name;
            FullyLoaded = view.FullyLoaded;
            ApiCallResponse = new ApiCallResponse(view.ApiCallResponse);
        }

    }
}