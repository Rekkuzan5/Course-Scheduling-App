using CourseScheduleApp.Models;
using CourseScheduleApp.Services;
using CourseScheduleApp.Views;
using Plugin.LocalNotifications;
using SQLite;
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
    public partial class Dashboard : Shell
    {
        public List<Term> TermList = new List<Term>();

        public Dashboard()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //DatabaseService.ClearSampleData();
            //await DatabaseService.LoadSampleData();

            var courseList = await DatabaseService.GetCourses();
            var assessmentList = await DatabaseService.GetAssessments();

            //var notifyRandom = new Random();
            //var notifyId = notifyRandom.Next(1000);

            //foreach (Course listedCourse in courseList)
            //{
            //    if (listedCourse.NotificationStart == true || listedCourse.NotificationEnd == true)
            //    {
            //        if (listedCourse.Start == DateTime.Today)
            //        {
            //            CrossLocalNotifications.Current.Show("Notice", $"{listedCourse.Name} begins today.", notifyId);
            //        }
            //        if (listedCourse.End == DateTime.Today)
            //        {
            //            CrossLocalNotifications.Current.Show("Notice", $"{listedCourse.Name} ends today.", notifyId);
            //        }
            //    }
            //}

            //var courseList = await DatabaseService.GetCourses();
            //var assessmentList = await DatabaseService.GetAssessments();

            foreach (var course in courseList)
            {
                if (course.NotificationStart && course.Start == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course Update!", course.Name.ToString() + " begins today!", 100);
                }
                if (course.NotificationEnd && course.End == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course Update!", course.Name.ToString() + " ends today!", 101);
                }
            }

            foreach (var assessment in assessmentList)
            {
                if (assessment.Notification && assessment.DueDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Update!", assessment.AssessmentName.ToString() + " is due today!", 100);
                }
            }
        }

        async void ViewTerms_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermPage());
        }

        async void Courses_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursePage());
        }

        async void Assessments_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentPage());
        }
        async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }
    }
}