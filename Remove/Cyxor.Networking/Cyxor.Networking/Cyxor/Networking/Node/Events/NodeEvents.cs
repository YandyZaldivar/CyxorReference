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
using System.Threading;
using System.Threading.Tasks;

namespace Cyxor.Networking
{
    using Config;
    using Events;

    public abstract partial class Node
    {
        public abstract class NodeEvents : NodeProperty
        {
            internal NodeEvents(Node node) : base(node) { }

            public event EventHandler<MessageLoggedEventArgs> MessageLogged;

            public event EventHandler<SslCertificateSelectingEventArgs> SslCertificateSelecting;
            public event EventHandler<SslCertificateValidatingEventArgs> SslCertificateValidating;

            public event EventHandler<CommandExecutingEventArgs> CommandExecuting;
            public event EventHandler<CommandExecuteCompletedEventArgs> CommandExecuteCompleted;

            public event EventHandler<DisconnectingEventArgs> Disconnecting;
            public event EventHandler<ConnectCompletedEventArgs> ConnectCompleted;
            public event EventHandler<DisconnectCompletedEventArgs> DisconnectCompleted;
            public event EventHandler<ConnectProgressChangedEventArgs> ConnectProgressChanged;

            public event EventHandler<PacketSendCompletedEventArgs> PacketSendCompleted;
            public event EventHandler<PacketReceiveCompletedEventArgs> PacketReceiveCompleted;
            public event EventHandler<PacketSendProgressChangedEventArgs> PacketSendProgressChanged;
            public event EventHandler<PacketReceiveProgressChangedEventArgs> PacketReceiveProgressChanged;

            public void RaiseEvent<TActionEventArgs>(EventHandler<TActionEventArgs> handler, TActionEventArgs e) where TActionEventArgs : ActionEventArgs
            {
                if (Node.Config.EventDispatching == EventDispatching.Concurrent)
                {
                    var invocationList = handler?.GetInvocationList();

                    if (invocationList?.Length > 1)
                    {
                        Parallel.ForEach(invocationList, d => d.DynamicInvoke(Node, e));
                        return;
                    }
                }

                handler?.Invoke(Node, e);
            }

            public virtual void RaiseEvent<TActionEventArgs>(TActionEventArgs e, bool detached = false) where TActionEventArgs : ActionEventArgs
            {
                if (!Node.Events.IsSubscribed(e.EventId))
                    return;

                if (!detached && !Node.Config.OverrideEvents)
                    return;

                switch (e.EventId)
                {
                    case NodeEventsId.MessageLogged: RaiseEvent(MessageLogged, e as MessageLoggedEventArgs); break;

                    case NodeEventsId.SslCertificateSelecting: RaiseEvent(SslCertificateSelecting, e as SslCertificateSelectingEventArgs); break;
                    case NodeEventsId.SslCertificateValidating: RaiseEvent(SslCertificateValidating, e as SslCertificateValidatingEventArgs); break;

                    case NodeEventsId.CommandExecuting: RaiseEvent(CommandExecuting, e as CommandExecutingEventArgs); break;
                    case NodeEventsId.CommandExecuteCompleted: RaiseEvent(CommandExecuteCompleted, e as CommandExecuteCompletedEventArgs); break;

                    case NodeEventsId.Disconnecting: RaiseEvent(Disconnecting, e as DisconnectingEventArgs); break;
                    case NodeEventsId.ConnectCompleted: RaiseEvent(ConnectCompleted, e as ConnectCompletedEventArgs); break;
                    case NodeEventsId.DisconnectCompleted: RaiseEvent(DisconnectCompleted, e as DisconnectCompletedEventArgs); break;
                    case NodeEventsId.ConnectProgressChanged: RaiseEvent(ConnectProgressChanged, e as ConnectProgressChangedEventArgs); break;

                    case NodeEventsId.PacketSendCompleted: RaiseEvent(PacketSendCompleted, e as PacketSendCompletedEventArgs); break;
                    case NodeEventsId.PacketReceiveCompleted: RaiseEvent(PacketReceiveCompleted, e as PacketReceiveCompletedEventArgs); break;
                    case NodeEventsId.PacketSendProgressChanged: RaiseEvent(PacketSendProgressChanged, e as PacketSendProgressChangedEventArgs); break;
                    case NodeEventsId.PacketReceiveProgressChanged: RaiseEvent(PacketReceiveProgressChanged, e as PacketReceiveProgressChangedEventArgs); break;
                }
            }

