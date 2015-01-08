using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public partial class CsrsList : PagedData.PagedData
    {
        public List<Csr> Csrs = new List<Csr>();

        public CsrsList()
        {
        }

        public CsrsList(CsrsList csrsList)
            : base(csrsList)
        {
            this.Csrs = new List<Csr>(csrsList.Csrs);
        }
    }
}