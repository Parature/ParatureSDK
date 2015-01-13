using System;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// A department's property.
    /// </summary>
    public class Department : ParaEntityBaseProperties
    {
        public string Name { get; set; }

        public string Description { get; set; }


        public Department()
        {
            Description = "";
            Name = "";
        }

        public Department(Department department)
        {
            Id = department.Id;
            Name = department.Name;
            Description = department.Description;
        }
    }
}