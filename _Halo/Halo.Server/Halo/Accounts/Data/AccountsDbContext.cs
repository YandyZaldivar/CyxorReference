/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using Microsoft.EntityFrameworkCore;

//#if !POMELO
//using MySQL.Data.EntityFrameworkCore.Extensions;
//#endif

namespace Halo.Accounts.Data
{
    public class AccountsDbContext : Cyxor.Data.MasterDbContext
    {
        public AccountsDbContext() { }

        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options) { }
    }
}
/* { Alimatic.Server } */
