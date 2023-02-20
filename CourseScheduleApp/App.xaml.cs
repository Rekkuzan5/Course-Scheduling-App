using CourseScheduleApp.Services;
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
            MainPage = new Dashboard();
        }

        protected override void OnStart()
        {
            // ** Have to run this method in the OnStart method of the application because OnAppearing does not work with Shell types in Xaml** //
            DatabaseService.LoadSampleData();
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
