using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class DepartmentsList : PagedData.PagedData
    {
        public List<Department> Departments = new List<Department>();
    }
}