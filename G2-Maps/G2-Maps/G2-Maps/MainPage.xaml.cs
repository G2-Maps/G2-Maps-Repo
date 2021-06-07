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
        public MainPage()
        {
            InitializeComponent();

            ShowItaly();

            DataManagement.CheckUpdates();
            DataManagement.GetDataFile_Async();
        }

        private void ShowItaly()
        {
            Position positionItaly = new Position(42.97197087627496, 12.553520745277323);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(positionItaly, Distance.FromKilometers(500));
            viewMap.MoveToRegion(mapSpan);

            Pin pin = new Pin
            {
                Label = "Italia",
                Address = "Click for more info",
                Position = positionItaly,
                Type = PinType.SavedPin
            };

            pin.InfoWindowClicked += async (s, args) =>
            {
                string pinName = ((Pin)s).Label;
                await DisplayAlert("Info Window Clicked", $"Info of {pinName}.", "Ok");
            };

            viewMap.Pins.Add(pin);
        }

        public async void LoadData(Pin s)
        {
            var _ = s.Label == "Friuli-Venezia-Giulia" ? "Friuli-Venezia Giulia"
                : s.Label == "Emilia Romagna" ? "Emilia-Romagna" : s.Label;
            var data = DataManagement.GetData_Regione(_);

            var fornitori = data.fornitoreTotal;

            String dati = $@"
Ultimo Aggiornamento :      {data.ultimo_aggiornamento.ToShortDateString()}

### Ultima Consegna ###

    Data Consegna : {data.consegneLatest.data_consegna.ToShortDateString()}
    Numero Dosi : {data.consegneLatest.numero_dosi.ToString()}
    Fornitore : {data.consegneLatest.fornitore}


### Consegne Totali ###

    Numero Dosi : {data.dosi_Total.ToString()}
    Dosi AstraZeneca : {fornitori["Vaxzevria (AstraZeneca)"].ToString()}
    Dosi Moderna : {fornitori["Moderna"].ToString()}
    Dosi Pfizer : {0 /*fornitori["Pfizer"].ToString()*/}
    Dosi Jonson&Jonson : {fornitori["Janssen"].ToString()}
";

            await DisplayAlert(s.Label, dati, "OK");
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
                    LoadData((Pin)s);
                };

                viewMap.Pins.Add(_pin);
            }
        }
    }
}
