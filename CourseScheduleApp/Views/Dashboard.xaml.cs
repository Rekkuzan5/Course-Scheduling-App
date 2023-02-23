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

            //** Does NOT work with Tabbed navigation using Shell **//
            await DatabaseService.LoadSampleData();

            var courseList = await DatabaseService.GetCourses();
            var assessmentList = await DatabaseService.GetAssessments();
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
            await Navigation.PushAsync(new Welcome());
        }
    }
}