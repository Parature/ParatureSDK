using System;
using System.Collections.Generic;
using System.Linq;
using ParatureSDK.Fields;

namespace ParatureSDK.ParaHelper
{
    internal class HelperMethods
    {
        internal static bool CustomFieldReset(Int64 customFieldid, IEnumerable<CustomField> fields)
        {
            if (customFieldid <= 0 || fields == null) return false;
            
            var modified = false;
            var matchingFields = fields.Where(cf => cf.Id == customFieldid);
            
            foreach (var cf in matchingFields)
            {
                if (cf.Options.Count > 0)
                {
                    var selectedFieldsTrue = cf.Options.Where(cfo => cfo.Selected);
                    foreach (var cfo in selectedFieldsTrue)
                    {
                        cfo.Selected = false;
                        modified = true;
                    }
                }

                break;
            }

            return modified;
        }
    }
}