using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Protocol
{
    /// <summary>
    /// Default packet structre with type and data attributes.
    /// </summary>
    [Serializable]
    public class Packet
    {

        #region Public properties

        /// <summary>
        /// Packet type.
        /// </summary>
        public PacketType Type { get; set; }

        /// <summary>
        /// Packet data.
        /// </summary>
        public object Content { get; set; }

        #endregion

        #region Private static helper objects

        /// <summary>
        /// <see cref="BinaryFormatter"/> neccesery to serialize/deserialize objects to/from byte arrays.
        /// </summary>
        private static BinaryFormatter formatter = new BinaryFormatter();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the packet with specific type and data.
        /// </summary>
        /// <param name="type">Type of packet.</param>
        /// <param name="content">Data to be placed in packet.</param>
        public Packet(PacketType type, object content)
        {
            Type = type;
            Content = content;
        }

        /// <summary>
        /// Initializes the packet deserializing the byte array.
        /// </summary>
        /// <param name="array">Byte array to be deserialized.</param>
        /// <exception cref="NullReferenceException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="SerializationException"/>
        public Packet(byte[] array)
        {
            //Simple deserialization using BinaryFormatter and MemoryStream
            var stream = new MemoryStream(array);
            Packet p = formatter.Deserialize(stream) as Packet;

            Type = p.Type;
            Content = p.Content;
        }

        /// <summary>
        /// Default constructor. Unsafe!
        /// </summary>
        private Packet()
        {
        }

        #endregion

        #region To byte array conversion

        /// <summary>
        /// Serializes current packet to byte array using <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <returns>Byte array representation of current object.</returns>
        public byte[] ToBytes()
        {
            //Simple serialization using BinaryFormatter and MemoryStream
            var stream = new MemoryStream();
            formatter.Serialize(stream, this);
            byte[] packet = stream.ToArray();
            return packet;
        }

        #endregion

    }
}
