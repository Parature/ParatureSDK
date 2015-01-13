using System;
using System.Collections.Generic;
using ParatureAPI.Fields;

namespace ParatureAPI.ParaHelper
{
    internal class HelperMethods
    {
        internal static bool CustomFieldReset(Int64 customFieldid, IEnumerable<CustomField> fields)
        {
            bool modified = false;
            if (customFieldid > 0 && fields != null)
            {
                foreach (var cf in fields)
                {
                    if (cf.CustomFieldID == customFieldid)
                    {
                        if (cf.CustomFieldOptionsCollection.Count > 0)
                        {
                            foreach (CustomFieldOptions cfo in cf.CustomFieldOptionsCollection)
                            {
                                if (cfo.IsSelected == true)
                                {
                                    cfo.IsSelected = false;
                                    modified = true;
                                }
                            }
                        }
                        if (cf.CustomFieldValue != "")
                        {
                            cf.CustomFieldValue = "";
                            modified = true;
                        }

                        break;
                    }
                }
            }

            return modified;
        }


        internal static string SafeHtmlDecode(string input)
        {
            return input.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
        }
    }
}