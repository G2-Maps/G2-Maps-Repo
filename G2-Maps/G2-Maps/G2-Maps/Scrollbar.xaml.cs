using System;
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
    public partial class Scrollbar : ContentPage
    {
        public Scrollbar()
        {
            InitializeComponent();
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

            await App.MyMainPage.DisplayAlert(s.Label, dati, "OK");
        }
    }
}