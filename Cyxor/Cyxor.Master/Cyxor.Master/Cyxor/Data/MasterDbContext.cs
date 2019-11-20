/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Microsoft.EntityFrameworkCore;

namespace Cyxor.Data
{
    using Models;

    public class MasterDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        public MasterDbContext() { }

        protected MasterDbContext(DbContextOptions options) : base(options) { }

        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured)
        //        return;

        //    optionsBuilder.UseInMemoryDatabase(nameof(Cyxor));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<AccountRole>().HasKey(p => new { p.AccountId, p.RoleId });
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
