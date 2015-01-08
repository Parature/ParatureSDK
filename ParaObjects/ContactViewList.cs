using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class ContactViewList : PagedData.PagedData
    {
        public List<ContactView> views = new List<ContactView>();

    }
}