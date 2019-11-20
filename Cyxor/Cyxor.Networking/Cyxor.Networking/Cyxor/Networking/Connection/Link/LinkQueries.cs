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

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Cyxor.Networking
{
    using static Utilities.Threading;

    sealed partial class Link
    {
        internal sealed class LinkQueries : LinkProperty
        {
            InterlockedInt InternalsTopId;
            InterlockedInt StandardsTopId;

            InterlockedInt InternalsCount;
            InterlockedInt StandardsCount;

            readonly ConcurrentQueue<int> InternalsQueue;
            readonly ConcurrentQueue<int> StandardsQueue;

            readonly ConcurrentDictionary<int, object> InternalsAnnul;
            readonly ConcurrentDictionary<int, object> StandardsAnnul;

            readonly ConcurrentDictionary<int, Delivery> InternalsQuery;
            readonly ConcurrentDictionary<int, Delivery> StandardsQuery;

            internal LinkQueries(Link link) : base(link)
            {
                InternalsTopId = new InterlockedInt();
                StandardsTopId = new InterlockedInt();

                InternalsCount = new InterlockedInt();
                StandardsCount = new InterlockedInt();

                InternalsQueue = new ConcurrentQueue<int>();
                StandardsQueue = new ConcurrentQueue<int>();

                InternalsAnnul = new ConcurrentDictionary<int, object>();
                StandardsAnnul = new ConcurrentDictionary<int, object>();

                InternalsQuery = new ConcurrentDictionary<int, Delivery>();
                StandardsQuery = new ConcurrentDictionary<int, Delivery>();
            }

            internal int Add(Delivery delivery, QueryType queryType)
            {
                if (!(queryType == QueryType.Internal ? InternalsQueue : StandardsQueue).TryDequeue(out var nextId))
                    nextId = (queryType == QueryType.Internal ? InternalsTopId : StandardsTopId).Increment();

                if (!(queryType == QueryType.Internal ? InternalsQuery : StandardsQuery).TryAdd(nextId, delivery))
                    Node.Log(LogCategory.Fatal, $"Can't add a new query id ({nextId}), key already exists.");

                (queryType == QueryType.Internal ? InternalsCount : StandardsCount).Increment();

                return nextId;
            }

            internal bool TryRemove(int queryId, QueryType queryType, out Delivery delivery, bool cancelled)
            {
                if ((queryType == QueryType.Internal ? InternalsQuery : StandardsQuery).TryRemove(queryId, out delivery))
                {
                    (queryType == QueryType.Internal ? InternalsCount : StandardsCount).Decrement();

                    if (!cancelled)
                        (queryType == QueryType.Internal ? InternalsQueue : StandardsQueue).Enqueue(queryId);
                    else
                        (queryType == QueryType.Internal ? InternalsAnnul : StandardsAnnul).TryAdd(queryId, null);

                    return true;
                }

                return false;
            }

            internal bool TryRemoveCancelled(int queryId, QueryType queryType)
            {
                if ((queryType == QueryType.Internal ? InternalsAnnul : StandardsAnnul).TryRemove(queryId, out var obj))
                {
                    (queryType == QueryType.Internal ? InternalsQueue : StandardsQueue).Enqueue(queryId);
                    return true;
                }

                return false;
            }

            internal void Reset()
            {
                foreach (var delivery in StandardsQuery.Select(p => p.Value))
                    Delivery.TryProcessReply(Connection, new Packet(delivery.Packet, Connection.Result));
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
