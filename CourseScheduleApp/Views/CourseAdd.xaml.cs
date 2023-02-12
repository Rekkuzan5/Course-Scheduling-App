using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CourseScheduleApp.Models;
using CourseScheduleApp.Services;
using Xamarin.Essentials;
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

			if (AddCourseTerm.SelectedItem == null)
			{
				await DisplayAlert("Error!", "Please select a term.", "Ok");
				return;
			}
			else if (AddCourseName.Text == null)
			{
				await DisplayAlert("Error!", "Please enter name for course.", "Ok");
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
			else if (AddCourseInst.Text == null)
			{
				await DisplayAlert("Error!", "Course must have an Instructor assigned.", "Ok");

				return;
			}
			else if (!ValidateEmail(AddInstEmail.Text) || !IsValidPhoneNumber(AddInstPhone.Text))
			{
				await DisplayAlert("Error!", "Enter a valid e-mail/phone number.", "Ok");
				return;
			}
			else
			{
				 //can use sms instead of email
				if (CourseNotes.Text != null)
				{
					var answer = await DisplayAlert("Please Choose", "Would you like to send a Notification email?", "Yes", "No");
					if (answer)
					{
						await SendEmail("Course Notes", CourseNotes.Text, AddInstEmail.Text);
					}
				}
				await DatabaseService.AddCourse(t.ID, AddCourseName.Text, (string)AddCourseStatus.SelectedItem,
									AddCourseStart.Date, AddCourseEnd.Date, AddCourseInst.Text, AddInstEmail.Text, AddInstPhone.Text,
									CourseNotes.Text, NotificationAdd.IsToggled, NotifyEnd.IsToggled);
				await Navigation.PopAsync();
			}
		}

		public async Task SendEmail(string subject, string body, string recipient)
		{
			try
			{
				await Email.ComposeAsync("Course Notes", CourseNotes.Text, AddInstEmail.Text);
			}
			catch (FeatureNotSupportedException)
			{
				// Email is not supported on this device
				await DisplayAlert("Error!", "Email is not supported on this device.", "Ok");
			}
			catch (Exception)
			{
				// Some other exception occurred
				await DisplayAlert("Error!", "Unable to process request.", "Ok");
			}
		}

		private bool ValidateEmail(string email)
		{
			try
			{
				MailAddress m = new MailAddress(email);

				return true;
			}
			catch (Exception)
			{
				//DisplayAlert("Error!", "Enter a valid email address.", "Ok");
				return false;
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