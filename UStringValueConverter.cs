using System.Text;
using NStack;

namespace Terminal.Gui.Issue306
{
    public class UStringValueConverter : IValueConverter
    {
        public object Convert(object value, object parameter = null)
        {
            var data = Encoding.ASCII.GetBytes(value.ToString());
            return ustring.Make(data);
        }
    }
}
