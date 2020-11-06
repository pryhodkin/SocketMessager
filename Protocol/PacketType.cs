namespace Protocol
{
    /// <summary>
    /// Lists all available packet types.
    /// </summary>
    public enum PacketType
    {
        /// <summary>
        /// User-To-User message.
        /// </summary>
        /// <remarks>
        /// Cast <see cref="Packet.Content"/> to <see cref="Protocol.Message"/>.
        /// </remarks>
        Message,

        /// <summary>
        /// Message to user with some useful information from server.
        /// </summary>
        /// <remarks>
        /// Cast <see cref="Packet.Content"/> to <see cref="Protocol.ServerMessage"/>.
        /// </remarks>
        ServerMessage
    }
}