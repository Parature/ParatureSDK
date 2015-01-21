using System;
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
        public object Value;

        /// <summary>
        /// Retrieve the value of this field, typecast to the provided type.
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <returns>Value of the field, or the default of the Type if the Value is null</returns>
        /// <exception cref="InvalidCastException">Thrown if the type provided is incorrect</exception>
        public T GetFieldValue<T>()
        {
            if (Value == null)
            {
                return default(T);
            }

            return (T)Value;
        }

        /// <summary>
        /// this indicates whether the field is editable or read only. 
        /// If it is a read only, inluding it in an update will not result in that field value being updated.
        /// </summary>
        public bool Editable;

        public ParaEnums.FieldDataType DataType;
    }
}
