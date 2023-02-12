using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseScheduleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //var dashboard = new Dashboard();
            //var navPage = new NavigationPage(dashboard);
            //MainPage = navPage;

            MainPage = new Dashboard();
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
