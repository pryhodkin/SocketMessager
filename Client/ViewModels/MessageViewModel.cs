using PropertyChanged;
using Protocol;
using System;
using System.ComponentModel;

namespace Client
{
    [AddINotifyPropertyChangedInterface]
    public class MessageViewModel : INotifyPropertyChanged
    {

        #region Public properties

        /// <summary>
        /// Message text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Date and time when Message was received.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Indicated who is sender of message: me or interlocutor.
        /// </summary>
        public bool IsMy { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes <see cref="MessagerViewModel"/>.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="Me">For indication who is sender of message.</param>
        public MessageViewModel(Message message, User Me)
        {
            Text = message.Text;
            Time = DateTime.Now;
            IsMy = message.Sender == Me;
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

    }
}
