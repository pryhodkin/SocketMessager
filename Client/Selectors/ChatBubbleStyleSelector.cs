using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public class ChatBubbleStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var message = (MessageViewModel)item;
            var app = App.Current;
            if (message.IsMy)
                return app.Resources["MyMessageStyle"] as Style;
            return app.Resources["InterlocatorMessageStyle"] as Style;
        }
    }
}
