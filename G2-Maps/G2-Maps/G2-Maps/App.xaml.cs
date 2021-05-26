using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace G2_Maps
{
    public partial class App : Application
    {
        public static MainPage MyMainPage { get; set; }

        public App()
        {
            InitializeComponent();

            MyMainPage = new MainPage();
            MainPage = MyMainPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
