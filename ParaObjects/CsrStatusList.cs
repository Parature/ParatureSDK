using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class CsrStatusList : PagedData.PagedData
    {
        public List<CsrStatus> CsrStatuses = new List<CsrStatus>();
        public CsrStatusList()
        {
        }
        public CsrStatusList(CsrStatusList csrstatuslist)
            : base(csrstatuslist)
        {
            this.CsrStatuses = new List<CsrStatus>(csrstatuslist.CsrStatuses);
        }

    }
}