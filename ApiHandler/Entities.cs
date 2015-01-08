using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.ApiHandler
{
    /// <summary>
    /// Contains all the methods to access shared entities (like CSRs, SLAs, departments, etc)
    /// </summary>
    public class Entities
    {
        public partial class Timezone
        {
            /// <summary>
            /// Returns a Timezone object with all of its properties filled.
            /// </summary>
            /// <param name="Csrid">
            ///The Timezone id that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>               
            public static ParaObjects.Timezone TimezoneGetDetails(Int64 TimezoneId, ParaCredentials ParaCredentials)
            {
                ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
                Timezone = TimezoneFillDetails(TimezoneId, ParaCredentials);
                return Timezone;
            }

            /// <summary>
            /// Returns an Timezone object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="TimezoneXML">
            /// The Timezone XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Timezone TimezoneGetDetails(XmlDocument TimezoneXML)
            {
                ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
                Timezone = XmlToObjectParser.TimezoneParser.TimezoneFill(TimezoneXML);

                return Timezone;
            }

            /// <summary>
            /// Returns an Timezone list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="TimezoneListXML">
            /// The Timezone List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static TimezonesList TimezoneGetList(XmlDocument TimezoneListXML)
            {
                TimezonesList TimezonesList = new TimezonesList();
                TimezonesList = XmlToObjectParser.TimezoneParser.TimezonesFillList(TimezoneListXML);

                TimezonesList.ApiCallResponse.xmlReceived = TimezoneListXML;

                return TimezonesList;
            }
            /// <summary>
            /// Get the list of Timezones from within your Parature license.
            /// </summary>
            public static TimezonesList TimezoneGetList(ParaCredentials ParaCredentials)
            {
                return TimezoneFillList(ParaCredentials, new EntityQuery.TimezoneQuery());
            }

            /// <summary>
            /// Get the list of Timezones from within your Parature license.
            /// </summary>
            public static TimezonesList TimezoneGetList(ParaCredentials ParaCredentials, EntityQuery.TimezoneQuery Query)
            {
                return TimezoneFillList(ParaCredentials, Query);
            }
            /// <summary>
            /// Fills a Timezone List object.
            /// </summary>
            private static TimezonesList TimezoneFillList(ParaCredentials ParaCredentials, EntityQuery.TimezoneQuery Query)
            {

                TimezonesList TimezoneList = new TimezonesList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.Timezone, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    TimezoneList = XmlToObjectParser.TimezoneParser.TimezonesFillList(ar.xmlReceived);
                }
                TimezoneList.ApiCallResponse = ar;
                return TimezoneList;
            }
            static ParaObjects.Timezone TimezoneFillDetails(Int64 TimezoneId, ParaCredentials ParaCredentials)
            {
                ParaObjects.Timezone Timezone = new ParaObjects.Timezone();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.Timezone, TimezoneId);
                if (ar.HasException == false)
                {
                    Timezone = XmlToObjectParser.TimezoneParser.TimezoneFill(ar.xmlReceived);
                }
                else
                {

                    Timezone.TimezoneID = 0;
                }

                return Timezone;
            }
        }

        public partial class Status
        {
            /// <summary>
            /// Returns a Status object with all of its properties filled.
            /// </summary>
            /// <param name="Csrid">
            ///The Status id that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>               
            public static ParaObjects.Status StatusGetDetails(Int64 StatusId, ParaCredentials ParaCredentials)
            {
                ParaObjects.Status Status = new ParaObjects.Status();
                Status = StatusFillDetails(StatusId, ParaCredentials);
                return Status;
            }

            /// <summary>
            /// Returns an Status object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="StatusXML">
            /// The Status XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Status StatusGetDetails(XmlDocument StatusXML)
            {
                ParaObjects.Status Status = new ParaObjects.Status();
                Status = XmlToObjectParser.StatusParser.StatusFill(StatusXML);

                return Status;
            }

            /// <summary>
            /// Returns an Status list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="StatusListXML">
            /// The Status List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static StatusList StatusGetList(XmlDocument StatusListXML)
            {
                StatusList StatussList = new StatusList();
                StatussList = XmlToObjectParser.StatusParser.StatusFillList(StatusListXML);

                StatussList.ApiCallResponse.xmlReceived = StatusListXML;

                return StatussList;
            }

            /// <summary>
            /// Get the list of Statuss from within your Parature license.
            /// </summary>
            public static StatusList StatusGetList(ParaCredentials ParaCredentials)
            {
                return StatusFillList(ParaCredentials, new EntityQuery.StatusQuery());
            }

            /// <summary>
            /// Get the list of Statuss from within your Parature license.
            /// </summary>
            public static StatusList StatusGetList(ParaCredentials ParaCredentials, EntityQuery.StatusQuery Query)
            {
                return StatusFillList(ParaCredentials, Query);
            }
            /// <summary>
            /// Fills a Status List object.
            /// </summary>
            private static StatusList StatusFillList(ParaCredentials ParaCredentials, EntityQuery.StatusQuery Query)
            {

                StatusList StatusList = new StatusList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.status, Query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    StatusList = XmlToObjectParser.StatusParser.StatusFillList(ar.xmlReceived);
                }
                StatusList.ApiCallResponse = ar;


                // Checking if the system needs to recursively call all of the data returned.
                if (Query.RetrieveAllRecords)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        StatusList objectlist = new StatusList();

                        if (StatusList.TotalItems > StatusList.Statuses.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            Query.PageNumber = Query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(ParaCredentials, ParaEnums.ParatureEntity.status, Query.BuildQueryArguments());

                            objectlist = XmlToObjectParser.StatusParser.StatusFillList(ar.xmlReceived);

                            if (objectlist.Statuses.Count == 0)
                            {
                                continueCalling = false;
                            }

                            StatusList.Statuses.AddRange(objectlist.Statuses);
                            StatusList.ResultsReturned = StatusList.Statuses.Count;
                            StatusList.PageNumber = Query.PageNumber;
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            StatusList.ApiCallResponse = ar;
                        }
                    }
                }

                return StatusList;
            }
            static ParaObjects.Status StatusFillDetails(Int64 StatusId, ParaCredentials ParaCredentials)
            {
                ParaObjects.Status Status = new ParaObjects.Status();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(ParaCredentials, ParaEnums.ParatureEntity.status, StatusId);
                if (ar.HasException == false)
                {
                    Status = XmlToObjectParser.StatusParser.StatusFill(ar.xmlReceived);
                }
                else
                {

                    Status.StatusID = 0;
                }

                return Status;
            }
        }

        public partial class Role
        {
            /// <summary>
            /// Returns a Role object with all of its properties filled.
            /// </summary>
            /// <param name="RoleID">
            /// The Role number that you would like to get the details of.
            /// Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="ParaCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>
            /// <returns></returns>
            public static ParaObjects.Role RoleGetDetails(Int64 RoleID, ParaCredentials ParaCredentials)
            {
                ParaObjects.Role Role = new ParaObjects.Role();
                Role = RoleFillDetails(RoleID, ParaCredentials);
                return Role;
            }

            /// <summary>
            /// Returns an role object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="RoleXML">
            /// The Role XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Role RoleGetDetails(XmlDocument RoleXML)
            {
                ParaObjects.Role role = new ParaObjects.Role();
                role = XmlToObjectParser.RoleParser.RoleFill(RoleXML);

                return role;
            }

            /// <summary>
            /// Returns an role list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="RoleListXML">
            /// The Role List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static RolesList RolesGetList(XmlDocument RoleListXML)
            {
                RolesList rolesList = new RolesList();
                rolesList = XmlToObjectParser.RoleParser.RolesFillList(RoleListXML);

                rolesList.ApiCallResponse.xmlReceived = RoleListXML;

                return rolesList;
            }

            /// <summary>
            /// Get the List of Roles from within your Parature license
            /// </summary>
            /// <param name="ParaCredentials"></param>
            /// <param name="Query"></param>
            /// <returns></returns>
            public static RolesList RolesGetList(ParaCredentials ParaCredentials, EntityQuery.RoleQuery Query, ParaEnums.ParatureModule Module)
            {
                return RoleFillList(ParaCredentials, Query, Module);
            }


            public static RolesList RolesGetList(ParaCredentials ParaCredentials, ParaEnums.ParatureModule Module)
            {
                return RoleFillList(ParaCredentials, new EntityQuery.RoleQuery(), Module);
            }

            /// <summary>
            /// Fills a Role list object
            /// </summary>
            /// <param name="paraCredentials"></param>
            /// <param name="query"></param>
            /// <param name="Module"></param>
            /// <returns></returns>
            private static RolesList RoleFillList(ParaCredentials paraCredentials, EntityQuery.RoleQuery query, ParaEnums.ParatureModule Module)
            {
                RolesList RolesList = new RolesList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, Module, ParaEnums.ParatureEntity.role, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    RolesList = XmlToObjectParser.RoleParser.RolesFillList(ar.xmlReceived);
                }
                RolesList.ApiCallResponse = ar;

                // Checking if the system needs to recursively call all of the data returned.
                if (query.RetrieveAllRecords)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        RolesList objectlist = new RolesList();

                        if (RolesList.TotalItems > RolesList.Roles.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.role, query.BuildQueryArguments());

                            objectlist = XmlToObjectParser.RoleParser.RolesFillList(ar.xmlReceived);

                            if (objectlist.Roles.Count == 0)
                            {
                                continueCalling = false;
                            }

                            RolesList.Roles.AddRange(objectlist.Roles);
                            RolesList.ResultsReturned = RolesList.Roles.Count;
                            RolesList.PageNumber = query.PageNumber;
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            RolesList.ApiCallResponse = ar;
                        }
                    }
                }

                return RolesList;
            }

            static ParaObjects.Role RoleFillDetails(Int64 roleId, ParaCredentials paraCredentials)
            {
                ParaObjects.Role Role = new ParaObjects.Role();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.role, roleId);
                if (ar.HasException == false)
                {
                    Role = XmlToObjectParser.RoleParser.RoleFill(ar.xmlReceived);
                }
                else
                {
                    Role.RoleID = 0;
                }
                return Role;
            }
        }

        public class Sla
        {
            /// <summary>
            /// Returns an SLA object with all of its properties filled.
            /// </summary>
            /// <param name="slaId">
            ///The SLA number that you would like to get the details of. 
            ///Value Type: <see cref="Int64" />   (System.Int64)
            ///</param>
            /// <param name="paraCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>               
            public static ParaObjects.Sla SLAGetDetails(Int64 slaId, ParaCredentials paraCredentials)
            {
                ParaObjects.Sla Sla = new ParaObjects.Sla();
                Sla = SlaFillDetails(slaId, paraCredentials);
                return Sla;
            }

            /// <summary>
            /// Returns an sla object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="slaXml">
            /// The Sla XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Sla SLAGetDetails(XmlDocument slaXml)
            {
                ParaObjects.Sla sla = new ParaObjects.Sla();
                sla = XmlToObjectParser.SlaParser.SlaFill(slaXml);

                return sla;
            }

            /// <summary>
            /// Get the list of SLAs from within your Parature license.
            /// </summary>
            public static SlasList SLAsGetList(ParaCredentials paraCredentials)
            {
                return SlaFillList(paraCredentials, new EntityQuery.SlaQuery());
            }

            /// <summary>
            /// Get the list of SLAs from within your Parature license.
            /// </summary>
            public static SlasList SLAsGetList(ParaCredentials paraCredentials, EntityQuery.SlaQuery query)
            {
                return SlaFillList(paraCredentials, query);
            }

            /// <summary>
            /// Returns an sla list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="slaListXml">
            /// The Sla List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static SlasList SLAsGetList(XmlDocument slaListXml)
            {
                SlasList slasList = new SlasList();
                slasList = XmlToObjectParser.SlaParser.SlasFillList(slaListXml);

                slasList.ApiCallResponse.xmlReceived = slaListXml;

                return slasList;
            }

            /// <summary>
            /// Fills a Sla list object.
            /// </summary>
            private static SlasList SlaFillList(ParaCredentials paraCredentials, EntityQuery.SlaQuery query)
            {

                SlasList SlasList = new SlasList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Sla, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    SlasList = XmlToObjectParser.SlaParser.SlasFillList(ar.xmlReceived);
                }
                SlasList.ApiCallResponse = ar;

                // Checking if the system needs to recursively call all of the data returned.
                if (query.RetrieveAllRecords)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        SlasList objectlist = new SlasList();

                        if (SlasList.TotalItems > SlasList.Slas.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Sla, query.BuildQueryArguments());

                            objectlist = XmlToObjectParser.SlaParser.SlasFillList(ar.xmlReceived);

                            if (objectlist.Slas.Count == 0)
                            {
                                continueCalling = false;
                            }

                            SlasList.Slas.AddRange(objectlist.Slas);
                            SlasList.ResultsReturned = SlasList.Slas.Count;
                            SlasList.PageNumber = query.PageNumber;
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            SlasList.ApiCallResponse = ar;
                        }
                    }
                }


                return SlasList;
            }

            private static ParaObjects.Sla SlaFillDetails(Int64 slaId, ParaCredentials paraCredentials)
            {
                ParaObjects.Sla Sla = new ParaObjects.Sla();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.Sla, slaId);
                if (ar.HasException == false)
                {
                    Sla = XmlToObjectParser.SlaParser.SlaFill(ar.xmlReceived);
                }
                else
                {

                    Sla.SlaID = 0;
                }

                //Sla.ApiCallResponse = ar;
                return Sla;
            }
        }

        public partial class Department
        {
            /// <summary>
            /// Returns a Department object with all of its properties filled.
            /// </summary>
            /// <param name="departmentid">
            ///The Department number that you would like to get the details of. 
            ///</param>
            /// <param name="paraCredentials">
            /// The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            /// </param>               
            public static ParaObjects.Department DepartmentGetDetails(Int64 departmentid, ParaCredentials paraCredentials)
            {
                ParaObjects.Department department = new ParaObjects.Department();
                department = DepartmentFillDetails(departmentid, paraCredentials);
                return department;
            }

            /// <summary>
            /// Returns a department object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="departmentXml">
            /// The department XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Department DepartmentGetDetails(XmlDocument departmentXml)
            {
                ParaObjects.Department department = new ParaObjects.Department();
                department = XmlToObjectParser.DepartmentParser.DepartmentFill(departmentXml);

                return department;
            }

            /// <summary>
            /// Returns an Department list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="departmentListXml">
            /// The Departments List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static DepartmentsList DepartmentsGetList(XmlDocument departmentListXml)
            {
                DepartmentsList departmentslist = new DepartmentsList();
                departmentslist = XmlToObjectParser.DepartmentParser.DepartmentsFillList(departmentListXml);

                departmentslist.ApiCallResponse.xmlReceived = departmentListXml;

                return departmentslist;
            }

            /// <summary>
            /// Get the list of Departments from within your Parature license.
            /// </summary>
            public static DepartmentsList DepartmentsGetList(ParaCredentials paraCredentials, EntityQuery.DepartmentQuery query)
            {
                return DepartmentFillList(paraCredentials, query);
            }
            /// <summary>
            /// Fills a Departmentslist object.
            /// </summary>
            private static DepartmentsList DepartmentFillList(ParaCredentials paraCredentials, EntityQuery.DepartmentQuery query)
            {

                DepartmentsList departmentsList = new DepartmentsList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Department, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    departmentsList = XmlToObjectParser.DepartmentParser.DepartmentsFillList(ar.xmlReceived);
                }
                departmentsList.ApiCallResponse = ar;
                return departmentsList;
            }

            private static ParaObjects.Department DepartmentFillDetails(Int64 departmentid, ParaCredentials paraCredentials)
            {
                ParaObjects.Department department = new ParaObjects.Department();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.Department, departmentid);
                if (ar.HasException == false)
                {
                    department = XmlToObjectParser.DepartmentParser.DepartmentFill(ar.xmlReceived);
                }
                else
                {
                    department.DepartmentID = 0;
                }
                department.ApiCallResponse = ar;

                return department;
            }
        }

        public class CustomerStatus
        {
            ///  <summary>
            ///  Returns an Customer object with all of its properties filled.
            ///  </summary>
            /// <param name="customerStatusId"></param>
            /// <param name="paraCredentials">
            ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            ///  </param>               
            public static ParaObjects.CustomerStatus CustomerStatusGetDetails(Int64 customerStatusId, ParaCredentials paraCredentials)
            {
                ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
                CustomerStatus = CustomerStatusFillDetails(customerStatusId, paraCredentials);
                return CustomerStatus;
            }

            /// <summary>
            /// Get the list of Customers from within your Parature license.
            /// </summary>
            public static CustomerStatusList CustomerStatusGetList(ParaCredentials paraCredentials)
            {
                return CustomerStatusFillList(paraCredentials, new EntityQuery.CustomerStatusQuery());
            }

            /// <summary>
            /// Get the list of Customers from within your Parature license.
            /// </summary>
            public static CustomerStatusList CustomerStatusGetList(ParaCredentials paraCredentials, EntityQuery.CustomerStatusQuery query)
            {
                return CustomerStatusFillList(paraCredentials, query);
            }

            /// <summary>
            /// Returns an CustomerStatus object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="customerStatusXml">
            /// The CustomerStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CustomerStatus CustomerStatusGetDetails(XmlDocument customerStatusXml)
            {
                ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
                CustomerStatus = XmlToObjectParser.CustomerStatusParser.CustomerStatusFill(customerStatusXml);

                CustomerStatus.ApiCallResponse.xmlReceived = customerStatusXml;
                CustomerStatus.ApiCallResponse.Objectid = CustomerStatus.StatusID;

                return CustomerStatus;
            }

            /// <summary>
            /// Returns an CustomerStatus list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="customerStatusListXml">
            /// The CustomerStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static CustomerStatusList CustomerStatusGetList(XmlDocument customerStatusListXml)
            {
                CustomerStatusList CustomerStatussList = new CustomerStatusList();
                CustomerStatussList = XmlToObjectParser.CustomerStatusParser.CustomerStatusFillList(customerStatusListXml);

                CustomerStatussList.ApiCallResponse.xmlReceived = customerStatusListXml;

                return CustomerStatussList;
            }

            /// <summary>
            /// Fills a Sla list object.
            /// </summary>
            private static CustomerStatusList CustomerStatusFillList(ParaCredentials paraCredentials, EntityQuery.CustomerStatusQuery query)
            {

                CustomerStatusList CustomerStatusList = new CustomerStatusList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Customer, ParaEnums.ParatureEntity.status, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    CustomerStatusList = XmlToObjectParser.CustomerStatusParser.CustomerStatusFillList(ar.xmlReceived);
                }
                CustomerStatusList.ApiCallResponse = ar;





                return CustomerStatusList;
            }

            private static ParaObjects.CustomerStatus CustomerStatusFillDetails(Int64 customerStatusId, ParaCredentials paraCredentials)
            {
                ParaObjects.CustomerStatus CustomerStatus = new ParaObjects.CustomerStatus();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.CustomerStatus, customerStatusId);
                if (ar.HasException == false)
                {
                    CustomerStatus = XmlToObjectParser.CustomerStatusParser.CustomerStatusFill(ar.xmlReceived);
                }
                else
                {

                    CustomerStatus.StatusID = 0;
                }

                return CustomerStatus;
            }
        }

        public class CsrStatus
        {
            ///  <summary>
            ///  Returns an Csr object with all of its properties filled.
            ///  </summary>
            ///  <param name="Csrid">
            /// The Csr number that you would like to get the details of. 
            /// Value Type: <see cref="Int64" />   (System.Int64)
            /// </param>
            /// <param name="csrStatusId"></param>
            /// <param name="paraCredentials">
            ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            ///  </param>               
            public static ParaObjects.CsrStatus CsrStatusGetDetails(Int64 csrStatusId, ParaCredentials paraCredentials)
            {
                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                CsrStatus = CsrStatusFillDetails(csrStatusId, paraCredentials);
                return CsrStatus;
            }
               
            /// <summary>
            /// Get the list of Csrs from within your Parature license.
            /// </summary>
            public static CsrStatusList CsrStatusGetList(ParaCredentials paraCredentials)
            {
                return CsrStatusFillList(paraCredentials, new EntityQuery.CsrStatusQuery());
            }

            /// <summary>
            /// Get the list of Csrs from within your Parature license.
            /// </summary>
            public static CsrStatusList CsrStatusGetList(ParaCredentials paraCredentials, EntityQuery.CsrStatusQuery query)
            {
                return CsrStatusFillList(paraCredentials, query);
            }

            /// <summary>
            /// Returns an CsrStatus object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="csrStatusXml">
            /// The CsrStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.CsrStatus CsrStatusGetDetails(XmlDocument csrStatusXml)
            {
                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                CsrStatus = XmlToObjectParser.CsrStatusParser.CsrStatusFill(csrStatusXml);

                CsrStatus.ApiCallResponse.xmlReceived = csrStatusXml;
                CsrStatus.ApiCallResponse.Objectid = CsrStatus.StatusID;

                return CsrStatus;
            }

            /// <summary>
            /// Returns an CsrStatus list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="csrStatusListXml">
            /// The CsrStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static CsrStatusList CsrStatusGetList(XmlDocument csrStatusListXml)
            {
                CsrStatusList CsrStatussList = new CsrStatusList();
                CsrStatussList = XmlToObjectParser.CsrStatusParser.CsrStatusFillList(csrStatusListXml);

                CsrStatussList.ApiCallResponse.xmlReceived = csrStatusListXml;

                return CsrStatussList;
            }

            /// <summary>
            /// Fills a Sla list object.
            /// </summary>
            private static CsrStatusList CsrStatusFillList(ParaCredentials paraCredentials, EntityQuery.CsrStatusQuery query)
            {

                CsrStatusList CsrStatusList = new CsrStatusList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Csr, ParaEnums.ParatureEntity.status, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    CsrStatusList = XmlToObjectParser.CsrStatusParser.CsrStatusFillList(ar.xmlReceived);
                }
                CsrStatusList.ApiCallResponse = ar;
                return CsrStatusList;
            }

            private static ParaObjects.CsrStatus CsrStatusFillDetails(Int64 csrStatusId, ParaCredentials paraCredentials)
            {
                ParaObjects.CsrStatus CsrStatus = new ParaObjects.CsrStatus();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.CsrStatus, csrStatusId);
                if (ar.HasException == false)
                {
                    CsrStatus = XmlToObjectParser.CsrStatusParser.CsrStatusFill(ar.xmlReceived);
                }
                else
                {

                    CsrStatus.StatusID = 0;
                }

                return CsrStatus;
            }
        }

        public class TicketStatus
        {
            ///  <summary>
            ///  Returns an Csr object with all of its properties filled.
            ///  </summary>
            /// <param name="ticketStatusId"></param>
            /// <param name="paraCredentials">
            ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            ///  </param>               
            public static ParaObjects.TicketStatus TicketStatusGetDetails(Int64 ticketStatusId, ParaCredentials paraCredentials)
            {
                ParaObjects.TicketStatus ticketStatus = new ParaObjects.TicketStatus();
                ticketStatus = TicketStatusFillDetails(ticketStatusId, paraCredentials);
                return ticketStatus;
            }

            /// <summary>
            /// Get the list of Csrs from within your Parature license.
            /// </summary>
            public static TicketStatusList TicketStatusGetList(ParaCredentials paraCredentials)
            {
                return TicketStatusFillList(paraCredentials, new EntityQuery.TicketStatusQuery());
            }
            /// <summary>
            /// Get the list of Csrs from within your Parature license.
            /// </summary>
            public static TicketStatusList TicketStatusGetList(ParaCredentials paraCredentials, EntityQuery.TicketStatusQuery query)
            {
                return TicketStatusFillList(paraCredentials, query);
            }

            /// <summary>
            /// Returns an ticketStatus object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ticketStatusXml">
            /// The TicketStatus XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.TicketStatus TicketStatusGetDetails(XmlDocument ticketStatusXml)
            {
                ParaObjects.TicketStatus ticketStatus = new ParaObjects.TicketStatus();
                ticketStatus = XmlToObjectParser.TicketStatusParser.TicketStatusFill(ticketStatusXml);

                ticketStatus.ApiCallResponse.xmlReceived = ticketStatusXml;
                ticketStatus.ApiCallResponse.Objectid = ticketStatus.StatusID;

                return ticketStatus;
            }

            /// <summary>
            /// Returns an ticketStatus list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="ticketStatusListXml">
            /// The TicketStatus List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static TicketStatusList TicketStatusGetList(XmlDocument ticketStatusListXml)
            {
                TicketStatusList ticketStatussList = new TicketStatusList();
                ticketStatussList = XmlToObjectParser.TicketStatusParser.TicketStatusFillList(ticketStatusListXml);

                ticketStatussList.ApiCallResponse.xmlReceived = ticketStatusListXml;

                return ticketStatussList;
            }

            /// <summary>
            /// Fills a Sla list object.
            /// </summary>
            private static TicketStatusList TicketStatusFillList(ParaCredentials paraCredentials, EntityQuery.TicketStatusQuery query)
            {

                TicketStatusList TicketStatusList = new TicketStatusList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Ticket, ParaEnums.ParatureEntity.status, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    TicketStatusList = XmlToObjectParser.TicketStatusParser.TicketStatusFillList(ar.xmlReceived);
                }
                TicketStatusList.ApiCallResponse = ar;

                // Checking if the system needs to recursively call all of the data returned.
                if (query.RetrieveAllRecords)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        TicketStatusList objectlist = new TicketStatusList();

                        if (TicketStatusList.TotalItems > TicketStatusList.TicketStatuses.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.TicketStatus, query.BuildQueryArguments());

                            objectlist = XmlToObjectParser.TicketStatusParser.TicketStatusFillList(ar.xmlReceived);

                            if (objectlist.TicketStatuses.Count == 0)
                            {
                                continueCalling = false;
                            }

                            TicketStatusList.TicketStatuses.AddRange(objectlist.TicketStatuses);
                            TicketStatusList.ResultsReturned = TicketStatusList.TicketStatuses.Count;
                            TicketStatusList.PageNumber = query.PageNumber;
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            TicketStatusList.ApiCallResponse = ar;
                        }
                    }
                }


                return TicketStatusList;
            }

            private static ParaObjects.TicketStatus TicketStatusFillDetails(Int64 ticketStatusId, ParaCredentials paraCredentials)
            {
                ParaObjects.TicketStatus TicketStatus = new ParaObjects.TicketStatus();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.TicketStatus, ticketStatusId);
                if (ar.HasException == false)
                {
                    TicketStatus = XmlToObjectParser.TicketStatusParser.TicketStatusFill(ar.xmlReceived);
                }
                else
                {

                    TicketStatus.StatusID = 0;
                }

                return TicketStatus;
            }
        }

        public class Queue
        {
            ///  <summary>
            ///  Returns a Queue object with all of its properties filled.
            ///  </summary>
            /// <param name="queueId"></param>
            /// <param name="paraCredentials">
            ///  The Parature Credentials class is used to hold the standard login information. It is very useful to have it instantiated only once, with the proper information, and then pass this class to the different methods that need it.
            ///  </param>               
            public static ParaObjects.Queue QueueGetDetails(Int64 queueId, ParaCredentials paraCredentials)
            {
                ParaObjects.Queue queue = new ParaObjects.Queue();
                queue = QueueFillDetails(queueId, paraCredentials);
                return queue;
            }

            /// <summary>
            /// Returns an queue object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="queueXml">
            /// The Queue XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ParaObjects.Queue QueueGetDetails(XmlDocument queueXml)
            {
                ParaObjects.Queue queue = new ParaObjects.Queue();
                queue = XmlToObjectParser.QueueParser.QueueFill(queueXml);

                return queue;
            }

            /// <summary>
            /// Returns an queue list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="queueListXml">
            /// The Queue List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static QueueList QueueGetList(XmlDocument queueListXml)
            {
                QueueList queuesList = new QueueList();
                queuesList = XmlToObjectParser.QueueParser.QueueFillList(queueListXml);

                queuesList.ApiCallResponse.xmlReceived = queueListXml;

                return queuesList;
            }

            /// <summary>
            /// Get the list of Queues from within your Parature license.
            /// </summary>
            public static QueueList QueueGetList(ParaCredentials paraCredentials)
            {
                return QueueFillList(paraCredentials, new EntityQuery.QueueQuery());
            }

            /// <summary>
            /// Get the list of Queues from within your Parature license.
            /// </summary>
            public static QueueList QueueGetList(ParaCredentials paraCredentials, EntityQuery.QueueQuery query)
            {
                return QueueFillList(paraCredentials, query);
            }
            /// <summary>
            /// Fills a Queue List object.
            /// </summary>
            private static QueueList QueueFillList(ParaCredentials paraCredentials, EntityQuery.QueueQuery query)
            {

                QueueList QueueList = new QueueList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Queue, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    QueueList = XmlToObjectParser.QueueParser.QueueFillList(ar.xmlReceived);
                }
                QueueList.ApiCallResponse = ar;

                // Checking if the system needs to recursively call all of the data returned.
                if (query.RetrieveAllRecords)
                {
                    bool continueCalling = true;
                    while (continueCalling)
                    {
                        QueueList objectlist = new QueueList();

                        if (QueueList.TotalItems > QueueList.Queues.Count)
                        {
                            // We still need to pull data

                            // Getting next page's data
                            query.PageNumber = query.PageNumber + 1;

                            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Queue, query.BuildQueryArguments());

                            objectlist = XmlToObjectParser.QueueParser.QueueFillList(ar.xmlReceived);

                            if (objectlist.Queues.Count == 0)
                            {
                                continueCalling = false;
                            }

                            QueueList.Queues.AddRange(objectlist.Queues);
                            QueueList.ResultsReturned = QueueList.Queues.Count;
                            QueueList.PageNumber = query.PageNumber;
                        }
                        else
                        {
                            // That is it, pulled all the items.
                            continueCalling = false;
                            QueueList.ApiCallResponse = ar;
                        }
                    }
                }

                return QueueList;
            }

            private static ParaObjects.Queue QueueFillDetails(Int64 queueId, ParaCredentials paraCredentials)
            {
                ParaObjects.Queue Queue = new ParaObjects.Queue();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectGetDetail(paraCredentials, ParaEnums.ParatureEntity.Queue, queueId);
                if (ar.HasException == false)
                {
                    Queue = XmlToObjectParser.QueueParser.QueueFill(ar.xmlReceived);
                }
                else
                {

                    Queue.QueueID = 0;
                }

                return Queue;
            }
        }

        public class ContactView
        {
            /// <summary>
            /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="viewListXml">
            /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static ContactViewList ViewGetList(XmlDocument viewListXml)
            {
                ContactViewList ViewsList = new ContactViewList();
                ViewsList = XmlToObjectParser.CustomerViewParser.ViewFillList(viewListXml);

                ViewsList.ApiCallResponse.xmlReceived = viewListXml;

                return ViewsList;
            }

            /// <summary>
            /// Get the list of Views from within your Parature license.
            /// </summary>
            public static ContactViewList ViewGetList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
            {
                return ViewFillList(paraCredentials, query);
            }

            /// <summary>
            /// Fills a View List object.
            /// </summary>
            private static ContactViewList ViewFillList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
            {

                ContactViewList ViewList = new ContactViewList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Customer, ParaEnums.ParatureEntity.view, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ViewList = XmlToObjectParser.CustomerViewParser.ViewFillList(ar.xmlReceived);
                }
                ViewList.ApiCallResponse = ar;
                return ViewList;
            }
        }

        public class AccountView
        {
            /// <summary>
            /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="viewListXml">
            /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static AccountViewList ViewGetList(XmlDocument viewListXml)
            {
                AccountViewList ViewsList = new AccountViewList();
                ViewsList = XmlToObjectParser.AccountViewParser.ViewFillList(viewListXml);

                ViewsList.ApiCallResponse.xmlReceived = viewListXml;

                return ViewsList;
            }

            /// <summary>
            /// Get the list of Views from within your Parature license.
            /// </summary>
            public static AccountViewList ViewGetList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
            {
                return ViewFillList(paraCredentials, query);
            }

            /// <summary>
            /// Fills a View List object.
            /// </summary>
            private static AccountViewList ViewFillList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
            {

                AccountViewList ViewList = new AccountViewList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Account, ParaEnums.ParatureEntity.view, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ViewList = XmlToObjectParser.AccountViewParser.ViewFillList(ar.xmlReceived);
                }
                ViewList.ApiCallResponse = ar;
                return ViewList;
            }
        }

        public class TicketView
        {
            /// <summary>
            /// Returns an View list object from a XML Document. No calls to the APIs are made when calling this method.
            /// </summary>
            /// <param name="viewListXml">
            /// The View List XML, is should follow the exact template of the XML returned by the Parature APIs.
            /// </param>
            public static TicketViewList ViewGetList(XmlDocument viewListXml)
            {
                TicketViewList ViewsList = new TicketViewList();
                ViewsList = XmlToObjectParser.TicketViewParser.ViewFillList(viewListXml);

                ViewsList.ApiCallResponse.xmlReceived = viewListXml;

                return ViewsList;
            }

            /// <summary>
            /// Get the list of Views from within your Parature license.
            /// </summary>
            public static TicketViewList ViewGetList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
            {
                return ViewFillList(paraCredentials, query);
            }

            /// <summary>
            /// Fills a View List object.
            /// </summary>
            private static TicketViewList ViewFillList(ParaCredentials paraCredentials, EntityQuery.ViewQuery query)
            {

                TicketViewList ViewList = new TicketViewList();
                ApiCallResponse ar = new ApiCallResponse();
                ar = ApiCallFactory.ObjectSecondLevelGetList(paraCredentials, ParaEnums.ParatureModule.Ticket, ParaEnums.ParatureEntity.view, query.BuildQueryArguments());
                if (ar.HasException == false)
                {
                    ViewList = XmlToObjectParser.TicketViewParser.ViewFillList(ar.xmlReceived);
                }
                ViewList.ApiCallResponse = ar;
                return ViewList;
            }
        }

    }
}