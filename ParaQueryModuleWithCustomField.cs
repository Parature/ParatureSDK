using System;
using System.Collections;
using System.Collections.Generic;
using ParatureAPI.Fields;

namespace ParatureAPI
{
    public abstract class ParaQueryModuleWithCustomField : ParaQuery
    {


        /// <summary>
        /// If you set this property to "True", ParaConnect will perform a schema call, determine the custom fields, then will make a call including all the of the objects custom fields.
        /// Caution: do not use the "IncludeCustomField" methods if you are setting this one to true, as the "IncludeCustomField" methods will be ignored.
        /// </summary>        
        public bool IncludeAllCustomFields
        {
            get { return _includeAllCustomFields; }
            set
            {
                _includeAllCustomFields = value;
            }
        }
        private bool _includeAllCustomFields = false;

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are date based. 
        /// </summary>
        /// <param name="CustomFieldID">
        /// The id of the custom field you would like to filter your query on.
        /// </param>
        /// <param name="Criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="value">
        /// The Date you would like to base your filter off.
        /// </param>        
        public void AddCustomFieldFilter(Int64 CustomFieldID, ParaEnums.QueryCriteria Criteria, DateTime value)
        {
            QueryFilterAdd("FID" + CustomFieldID.ToString(), Criteria, value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are NOT multi values. 
        /// </summary>
        /// <param name="CustomFieldID">
        /// The id of the custom field you would like to filter your query on.
        /// </param>
        /// <param name="Criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="value">
        /// The value you would like the custom field to have, for this filter.
        /// </param>
        public void AddCustomFieldFilter(Int64 CustomFieldID, ParaEnums.QueryCriteria Criteria, string value)
        {
            //QueryFilterAdd("FID" + CustomFieldID, Criteria, HttpUtility.UrlEncode(value));          
            QueryFilterAdd("FID" + CustomFieldID, Criteria, ProcessEncoding(value));
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are NOT multi values. 
        /// </summary>
        /// <param name="CustomFieldID">
        /// The id of the custom field you would like to filter your query on.
        /// </param>
        /// <param name="Criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="value">
        /// The value you would like the custom field to have, for this filter.
        /// </param>
        public void AddCustomFieldFilter(Int64 CustomFieldID, ParaEnums.QueryCriteria Criteria, bool value)
        {
            string filter = "0";
            if (value)
            {
                filter = "1";
            }
            else
            {
                filter = "0";
            }
            QueryFilterAdd("FID" + CustomFieldID, Criteria, filter);
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are multi values (dropdown, radio buttons, etc). 
        /// </summary>
        /// <param name="CustomFieldID">
        /// The id of the multi value custom field you would like to filter your query on.
        /// </param>
        /// <param name="Criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="CustomFieldOptionID">
        /// The list of all custom field options (for the customFieldID you specified) that need to be selected for an item to qualify to be returned when you run your query.
        /// </param>
        public void AddCustomFieldFilter(Int64 CustomFieldID, ParaEnums.QueryCriteria Criteria, Int64[] CustomFieldOptionID)
        {
            if (CustomFieldOptionID.Length > 0)
            {
                int i = 0;
                string filtering = "";
                string separator = "";

                for (i = 0; i < CustomFieldOptionID.Length; i++)
                {
                    separator = ",";
                    if (i == 0)
                    {
                        if (CustomFieldOptionID.Length > 1)
                        {
                            separator = "";
                        }
                    }
                    filtering = filtering + separator + CustomFieldOptionID[i].ToString();
                }

                QueryFilterAdd("FID" + CustomFieldID, Criteria, filtering);
            }
        }

        /// <summary>
        /// Adds a custom field based filter to the query. Use this method for Custom Fields that are multi values (dropdown, radio buttons, etc).
        /// </summary>
        /// <param name="CustomFieldID">
        /// The id of the multi value custom field you would like to filter your query on.
        /// </param>
        /// <param name="Criteria">
        /// The criteria you would like to apply to this custom field
        /// </param>
        /// <param name="CustomFieldOptionID">
        /// The custom field option (for the customFieldID you specified) that need to be selected for an item to qualify to be returned when you run your query.
        /// </param>
        public void AddCustomFieldFilter(Int64 CustomFieldID, ParaEnums.QueryCriteria Criteria, Int64 CustomFieldOptionID)
        {
            QueryFilterAdd("FID" + CustomFieldID, Criteria, CustomFieldOptionID.ToString());
        }

        /// <summary>
        /// Add a custom field to the query returned returned 
        /// </summary>
        public void IncludeCustomField(Int64 CustomFieldid)
        {
            IncludedFieldsCheckAndDeleteRecord(CustomFieldid.ToString());
            _IncludedFields.Add(CustomFieldid.ToString());
        }

        /// <summary>
        /// Include all the custom fields, included in the collection passed to this method, 
        /// to the api call. These custom fields will be returned with the objects receiveds from the APIs.
        /// This is very useful if you have a schema objects and would like to query with all custom fields returned.
        /// </summary>
        public void IncludeCustomField(IEnumerable<CustomField> CustomFields)
        {
            foreach (var cf in CustomFields)
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
            bool dontAdd = false;
            if (TotalOnly == false)
            {

                //Reset the query filters.

                if (_IncludedFields != null)
                {
                    if (_IncludedFields.Count > 0)
                    {
                        string fieldsList = "_fields_=";
                        for (int j = 0; j < _IncludedFields.Count; j++)
                        {
                            if (j > 0)
                            {
                                fieldsList = fieldsList + ",";
                            }
                            fieldsList = fieldsList + _IncludedFields[j].ToString();

                        }


                        for (int i = 0; i < _QueryFilters.Count; i++)
                        {
                            if (_QueryFilters[i].ToString() == fieldsList)
                            {
                                dontAdd = true;
                                break;
                            }
                        }

                        if (!dontAdd)
                        {
                            _QueryFilters.Add(fieldsList);
                        }
                    }
                }


            }
            else
            {
                IncludeAllCustomFields = false;
            }
        }



        /// <summary>
        /// Add a sort order to the Query, based on a custom field.
        /// </summary>
        /// <param name="CustomfieldId">The id of the custom field you would like to filter upon.</param>       
        public bool AddSortOrder(Int64 CustomfieldId, ParaEnums.QuerySortBy SortDirection)
        {
            if (_SortByFields.Count < 5)
            {
                _SortByFields.Add("FID" + CustomfieldId.ToString() + "_" + SortDirection.ToString().ToLower() + "_");
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}