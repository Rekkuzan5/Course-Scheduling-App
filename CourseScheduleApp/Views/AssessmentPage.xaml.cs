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
    public partial class AssessmentPage : ContentPage
    {
        public AssessmentPage()
        {
            InitializeComponent();
        }
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			AssessCollectionView.ItemsSource = await DatabaseService.GetAssessments();
		}

		async void AddAssessment_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AssessmentAdd());
		}

		async void AssessCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				Assessment assess = (Assessment)e.CurrentSelection.FirstOrDefault();
				//await Navigation.PushAsync(new AssessmentEdit(assess));
			}
		}
	}
}