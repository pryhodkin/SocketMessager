using System;

namespace Protocol
{
    /// <summary>
    /// Message structure with sender, receiver and text.
    /// </summary>
    [Serializable]
    public class Message
    {

        #region Public properties

        /// <summary>
        /// Message sender.
        /// </summary>
        public User Sender { get; set; }

        /// <summary>
        /// Message Receiver
        /// </summary>
        public User Receiver { get; set; }

        /// <summary>
        /// Message text.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the message with all its properties.
        /// </summary>
        /// <param name="sender">Value for <see cref="Sender"/></param>
        /// <param name="receiver">Value for <see cref="Receiver"/>.</param>
        /// <param name="text">Value for <see cref="Text"/>.</param>
        public Message(User sender, User receiver, string text)
        {
            Sender = sender;
            Receiver = receiver;
            Text = text;
        }

        #endregion

    }
}
