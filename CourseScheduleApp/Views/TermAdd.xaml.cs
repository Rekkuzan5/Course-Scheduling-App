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
    public partial class TermAdd : ContentPage
    {
        public TermAdd()
        {
            InitializeComponent();
        }

		async void SaveTerm_Clicked(object sender, EventArgs e)
		{
			if (TermName.Text == null)
			{
				await DisplayAlert("Error!", "You must create a Term name", "Ok");

				return;
			}
			if (TermStartDate.Date >= TermEndDate.Date)
			{
				await DisplayAlert("Error!", "Start date cannot be greater than end date", "Ok");

				return;
			}
			else
			{
				await DatabaseService.AddTerm(TermName.Text, TermStartDate.Date, TermEndDate.Date);
				await Navigation.PopAsync();
			}
		}

		async void CancelTerm_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		private void TermStartDate_DateSelected(object sender, DateChangedEventArgs e)
		{

		}

		private void TermEndDate_DateSelected(object sender, DateChangedEventArgs e)
		{

		}
	}
}