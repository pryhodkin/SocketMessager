using Protocol;
using System;
using System.Linq;
using System.Threading;

namespace Server
{
    /// <summary>
    /// Data handlers.
    /// </summary>
    static class Handler
    {

        #region Main packet handler

        /// <summary>
        /// Packet handler.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="buffer">The data what was received from client (packet).</param>
        /// <param name="sender">The client we received the data from.</param>
        public static void HandlePacket(ServerSocket server, byte[] buffer, Client sender)
        {
            //Null checks.
            if (buffer is null || server is null) return;
            //Handling packet.
            try
            {
                var packet = new Packet(buffer);
                switch (packet.Type)
                {
                    //Handling message
                    case PacketType.Message:
                        var message = (Message)packet.Content;
                        HandleMessage(server, message);
                        break;
                    //Handling server message
                    case PacketType.ServerMessage:
                        var serverMessage = (ServerMessage)packet.Content;
                        HandleServerMessage(server, serverMessage, sender);
                        break;
                }
            }
            catch
            {
                HandleWrongPacket(sender, buffer);
            }

        }

        #endregion

        #region Minor handlers for every packet type

        /// <summary>
        /// Server message handler.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="serverMessage">The server message we received from client.</param>
        /// <param name="sender">The client we received server message from.</param>
        public static void HandleServerMessage(ServerSocket server, ServerMessage serverMessage, Client sender)
        {
            switch (serverMessage.Type)
            {
                //Handle new user connected.
                case ServerMessageType.NewUserConnected:
                    sender.User = (User)serverMessage.Content;
                    var clients = server.Clients.Where(c => c.User != sender.User);
                    foreach (var client in clients)
                    {
                        //Add chat with new user to every connected user. 
                        serverMessage.Content = sender.User;
                        serverMessage.Receiver = client.User;
                        client.SendMessage(serverMessage);
                    }
                    //Add chats with every connected user to new user's chatlist.
                    foreach (var client in clients)
                    {
                        //Franckly, I don't know why it doesn't work without sleep.
                        //I daresay this is due to the fact that the sender doesn't have enought time to process packets.
                        Thread.Sleep(50);
                        serverMessage.Content = client.User;
                        serverMessage.Receiver = sender.User;
                        sender.SendMessage(serverMessage);
                    }
                    break;
                //Handle user disconnect.
                case ServerMessageType.UserDisconected:
                    sender.Socket.Close();
                    server.Clients.Remove(sender);
                    foreach (var client in server.Clients)
                    {
                        serverMessage.Receiver = client.User;
                        client.SendMessage(serverMessage);
                    }
                    break;
            }
        }

        /// <summary>
        /// Message handler.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="message">The message we received from client.</param>
        public static void HandleMessage(ServerSocket server, Message message)
        {
            //Sender null check.
            if (message.Sender is null) return;
            //Receiver null check.
            if (message.Receiver is null)
            {
                //Serches specified user in clients list.
                var sender = GetClient(server, message.Sender);
                //Sends notification to sender about unspecified reseiver.
                var answer = new ServerMessage(ServerMessageType.UnspecifiedReceiver, "Receiver was null.", message.Sender);
                sender.SendMessage(answer);

                return;
            }

            //After all checks simply send message.
            var client = GetClient(server, message.Receiver);
            if (client is { })
                client.SendMessage(message);
        }

        /// <summary>
        /// Reaction to wrong packet.
        /// </summary>
        /// <param name="sender"><paramref name="packet"/> was sent by <paramref name="sender"/>.</param>
        /// <param name="packet">Packet that was sent.</param>
        public static void HandleWrongPacket(Client sender, byte[] packet)
        {
            //Notifies client about wrong packet structure.
            var answer = new ServerMessage(ServerMessageType.WrongPacketStructure, packet, sender.User);
            sender.SendMessage(answer);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Get the client that represents this user.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private static Client GetClient(ServerSocket server, User user)
        {
            var result = server.Clients.First(client => client.User == user);

            return result;
        }

        #endregion
    }
}
