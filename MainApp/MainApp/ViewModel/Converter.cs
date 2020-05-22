using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace MainApp.ViewModel
{
    class Converter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            MouseEventArgs args = (MouseEventArgs)value;
            FrameworkElement element = (FrameworkElement)parameter;
            Point point = args.GetPosition(element);
            return point;
        }
    }
}
