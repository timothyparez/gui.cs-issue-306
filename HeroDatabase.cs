using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Terminal.Gui.Issue306
{
    public class HeroDatabase : SQLiteAsyncConnection
    {   
        public bool Initialized {get; private set;} = false;

        public HeroDatabase(string filename)
            : base(filename)
        {            
            Initialize();            
        }

        public async void Initialize()
        {
            if (!await TableExists("Hero"))
            {
                await this.CreateTableAsync<Hero>();

                /* Some sample data */
                var people = new List<Hero>();
                people.Add(new Hero("Bruce", "Wayne", "Batman"));
                people.Add(new Hero("Barry", "Allen", "The Flash"));
                people.Add(new Hero("Clark", "Kent", "Sueperman"));
                people.Add(new Hero("Tony", "Stark", "Iron Man"));
                people.Add(new Hero("Peter", "Parker", "Spiderman"));
                people.Add(new Hero("Miguel", "De Icaza", "Monoman"));
                await this.InsertAllAsync(people);
            }

            //This code won't be reached until the user moves the mouse or switches control focus
            Initialized = true;
        }

        public async Task<bool> TableExists(string table)
        {
            var result = await this.ExecuteScalarAsync<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name='{table}';");
            return (!string.IsNullOrWhiteSpace(result));
        }

        public async Task<List<Hero>> ListHeroesAsync()
        {
            return await this.Table<Hero>().ToListAsync();
        }
    }

}
