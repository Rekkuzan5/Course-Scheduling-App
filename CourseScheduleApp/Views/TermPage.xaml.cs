using CourseScheduleApp.Models;
using CourseScheduleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseScheduleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        public TermPage()
        {
            InitializeComponent();
        }
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			TermCollectionView.ItemsSource = await DatabaseService.GetTerm();
		}

		async void AddTerm_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new TermAdd());
		}

		async void TermCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            if (e.CurrentSelection != null)
            {
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new TermEdit(term));
            }
        }
	}
}