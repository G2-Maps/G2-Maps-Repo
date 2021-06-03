using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using G2_Maps.Model;

namespace G2_Maps
{
    public partial class MainPage : ContentPage
    {
        public static Data DataPage { get; set; } = new Data();
        public static Scrollbar ScrollbarPage { get; set; } = new Scrollbar();

        public MainPage()
        {
            InitializeComponent();

            MainFrame.Content = DataPage.Content;

            DataManagement.CheckUpdates();
            DataManagement.GetDataFile_Async();
        }

        public async void  DisplayPins_Async(Regions regions)
        {
            foreach (var region in regions.data)
            {
                Pin _pin = new Pin()
                {
                    Address = region.name,
                    Position = new Position(region.coordinates.x, region.coordinates.y)
                };

                _pin.InfoWindowClicked += async (s, args) =>
                {
                    LoadData_Async((Pin)s);
                };
            }
        }

        public async void LoadData_Async(Pin s)
        {
            var data = DataManagement.GetData_Regione(s.Address);
        }
    }
}
