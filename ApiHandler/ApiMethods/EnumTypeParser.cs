using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParatureSDK.ParaObjects;

namespace ParatureSDK.ApiHandler.ApiMethods
{
    internal class EnumTypeParser
    {
        internal static ParaEnums.ParatureModule _module<TEntity>() where TEntity : ParaEntity
        {
            var type = typeof (TEntity);
            var typeName = type.Name;
            ParaEnums.ParatureModule module;
            var success = Enum.TryParse(typeName, true, out module);

            return module;
        }

        internal static ParaEnums.ParatureEntity _entityType<TEntity>() where TEntity : ParaEntityBaseProperties
        {
            var type = typeof (TEntity);
            var typeName = type.Name;
            switch (typeName)
            {
                case "CustomerRole":
                    typeName = "Role";
                    break;
                case "CsrRole":
                    typeName = "Role";
                    break;
                default:
                    break;
            }
            ParaEnums.ParatureEntity entity;
            var success = Enum.TryParse(typeName, true, out entity);

            return entity;
        }
    }
}