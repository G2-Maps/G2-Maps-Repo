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
        }
    }
}
