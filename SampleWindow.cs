//#define WORKAROUND

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

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

    public class SampleWindow : Window
    {
        public List<Person> People {get; set;}

        public ViewModel DataContext {get; set;}

        private ListView listView;        
        private Label label;
        private Label label2;
        private Button button;


        public SampleWindow(string title, int padding)
            : base(title, padding)
        {

        }

        public SampleWindow(string title)
            : base(title)
        {
            People = new List<Person>()
            {
                new Person("Bruce", "Wayne", "Batman"),
                new Person("Barry", "Allen", "The Flash"),
                new Person("Clark", "Kent", "Sueperman"),
                new Person("Tony", "Stark", "Iron Man"),
                new Person("Peter", "Parker", "Spiderman"),
                new Person("Miguel", "De Icaza", "Monoman")
            }; 

            DataContext = new ViewModel();

            listView = new ListView(new List<Person>());
            listView.X = 1;
            listView.Y = 5;
            listView.Width = Dim.Fill();
            listView.Height = Dim.Fill() - 1;

            label = new Label(1, 2, "Press the 'Load Data' button once, data should load after 2 seconds but it does not until you press it again");
            label2 = new Label(1, 3, "");

            var binding = new Binding(this, "Text", label2, "Text");

            button = new Button("Load Data");
            button.X = 1;
            button.Y = 1;

            int x = 0;
            button.Clicked += () =>
            {      

#if WORKAROUND                                                                                                                 
                Application.MainLoop.Invoke(() =>
                {            
                    var source = Task.Run(async () => await LoadDataAsync()).Result;  
                    listView.SetSource(source);
                    DataContext.Text = $"Data Load Count: {x++}";                    
                });
#else 
                Application.MainLoop.Invoke(async () =>
                {            
                    var source = await LoadDataAsync();  
                    listView.SetSource(source);
                    DataContext.Text = $"Data Load Count: {x++}";                    
                });             
#endif
                
            };

            this.Add(button, label, label2, listView);             
        }

        public async Task<List<Person>> LoadDataAsync()
        {
            await Task.Delay(2000);
            return People;
        }
    }
}
