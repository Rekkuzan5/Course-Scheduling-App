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
    public partial class CoursePage : ContentPage
    {
        public CoursePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CourseCollectionView.ItemsSource = await DatabaseService.GetCourses();
        }

        async void AddCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CourseAdd());
        }

        async void CourseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Course course = (Course)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new CourseEdit(course));
            }
        }
    }
    // need to add more to class
}