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
        private static String[] Files = 
            {
                "consegne-vaccini-latest.json",
                "punti-somministrazione-latest.json"
            };

        private static String GitHubUrl = "https://raw.githubusercontent.com/italia/covid19-opendata-vaccini/master/dati/";


        public static async void CheckUpdates()
        {
            try
            {


                HttpClient httpClient = new HttpClient();
                String file = "last-update-dataset.json";
                String fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);


                String url = GitHubUrl + file;

                String web_result = await httpClient.GetStringAsync(url);
                String local_result = !File.Exists(fileName) ? String.Empty : File.ReadAllText(fileName);

                if (!web_result.Equals(local_result))
                {
                    GetDataOnline_Async();
                }

            }
            catch (Exception)
            {
                await App.MyMainPage.DisplayAlert("Errore di connessione", "Nessuna connessione internet, utilizzo dei dati locali in corso...\n" +
                    "C'e` la probabilita` che i dati non siano aggiornati", "Ok");
            }

        }

        public static async void GetDataFile_Async()
        {
            foreach (String file in Files)
            {
                String fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);

                if (!File.Exists(fileName))
                {
                    await App.MyMainPage.DisplayAlert("Errore nei dati", "Dati locali non disponibili", "Chiudi l'applicazione");
                    throw new Exception();
                }
            }

            String result;

            result = File.ReadAllText(Files[0]);
            var consegne_vaccini_latest = JsonConvert.DeserializeObject<Consegne_Vaccini_Latest>(result);

            result = File.ReadAllText(Files[1]);
            var punti_somministrazione_latest = JsonConvert.DeserializeObject<Punti_Somministrazione_Latest>(result);
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

        private static async void GetDataOnline_Async()
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                foreach (String file in Files)
                {
                    String fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
                    String url = GitHubUrl + file;
                    String result = await httpClient.GetStringAsync(url);
                    File.WriteAllText(fileName, result);
                }
            }
            catch (Exception)
            {
                await App.MyMainPage.DisplayAlert("Errore di connessione", "Nessuna connessione internet, utilizzo dei dati locali in corso...\n" +
                    "C'e` la probabilita` che i dati non siano aggiornati", "Ok");
            }
            
        }
    }
}
