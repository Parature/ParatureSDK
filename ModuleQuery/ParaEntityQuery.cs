using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ParatureSDK.Fields;

namespace ParatureSDK.ModuleQuery
{
    public abstract class ParaEntityQuery : ParaQuery
    {
        /// <summary>
        /// If you set this property to "True", ParaConnect will perform a schema call, determine the custom fields, then will make a call including all the of the objects custom fields.
        /// Caution: do not use the "IncludeCustomField" methods if you are setting this one to true, as the "IncludeCustomField" methods will be ignored.
        /// </summary>        
        public bool IncludeAllCustomFields { get; set; }

        public ParaEntityQuery()
        {
            IncludeAllCustomFields = false;
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are date based. 
        /// </summary>
        /// <param name="customFieldId">
        /// The id of the custom field you would like to filter your query on.
        /// </param>
        /// <param name="criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="value">
        /// The Date you would like to base your filter off.
        /// </param>        
        public void AddCustomFieldFilter(Int64 customFieldId, ParaEnums.QueryCriteria criteria, DateTime value)
        {
            QueryFilterAdd("FID" + customFieldId.ToString(), criteria, value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are NOT multi values. 
        /// </summary>
        /// <param name="customFieldId">
        /// The id of the custom field you would like to filter your query on.
        /// </param>
        /// <param name="criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="value">
        /// The value you would like the custom field to have, for this filter.
        /// </param>
        public void AddCustomFieldFilter(Int64 customFieldId, ParaEnums.QueryCriteria criteria, string value)
        {        
            QueryFilterAdd("FID" + customFieldId, criteria, ProcessEncoding(value));
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are NOT multi values. 
        /// </summary>
        /// <param name="customFieldId">
        /// The id of the custom field you would like to filter your query on.
        /// </param>
        /// <param name="criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="value">
        /// The value you would like the custom field to have, for this filter.
        /// </param>
        public void AddCustomFieldFilter(Int64 customFieldId, ParaEnums.QueryCriteria criteria, bool value)
        {
            var filter = value 
                ? "1" 
                : "0";
            QueryFilterAdd("FID" + customFieldId, criteria, filter);
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are multi values (dropdown, radio buttons, etc). 
        /// </summary>
        /// <param name="customFieldId">
        /// The id of the multi value custom field you would like to filter your query on.
        /// </param>
        /// <param name="criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="customFieldOptionId">
        /// The list of all custom field options (for the customFieldID you specified) that need to be selected for an item to qualify to be returned when you run your query.
        /// </param>
        public void AddCustomFieldFilter(Int64 customFieldId, ParaEnums.QueryCriteria criteria, Int64[] customFieldOptionId)
        {
            if (customFieldOptionId.Length <= 0) return;

            var filtering = "";

            for (var i = 0; i < customFieldOptionId.Length; i++)
            {
                var separator = ",";
                if (i == 0)
                {
                    if (customFieldOptionId.Length > 1)
                    {
                        separator = "";
                    }
                }
                filtering = filtering + separator + customFieldOptionId[i].ToString();
            }

            QueryFilterAdd("FID" + customFieldId, criteria, filtering);
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are multi values (dropdown, radio buttons, etc).
        /// </summary>
        /// <param name="customFieldId">
        /// The id of the multi value custom field you would like to filter your query on.
        /// </param>
        /// <param name="criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="customFieldOptionId">
        /// The custom field option (for the customFieldID you specified) that need to be selected for an item to qualify to be returned when you run your query.
        /// </param>
        public void AddCustomFieldFilter(Int64 customFieldId, ParaEnums.QueryCriteria criteria, Int64 customFieldOptionId)
        {
            QueryFilterAdd("FID" + customFieldId, criteria, customFieldOptionId.ToString());
        }

        /// <summary>
        /// Add a custom field to the query returned returned 
        /// </summary>
        public void IncludeCustomField(Int64 customFieldid)
        {
            IncludedFieldsCheckAndDeleteRecord(customFieldid.ToString());
            _IncludedFields.Add(customFieldid.ToString());
        }

        /// <summary>
        /// Include all the custom fields, included in the collection passed to this method, 
        /// to the api call. These custom fields will be returned with the objects receiveds from the APIs.
        /// This is very useful if you have a schema objects and would like to query with all custom fields returned.
        /// </summary>
        public void IncludeCustomField(IEnumerable<CustomField> customFields)
        {
            foreach (var cf in customFields)
            {
                IncludeCustomField(cf.Id);
            }

        }

        /// <summary>
        /// Checking if a record exists in the _includedFields array, and delete it if it did.
        /// </summary>
        protected void IncludedFieldsCheckAndDeleteRecord(string nameValue)
        {
            ArrayCheckAndDeleteRecord(_IncludedFields, nameValue);
        }

        /// <summary>
        /// Provides the string array of all dynamic filtering and fields to include that will be further processed
        /// by the module specific object passed to the APIs, to include statis filtering.
        /// </summary>
        new internal ArrayList BuildQueryArguments()
        {
            // The following method is called here, instead of having all the logic in "BuildQueryArguments",
            // it has been externalized so that it can be separately called from inherited classes. Certain 
            // inherited classes override buildQuaryArguments    

            _QueryFilters = new ArrayList();
            BuildCustomFieldQueryArguments();
            BuildParaQueryArguments();

            return _QueryFilters;
        }

        /// <summary>
        /// Build all related query arguments related to custom fields.
        /// </summary>
        private void BuildCustomFieldQueryArguments()
        {
            var dontAdd = false;
            IncludeAllCustomFields = false;

            if (TotalOnly == true
                || _IncludedFields == null
                || _IncludedFields.Count <= 0)
            {
                return;
            }

            var fieldsList = "_fields_=";
            for (var j = 0; j < _IncludedFields.Count; j++)
            {
                if (j > 0)
                {
                    fieldsList = fieldsList + ",";
                }
                fieldsList = fieldsList + _IncludedFields[j].ToString();
            }


            if (_QueryFilters.Cast<object>().Any(t => t.ToString() == fieldsList))
            {
                dontAdd = true;
            }

            if (dontAdd == false)
            {
                _QueryFilters.Add(fieldsList);
            }
        }


        /// <summary>
        /// Add a sort order to the Query, based on a custom field.
        /// </summary>
        /// <param name="customFieldId">The id of the custom field you would like to filter upon.</param>
        /// <param name="sortDirection"></param>       
        public bool AddSortOrder(Int64 customFieldId, ParaEnums.QuerySortBy sortDirection)
        {
            if (_SortByFields.Count < 5)
            {
                _SortByFields.Add("FID" + customFieldId.ToString() + "_" + sortDirection.ToString().ToLower() + "_");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}