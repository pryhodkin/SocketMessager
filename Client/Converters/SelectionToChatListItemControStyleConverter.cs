using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Client
{
    class SelectionToChatListItemControStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = (bool)value;
            var app = App.Current;
            if (isSelected)
                return app.Resources["SelectedChatListItemContolStyle"] as Style;
            return app.Resources["ChatListItemControlStyle"] as Style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
