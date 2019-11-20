/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;

namespace Alimatic
{
    using Cyxor.Networking;

    public partial class Network
    {
        public class NetworkEvents : MasterEvents
        {
            Network Network;

            protected internal NetworkEvents(Network network) : base(network)
            {
                Network = network;
            }

            public event EventHandler<FakeCompletedEventArgs> FakeCompleted;

            public override void RaiseEvent<TActionEventArgs>(TActionEventArgs e, bool detached = false)
            {
                if (!detached && !Node.Config.OverrideEvents)
                    return;

                switch (e.EventId)
                {
                    case NetworkEventsId.FakeCompleted: RaiseEvent(FakeCompleted, e as FakeCompletedEventArgs); break;

                    default: base.RaiseEvent(e, detached); break;
                }
            }

            public override void OnEvent<TActionEventArgs>(TActionEventArgs e)
            {
                switch (e.EventId)
                {
                    case NetworkEventsId.FakeCompleted: Network.OnFakeCompleted(e as FakeCompletedEventArgs); break;

                    default: base.OnEvent(e); break;
                }
            }

            public override bool IsSubscribed(int eventId)
            {
                switch (eventId)
                {
                    case NetworkEventsId.FakeCompleted: return FakeCompleted != null;

                    default: return base.IsSubscribed(eventId);
                }
            }
        }
    }
}
/* { Alimatic.Server } */
