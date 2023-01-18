using CourseScheduleApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseScheduleApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        async void AddGadget_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GadgetAdd());
        }

        async void viewGadgets_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GadgetList());
        }

        async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }
    }
}