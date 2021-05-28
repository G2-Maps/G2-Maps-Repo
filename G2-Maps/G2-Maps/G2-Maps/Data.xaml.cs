using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace G2_Maps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Data : ContentPage
    {
        public Data()
        {
            InitializeComponent();

            MainPage.ScrollbarPage = new Scrollbar();
            FrameScollbar.Content = MainPage.ScrollbarPage.Content;

            Position positionItaly = new Position(42.97197087627496, 12.553520745277323);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(positionItaly, Distance.FromKilometers(500));
            viewMap.MoveToRegion(mapSpan);

            Pin pin = new Pin
            {
                Label = "Italia",
                Address = "Click for more info",
                Position = positionItaly
            };

            pin.InfoWindowClicked += async (s, args) =>
            {
                string pinName = ((Pin)s).Label;
                await DisplayAlert("Info Window Clicked", $"Info of {pinName}.", "Ok");
            };

            viewMap.Pins.Add(pin);
        }
    }
}