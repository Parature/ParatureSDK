using System;

namespace ParatureAPI.ParaObjects
{
    public partial class Role
    {
        public Int64 RoleID;
        public string Name;
        public string Description;
        public Role()
        {
            RoleID = 0;
            Name = "";
            Description = "";
        }
        public Role(Role role)
        {
            this.RoleID = role.RoleID;
            this.Name = role.Name;
            this.Description = role.Description;
        }
        public Role(Int64 RoleID, string Name, string Description)
        {
            this.RoleID = RoleID;
            this.Name = Name;
            this.Description = Description;
        }
    }
}