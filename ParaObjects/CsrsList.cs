using System.Collections.Generic;

namespace ParatureAPI.ParaObjects
{
    public class CsrsList : PagedData.PagedData
    {
        public List<Csr> Csrs = new List<Csr>();

        public CsrsList()
        {
        }

        public CsrsList(CsrsList csrsList)
            : base(csrsList)
        {
            Csrs = new List<Csr>(csrsList.Csrs);
        }
    }
}