using System;

namespace Protocol
{
    /// <summary>
    /// Server message structure with receiver and data.
    /// </summary>
    [Serializable]
    public class ServerMessage
    {

        #region Public properties

        /// <summary>
        /// Server message type.
        /// </summary>
        public ServerMessageType Type { get; set; }

        /// <summary>
        /// Server message data.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Server message receiver.
        /// </summary>
        public User Receiver { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the server message with all its properties.
        /// </summary>
        /// <param name="type">Value for <see cref="Type"/></param>
        /// <param name="content">Value for <see cref="Content"/></param>
        /// <param name="receiver">Value for <see cref="Receiver"/></param>
        public ServerMessage(ServerMessageType type, object content, User receiver)
        {
            Type = type;
            Content = content;
            Receiver = receiver;
        }

        #endregion

    }
}
