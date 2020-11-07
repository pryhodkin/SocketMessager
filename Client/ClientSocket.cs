using Protocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public class ClientSocket
    {
        private Socket socket;
        private byte[] buffer;
        public PacketHandler Handler { get; set; }

        public ClientSocket()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
        }

        public void Connect(string ipAddress, int port)
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            socket.BeginConnect(endPoint, ConnectCallback, endPoint);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);
            if (socket.Connected)
            {
                int n = socket.ReceiveBufferSize;
                buffer = new byte[n];
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, null);
            }
            else
            {
                Thread.Sleep(1000);
                var endPoint = (IPEndPoint)result.AsyncState;
                Connect(endPoint.Address.ToString(), endPoint.Port);
            }
        }

        private void ReceivedCallback(IAsyncResult result)
        {
            SocketError errorCode;
            try
            {
                int n = socket.EndReceive(result, out errorCode);

                byte[] packet = new byte[n];
                Array.Copy(buffer, packet, n);

                if (n != 0)
                    Handler(packet);
                n = socket.ReceiveBufferSize;
                buffer = new byte[n];
                if (socket.Connected)
                {
                    socket.BeginReceive(buffer, 0, n, SocketFlags.None, ReceivedCallback, null);
                }
            }
            catch (ObjectDisposedException) { }
            catch (ArgumentException) { }
        }

        public void SendMessage(Packet packet)
        {
            socket.Send(packet.ToBytes());
        }

        public delegate void PacketHandler(byte[] packet);

        public void Disconnect()
        {
            socket.Close();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
