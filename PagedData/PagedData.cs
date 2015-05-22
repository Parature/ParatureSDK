using System.Collections;
using System.Xml.Serialization;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.PagedData
{
    /// <summary>
    /// Designed to provide properties for paged results.
    /// </summary>
    public abstract class PagedData
    {
        /// <summary>
        /// Total number of items that matched your request
        /// </summary>
        [XmlAttribute("total")]
        public int TotalItems = 0;
        /// <summary>
        /// The number of items returned with the current call.
        /// </summary>
        [XmlAttribute("results")]
        public int ResultsReturned = 0;
        /// <summary>
        /// The maximum number of items returned per call.
        /// </summary>
        [XmlAttribute("page-size")]
        public int PageSize = 0;
        /// <summary>
        /// Number of this page
        /// </summary>
        [XmlAttribute("page")]
        public int PageNumber = 1;
        /// <summary>
        /// Contains all the information regarding the API Call that was made.
        /// </summary>
        public ApiCallResponse ApiCallResponse = new ApiCallResponse();

        /// <summary>
        /// 
        /// </summary>
        protected PagedData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedData"></param>
        protected PagedData(PagedData pagedData)
        {
            TotalItems = pagedData.TotalItems;
            ResultsReturned = pagedData.ResultsReturned;
            PageSize = pagedData.PageSize;
            PageNumber = pagedData.PageNumber;
            ApiCallResponse = new ApiCallResponse(pagedData.ApiCallResponse);
        }
    }
}