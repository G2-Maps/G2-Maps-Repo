using System;
using System.Collections.Generic;
using System.Text;
using G2_Maps;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;

namespace G2_Maps.Model
{
    class DataManagement
    {
        public static async Task GetFromInternet_Async()
        {
            HttpClient httpClient = new HttpClient();

            String url = "https://raw.githubusercontent.com/italia/covid19-opendata-vaccini/master/dati/punti-somministrazione-latest.json";
            String result = await httpClient.GetStringAsync(url);
            var punti_somministrazione_latest = JsonConvert.DeserializeObject<Punti_Somministrazione_Latest>(result);

            url = "https://raw.githubusercontent.com/italia/covid19-opendata-vaccini/master/dati/consegne-vaccini-latest.json";
            result = await httpClient.GetStringAsync(url);
            var consegne_vaccini_latest = JsonConvert.DeserializeObject<Consegne_Vaccini_Latest>(result);

            var print = punti_somministrazione_latest.data.Where(x => x.comune == "MONZA").ToList().First();
            App.MyMainPage.Alert($"{print.comune}, {print.provincia}, {print.nome_area}");
        }

        private static void GetData_Provincia(Punti_Somministrazione_Latest punti_Somministrazione_Latest, Consegne_Vaccini_Latest consegne_Vaccini_Latest, String name)
        {
            
        }
    }
}
