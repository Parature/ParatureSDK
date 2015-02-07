using System;
using System.Xml.Serialization;

namespace ParatureSDK.Fields
{
    public class Field
    {
        [XmlAttribute(AttributeName = "display-name")]
        public string Name = "";
        [XmlAttribute(AttributeName = "required")]
        public bool Required;
        [XmlAttribute(AttributeName = "dependent")]
        public bool Dependent = false;
        public Int32 MaxLength = 0;
        /// <summary>
        /// this indicates whether the field is editable or read only. 
        /// If it is a read only, inluding it in an update will not result in that field value being updated.
        /// </summary>
        [XmlAttribute(AttributeName = "editable")]
        public bool Editable;

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


        [XmlAttribute("data-type")]
        public ParaEnums.FieldDataType DataType;
    }
}
