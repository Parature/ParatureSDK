using System.Collections.Generic;

namespace ParatureAPI.PagedData
{
    /// <summary>
    /// Instantiate this class to hold the result set of a list call to APIs. Whenever you need to get a list of 
    /// Accounts
    /// </summary>
    public class AccountsList : PagedData
    {
        public List<ParaObjects.Account> Accounts = new List<ParaObjects.Account>();

        public AccountsList()
        {
        }

        public AccountsList(AccountsList accountsList)
            : base(accountsList)
        {
            Accounts = new List<ParaObjects.Account>(accountsList.Accounts);
        }
    }
}