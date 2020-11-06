using Protocol;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.ComponentModel;
namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object _syncLock = new object();
        public MessagerViewModel Messager { get; set; }
        public MainWindow()
        {
            
            Messager = new MessagerViewModel();
            DataContext = this;
            foreach(var chat in Messager.Chats)
                BindingOperations.EnableCollectionSynchronization(chat.Messages, _syncLock);
            InitializeComponent();
        }
        #region RadioButtons Handler
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var chatControl = (ChatListItemControl)button.Content;
            var chat = (ChatViewModel)chatControl.DataContext;
            Messager.SelectedChat = chat;
        }

        #endregion

        #region TextBoxs passive effects
        private void nicknameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (nicknameTextBox.Text == "type your nickname...")
            {
                nicknameTextBox.Foreground = (Brush)Application.Current.Resources["BlackBrush"];
                nicknameTextBox.Text = "";
            }
        }

        private void nicknameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (nicknameTextBox.Text == "")
            {
                nicknameTextBox.Foreground = Application.Current.Resources["GreyBrush"] as Brush;
                nicknameTextBox.Text = "type your nickname...";
            }
        }

        private void messageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text == "type your message...")
            {
                messageTextBox.Foreground = (Brush)Application.Current.Resources["BlackBrush"];
                messageTextBox.Text = "";
            }
        }

        private void messageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text == "")
            {
                messageTextBox.Foreground = Application.Current.Resources["GreyBrush"] as Brush;
                messageTextBox.Text = "type your message...";
            }
        }

        #endregion

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (!Messager.IsConnected)
            {
                Messager.Socket.Connect("127.0.0.1", 1029);
                Messager.Me = new User(nicknameTextBox.Text);
                Messager.NotifyConnect(Messager.Me);
                Messager.IsConnected = true;
                button.Content = "Disconnect";
                sendButton.IsEnabled = true;
            }
            else
            {
                Messager.NotifyDisconnect();
                Messager.Socket.Disconnect();
                Messager.IsConnected = false;
                Messager.Chats.Clear();
                Messager.SelectedChat = null;
                button.Content = "Connect";

                
                 sendButton.IsEnabled = false;
            }
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text == "type your message...") return;
            Messager.Me = new User(nicknameTextBox.Text);
            Messager.SendMessage(messageTextBox.Text);
            messageTextBox.Text = "";
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Messager.IsConnected)
            {
                Messager.NotifyDisconnect();
                Messager.Socket.Disconnect();
            }
        }
    }
}
