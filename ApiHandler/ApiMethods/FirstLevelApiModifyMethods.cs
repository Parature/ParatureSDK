using System;
using ParatureSDK.ModuleQuery;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.ApiHandler.ApiMethods
{
    public abstract class FirstLevelApiModifyMethods<TEntity, TQuery>
        where TEntity : ParaEntity, new()
        where TQuery : ParaEntityQuery
    {}
}