            public virtual void OnEvent<TActionEventArgs>(TActionEventArgs e) where TActionEventArgs : ActionEventArgs
            {
                switch (e.EventId)
                {
                    case NodeEventsId.MessageLogged: Node.OnMessageLogged(e as MessageLoggedEventArgs); break;

                    case NodeEventsId.SslCertificateSelecting: Node.OnSslCertificateSelecting(e as SslCertificateSelectingEventArgs); break;
                    case NodeEventsId.SslCertificateValidating: Node.OnSslCertificateValidating(e as SslCertificateValidatingEventArgs); break;

                    case NodeEventsId.CommandExecuting: Node.OnCommandExecuting(e as CommandExecutingEventArgs); break;
                    case NodeEventsId.CommandExecuteCompleted: Node.OnCommandExecuteCompleted(e as CommandExecuteCompletedEventArgs); break;

                    case NodeEventsId.Disconnecting: Node.OnDisconnecting(e as DisconnectingEventArgs); break;
                    case NodeEventsId.ConnectCompleted: Node.OnConnectCompleted(e as ConnectCompletedEventArgs); break;
                    case NodeEventsId.DisconnectCompleted: Node.OnDisconnectCompleted(e as DisconnectCompletedEventArgs); break;
                    case NodeEventsId.ConnectProgressChanged: Node.OnConnectProgressChanged(e as ConnectProgressChangedEventArgs); break;

                    case NodeEventsId.PacketSendCompleted: Node.OnPacketSendCompleted(e as PacketSendCompletedEventArgs); break;
                    case NodeEventsId.PacketReceiveCompleted: Node.OnPacketReceiveCompleted(e as PacketReceiveCompletedEventArgs); break;
                    case NodeEventsId.PacketSendProgressChanged: Node.OnPacketSendProgressChanged(e as PacketSendProgressChangedEventArgs); break;
                    case NodeEventsId.PacketReceiveProgressChanged: Node.OnPacketReceiveProgressChanged(e as PacketReceiveProgressChangedEventArgs); break;
                }
            }

            public virtual bool IsSubscribed(int eventId)
            {
                switch (eventId)
                {
                    case NodeEventsId.MessageLogged: return MessageLogged != null;

                    case NodeEventsId.SslCertificateSelecting: return SslCertificateSelecting != null;
                    case NodeEventsId.SslCertificateValidating: return SslCertificateValidating != null;

                    case NodeEventsId.CommandExecuting: return CommandExecuting != null;
                    case NodeEventsId.CommandExecuteCompleted: return CommandExecuteCompleted != null;

                    case NodeEventsId.Disconnecting: return Disconnecting != null;
                    case NodeEventsId.ConnectCompleted: return ConnectCompleted != null;
                    case NodeEventsId.DisconnectCompleted: return DisconnectCompleted != null;
                    case NodeEventsId.ConnectProgressChanged: return ConnectProgressChanged != null;

                    case NodeEventsId.PacketSendCompleted: return PacketSendCompleted != null;
                    case NodeEventsId.PacketReceiveCompleted: return PacketReceiveCompleted != null;
                    case NodeEventsId.PacketSendProgressChanged: return PacketSendProgressChanged != null;
                    case NodeEventsId.PacketReceiveProgressChanged: return PacketReceiveProgressChanged != null;

                    default: throw new InvalidOperationException("The event type doesn’t exist");
                }
            }

            public void Process()
            {
                if (Node.Config.EventDispatching != EventDispatching.Manual)
                    throw new InvalidOperationException("To use this method set 'Node.Config.EventDispatching = EventDispatching.Manual'.");

                if (Node.EventQueue.Count == 0)
                    return;

                while (Node.EventQueue.TryDequeue(out var e))
                    Post(e, redirected: true);
            }

            public async Task ProcessAsync()
            {
                if (Node.Config.EventDispatching != EventDispatching.Manual)
                    throw new InvalidOperationException("To use this method set 'Node.Config.EventDispatching = EventDispatching.Manual'.");

                if (Node.EventQueue.Count == 0)
                    while (Node.EventQueue.TryDequeue(out var e))
                        if (e is AsyncActionEventArgs eAsync)
                            await Post(eAsync, redirected: true);
                        else
                            Post(e, redirected: true);
            }

