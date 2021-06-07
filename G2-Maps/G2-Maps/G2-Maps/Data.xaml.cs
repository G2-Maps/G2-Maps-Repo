﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

using G2_Maps.Model;

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

        public void DisplayPins(Regions regions)
        {
            foreach (var region in regions.data)
            {
                Pin _pin = new Pin()
                {
                    Label = region.name,
                    Address = "Click for more info",
                    Position = new Position(region.coordinates.x, region.coordinates.y)
                };

                _pin.InfoWindowClicked += async (s, args) =>
                {
                    MainPage.ScrollbarPage.LoadData((Pin)s);
                };

                viewMap.Pins.Add(_pin);
            }
        }
    }
}