using System;
using System.Collections.Generic;
using System.Text;

namespace Cardyan.Accounting.Models.ApiModels
{
    public class AccountTree
    {
        public Account Root { get; set; }
        public List<AccountTree> Children { get; set; }

        public AccountTree()
        {
            Children = new List<AccountTree>();
        }
    }
}
