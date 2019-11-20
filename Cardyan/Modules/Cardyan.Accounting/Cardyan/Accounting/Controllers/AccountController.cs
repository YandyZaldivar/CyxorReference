using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileObjects.AgileMapper.Extensions;
using Cardyan.Accounting.Models.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Cardyan.Accounting.Controllers
{
    using Models;

    public class AccountController : CardyanDbContextController<Account>
    {
        public async Task<List<AccountTree>> Tree()
        {
            var accounts = await DbContext.Accounts.GroupBy(a => a.ParentId).ToListAsync();
            return BuildTree(accounts, null);
        }

        private List<AccountTree> BuildTree(List<IGrouping<int?, Account>> accounts, int? parent)
        {
            var selectedAccounts = accounts.Find(g => g.Key == parent);
            List<AccountTree> accountsTree = new List<AccountTree>();
            if (null != selectedAccounts)
            {
                foreach (var selectedAccount in selectedAccounts)
                {
                    selectedAccount.Parent = null;
                    var accountTree = new AccountTree
                    {
                        Root = selectedAccount,
                        Children = BuildTree(accounts, selectedAccount.Id)
                    };
                    accountsTree.Add(accountTree);
                }

                return accountsTree;
            }

            return null;
        }
    }
}
