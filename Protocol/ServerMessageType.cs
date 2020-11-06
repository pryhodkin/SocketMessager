namespace Protocol
{
    /// <summary>
    /// Lists all available server message types.
    /// </summary>
    public enum ServerMessageType
    {
        /// <summary>
        /// The user received the message.
        /// </summary>
        MessageReceived,

        /// <summary>
        /// The error, indicates that user with such username already exists.
        /// </summary>
        UserIsAlreadyExist,

        /// <summary>
        /// The new user connected to server.
        /// </summary>
        /// <remarks>
        /// Cast <see cref="ServerMessage.Content"/> to <see cref="User"/>.
        /// </remarks>
        NewUserConnected,

        /// <summary>
        /// Some user disconnected from server.
        /// </summary>
        /// <remarks>
        /// Cast <see cref="ServerMessage.Content"/> to <see cref="User"/>.
        /// </remarks>
        UserDisconected,

        /// <summary>
        /// The receiver wasn't specified in last message.
        /// </summary>
        UnspecifiedReceiver,

        /// <summary>
        /// Client's packet has some mistakes and cannot be deserialized.
        /// </summary>
        /// <remarks>
        /// Cast <see cref="ServerMessage.Content"/> to <see cref="byte[]"/>.
        /// </remarks>
        WrongPacketStructure
    }
}