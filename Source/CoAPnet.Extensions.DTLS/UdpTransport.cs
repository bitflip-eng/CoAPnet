﻿using CoAPnet.Exceptions;
using CoAPnet.Transport;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Net;
using System.Net.Sockets;

namespace CoAPnet.Extensions.DTLS
{
    public sealed class UdpTransport : DatagramTransport, IDisposable
    {
        readonly CoapTransportLayerConnectOptions _connectOptions;
        
        EndPoint _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        
        Socket _socket;

        public UdpTransport(CoapTransportLayerConnectOptions connectOptions)
        {
            _connectOptions = connectOptions ?? throw new ArgumentNullException(nameof(connectOptions));

            _socket = new Socket(connectOptions.EndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        }

        public int GetReceiveLimit()
        {
            return 66000;
        }

        public int GetSendLimit()
        {
            return 66000;
        }

        public int Receive(byte[] buf, int off, int len, int waitMillis)
        {
            if (buf is null)
            {
                throw new ArgumentNullException(nameof(buf));
            }
            
            ThrowIfNotConnected();

            return _socket.ReceiveFrom(buf, off, len, SocketFlags.None, ref _remoteEndPoint);
        }

        public void Send(byte[] buf, int off, int len)
        {
            if (buf is null)
            {
                throw new ArgumentNullException(nameof(buf));
            }

            ThrowIfNotConnected();

            _socket.SendTo(buf, off, len, SocketFlags.None, _connectOptions.EndPoint);
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            try
            {
                _socket?.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                // There is no need to call "Disconnect" because we use UDP.
                _socket?.Dispose();

                _socket = null;
            }
        }
        
        void ThrowIfNotConnected()
        {
            if (_socket == null)
            {
                throw new CoapCommunicationException("The connection is closed.", null);
            }
        }
    }
}
