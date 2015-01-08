using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class SlasList : PagedData.PagedData
    {
        public List<Sla> Slas = new List<Sla>();

        public SlasList()
        {
        }

        public SlasList(SlasList slasList)
            : base(slasList)
        {
            this.Slas = new List<Sla>(slasList.Slas);
        }
    }
}