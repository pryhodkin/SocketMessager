using System;
using System.Globalization;
using System.Windows.Data;

namespace Client
{
    class BoolToConnectButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isConnected = (bool)value;
            if (isConnected)
                return "Disconnect";
            return "Connect";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
