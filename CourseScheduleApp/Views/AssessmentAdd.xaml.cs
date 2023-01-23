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
    public partial class AssessmentAdd : ContentPage
    {
        public AssessmentAdd()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var assessmentList = await DatabaseService.GetCourses();
            AssessTermAdd.ItemsSource = (System.Collections.IList)assessmentList;
        }

        async void SaveAssessment_Clicked(object sender, EventArgs e)
        {
            Course c = (Course)AssessTermAdd.SelectedItem;

            if (TypeAssess.SelectedItem == null)
            {
                await DisplayAlert("Error!", "Please select an assessment type.", "Ok");
                return;
            }
            else if (AssessTermAdd.SelectedItem == null)
            {
                await DisplayAlert("Error!", "Please select a course.", "Ok");
                return;
            }

            var getList = await DatabaseService.GetAssessment(c.ID);
            int o = 0;
            int p = 0;

            foreach (Assessment a in getList)
            {
                if (a.Type == "Objective Assessment")
                {
                    o++;
                }
                if (a.Type == "Performance Assessment")
                {
                    p++;
                }
            }

            if ((string)TypeAssess.SelectedItem == "Objective Assessment" && o > 0)
            {
                await DisplayAlert("Error!", "You may only have one Objective Assessment for this course.", "Ok");
                return;
            }
            if ((string)TypeAssess.SelectedItem == "Performance Assessment" && p > 0)
            {
                await DisplayAlert("Error!", "You may only have one Performance Assessment for this course.", "Ok");
                return;
            }

            else
            {
                await DatabaseService.AddAssessment(c.ID, (string)TypeAssess.SelectedItem,
                                        AddDueDate.Date, NotificationAdd.IsToggled);
                await Navigation.PopAsync();
            }

        }

        async void CancelAssessment_Clicked(object sender, EventArgs e)
        {
                await Navigation.PopAsync();

        }
        private void AddDueDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}