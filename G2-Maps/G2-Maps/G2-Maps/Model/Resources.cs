using System;
using System.Collections.Generic;
using System.Text;
using G2_Maps;

namespace G2_Maps.Model
{
    public class LastUpdate
    {
        public DateTime ultimo_aggiornamento { get; set; }
    }

    public class Punti_Somministrazione_Latest
    {
        public Schema schema { get; set; }
        public Datum[] data { get; set; }

        public class Schema
        {
            public Field[] fields { get; set; }
            public string[] primaryKey { get; set; }
            public string pandas_version { get; set; }
        }

        public class Field
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public class Datum
        {
            public int index { get; set; }
            public string area { get; set; }
            public string provincia { get; set; }
            public string comune { get; set; }
            public string presidio_ospedaliero { get; set; }
            public string codice_NUTS1 { get; set; }
            public string codice_NUTS2 { get; set; }
            public int codice_regione_ISTAT { get; set; }
            public string nome_area { get; set; }
        }
    }

    public class Consegne_Vaccini_Latest
    {
        public Schema schema { get; set; }
        public Datum[] data { get; set; }

        public class Schema
        {
            public Field[] fields { get; set; }
            public string[] primaryKey { get; set; }
            public string pandas_version { get; set; }
        }

        public class Field
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public class Datum
        {
            public int index { get; set; }
            public string area { get; set; }
            public string fornitore { get; set; }
            public int numero_dosi { get; set; }
            public DateTime data_consegna { get; set; }
            public string codice_NUTS1 { get; set; }
            public string codice_NUTS2 { get; set; }
            public int codice_regione_ISTAT { get; set; }
            public string nome_area { get; set; }
        }
    }

    /*
        ultima consegna:
        -fornitore
        -dosi
        -data

        consegna tot:
        -n consegne tot
        -consegne tot x fornitore
        -grafico x=data,y=consegne,colore=fornitore (grafico a punti)
     */


    public class Regions
    {
        public Region[] data { get; set; }
    }

    public class Region
    {
        public string name { get; set; }
        public Coordinates coordinates { get; set; }
    }

    public class Coordinates
    {
        public float x { get; set; }
        public float y { get; set; }
    }


    public class SummaryData
    {
        public int dosi_Total { get; set; }
        public Dictionary<String, int> fornitoreTotal { get; set; }
        public ShortData consegneLatest {get; set;}
        public IEnumerable<ShortData> consegneVaccini { get; set; }
    }

    public class ShortData
    {
        public DateTime data_consegna { get; set; }
        public string fornitore { get; set; }
        public int numero_dosi { get; set; }
    }

}
