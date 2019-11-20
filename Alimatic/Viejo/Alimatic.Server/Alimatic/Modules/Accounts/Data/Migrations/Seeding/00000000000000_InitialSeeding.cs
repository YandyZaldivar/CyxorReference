/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Text;
using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Alimatic.Accounts.Data.Migrations
{
    using Cyxor.Models;
    using Cyxor.Networking.Config.Server;

    [DbContext(typeof(AccountsDbContext))]
    [Migration("00000000000000_InitialSeeding")]
    //public class InitialSeeding : Migration
    public class InitialSeeding : Initial
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var scope = Network.Instance.ConnectionScope;

            try
            {
                scope = scope ?? Network.Instance.CreateScope();

                var dbContext = scope.GetService<AccountsDbContext>();

                var account = new Account { Id = 1, Name = DatabaseConfig.DefaultRootAccountName.ToLowerInvariant(), SecurityLevel = -1 };

                var bytes = Encoding.UTF8.GetBytes($"{account.Name}:{DatabaseConfig.DefaultRootAccountPassword}");
                account.Password = Convert.ToBase64String(SHA256.Create().ComputeHash(bytes));

                dbContext.Accounts.Add(account);

                dbContext.SaveChanges();
            }
            catch
            {
                System.Threading.Thread.Sleep(1000);

                //using (var scope = Network.Instance.CreateScope())
                //{
                //    var dbContext = scope.GetService<AccountsDbContext>();

                //    var account = new Account { Id = 1, Name = DatabaseConfig.DefaultRootAccountName, Security = -1 };

                //    var bytes = Encoding.UTF8.GetBytes($"{account.Name}:{DatabaseConfig.DefaultRootAccountPassword.ToLowerInvariant()}");
                //    account.PasswordHash = Convert.ToBase64String(SHA256.Create().ComputeHash(bytes));

                //    dbContext.Accounts.Add(account);

                //    dbContext.SaveChanges();
                //}
            }
            finally
            {
                if (scope != Network.Instance.ConnectionScope)
                    scope.Dispose();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var scope = Network.Instance.CreateScope())
            {
                var dbContext = scope.GetService<AccountsDbContext>();

                var account = dbContext.Accounts.Find(1);
                dbContext.Accounts.Remove(account);

                dbContext.SaveChanges();
            }
        }
    }
}
/* { Alimatic.Server } */
