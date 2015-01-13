using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class DepartmentsList : PagedData.PagedData
    {
        public List<Department> Departments = new List<Department>();

        public DepartmentsList()
        {
        }

        public DepartmentsList(DepartmentsList departmentsList)
            : base(departmentsList)
        {
            Departments = new List<Department>(departmentsList.Departments);
        }
    }
}