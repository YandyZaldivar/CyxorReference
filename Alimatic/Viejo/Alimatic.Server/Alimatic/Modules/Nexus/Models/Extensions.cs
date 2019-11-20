using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Models
{
    using Data;

    using Cyxor.Networking;

    public static class Extensions
    {
        const string NexusUser = "NexusUser";

        //public static async void SetNexusUser(this MasterConnection connection)
        //{
        //    using (var scope = connection.Node.CreateScope())
        //        connection.Tags[nameof(NexusUser)] = await scope.GetService<NexusDbContext>().Users.AsNoTracking().SingleOrDefaultAsync(p => p.AccountId == connection.Account.Id);
        //}

        public static async ValueTask<User> GetNexusUserAsync(this MasterConnection connection)
        {
            if (!connection.Tags.TryGetValue(nameof(NexusUser), out var value))
                using (var scope = connection.Node.CreateScope())
                    connection.Tags[nameof(NexusUser)] = value = await scope.GetService<NexusDbContext>().
                        Users.AsNoTracking().SingleOrDefaultAsync(p => p.AccountId == connection.Account.Id);

            return value as User;
        }
    }
}
