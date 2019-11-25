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


        public SampleWindow(string title, int padding)
            : base(title, padding)
        {

        }

        public SampleWindow(string title)
            : base(title)
        {            

            DataContext = new ViewModel();

            listView = new ListView(new List<Hero>());
            listView.X = 1;
            listView.Y = 5;
            listView.Width = Dim.Fill();
            listView.Height = Dim.Fill() - 1;

            label = new Label(1, 2, "Press the 'Load Data' button once, data should load after 2 seconds but it does not until you press it again");
            label2 = new Label(1, 3, "");

            var ustringConverter = new UStringValueConverter();
            var listWrapperConverter = new ListWrapperConverter();

            var labelBinding = new Binding(this, "Text", label2, "Text", ustringConverter);
            var listBinding = new Binding(this, "Heroes", listView, "Source", listWrapperConverter);
            

            button = new Button("Load Data");
            button.X = 1;
            button.Y = 1;            
            button.Clicked += () => DataContext.Refresh();

            this.Add(button, label, label2, listView);             
        }

        
    }

}
