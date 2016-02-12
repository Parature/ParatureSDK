using System;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.EntityQuery;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    /// <summary>
    /// Methods for retrieving the department entities
    /// </summary>
    public class Department : FirstLevelApiGetMethods<ParaObjects.Department, DepartmentQuery>
    {}
}