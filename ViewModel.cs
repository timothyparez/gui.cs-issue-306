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
        private List<Hero> heroes;

        private HeroDatabase database;
        private int x = 0;


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

        public List<Hero> Heroes
        {
            get => heroes;
            set
            {
                if (value != heroes)
                {
                    heroes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Heroes)));
                }
            }
        }

        public ViewModel()
        {
            database = new HeroDatabase("heroes.sqlite");
        }

        public void Refresh()
        {

#if WORKAROUND                                                                                                                 
                Application.MainLoop.Invoke(() =>
                {            
                    if (!database.Initialized)
                    {
                        Text = "Database not ready";
                    }
                    else
                    {
                        Heroes = Task.Run(async () => await database.ListHeroesAsync()).Result;                          
                        Text = $"Data Load Count: {x++}";                    
                    }
                });
#else 
                Application.MainLoop.Invoke(async () =>
                {                                
                    if (!database.Initialized)
                    {
                        Text = "Database not ready";
                    }
                    else
                    {
                        Heroes = await database.ListHeroesAsync();   
                        Text = $"Data Load Count: {x++}";                    
                    }
                });             
#endif
        }

        public async Task<List<Hero>> LoadDataAsync()
        {
            return await database.ListHeroesAsync();            
        }
    }

}
