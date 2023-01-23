using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseScheduleApp.Models;
using CourseScheduleApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseScheduleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseAdd : ContentPage
    {
        public CourseAdd()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var termList = await DatabaseService.GetTerm();
            AddCourseTerm.ItemsSource = (System.Collections.IList)termList;
        }

        async void SaveCourse_Clicked(object sender, EventArgs e)
        {
			Term t = (Term)AddCourseTerm.SelectedItem;

			if (/*!validEmail(AddInstEmail.Text) && */!IsValidPhoneNumber(AddInstPhone.Text))
			{
				await DisplayAlert("Error!", "Enter a valid phone number.", "Ok");
				return;
			}
			else if (AddCourseTerm.SelectedItem == null)
			{
				await DisplayAlert("Error!", "Please select a term.", "Ok");
				return;
			}
			else if (AddCourseStatus.SelectedItem == null)
			{
				await DisplayAlert("Error!", "Please select a course status.", "Ok");
				return;
			}
			else if (AddCourseStart.Date > AddCourseEnd.Date)
			{
				await DisplayAlert("Error!", "Start date cannot be greater than end date", "Ok");

				return;
			}
			else
			{
				// can use sms instead of email
				//if (CourseNotes.Text != null)
				//{
				//	var answer = await DisplayAlert("Please Choose", "Would you like to send a Notification email?", "Yes", "No");
				//	if (answer)
				//	{
				//		await SendEmail("You Have Notes", CourseNotes.Text);
				//	}
				//}
				await DatabaseService.AddCourse(t.ID, AddCourseName.Text, (string)AddCourseStatus.SelectedItem,
									AddCourseStart.Date, AddCourseEnd.Date, AddCourseInst.Text, AddInstEmail.Text, AddInstPhone.Text,
									CourseNotes.Text, NotificationAdd.IsToggled, NotifyEnd.IsToggled);
				await Navigation.PopAsync();
			}
		}
		public static bool IsValidPhoneNumber(string phoneNumber)
		{
			if (string.IsNullOrWhiteSpace(phoneNumber))
			{
				return false;
			}
			var pattern = @"^[\+]?[{1}]?[(]?[2-9]\d{2}[)]?[-\s\.]?[2-9]\d{2}[-\s\.]?[0-9]{4}$";
			var options = System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase;
			return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, pattern, options);
		}

		async void CancelCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public void AddCourseStart_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        public void AddCourseEnd_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}