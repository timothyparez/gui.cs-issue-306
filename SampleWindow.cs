using System.Collections.Generic;
using System.Threading.Tasks;

namespace Terminal.Gui.Issue306
{
    public class SampleWindow : Window
    {
        public List<Person> People {get; set;}

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

            var listView = new ListView(new List<Person>());
            listView.X = 1;
            listView.Y = 4;
            listView.Width = Dim.Fill();
            listView.Height = Dim.Fill() - 1;

            var label = new Label(1, 2, "Press the 'Load Data' button once, data should load after 2 seconds but it does not until you press it again");

            var button = new Button("Load Data");
            button.X = 1;
            button.Y = 1;

            button.Clicked += () =>
            {
                Application.MainLoop.Invoke(() =>
                {
                    var items = Task.Run(async () => await LoadDataAsync()).Result;
                    listView.SetSource(items);
                });
            };

            this.Add(button, label, listView);             
        }

        public async Task<List<Person>> LoadDataAsync()
        {
            await Task.Delay(2000);
            return People;
        }
    }
}
