//#define WORKAROUND

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Terminal.Gui.Issue306
{
    public class SampleWindow : Window
    {        

        public ViewModel DataContext {get; set;}

        private ListView listView;        
        private Label label;
        private Label label2;
        private Button button;
        private HeroDatabase database;


        public SampleWindow(string title, int padding)
            : base(title, padding)
        {

        }

        public SampleWindow(string title)
            : base(title)
        {
            database = new HeroDatabase("heroes.sqlite");

            DataContext = new ViewModel();

            listView = new ListView(new List<Hero>());
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
                    if (!database.Initialized)
                    {
                        DataContext.Text = "Database not ready";
                    }
                    else
                    {
                        var source = Task.Run(async () => await LoadDataAsync()).Result;  
                        listView.SetSource(source);
                        DataContext.Text = $"Data Load Count: {x++}";                    
                    }
                });
#else 
                Application.MainLoop.Invoke(async () =>
                {                                
                    if (!database.Initialized)
                    {
                        DataContext.Text = "Database not ready";
                    }
                    else
                    {
                        var source = await LoadDataAsync();  
                        listView.SetSource(source);
                        DataContext.Text = $"Data Load Count: {x++}";                    
                    }
                });             
#endif
                
            };

            this.Add(button, label, label2, listView);             
        }

        public async Task<List<Hero>> LoadDataAsync()
        {
            return await database.ListHeroesAsync();            
        }
    }

}
