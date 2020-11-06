using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using Protocol;
using System.Collections.ObjectModel;

namespace Server
{
    class ServerSocket
    {
        /// <summary>
        /// Binded socket to <see cref="IPEndPoint"> of server.
        /// </summary>
        private Socket socket { get; set; }

        /// <summary>
        /// Clients list.
        /// </summary>
        public List<Client> Clients { get; private set; } = new List<Client>();

        #region Constructor

        /// <summary>
        /// Initialize the server with tcp <see cref="Socket"/>.
        /// </summary>
        public ServerSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion

        #region Bind & Listen

        /// <summary>
        /// Binds server's <see cref="Socket"/> to exect <paramref name="port"/>.
        /// </summary>
        /// <param name="port">Port to bind.</param>
        public void Bind(int port)
        {
            var endPoint = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(endPoint);
            Console.WriteLine($"Binded to {endPoint.Address}, port: {port}...");
        }

        /// <summary>
        /// Switches server to a listening mode.
        /// </summary>
        /// <param name="backlog">The maximum length of the pending connections queue.</param>
        public void Listen(int backlog)
        {
            socket.Listen(backlog);
            Console.WriteLine($"Started to listening with backlog = {backlog}");
        }

        #endregion

        #region Accept & Callback

        /// <summary>
        /// Starts accepting the clients.
        /// </summary>
        public void Accept()
        {
            socket.BeginAccept(AcceptedCallback, null);
            Console.WriteLine("Started waiting for clients...");
        }

        /// <summary>
        /// Accepts the client that is trying to connect.
        /// </summary>
        /// <param name="result">Asyncronous result.</param>
        private void AcceptedCallback(IAsyncResult result)
        {
            Socket clientSocket = socket.EndAccept(result);
            Console.WriteLine($"Accepted new client with endpoint: {clientSocket.RemoteEndPoint}...");
            var client = new Client(clientSocket, new User(""));
            Clients.Add(client);
            Accept();
            int n = clientSocket.ReceiveBufferSize;
            client.Buffer = new byte[n];
            clientSocket.BeginReceive(Clients.Last().Buffer, 0, Clients.Last().Buffer.Length, SocketFlags.None, ReceivedCallback, client);
            Console.WriteLine("Waiting for packets from client...");
        }

        #endregion

        #region Reseive Callback

        /// <summary>
        /// Processing data from clients.
        /// </summary>
        /// <param name="result">Asyncronous result.</param>
        public void ReceivedCallback(IAsyncResult result)
        {
            //Receive data from socket to buffer and copy to packet.
            Client client = result.AsyncState as Client;
            SocketError error;
            int n = client.Socket.EndReceive(result, out error);
            byte[] packet = new byte[n];
            Array.Copy(client.Buffer, packet, n);
            Console.WriteLine($"Received packet from client {client}");

            //Handle packet.
            Handler.HandlePacket(this, packet, client);
            Console.WriteLine($"Handled packet from client {client}");

            //Reset buffer and start waiting for next packet from client.
            if (client.Socket.Connected)
            {
                n = client.Socket.ReceiveBufferSize;
                client.Buffer = new byte[n];
                client.Socket.BeginReceive(client.Buffer, 0, n, SocketFlags.None, ReceivedCallback, client);
                Console.WriteLine("Waiting for packets from client...");
            }
        }

        #endregion
    }
}
