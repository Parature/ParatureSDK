using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class RolesList : PagedData.PagedData
    {
        public List<Role> Roles = new List<Role>();

        public RolesList()
        {
        }
        public RolesList(RolesList rolesList)
            : base(rolesList)
        {
            this.Roles = new List<Role>(rolesList.Roles);
        }
    }
}