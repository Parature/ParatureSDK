using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace ParaConnect
{
    /// <summary>
    /// Paraenums contains all the available enumeration for your Parature Connector.
    /// </summary>
    public class Paraenums
    {
        /// <summary>
        /// Contains Enum Helpers
        /// </summary>
        public class EnumHelpers
        {
            /// <summary>
            /// Resolves the server farm name to the a user friendly name.
            /// </summary>
            public static String ServerFarmFriendlyName(Paraenums.ServerFarm sf)
            {
                Type type = sf.GetType();
                MemberInfo[] memInfo = type.GetMember(sf.ToString());

                if (null != memInfo && memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(Paraenums.FriendlyName), false);
                    if (null != attrs && attrs.Length > 0)
                        return ((Paraenums.FriendlyName)attrs[0]).ToString();
                }

                return sf.ToString();
            }

            /// <summary>
            /// Resolves the server farm to its sandbox equivalent.
            /// </summary>
            public static Paraenums.ServerFarm SandboxServerFarm(Paraenums.ServerFarm sf)
            {
                Type type = sf.GetType();
                MemberInfo[] memInfo = type.GetMember(sf.ToString());

                if (null != memInfo && memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(Paraenums.SandboxFarm), false);
                    if (null != attrs && attrs.Length > 0)
                        return ((Paraenums.SandboxFarm)attrs[0]).Sandbox;
                }

                return sf;
            }

            /// <summary>
            /// Returns a list of all Production Server Farms
            /// </summary>
            public static List<Paraenums.ServerFarm> ProductionServerFarms()
            {
                List<Paraenums.ServerFarm> rtn = new List<Paraenums.ServerFarm>();

                foreach (Paraenums.ServerFarm farm in (Enum.GetValues(typeof(Paraenums.ServerFarm))))
                {
                    Type type = farm.GetType();
                    MemberInfo[] memInfo = type.GetMember(farm.ToString());

                    if (null != memInfo && memInfo.Length > 0)
                    {
                        object[] attrs = memInfo[0].GetCustomAttributes(typeof(Paraenums.SandboxFarm), false);
                        if (null != attrs && attrs.Length > 0)
                        {
                            if (((Paraenums.SandboxFarm)attrs[0]).Sandbox != farm)
                            {
                                if (!rtn.Contains(farm))
                                {
                                    rtn.Add(farm);
                                }
                            }
                        }
                    }
                }
                return rtn;
            }

            /// <summary>
            /// Returns a list of all Sandbox Server Farms
            /// </summary>
            public static List<Paraenums.ServerFarm> SandboxServerFarms()
            {
                List<Paraenums.ServerFarm> rtn = new List<Paraenums.ServerFarm>();

                foreach (Paraenums.ServerFarm farm in (Enum.GetValues(typeof(Paraenums.ServerFarm))))
                {
                    Type type = farm.GetType();
                    MemberInfo[] memInfo = type.GetMember(farm.ToString());

                    if (null != memInfo && memInfo.Length > 0)
                    {
                        object[] attrs = memInfo[0].GetCustomAttributes(typeof(Paraenums.SandboxFarm), false);
                        if (null != attrs && attrs.Length > 0)
                        {
                            if (((Paraenums.SandboxFarm)attrs[0]).Sandbox == farm)
                            {
                                if (!rtn.Contains(farm))
                                {
                                    rtn.Add(farm);
                                }
                            }
                        }
                    }
                }
                return rtn;
            }

            /// <summary>
            /// Returns a list of all Public Server Farms
            /// </summary>
            public static List<Paraenums.ServerFarm> PublicServerFarms()
            {
                List<Paraenums.ServerFarm> rtn = new List<Paraenums.ServerFarm>();

                foreach (Paraenums.ServerFarm farm in (Enum.GetValues(typeof(Paraenums.ServerFarm))))
                {
                    Type type = farm.GetType();
                    MemberInfo[] memInfo = type.GetMember(farm.ToString());

                    if (null != memInfo && memInfo.Length > 0)
                    {
                        object[] attrs = memInfo[0].GetCustomAttributes(typeof(Paraenums.isPublic), false);
                        if (null != attrs && attrs.Length > 0)
                        {
                            if (((Paraenums.isPublic)attrs[0]).IsPublic)
                            {
                                if (!rtn.Contains(farm))
                                {
                                    rtn.Add(farm);
                                }
                            }
                        }
                    }
                }

                return rtn;
            }

            /// <summary>
            /// From an accountid, departmentid and token, determines the farm we are in.
            /// </summary>
            public static Paraenums.ServerFarm DetermineServerFarm(Int32 Accountid, Int32 Departmentid, string Token)
            {
                bool IsFound = false;
                Paraenums.ServerFarm server = ServerFarm.SCO;

                Paraenums.ApiVersion apiversion = Paraenums.ApiVersion.v1;
                ParaCredentials pc;

                foreach (Paraenums.ServerFarm farmEnum in Paraenums.EnumHelpers.ProductionServerFarms())
                {
                    pc = new ParaCredentials(Token, farmEnum, apiversion, Accountid, Departmentid, "'1J2*Ll~+?uuE*^e43tFGWf%|t#QD", "6b5,DK!cmw,u6`=iLl-`FP.Tcf+/F");
                   
                    if (ApiHandler.TestApiConnection(pc))
                    {
                        server = farmEnum;
                        IsFound = true;
                        break;
                    }
                }

                if (IsFound == false)
                {                    
                    pc = new ParaCredentials(Token, ServerFarm.Demo, apiversion, Accountid, Departmentid, "'1J2*Ll~+?uuE*^e43tFGWf%|t#QD", "6b5,DK!cmw,u6`=iLl-`FP.Tcf+/F");
                    if (ApiHandler.TestApiConnection(pc))
                    {
                        server = ServerFarm.Demo;
                        IsFound = true;
                    }
                }

                return server;
            }
        }

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
            open,
            closed
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
        /// Mainly used in Synchronization integration projects to mark what should be done with an item.
        /// </summary>
        public enum Operation
        {
            Create,
            Update,
            Delete,
            Transition,
            Ignore
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

        public enum QueryAccountProperties
        {
            Accountid = 10,
            SlaID = 20
        }

        /// <summary>
        /// Lists the possible type of actions you can run against an object (like ticket, for example)
        /// </summary>
        public enum ActionType
        {
            Assign = 0,
            Assign_Queue = 1,
            Grab = 2,
            Solve = 3,
            Copy = 4,
            Escalate = 5,
            Ask = 6,
            Comment = 7,
            Provide = 8,
            CommentTech = 9,
            Other = 10
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
        /// When making API calls, there are only certain type of operations that can be done. This enum provides you with the possible values.
        /// </summary>
        internal enum ApiCallType
        {
            /// <summary>
            /// Read a specific object's information
            /// </summary>
            Read = 0,
            /// <summary>
            /// List items for an object
            /// </summary>
            List = 1,
            /// <summary>
            /// Create an object
            /// </summary>
            Create = 2,
            /// <summary>
            /// Update an object
            /// </summary>
            Update = 3,
            /// <summary>
            /// Delete an object
            /// </summary>
            Delete = 4,
        }


        /// <summary>
        /// Represents the data type of the custom field returned.
        /// </summary>
        public enum CustomFieldDataType
        {
            /// <summary>
            /// Usually a textbox, textarea or a similar field type
            /// </summary>
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
            Date = 8,
            /// <summary>
            /// A Boolean field: a field that holds a True or False value
            /// </summary>
            boolean = 5,
            /// <summary>
            /// An attachment field. Used whenever there is an attachment to an object.
            /// </summary>
            attachment = 6,
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
            Unknown = 7
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



        internal class EnumDescriptionAttribute : Attribute
        {
            private string m_strDescription;
            public EnumDescriptionAttribute(string strPrinterName)
            {
                m_strDescription = strPrinterName;
            }

            public string Description
            {
                get { return m_strDescription; }
            }
        }

        internal class FriendlyName : Attribute
        {
            private readonly string name;
            public FriendlyName(string name) { this.name = name; }
            public override string ToString()
            {
                return name.ToString();
            }
        }

        internal class SandboxFarm : Attribute
        {
            private readonly ServerFarm farm;
            public SandboxFarm(ServerFarm farm) { this.farm = farm; }
            public ServerFarm Sandbox
            {
                get { return farm; }
            }
        }

        internal class isPublic : Attribute
        {
            private readonly bool value;
            public isPublic(bool isPublic) { this.value = isPublic; }
            public bool IsPublic
            {
                get { return value; }
            }
        }

        /// <summary>
        /// This enum lists all the possible server farms that support API calls. Choose one of these values when instantiating a credentials class.
        /// </summary>
        public enum ServerFarm
        {
            /// <summary>
            /// D1
            /// </summary>
            [EnumDescription("https://d1.parature.com/")]
            [FriendlyName("D1")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD1)]
            D1, // "www.parature.net",
            /// <summary>
            /// D1 Sandbox
            /// </summary>
            [EnumDescription("https://d1-sandbox.parature.com/")]
            [FriendlyName("D1-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD1)]
            SandboxD1,
            /// <summary>
            /// S5
            /// </summary>
            [EnumDescription("https://s5.parature.com/")]
            [FriendlyName("S5")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS5)]
            S5,
            /// <summary>
            /// S5 Sandbox
            /// </summary>
            [EnumDescription("https://s5-sandbox.parature.com/")]
            [FriendlyName("S5-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS5)]
            SandboxS5,
            /// <summary>
            /// SCO
            /// </summary>
            [EnumDescription("https://www.supportcenteronline.com/")]
            [FriendlyName("SCO")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxSCO)]
            SCO, // "www.parature.net",
            /// <summary>
            /// SCO Sandbox
            /// </summary>
            [EnumDescription("https://sco-sandbox.parature.com/")]
            [FriendlyName("SCO-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxSCO)]
            SandboxSCO,
            /// <summary>
            /// S3
            /// </summary>
            [EnumDescription("https://s3.parature.com/")]
            [FriendlyName("S3")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS3)]
            S3,
            /// <summary>
            /// S3 Sandbox
            /// </summary>
            [EnumDescription("https://s3-sandbox.parature.com/")]
            [FriendlyName("S3-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS3)]
            SandboxS3,
            /// <summary>
            /// Demo
            /// </summary>
            [EnumDescription("https://demo.parature.com/")]
            [FriendlyName("Demo")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.Demo)]
            Demo,
            /// <summary>
            /// S2
            /// </summary>
            [EnumDescription("https://s2.parature.com/")]
            [FriendlyName("S2")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS2)]
            S2,
            /// <summary>
            /// S2 Sandbox
            /// </summary>
            [EnumDescription("https://s2-sandbox.parature.com/")]
            [FriendlyName("S2-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS2)]
            SandboxS2,
            /// <summary>
            /// S4 Sandbox
            /// </summary>
            [EnumDescription("https://s4.parature.com/")]
            [FriendlyName("S4")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS4)]
            S4,
            /// <summary>
            /// S4 Sandbox
            /// </summary>
            [EnumDescription("https://s4-sandbox.parature.com/")]
            [FriendlyName("S4-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS4)]
            SandboxS4,
            /// <summary>
            /// d2.parature.com, Server address unresolved yet in the list of enums, please do not use yet.
            /// </summary>
            [EnumDescription("https://d2.parature.com/")]
            [FriendlyName("D2")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD2)]
            D2,
            /// <summary>
            /// D2 Sandbox
            /// </summary>
            [EnumDescription("https://d2-sandbox.parature.com/")]
            [FriendlyName("D2-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD2)]
            SandboxD2,
            /// <summary>
            /// d3.parature.com, Server address unresolved yet in the list of enums, please do not use yet. 
            /// </summary>
            [EnumDescription("https://d3.parature.com/")]
            [FriendlyName("D3")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD3)]
            D3,
            /// <summary>
            /// d3-sandbox.parature.com, Server address unresolved yet in the list of enums, please do not use yet. 
            /// </summary>
            [EnumDescription("https://d3-sandbox.parature.com/")]
            [FriendlyName("D3-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD3)]
            SandboxD3,            
            /// <summary>
            /// d5.parature.com
            /// </summary>
            [EnumDescription("https://d5.parature.com/")]
            [FriendlyName("D5")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D5Sandbox)]
            D5,
            /// <summary>
            /// d5-sandbox.parature.com
            /// </summary>
            [EnumDescription("https://d5-sandbox.parature.com/")]
            [FriendlyName("D5-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D5Sandbox)]
            D5Sandbox,
            /// <summary>
            /// 
            /// </summary>
            [EnumDescription("https://d6.parature.com/")]
            [FriendlyName("D6")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D6Sandbox)]
            D6,
            /// <summary>
            /// 
            /// </summary>
            [EnumDescription("https://d6-sandbox.parature.com/")]
            [FriendlyName("D6-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D6Sandbox)]
            D6Sandbox,
            /// <summary>
            /// D17
            /// </summary>
            [EnumDescription("https://d17.parature.com/")]
            [FriendlyName("D17")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD17)]
            D17,
            /// <summary>
            /// D2 Sandbox
            /// </summary>
            [EnumDescription("https://d17-sandbox.parature.com/")]
            [FriendlyName("D17-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD17)]
            SandboxD17,
            /// <summary>
            /// D17 Preview
            /// </summary>
            [EnumDescription("https://d17-preview.parature.com/")]
            [FriendlyName("D17-Preview")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD17)]
            PreviewD17,
            /// <summary>
            /// D4
            /// </summary>
            [EnumDescription("https://d4.parature.com/")]
            [FriendlyName("D4")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD4)]
            D4,
            /// <summary>
            /// D4 Sandbox
            /// </summary>
            [EnumDescription("https://d4-sandbox.parature.com/")]
            [FriendlyName("D4-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD4)]
            SandboxD4,
            /// <summary>
            /// D7
            /// </summary>
            [EnumDescription("https://d7.parature.com/")]
            [FriendlyName("D7")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD7)]
            D7,
            /// <summary>
            /// D7 Sandbox
            /// </summary>
            [EnumDescription("https://d7-sandbox.parature.com/")]
            [FriendlyName("D7-Sandbox")]
            [isPublic(false)]
            [SandboxFarm(ServerFarm.SandboxD7)]
            SandboxD7,
            /// <summary>
            /// D9
            /// </summary>
            [EnumDescription("https://d9.parature.com/")]
            [FriendlyName("D9")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD9)]
            D9,
            /// <summary>
            /// D9 Sandbox
            /// </summary>
            [EnumDescription("https://d9-sandbox.parature.com/")]
            [FriendlyName("D9-Sandbox")]
            [isPublic(false)]
            [SandboxFarm(ServerFarm.SandboxD9)]
            SandboxD9,
            /// <summary>
            /// D11
            /// </summary>
            [EnumDescription("https://d11.parature.com/")]
            [FriendlyName("D11")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD11)]
            D11,
            /// <summary>
            /// D11 Sandbox
            /// </summary>
            [EnumDescription("https://d11-sandbox.parature.com/")]
            [FriendlyName("D11-Sandbox")]
            [isPublic(false)]
            [SandboxFarm(ServerFarm.SandboxD11)]
            SandboxD11,
            /// <summary>
            /// D12
            /// </summary>
            [EnumDescription("https://d12.parature.com/")]
            [FriendlyName("D12")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD12)]
            D12,
            /// <summary>
            /// D11 Sandbox
            /// </summary>
            [EnumDescription("https://d12-sandbox.parature.com/")]
            [FriendlyName("D12-Sandbox")]
            [isPublic(false)]
            [SandboxFarm(ServerFarm.SandboxD12)]
            SandboxD12,
            /// <summary>
            /// S7
            /// </summary>
            [EnumDescription("https://s7.parature.com/")]
            [FriendlyName("S7")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS7)]
            S7,
            /// <summary>
            /// S7 Sandbox
            /// </summary>
            [EnumDescription("https://s7-sandbox.parature.com/")]
            [FriendlyName("S7-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS7)]
            SandboxS7,
            /// <summary>
            /// S6
            /// </summary>
            [EnumDescription("https://s6.parature.com/")]
            [FriendlyName("S6")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS6)]
            S6,
            /// <summary>
            /// S6 Sandbox
            /// </summary>
            [EnumDescription("https://s6-sandbox.parature.com/")]
            [FriendlyName("S6-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS6)]
            SandboxS6,
            /// <summary>
            /// D13
            /// </summary>
            [EnumDescription("https://d13.parature.com/")]
            [FriendlyName("D13")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD13)]
            D13,
            /// <summary>
            /// D13 Sandbox
            /// </summary>
            [EnumDescription("https://d13-sandbox.parature.com/")]
            [FriendlyName("D13-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD13)]
            SandboxD13,
            /// <summary>
            /// D14
            /// </summary>
            [EnumDescription("https://d14.parature.com/")]
            [FriendlyName("D14")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD14)]
            D14,
            /// <summary>
            /// D14 Sandbox
            /// </summary>
            [EnumDescription("https://d14-sandbox.parature.com/")]
            [FriendlyName("D14-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD14)]
            SandboxD14,
            /// <summary>
            /// D8
            /// </summary>
            [EnumDescription("https://d8.parature.com/")]
            [FriendlyName("D8")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD8)]
            D8,
            /// <summary>
            /// D8 Sandbox
            /// </summary>
            [EnumDescription("https://d8-sandbox.parature.com/")]
            [FriendlyName("D8-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD8)]
            SandboxD8,
            /// <summary>
            /// D10
            /// </summary>
            [EnumDescription("https://d10.parature.com/")]
            [FriendlyName("D10")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD10)]
            D10,
            /// <summary>
            /// D10 Sandbox
            /// </summary>
            [EnumDescription("https://d10-sandbox.parature.com/")]
            [FriendlyName("D10-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD10)]
            SandboxD10,
            /// <summary>
            /// D15
            /// </summary>
            [EnumDescription("https://d15.parature.com/")]
            [FriendlyName("D15")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD15)]
            D15,
            /// <summary>
            /// D15 Sandbox
            /// </summary>
            [EnumDescription("https://d15-sandbox.parature.com/")]
            [FriendlyName("D15-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD15)]
            SandboxD15,
            /// <summary>
            /// D16
            /// </summary>
            [EnumDescription("https://d16.parature.com/")]
            [FriendlyName("D16")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD16)]
            D16,
            /// <summary>
            /// D16 Sandbox
            /// </summary>
            [EnumDescription("https://d16-sandbox.parature.com/")]
            [FriendlyName("D16-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD16)]
            SandboxD16,
            /// <summary>
            /// G1
            /// </summary>
            [EnumDescription("https://g1.parature.com/")]
            [FriendlyName("G1")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxG1)]
            G1,
            /// <summary>
            /// G1 Sandbox
            /// </summary>
            [EnumDescription("https://g1-sandbox.parature.com/")]
            [FriendlyName("G1-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxG1)]
            SandboxG1,
            /// <summary>
            /// D17-sb1
            /// </summary>
            [EnumDescription("https://ibm-sb1.parature.com/")]
            [FriendlyName("D17 SB1")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D17SB1)]
            D17SB1,
            /// <summary>
            /// D17-sb2
            /// </summary>
            [EnumDescription("https://ibm-sb2.parature.com/")]
            [FriendlyName("D17 SB2")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D17SB2)]
            D17SB2,
            /// <summary>
            /// D17-sb3
            /// </summary>
            [EnumDescription("https://ibm-sb3.parature.com/")]
            [FriendlyName("D17 SB3")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D17SB3)]
            D17SB3,
            /// <summary>
            /// D17-sb4
            /// </summary>
            [EnumDescription("https://ibm-sb4.parature.com/")]
            [FriendlyName("D17 SB4")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D17SB4)]
            D17SB4,
            /// <summary>
            /// D17-sb5
            /// </summary>
            [EnumDescription("https://ibm-sb5.parature.com/")]
            [FriendlyName("D17 SB5")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D17SB5)]
            D17SB5,
            /// <summary>
            /// D18 Sandbox
            /// </summary>
            [EnumDescription("https://d18-sandbox.parature.com/")]
            [FriendlyName("D18-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD18)]
            SandboxD18,
            /// <summary>
            /// D18
            /// </summary>
            [EnumDescription("https://d18.parature.com/")]
            [FriendlyName("D18")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD18)]
            D18,
            /// <summary>
            /// D19 Sandbox
            /// </summary>
            [EnumDescription("https://d19-sandbox.parature.com/")]
            [FriendlyName("D19-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD19)]
            SandboxD19,
            /// <summary>
            /// D19
            /// </summary>
            [EnumDescription("https://d19.parature.com/")]
            [FriendlyName("D19")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD19)]
            D19,
            /// <summary>
            /// D20 Sandbox
            /// </summary>
            [EnumDescription("https://d20-sandbox.parature.com/")]
            [FriendlyName("D20-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD20)]
            SandboxD20,
            /// <summary>
            /// D20
            /// </summary>
            [EnumDescription("https://d20.parature.com/")]
            [FriendlyName("D20")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD20)]
            D20,

            /// <summary>
            /// BBCRM
            /// </summary>
            [EnumDescription("https://bbcrm.edusupportcenter.com/")]
            [FriendlyName("BBCRM")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxBBCRM)]
            BBCRM,

            /// <summary>
            /// BBCRM Sandbox
            /// </summary>
            [EnumDescription("https://bbcrm-test.edusupportcenter.com/")]
            [FriendlyName("BBCRM-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxBBCRM)]
            SandboxBBCRM,

            /// <summary>
            /// D1 Stage
            /// </summary>
            [EnumDescription("https://d1-stage.parature.net/")]
            [FriendlyName("D1Stage")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.D1Stage)]
            D1Stage,

            /// <summary>
            /// D21 SB
            /// </summary>
            [EnumDescription("https://d21-sandbox.parature.com/")]
            [FriendlyName("D21-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD21)]
            SandboxD21,
            /// <summary>
            /// D21
            /// </summary>
            [EnumDescription("https://d21.parature.com/")]
            [FriendlyName("D21")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD21)]
            D21,
            /// <summary>
            /// D22 SB
            /// </summary>
            [EnumDescription("https://d22-sandbox.parature.com/")]
            [FriendlyName("D22-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD22)]
            SandboxD22,
            /// <summary>
            /// D22
            /// </summary>
            [EnumDescription("https://d22.parature.com/")]
            [FriendlyName("D22")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD22)]
            D22,
            /// <summary>
            /// S8 SB
            /// </summary>
            [EnumDescription("https://s8-sandbox.parature.com/")]
            [FriendlyName("S8-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS8)]
            SandboxS8,
            /// <summary>
            /// S8
            /// </summary>
            [EnumDescription("https://s8.parature.com/")]
            [FriendlyName("S8")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS8)]
            S8,
            /// <summary>
            /// S9 SB
            /// </summary>
            [EnumDescription("https://s9-sandbox.parature.com/")]
            [FriendlyName("S9-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS9)]
            SandboxS9,
            /// <summary>
            /// S9
            /// </summary>
            [EnumDescription("https://s9.parature.com/")]
            [FriendlyName("S9")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS9)]
            S9,
            /// <summary>
            /// S10 SB
            /// </summary>
            [EnumDescription("https://s10-sandbox.parature.com/")]
            [FriendlyName("S10-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS10)]
            SandboxS10,
            /// <summary>
            /// S10
            /// </summary>
            [EnumDescription("https://s10.parature.com/")]
            [FriendlyName("S10")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxS10)]
            S10,
            /// <summary>
            /// D23 SB
            /// </summary>
            [EnumDescription("https://d23-sandbox.parature.com/")]
            [FriendlyName("d23-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD23)]
            SandboxD23,
            /// <summary>
            /// D23
            /// </summary>
            [EnumDescription("https://d23.parature.com/")]
            [FriendlyName("d23")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD23)]
            D23,
            /// <summary>
            /// D24 SB
            /// </summary>
            [EnumDescription("https://d24-sandbox.parature.com/")]
            [FriendlyName("d24-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD24)]
            SandboxD24,
            /// <summary>
            /// D24
            /// </summary>
            [EnumDescription("https://d24.parature.com/")]
            [FriendlyName("d24")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD24)]
            D24,
            /// <summary>
            /// D25 SB
            /// </summary>
            [EnumDescription("https://d25-sandbox.parature.com/")]
            [FriendlyName("d25-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD25)]
            SandboxD25,
            /// <summary>
            /// D25
            /// </summary>
            [EnumDescription("https://d25.parature.com/")]
            [FriendlyName("d25")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD25)]
            D25,
            /// <summary>
            /// D26 SB
            /// </summary>
            [EnumDescription("https://d26-sandbox.parature.com/")]
            [FriendlyName("d26-Sandbox")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD26)]
            SandboxD26,
            /// <summary>
            /// D26
            /// </summary>
            [EnumDescription("https://d26.parature.com/")]
            [FriendlyName("d26")]
            [isPublic(true)]
            [SandboxFarm(ServerFarm.SandboxD26)]
            D26
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
