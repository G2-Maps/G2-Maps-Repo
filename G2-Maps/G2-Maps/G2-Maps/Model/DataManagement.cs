using System;
using System.IO;
using System.Text;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;

using G2_Maps;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace G2_Maps.Model
{
    class DataManagement
    {
        public static async Task GetFromInternet_Async()
        {
            HttpClient httpClient = new HttpClient();

            //https://github.com/italia/covid19-opendata-vaccini/blob/master/dati/last-update-dataset.json

            String url = "https://raw.githubusercontent.com/italia/covid19-opendata-vaccini/master/dati/punti-somministrazione-latest.json";
            String result = await httpClient.GetStringAsync(url);
            var punti_somministrazione_latest = JsonConvert.DeserializeObject<Punti_Somministrazione_Latest>(result);

            url = "https://raw.githubusercontent.com/italia/covid19-opendata-vaccini/master/dati/consegne-vaccini-latest.json";
            result = await httpClient.GetStringAsync(url);
            var consegne_vaccini_latest = JsonConvert.DeserializeObject<Consegne_Vaccini_Latest>(result);

            var print = punti_somministrazione_latest.data.Where(x => x.comune == "MONZA").ToList().First();
            App.MyMainPage.Alert($"{print.comune}, {print.provincia}, {print.nome_area}");
        }

        private static void GetData_Regione(Punti_Somministrazione_Latest punti_Somministrazione_Latest, Consegne_Vaccini_Latest consegne_Vaccini_Latest, String name)
        {
            name = name.ToUpper();

            var puntiSomministrazione = punti_Somministrazione_Latest.data.Where(p => p.nome_area.ToUpper().Equals(name));

            var consegneVaccini = consegne_Vaccini_Latest.data.Where(p => p.nome_area.ToUpper().Equals(name))
                                    .OrderBy(c => c.data_consegna)
                                    .Select(f => new 
                                    { 
                                        data_consegna = f.data_consegna, 
                                        fornitore = f.fornitore, 
                                        numero_dosi = f.numero_dosi
                                    });

            var consegneLatest = consegneVaccini.Last();

            //Fornitore, dosi
            Dictionary<String, int> fornitoreTotal = new Dictionary<string, int>();

            foreach (var item in consegneVaccini.GroupBy(c => c.fornitore).ToList())
            {
                fornitoreTotal.Add(item.Key, item.Sum(c => c.numero_dosi));
            }

            int numero_dosiTotal = fornitoreTotal.Values.Sum();
        }

        private static async void DataSaving()
        {

        }
    }
}
