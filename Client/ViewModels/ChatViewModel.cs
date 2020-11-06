using PropertyChanged;
using Protocol;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Client
{
    /// <summary>
    /// View model for chat, contains collection of messages and information about interlocator.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class ChatViewModel : INotifyPropertyChanged
    {

        #region Public properties

        /// <summary>
        /// Collection of messages.
        /// </summary>
        public ObservableCollection<MessageViewModel> Messages { get; set; }

        /// <summary>
        /// Person to communicate with.
        /// </summary>
        public User Interlocutor { get; set; }

        /// <summary>
        /// Indicates selected thid object or not.
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes chat with <paramref name="interlocutor"/>.
        /// </summary>
        /// <param name="interlocutor">Person to communicate with.</param>
        public ChatViewModel(User interlocutor)
        {
            Interlocutor = interlocutor;
            Messages = new ObservableCollection<MessageViewModel>();
            IsSelected = false;
        }

        #endregion

        #region Methods to modify chat

        /// <summary>
        /// Adds message to chat.
        /// </summary>
        /// <param name="message">Message to be added to chat.</param>
        public void AddMessage(Message message, User me)
        {
            //Wraps the message to view model type.
            MessageViewModel messageViewModel = new MessageViewModel(message, me);

            Messages.Add(messageViewModel);
        }

        #endregion

        #region INotifyPropertyChanged interface implementation

        /// <summary>
        /// Fires when one of object's property changes its value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

    }
}
