using System;

namespace ParatureAPI.ParaObjects
{
    /// <summary>
    /// A department's property.
    /// </summary>
    public partial class Department : ObjectBaseProperties
    {
        private Int64 _DepartmentID = 0;

        public Int64 DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }

        private string _name = "";

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description = "";

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        public Department()
        {
        }

        public Department(Department department)
        {
            this.DepartmentID = department.DepartmentID;
            this.Name = department.Name;
            this.Description = department.Description;
        }
    }
}