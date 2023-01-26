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
    public partial class AssessmentEdit : ContentPage
    {
        Assessment selectedAssessment { get; set; }
        public AssessmentEdit(Assessment assessment)
        {
            InitializeComponent();
            selectedAssessment = assessment;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var courseList = await DatabaseService.GetCourses();

            CourseSelect.ItemsSource = (System.Collections.IList)courseList;

            AssessmentId.Text = selectedAssessment.ID.ToString();
            AssessmentType.SelectedItem = selectedAssessment.Type;
            CourseSelect.Title = "Course Select";
            DueDate.Date = selectedAssessment.DueDate.Date;
            NotifyEdit.IsToggled = selectedAssessment.Notification;

            foreach (var course in courseList)
            {
                if (course.ID == selectedAssessment.CourseID)
                {
                    CourseSelect.SelectedItem = course;
                    break;
                }
            }
        }

        private void SaveEdit_Clicked(object sender, EventArgs e)
        {

        }

        async void CancelEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void DeleteAssessment_Clicked(object sender, EventArgs e)
        {
            var id = int.Parse(AssessmentId.Text);

            var confirmDelete = await DisplayAlert("Confirm", "Are you sure you want to delete this assessment?", "Ok", "Cancel");

            if (confirmDelete)
            {
                await DatabaseService.RemoveAssessment(id);
                await Navigation.PopAsync();
            }
            else
            {
                return;
            }
        }
        private void DueDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}