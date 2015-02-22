using System;
using System.Xml;
using ParatureSDK.EntityQuery;
using ParatureSDK.ParaObjects;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler.Entities
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
            department = ParaEntityParser.EntityFill<ParaObjects.Department>(departmentXml);

            return department;
        }

        /// <summary>
        /// Returns an Department list object from a XML Document. No calls to the APIs are made when calling this method.
        /// </summary>
        /// <param name="departmentListXml">
        /// The Departments List XML, is should follow the exact template of the XML returned by the Parature APIs.
        /// </param>
        public static ParaEntityList<ParaObjects.Department> DepartmentsGetList(XmlDocument departmentListXml)
        {
            var departmentslist = new ParaEntityList<ParaObjects.Department>();
            departmentslist = ParaEntityParser.FillList<ParaObjects.Department>(departmentListXml);

            departmentslist.ApiCallResponse.XmlReceived = departmentListXml;

            return departmentslist;
        }

        /// <summary>
        /// Get the list of Departments from within your Parature license.
        /// </summary>
        public static ParaEntityList<ParaObjects.Department> DepartmentsGetList(ParaCredentials paraCredentials, DepartmentQuery query)
        {
            return DepartmentFillList(paraCredentials, query);
        }
        /// <summary>
        /// Fills a Departmentslist object.
        /// </summary>
        private static ParaEntityList<ParaObjects.Department> DepartmentFillList(ParaCredentials paraCredentials, DepartmentQuery query)
        {

            var departmentsList = new ParaEntityList<ParaObjects.Department>();
            ApiCallResponse ar = new ApiCallResponse();
            ar = ApiCallFactory.ObjectGetList(paraCredentials, ParaEnums.ParatureEntity.Department, query.BuildQueryArguments());
            if (ar.HasException == false)
            {
                departmentsList = ParaEntityParser.FillList<ParaObjects.Department>(ar.XmlReceived);
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
                department = ParaEntityParser.EntityFill<ParaObjects.Department>(ar.XmlReceived);
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