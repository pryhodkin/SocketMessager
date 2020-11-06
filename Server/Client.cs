using Protocol;
using System.Net.Sockets;

namespace Server
{
    class Client
    {

        #region Public properties

        /// <summary>
        /// <see cref="System.Net.Sockets.Socket"/> to communicate with client.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// User that represents by this object.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Buffer to receive data from remote client.
        /// </summary>
        public byte[] Buffer { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize client with all its properties.
        /// </summary>
        /// <param name="socket">Value for <see cref="Socket"/></param>
        /// <param name="user">Value for <see cref="User"/></param>
        public Client(Socket socket, User user)
        {
            Socket = socket;
            User = user;
        }

        #endregion

        #region SendMessage method overloads

        /// <summary>
        /// Sends server message to its receiver.
        /// </summary>
        /// <param name="message">Server message to be sent.</param>
        public void SendMessage(ServerMessage message)
        {
            //Receiver null check.
            if (message.Receiver is null) return;

            //Packing message to packet and sending it.
            var packet = new Packet(PacketType.ServerMessage, message);
            Socket.Send(packet.ToBytes());
        }

        /// <summary>
        /// Sends message to its receiver.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        public void SendMessage(Message message)
        {
            //Receiver null check.
            if (message.Receiver is null) return;

            //Packing message to packet and sending it.
            var packet = new Packet(PacketType.Message, message);
            Socket.Send(packet.ToBytes());
        }

        #endregion

        #region ToString override

        ///<inheritdoc cref="System.Object.ToString"/>
        public override string ToString()
        {
            return User.ToString();
        }

        #endregion

    }
}
