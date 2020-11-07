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
                nicknameTextBox.Foreground = (Brush)Application.Current.Resources["DarkGrayBrush"];
                nicknameTextBox.Text = "";
            }
        }

        private void nicknameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (nicknameTextBox.Text == "")
            {
                nicknameTextBox.Foreground = Application.Current.Resources["Brush2"] as Brush;
                nicknameTextBox.Text = "type your nickname...";
            }
        }

        private void messageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text == "type your message...")
            {
                messageTextBox.Foreground = (Brush)Application.Current.Resources["DarkGrayBrush"];
                messageTextBox.Text = "";
            }
        }

        private void messageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text == "")
            {
                messageTextBox.Foreground = Application.Current.Resources["Brush2"] as Brush;
                messageTextBox.Text = "type your message...";
            }
        }

        #endregion

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
