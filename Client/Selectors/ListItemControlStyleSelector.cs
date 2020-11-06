using System.Windows;
using System.Windows.Controls;

namespace Client
{
    class ListItemControlStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var listIitem = (ChatListItemControl)container;
            var app = App.Current;
            return app.Resources["ChatListItemControlStyle"] as Style;
        }
    }
}
