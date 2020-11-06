using PropertyChanged;
using Protocol;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;

namespace Client
{
    [AddINotifyPropertyChangedInterface]
    public class MessagerViewModel : INotifyPropertyChanged
    {

        #region Public Properties

        /// <summary>
        /// Chats list.
        /// </summary>
        public ObservableCollection<ChatViewModel> Chats { get; set; }

        /// <summary>
        /// UI syncronization context.
        /// </summary>
        public SynchronizationContext synchronizationContext { get; set; }

        /// <summary>
        /// Information about current client.
        /// </summary>
        public User Me { get; set; }

        /// <summary>
        /// Connection indicator.
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// The wrapper over <see cref="System.Net.Sockets.Socket"/>.
        /// </summary>
        public ClientSocket Socket { get; set; }

        /// <summary>
        /// Current chat.
        /// </summary>
        public ChatViewModel SelectedChat
        {
            get => selectedChat;
            set
            {
                if (value != selectedChat)
                {
                    //If chat is not null marks it as selected.
                    if (selectedChat is { }) selectedChat.IsSelected = false;
                    if (value is { }) value.IsSelected = true;
                    selectedChat = value;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes <see cref="MessagerViewModel"/> with default values of all its properties.
        /// </summary>
        public MessagerViewModel()
        {
            Chats = new AsyncObservableCollection<ChatViewModel>();
            Me = new User("");
            IsConnected = false;
            Socket = new ClientSocket();
            Socket.Handler += ReceiveMessage;
            SelectedChat = null;
            synchronizationContext = SynchronizationContext.Current;
        }

        #endregion

        #region Private fields

        /// <summary>
        /// Helper object to fix bug with incorrect firing <see cref="AsyncObservableCollection{T}.OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs)"/> event.
        /// </summary>
        private object _syncLock = new object();

        /// <summary>
        /// Private field of property <see cref="SelectedChat"/>.
        /// </summary>
        private ChatViewModel selectedChat;

        #endregion

        #region SendMessage

        /// <summary>
        /// Sends message.
        /// </summary>
        /// <param name="text">Message text.</param>
        public void SendMessage(string text)
        {
            var message = new Message(Me, SelectedChat.Interlocutor, text);

            var packet = new Packet(PacketType.Message, message);

            Socket.SendMessage(packet);
            SelectedChat.AddMessage(message, Me);
        }

        #endregion

        #region Packet handler

        /// <summary>
        /// Main packet handler.
        /// </summary>
        /// <param name="buffer">Packet to handle.</param>
        public void ReceiveMessage(byte[] buffer)
        {
            var packet = new Packet(buffer);
            //Handle server message.
            if (packet.Type == PacketType.ServerMessage)
            {
                var serverMessage = (ServerMessage)packet.Content;
                switch (serverMessage.Type)
                {
                    //Handle new user connected.
                    case ServerMessageType.NewUserConnected:
                        var user = (User)serverMessage.Content;
                        synchronizationContext.Send(AddChat, user);
                        break;

                    //Handle user disconnected.
                    case ServerMessageType.UserDisconected:
                        var disconnectedUser = (User)serverMessage.Content;
                        synchronizationContext.Send(RemoveChat, disconnectedUser);
                        break;

                    //handle wrong packet structure.
                    case ServerMessageType.WrongPacketStructure:
                        //
                        break;
                }
            }
            //handle message
            else
            {
                var message = (Message)packet.Content;
                var chat = Chats.First(c => c.Interlocutor == message.Sender);
                chat.AddMessage(message, Me);
            }
        }

        #endregion

        #region Notify connect and disconnect

        /// <summary>
        /// Sends client information to server.
        /// </summary>
        /// <param name="user">Client information.</param>
        public void NotifyConnect(User user)
        {
            ServerMessage notification = new ServerMessage(ServerMessageType.NewUserConnected, user, null);

            Socket.SendMessage(new Packet(PacketType.ServerMessage, notification));
        }

        /// <summary>
        /// Notifies that client wants to disconnect.
        /// </summary>
        public void NotifyDisconnect()
        {
            var serverMessage = new ServerMessage(ServerMessageType.UserDisconected, Me, null);
            var packet = new Packet(PacketType.ServerMessage, serverMessage);
            Socket.SendMessage(packet);
        }

        #endregion

        #region Methods to edit Chats asyncronous

        /// <summary>
        /// Adds chat to <see cref="Chats"/>.
        /// </summary>
        /// <param name="state">Instance of class <see cref="User"/>, must be used to initialize chat.</param>
        public void AddChat(object state)
        {
            var user = (User)state;
            var chat = new ChatViewModel(user);
            //Bug fix.
            BindingOperations.EnableCollectionSynchronization(chat.Messages, _syncLock);
            Chats.Add(chat);
            if (Chats.Count == 1)
                SelectedChat = Chats[0];
        }

        /// <summary>
        /// Removes chat from <see cref="Chats"/>.
        /// </summary>
        /// <param name="state">Interlocutor, instance of class <see cref="User"/>.</param>
        public void RemoveChat(object state)
        {
            var user = (User)state;
            var chat = Chats.First(chat => chat.Interlocutor == user);
            Chats.Remove(chat);
            if (SelectedChat == chat)
                SelectedChat = null;
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
