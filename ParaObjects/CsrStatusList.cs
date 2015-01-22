using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class CsrStatusList : PagedData.PagedData
    {
        public List<CsrStatus> CsrStatuses = new List<CsrStatus>();
    }
}