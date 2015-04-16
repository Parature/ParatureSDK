using System.Xml.Serialization;

namespace ParatureSDK
{
    /// <summary>
    /// Paraenums contains all the available enumeration for your Parature Connector.
    /// </summary>
    public class ParaEnums
    {
        /// <summary>
        /// This is used when using the sorting capabilities of the APIs. It helps set up the sorting direction.
        /// </summary>
        public enum QuerySortBy
        {
            Asc,
            Desc
        }


        public enum TicketStatusType
        {
            All,
            Open,
            Closed
        }

        /// <summary>
        /// The Type of the performer that ran the action history, it is whether a CSR or a Customer.
        /// </summary>
        public enum ActionHistoryPerformerType
        {
            Csr,
            Customer,
            System
        }

        /// <summary>
        /// This enum defines the behaviour of the api handler when unhandled errors occured: should it retry the call or not.
        /// </summary>
        public enum AutoRetryMode
        {
            None,
            WebApplication,
            ConsoleApp,
            Auto
        }

        /// <summary>
        /// List all possible criteria you can use when querying a list of objects.
        /// </summary>
        public enum QueryCriteria
        {
            /// <summary>
            /// Use this for string searches, where you are looking for the object's field to have a string
            /// like the one you are looking for.
            /// </summary>
            Like,
            /// <summary>
            /// Can be used for dates, as well as numeric values. Equivalent to a ">" sign.
            /// </summary>
            MoreThan,
            /// <summary>
            /// Can be used for dates, as well as numeric values.
            /// </summary>
            LessThan,
            /// <summary>
            /// Can be used for dates, as well as numeric values. Equivalent to a "=" sign.
            /// </summary>
            Equal

        }


        /// <summary>
        /// Use to specify if field contains or does not contain a value
        /// </summary>
        public enum FieldValueFilter
        {
            /// <summary>
            /// Use for filtering if a field that does not contain a value
            /// </summary>
            IsNull,
            /// <summary>
            /// Use for filtering if a field contains any value
            /// </summary>
            IsNotNull
        }

        /// <summary>
        /// When reading an object: Account, Ticket, Contact, etc, this enum will provide indications regarding the level of depth of the information returned.
        /// </summary>
        public enum RequestDepth
        {
            /// <summary>
            /// The Standard depth will only return the id and name of the second level objects linked to the main object you are requesting. If you request a Ticket details, for example, it will also return a Contact object, but only the id and the name of the contact object will be filled. All other properties of that object will be null.
            /// </summary>
            Standard = 1,
            /// <summary>
            /// The Second Level depth will go and fill all of the second level objects properties. If you request a Ticket details, for example, it will also return a Contact object with all properties filled. However, if the contact has products objects associated with it, only the product id and name of the returned product objects will be filled. All other product properties will be null.
            /// </summary>
            SecondLevel = 2,
            /// <summary>
            /// The Third Level depth will go and fill all of the second and third level objects properties
            /// </summary>
            ThirdLevel = 3
        }

        /// <summary>
        /// Represents the data type of the field returned.
        /// </summary>
        public enum FieldDataType
        {
            /// <summary>
            /// Usually a textbox, textarea or a similar field type
            /// </summary>
            [XmlEnum("string")]
            String = 0,
            /// <summary>
            /// DropdownList, CheckBoxList, etc
            /// </summary>
            Option = 1,
            /// <summary>
            /// A read only field
            /// </summary>
            ReadOnly = 2,
            /// <summary>
            /// An Integer field
            /// </summary>
            Int = 3,
            /// <summary>
            /// A date/time field
            /// </summary>
            DateTime = 4,
            /// <summary>
            /// A date/time field
            /// </summary>
            USDate = 14,
            /// <summary>
            /// A date/time field
            /// </summary>
            [XmlEnum("date")]
            Date = 8,
            /// <summary>
            /// A Boolean field: a field that holds a True or False value
            /// </summary>
            [XmlEnum("boolean")]
            Boolean = 5,
            /// <summary>
            /// An attachment field. Used whenever there is an attachment to an object.
            /// </summary>
            Attachment = 6,
            /// <summary>
            /// A float data type
            /// </summary>
            Float = 9,
            /// <summary>
            /// A Email data type
            /// </summary>
            Email = 10,
            /// <summary>
            /// A US Phone data type
            /// </summary>
            UsPhone = 11,
            /// <summary>
            /// A URL data type
            /// </summary>
            Url = 12,
            /// <summary>
            /// A International Phone data type
            /// </summary>
            InternationalPhone = 13,
            /// <summary>
            /// Returns this value only if it couldn't determine the type of the field. Please let us know if you encounter a field of this type.
            /// </summary>            
            Unknown = 7,
            /// <summary>
            /// Reference to another Parature Module
            /// </summary>
            EntityReference = 15,
            Role = 16,
            Sla = 17,
            Folder = 18,
            Status = 19,
            Action = 20,
            Timezone = 21,
            Eula = 22,
            Department = 23,
            Queue = 24,
            History = 25,
        }


