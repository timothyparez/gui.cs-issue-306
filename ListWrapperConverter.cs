using System.Collections;

namespace Terminal.Gui.Issue306
{
    public class ListWrapperConverter : IValueConverter
    {
        public object Convert(object value, object parameter = null)
        {
            var wrapper = new CustomListWrapper((IList)value);
            return wrapper;            
        }
    }
}
