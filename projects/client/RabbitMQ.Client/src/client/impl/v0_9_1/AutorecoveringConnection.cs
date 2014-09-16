// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 1.1.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (C) 2007-2014 GoPivotal, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//---------------------------------------------------------------------------
//
// The MPL v1.1:
//
//---------------------------------------------------------------------------
//  The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in
//  compliance with the License. You may obtain a copy of the License
//  at http://www.mozilla.org/MPL/
//
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
//  the License for the specific language governing rights and
//  limitations under the License.
//
//  The Original Code is RabbitMQ.
//
//  The Initial Developer of the Original Code is GoPivotal, Inc.
//  Copyright (c) 2007-2014 GoPivotal, Inc.  All rights reserved.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using RabbitMQ.Client.Impl;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace RabbitMQ.Client.Framing.Impl.v0_9_1
{
    public class AutorecoveringConnection : IConnection
    {
        protected Connection m_delegate;

        public AutorecoveringConnection(ConnectionFactory factory, IFrameHandler frameHandler)
        {
            m_delegate = new Connection(factory, false, frameHandler);
        }


        public event ConnectionShutdownEventHandler ConnectionShutdown
        {
            add
            {
                m_delegate.ConnectionShutdown += value;
            }
            remove
            {
                m_delegate.ConnectionShutdown -= value;
            }
        }

        public event ConnectionBlockedEventHandler ConnectionBlocked
        {
            add
            {
                m_delegate.ConnectionBlocked += value;
            }
            remove
            {
                m_delegate.ConnectionBlocked -= value;
            }
        }

        public event ConnectionUnblockedEventHandler ConnectionUnblocked
        {
            add
            {
                m_delegate.ConnectionUnblocked += value;
            }
            remove
            {
                m_delegate.ConnectionUnblocked -= value;
            }
        }

        public void HandleConnectionBlocked(string reason)
        {
            m_delegate.HandleConnectionBlocked(reason);
        }

        public void HandleConnectionUnblocked()
        {
            m_delegate.HandleConnectionUnblocked();
        }

        public event CallbackExceptionEventHandler CallbackException
        {
            add
            {
                m_delegate.CallbackException += value;
            }
            remove
            {
                m_delegate.CallbackException -= value;
            }
        }

        public AmqpTcpEndpoint Endpoint
        {
            get
            {
                return m_delegate.Endpoint;
            }
        }

        IProtocol IConnection.Protocol
        {
            get
            {
                return Endpoint.Protocol;
            }
        }

        public ProtocolBase Protocol
        {
            get
            {
                return (ProtocolBase)m_delegate.Protocol;
            }
        }

        public ushort ChannelMax
        {
            get
            {
                return m_delegate.ChannelMax;
            }
        }

        public uint FrameMax
        {
            get
            {
                return m_delegate.FrameMax;
            }
        }

        public ushort Heartbeat
        {
            get
            {
                return m_delegate.Heartbeat;
            }
        }

        public IDictionary<string, object> ClientProperties
        {
            get
            {
                return m_delegate.ClientProperties;
            }
        }

        public IDictionary<string, object> ServerProperties
        {
            get
            {
                return m_delegate.ServerProperties;
            }
        }

        public AmqpTcpEndpoint[] KnownHosts
        {
            get
            {
                return m_delegate.KnownHosts;
            }
            set
            {
                m_delegate.KnownHosts = value;
            }
        }

        public ShutdownEventArgs CloseReason
        {
            get
            {
                return m_delegate.CloseReason;
            }
        }

        public bool IsOpen
        {
            get
            {
                return m_delegate.IsOpen;
            }
        }

        public bool AutoClose
        {
            get
            {
                return m_delegate.AutoClose;
            }
            set
            {
                m_delegate.AutoClose = value;
            }
        }

        public IModel CreateModel()
        {
            return m_delegate.CreateModel();
        }

        public IList<ShutdownReportEntry> ShutdownReport
        {
            get
            {
                return m_delegate.ShutdownReport;
            }
        }

        void IDisposable.Dispose()
        {
            Abort();
            if (ShutdownReport.Count > 0)
            {
                foreach (ShutdownReportEntry entry in ShutdownReport)
                {
                    if (entry.Exception != null)
                        throw entry.Exception;
                }
                throw new OperationInterruptedException(null);
            }
        }

        ///<summary>API-side invocation of connection.close.</summary>
        public void Close()
        {
            m_delegate.Close();
        }

        ///<summary>API-side invocation of connection.close.</summary>
        public void Close(ushort reasonCode, string reasonText)
        {
            m_delegate.Close(reasonCode, reasonText);
        }

        ///<summary>API-side invocation of connection.close with timeout.</summary>
        public void Close(int timeout)
        {
            m_delegate.Close(timeout);
        }

        ///<summary>API-side invocation of connection.close with timeout.</summary>
        public void Close(ushort reasonCode, string reasonText, int timeout)
        {
            m_delegate.Close(reasonCode, reasonText, timeout);
        }

        public void Close(ShutdownEventArgs reason)
        {
            m_delegate.Close(reason);
        }

        ///<summary>API-side invocation of connection abort.</summary>
        public void Abort()
        {
            m_delegate.Abort();
        }

        ///<summary>API-side invocation of connection abort.</summary>
        public void Abort(ushort reasonCode, string reasonText)
        {
            m_delegate.Abort(reasonCode, reasonText);
        }

        ///<summary>API-side invocation of connection abort with timeout.</summary>
        public void Abort(int timeout)
        {
            m_delegate.Abort(timeout);
        }

        ///<summary>API-side invocation of connection abort with timeout.</summary>
        public void Abort(ushort reasonCode, string reasonText, int timeout)
        {
            m_delegate.Abort(reasonCode, reasonText, timeout);
        }

    }
}