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

namespace Cyxor.Networking.Events
{
    using static Utilities.Threading;

    public abstract class AsyncActionEventArgs : ActionEventArgs, IAwaiter, IReferenceCounted
    {
        //bool Acquired = false;
        InterlockedInt References;

        Awaitable awaitable;
        Awaitable Awaitable
        {
            get
            {
                System.Threading.Interlocked.CompareExchange(ref awaitable, ExchangeAwaitable, null);
                return awaitable;
            }
        }

        Awaitable exchangeAwaitable = null;
        Awaitable ExchangeAwaitable => exchangeAwaitable ?? (exchangeAwaitable = new Awaitable());

        public Awaitable GetAwaiter() => Awaitable;
        public bool IsCompleted => awaitable?.IsCompleted ?? false;
        public Awaitable ConfigureAwait(bool continueOnCapturedContext) => Awaitable.ConfigureAwait(continueOnCapturedContext);

        //Result IAwaiter.GetResult() => awaitable?.GetResult() ?? default(Result);
        //void INotifyCompletion.OnCompleted(Action continuation) => (awaitable as INotifyCompletion).OnCompleted(continuation);

        protected AsyncActionEventArgs(Node node) : base(node) { }

        protected virtual void OnCompleted() { }

        public virtual int Acquire()
        {
            if (awaitable?.IsCompleted ?? false)
                Node.Log(LogCategory.Fatal, "");

            var value = References.Increment();

            if (value > 1)
            {
                //Acquired = true;
                //Node.Log(LogCategory.Trace, $"Event '[Id:{EventId}]-{GetType().Name}-' acquired by user.");
            }

            return value;
        }

        public virtual int Release()
        {
            if (awaitable?.IsCompleted ?? false)
                Node.Log(LogCategory.Fatal, "");

            var value = References.Decrement();

            if (value < 0)
                Node.Log(LogCategory.Fatal, "");

            if (value == 0)
            {
                if (System.Threading.Interlocked.CompareExchange(ref awaitable, Awaitable.CompletedAwaitable, null) != null)
                    awaitable.TrySetResult(Result.Success);

                OnCompleted();

                //if (Acquired)
                //    Node.Log(LogCategory.Trace, $"Event '[Id:{EventId}]-{GetType().Name}-' released by user.");
            }

            return value;
        }

        public Reference Reference => new Reference(this);

        internal void Reset()
        {
            if (References.Value != 0)
                Node.Log(LogCategory.Fatal, "");

            //Acquired = false;
            References.Value = 0;

            exchangeAwaitable?.Reset();
            awaitable = exchangeAwaitable;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
