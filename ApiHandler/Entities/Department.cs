using System;
using System.Xml;
using ParatureAPI.ParaObjects;

namespace ParatureAPI.ApiHandler.Entities
{
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
                department.Id = 0;
            }
            department.ApiCallResponse = ar;

            return department;
        }
    }
}