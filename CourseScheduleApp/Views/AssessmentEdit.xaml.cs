using CourseScheduleApp.Models;
using CourseScheduleApp.Services;
using SQLite;
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
            AssessmentName.Text = selectedAssessment.AssessmentName;
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

        async void SaveEdit_Clicked(object sender, EventArgs e)
        {
            Course c = (Course)CourseSelect.SelectedItem;

            if (AssessmentType.SelectedItem == null)
            {
                await DisplayAlert("Error!", "Please select an assessment type.", "Ok");
                return;
            }
            else if (AssessmentName.Text == null)
            {
                await DisplayAlert("Error!", "Please enter a name for assessment.", "Ok");
                return;
            }
            else if (CourseSelect.SelectedItem == null)
            {
                await DisplayAlert("Error!", "Please select a course.", "Ok");
                return;
            }

            var getlist = await DatabaseService.GetAssessment(c.ID);
            //int o = 0;
            //int p = 0;

            //foreach (Assessment a in getList)
            //{
            //    // Validates Assessment IDs and ignores if they're the same or else you can 
            //    // save duplicate assessments
            //    if (a.ID != a.ID)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        if (a.Type == "Objective Assessment")
            //        {
            //            o++;
            //        }
            //        if (a.Type == "Performance Assessment")
            //        {
            //            p++;
            //        }
            //    }

            //    if (AssessmentType.SelectedItem.ToString() == "Objective Assessment" && o == 0)
            //    {
            //        await DisplayAlert("Error!", "You may only have one Objective Assessment for this course.", "Ok");
            //        return;
            //    }
            //    if (AssessmentType.SelectedItem.ToString() == "Performance Assessment" && p == 0)
            //    {
            //        await DisplayAlert("Error!", "You may only have one Performance Assessment for this course.", "Ok");
            //        return;
            //    }


            using (SQLiteConnection con = new SQLiteConnection(Services.DatabaseService.dbPath))
            {
                var objectiveCount = con.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseID = '{c.ID}' AND Type = 'Objective Assessment'");
                var performanceCount = con.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseID = '{c.ID}' AND Type = 'Performance Assessment'");
                if (selectedAssessment.Type.ToString() == "Objective Assessment" && objectiveCount.Count == 0)
                {
                    selectedAssessment.Type = AssessmentType.SelectedItem.ToString();
                    await DatabaseService.UpdateAssessment(Int32.Parse(AssessmentId.Text), c.ID, selectedAssessment.Type, AssessmentName.Text,
                                                DueDate.Date, NotifyEdit.IsToggled);
                    await Navigation.PopAsync();
                }
                else if (selectedAssessment.Type.ToString() == "Performance Assessment" && objectiveCount.Count == 0)
                {
                    selectedAssessment.Type = AssessmentType.SelectedItem.ToString();
                    await DatabaseService.UpdateAssessment(Int32.Parse(AssessmentId.Text), c.ID, selectedAssessment.Type, AssessmentName.Text,
                                           DueDate.Date, NotifyEdit.IsToggled);
                await Navigation.PopAsync();
                }
                else if (selectedAssessment.Type.ToString() == "Performance Assessment" && performanceCount.Count == 1 ||
                         selectedAssessment.Type.ToString() == "Objective Assessment" && objectiveCount.Count == 1)
                {
                    await DisplayAlert("Error!", "You may only have one Objective Assessment for this course.", "Ok");
                }
            }
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