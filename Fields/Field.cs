﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParatureAPI.Fields
{
    public class Field
    {
        public string Name = "";
        public bool Required;
        public bool Dependent = false;
        public Int32 MaxLength = 0;

        /// <summary>
        /// This Value will be populated with the field's value. 
        /// For example, if this is a textbox field, this will hold the textbox's default field.
        /// </summary>
        public string Value = "";

        /// <summary>
        /// this indicates whether the field is editable or read only. 
        /// If it is a read only, inluding it in an update will not result in that field value being updated.
        /// </summary>
        public bool Editable;

        public ParaEnums.FieldDataType DataType;
    }
}
