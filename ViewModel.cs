//#define WORKAROUND

using System.ComponentModel;

namespace Terminal.Gui.Issue306
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string text;

        public string Text
        {
            get => text;
            set 
            {
                if (value != text)
                {
                    text = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }
        }
    }

}
