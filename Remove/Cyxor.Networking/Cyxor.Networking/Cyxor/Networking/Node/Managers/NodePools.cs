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

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cyxor.Networking
{
    using Serialization;

    public abstract partial class Node
    {
        internal sealed class NodePools : NodeProperty
        {
            volatile int State = 0;
            const int Delay = 10000;
            CancellationTokenSource Cts;

            ConcurrentPool<Box> Boxes { get; set; }
            ConcurrentPool<Link> Links { get; set; }
            ConcurrentPool<Serializer> Buffers { get; set; }
            ConcurrentPool<Delivery> Deliveries { get; set; }

            internal NodePools(Node node) : base(node)
            {
                Boxes = new ConcurrentPool<Box>(BoxFactory);
                Links = new ConcurrentPool<Link>(LinkFactory);
                Buffers = new ConcurrentPool<Serializer>(BufferFactory);
                Deliveries = new ConcurrentPool<Delivery>(DeliveryFactory);
            }

            internal async void Initialize()
            {
                if (State != 0)
                    Node.Log(LogCategory.Fatal, "Wait sequence invalid state.");

                State++;

                Cts = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        await Utilities.Task.Delay(Delay, Cts.Token).ConfigureAwait(false);

                        if (Cts.IsCancellationRequested)
                            return;

                        ProcessPool(Buffers);
                        ProcessPool(Links);
                        ProcessPool(Boxes);
                        ProcessPool(Deliveries);
                    }
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (Exception exc)
                {
                    Node.Log(LogCategory.Fatal, exception: exc);
                }
                finally
                {
                    Cts.Dispose();
                    Cts = null;
                    State++;
                }
            }

            internal void Reset()
            {
                if (State != 1)
                    return;

                State++;
                Cts.Cancel();
                SpinWait.SpinUntil(() => State == 3);

                Links.Clear();
                Boxes.Clear();
                Buffers.Clear();
                Deliveries.Clear();

                State = 0;
            }

            void ProcessPool<T>(ConcurrentPool<T> pool)
            {
                

                var popCounter = pool.PopCounter.Exchange(0);

                if (pool.PopHistory.Count == 10)
                    pool.PopHistory.Dequeue();

                pool.PopHistory.Enqueue(popCounter);

                if (pool.PopHistory.Count < 5)
                    return;

                var max = pool.PopHistory.Max();
                var avg = (int)pool.PopHistory.Average();

                var itemsChanged = 0;
                var itemsCount = pool.Count;

                if (itemsCount > max + avg)
                {
                    var items = new T[itemsChanged = itemsCount - max];
                    pool.TryPopRange(items);

                    if (pool.IsTDisposable)
                        foreach (var item in items)
                            (item as IDisposable)?.Dispose();
                }
                else if (itemsCount < avg)
                {
                    var items = new T[itemsChanged = avg - itemsCount];

                    for (int i = 0; i < items.Length; i++)
                        items[i] = pool.CreateNewItem();

                    pool.PushRange(items);
                }


            }

            #region Buffer

            Serializer BufferFactory()
            {
                var serializer = new Serializer();
                serializer.SetCapacity(Node.Config.IOBufferSize);
                //serializer.SetCapacity(128);

                return serializer;
            }

            internal Serializer PopBuffer() => Buffers.Pop();

            internal void PushBuffer(Serializer serializer)
            {
                serializer.Reset(Node.Config.IOBufferSize);
                Buffers.Push(serializer);
            }

            #endregion Buffer

            #region Box

            Box BoxFactory()
            {
                var box = new Box();
                box.Node = Node;

                return box;
            }

            internal Box PopBox()
            {
                return Boxes.Pop();
            }

            internal void PushBox(Box box)
            {
                if (box == null)
                    throw new ArgumentNullException();

                // TODO: Remove, this is no longer necessary
                //if (box.References.Value != 0)
                //    throw new InvalidOperationException();

                box.Reset(fullReset: true);

                Boxes.Push(box);
            }

            #endregion Box

            #region Delivery

            Delivery DeliveryFactory() => new Delivery(Node);
            internal Delivery PopDelivery() => Deliveries.Pop();
            internal void PushDelivery(Delivery delivery) => Deliveries.Push(delivery);

            #endregion Delivery

            #region Link

            Link LinkFactory() => new Link(Node);
            internal Link PopLink() => Links.Pop();
            internal void Push(Link link) => Links.Push(link);

            #endregion Delivery
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
