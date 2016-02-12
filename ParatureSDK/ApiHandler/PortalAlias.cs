using System;
using System.Xml;
using ParatureSDK.ApiHandler.ApiMethods;
using ParatureSDK.ParaObjects;
using ParatureSDK.Query.EntityQuery;
using ParatureSDK.XmlToObjectParser;

namespace ParatureSDK.ApiHandler
{
    public class PortalAlias : FirstLevelApiGetMethods<ParaObjects.PortalAlias, PortalAliasQuery>
    { }
}