            public TActionEventArgs Post<TActionEventArgs>(TActionEventArgs e, bool redirected = false)
                where TActionEventArgs : ActionEventArgs
            {
                // TODO: implement the type and the iseventtypesubscribed in every event class !Partially done!
                //var xx = hija.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var raiseEvent = IsSubscribed(e.EventId) || (!(GetType() == typeof(Server) && !(GetType() == typeof(Client))));

                var asyncActionEventArgs = e as AsyncActionEventArgs;

                if (!redirected)
                {
                    if (!raiseEvent)
                        Node.Log(LogCategory.Information, "Unregistered event");
                    else if (Node.Config.EventDispatching == EventDispatching.Manual)
                    {
                        asyncActionEventArgs?.Acquire();
                        Node.EventQueue.Enqueue(e);
                        return e;
                    }
                    else if (Node.Config.EventDispatching == EventDispatching.Synchronized)
                    {
                        if (Node.Config.SynchronizationContextManagedThreadId != Thread.CurrentThread.ManagedThreadId)
                        {
                            asyncActionEventArgs?.Acquire();
                            Node.Config.SynchronizationContext.Post(state => Post(state as TActionEventArgs, redirected: true), e);
                            return e;
                        }
                    }
                }

                try
                {
                    if (!redirected)
                        asyncActionEventArgs?.Acquire();

                    //if (!(packetReceiveCompletedEventArgs?.Packet?.Internal ?? false))
                    //    e.Action();

                    e.Action();

                    if (e is PacketReceiveCompletedEventArgs packetReceiveCompletedEventArgs)
                        Node.Controllers.Post(packetReceiveCompletedEventArgs);
                }
                catch (Exception ex)
                {
                    if (e is MessageLoggedEventArgs)
                    {
                        var reason = $"User exception detected in the log event handler. {Environment.NewLine}{ex.Message}";

#pragma warning disable 4014
                        Node.DisconnectAsync(new Result(comment: reason));
#pragma warning restore 4014
                    }
                    else
                        Node.Log(LogCategory.Error, exception: ex);
                }
                finally
                {
                    asyncActionEventArgs?.Release();
                }

                return e;
            }

            //            public TActionEventArgs Post<TActionEventArgs>(TActionEventArgs e, bool fromQueue = false)
            //                where TActionEventArgs : ActionEventArgs
            //            {
            //                // TODO: implement the type and the iseventtypesubscribed in every event class !Partially done!
            //                //var xx = hija.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            //                var raiseEvent = IsSubscribed(e.EventId) || (!(GetType() == typeof(Server) && !(GetType() == typeof(Client))));



            //                if (!raiseEvent)
            //                    Node.Log(LogCategory.Information, "Unregistered event");
            //                else if (Node.Config.EventDispatching == EventDispatching.Manual)
            //                {
            //                    if (!fromQueue)
            //                    {
            //                        Node.EventQueue.Enqueue(e);
            //                        return e;
            //                    }
            //                }
            //                else if (Node.Config.EventDispatching == EventDispatching.Synchronized)
            //                {
            //                    if (Node.Config.SynchronizationContext != SynchronizationContext.Current)
            //                    {
            //                        Node.Config.SynchronizationContext.Post(state => Post(state as TActionEventArgs), e);
            //                        return e;
            //                    }
            //                }

            //                var asyncActionEventArgs = e as AsyncActionEventArgs;

            //                try
            //                {
            //                    asyncActionEventArgs?.Acquire();

            //                    var packetReceiveCompletedEventArgs = e as PacketReceiveCompletedEventArgs;

            //                    //if (!(packetReceiveCompletedEventArgs?.Packet?.Internal ?? false))
            //                    //    e.Action();

            //                    e.Action();

            //                    if (packetReceiveCompletedEventArgs != null)
            //                        Node.Controllers.Post(packetReceiveCompletedEventArgs);
            //                }
            //                catch (Exception ex)
            //                {
            //                    if (e is MessageLoggedEventArgs)
            //                    {
            //                        var reason = $"User exception detected in the log event handler. {Environment.NewLine}{ex.Message}";

            //#pragma warning disable 4014
            //                        Node.DisconnectAsync(reason);
            //#pragma warning restore 4014
            //                    }
            //                    else
            //                        Node.Log(LogCategory.Error, exception: ex);
            //                }
            //                finally
            //                {
            //                    asyncActionEventArgs?.Release();
            //                }

            //                return e;
            //            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