        /// <summary>
        /// Provides with the possible calls methods to the APIs.
        /// </summary>
        internal enum ApiCallHttpMethod
        {
            /// <summary>
            /// Used when getting data (read, list or schema call)
            /// </summary>
            Get = 0,
            /// <summary>
            /// Creates an object
            /// </summary>
            Post = 1,
            /// <summary>
            /// Updates an object
            /// </summary>
            Update = 2,
            /// <summary>
            /// Deletes an object
            /// </summary>
            Delete = 3
        }




        /// <summary>
        /// This enum lists all the possible modules within Parature. Choose one of these values when calling a method that requires a module.
        /// </summary>
        public enum ParatureModule
        {
            /// <summary>
            /// Tickets module
            /// </summary>
            Ticket = 0,
            /// <summary>
            /// Account module
            /// </summary>
            Account = 1,
            /// <summary>
            /// Customer module
            /// </summary>
            Customer = 2,
            /// <summary>
            /// Downloads module
            /// </summary>
            Download = 3,
            /// <summary>
            /// Knowledge base module
            /// </summary>
            Article = 4,
            /// <summary>
            /// Products module
            /// </summary>
            Product = 5,
            /// <summary>
            /// Assets module
            /// </summary>
            Asset = 6,
            /// <summary>
            /// CSR module
            /// </summary>
            Csr = 7,
            /// <summary>
            /// Chat Module
            /// </summary>
            Chat=8

        }

        /// <summary>
        /// This enum lists all the possible complex entities within Parature. Choose one of these values when calling a method that requires an entity.
        /// </summary>
        public enum ParatureEntity
        {
            /// <summary>
            /// 
            /// </summary>
            ArticleFolder = 200,
            /// <summary>
            /// 
            /// </summary>
            DownloadFolder = 100,
            /// <summary>
            /// 
            /// </summary>
            Sla = 0,
            /// <summary>
            /// 
            /// </summary>
            ProductFolder = 1,
            /// <summary>
            /// 
            /// </summary>
            Queue = 2,
            /// <summary>
            /// 
            /// </summary>
            Action = 3,
            /// <summary>
            /// 
            /// </summary>
            status = 4,
            /// <summary>
            /// 
            /// </summary>
            TicketStatus = 5,
            /// <summary>
            /// 
            /// </summary>
            CsrStatus = 6,
            /// <summary>
            /// 
            /// </summary>
            role = 7,
            /// <summary>
            /// 
            /// </summary>
            view = 8,
            /// <summary>
            /// 
            /// </summary>
            Department = 9,
            /// <summary>
            /// 
            /// </summary>
            Timezone = 10,
            /// <summary>
            /// 
            /// </summary>
            CustomerStatus = 11,
            ChatTranscript=12
        }

        /// <summary>
        /// The version of the API being used. This is mainly needed in the credentials class.
        /// </summary>
        public enum ApiVersion
        {
            /// <summary>
            /// version 1 
            /// </summary>
            v1 = 1
        }

        /// <summary>
        /// The format you would like the output to be in.
        /// </summary>
        public enum OutputFormat
        {
            /// <summary>
            /// Regular call, fills your objects normally. Unless you need a json or a Rss feed, please use this one.
            /// </summary>
            native = 1,
            /// <summary>
            /// Returns an Rss
            /// </summary>
            rss = 2,
            /// <summary>
            /// Returns a json format
            /// </summary>
            json = 3
        }
    }
}
