/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

namespace Alimatic
{
    using Cyxor.Networking;
    using Cyxor.Networking.Events;

    public sealed class FakeCompletedEventArgs : ActionEventArgs
    {
        public override int EventId => Network.NetworkEventsId.FakeCompleted;

        public readonly Result Result;

        internal FakeCompletedEventArgs(Node node, Result result) : base(node)
        {
            Result = result;
        }
    }
}
/* { Alimatic.Server } */
