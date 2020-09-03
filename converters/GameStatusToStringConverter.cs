using System;
using System.Globalization;
using System.Windows.Data;
using tictactoe.models;

namespace tictactoe.converters
{
    class GameStatusToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case TicTacToe.Status.Busy:
                        return "aan beurt";
                case TicTacToe.Status.Draw:
                    return "gelijkspel";
                case TicTacToe.Status.Win:
                    return "gewonnen";
                default:
                    throw new NotImplementedException("onbegrijpelijke status");
                        
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
