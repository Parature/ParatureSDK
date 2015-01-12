using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.ApiHandler.Entities
{
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
}