using System;
using System.IO;
using System.Text;

using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace G2_Maps.Model
{
    class DataManagement
    {
        private static Consegne_Vaccini_Latest Consegne_Vaccini_Latest;
        private static String last_update = "";
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
                String file = "last-update-dataset.json";
                String fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);


                String url = GitHubUrl + file;


                last_update = Internet(url);

                String local_result = !File.Exists(fileName) ? String.Empty : File.ReadAllText(fileName);

                if (!last_update.Equals(local_result))
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

        public static String Internet(String url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            var _ = reader.ReadToEnd();
            return _;
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

            String _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Regions.json");

            if (!File.Exists(_fileName))
            {
                await App.MyMainPage.DisplayAlert("Errore nei dati", "Dati locali non disponibili", "Chiudi l'applicazione");
                throw new Exception();
            }



            String result;

            result = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Files[0]));
            Consegne_Vaccini_Latest = JsonConvert.DeserializeObject<Consegne_Vaccini_Latest>(result);

            result = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Files[1]));
            var punti_somministrazione_latest = JsonConvert.DeserializeObject<Punti_Somministrazione_Latest>(result);

            result = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Regions.json"));
            var regions = JsonConvert.DeserializeObject<Regions>(result);

            MainPage.DataPage.DisplayPins(regions);
        }

        public static SummaryData GetData_Regione(String name) 
        {
            var b = Consegne_Vaccini_Latest.data;
            var consegneVaccini = b.Where(p => p.nome_area.Equals(name))
                                    .OrderBy(c => c.data_consegna)
                                    .Select(f => new ShortData
                                    { 
                                        data_consegna = f.data_consegna, 
                                        fornitore = f.fornitore, 
                                        numero_dosi = f.numero_dosi
                                    }).ToList();

            var consegneLatest = consegneVaccini.Last();

            //Fornitore, dosi
            Dictionary<String, int> fornitoreTotal = new Dictionary<string, int>();

            foreach (var item in consegneVaccini.GroupBy(c => c.fornitore).ToList())
            {
                fornitoreTotal.Add(item.Key, item.Sum(c => c.numero_dosi));
            }

            int numero_dosiTotal = fornitoreTotal.Values.Sum();

            var a = new SummaryData
            {
                ultimo_aggiornamento = JsonConvert.DeserializeObject<LastUpdate>(last_update).ultimo_aggiornamento,
                dosi_Total = numero_dosiTotal,
                fornitoreTotal = fornitoreTotal,
                consegneLatest = consegneLatest,
                consegneVaccini = consegneVaccini
            };

            return a;
        }

        private static async void GetDataOnline_Async()
        {
            try
            {
                foreach (String file in Files)
                {
                    String fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
                    String url = GitHubUrl + file;
                    String result = Internet(url);
                    File.WriteAllText(fileName, result);
                }

                String _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Regions.json");
                String _url = "https://raw.githubusercontent.com/G2-Maps/G2-Maps-Repo/main/Regions.json";
                String _result = Internet(_url);
                File.WriteAllText(_fileName, _result);

            }
            catch (Exception)
            {
                await App.MyMainPage.DisplayAlert("Errore di connessione", "Nessuna connessione internet, utilizzo dei dati locali in corso...\n" +
                    "C'e` la probabilita` che i dati non siano aggiornati", "Ok");
            }
            
        }
    }
}
