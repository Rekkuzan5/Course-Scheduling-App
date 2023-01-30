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
    public partial class CourseEdit : ContentPage
    {
        Course myCourse { get; set; }
        public CourseEdit(Course selectedCourse)
        {
            InitializeComponent();

            myCourse = selectedCourse;
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			var termList = await DatabaseService.GetTerm();
			TermSelect.ItemsSource = (System.Collections.IList)termList;

			CourseId.Text = myCourse.ID.ToString();
			TermSelect.Title = "Term Select";
			CourseName.Text = myCourse.Name.ToString();
			CourseStatus.SelectedItem = myCourse.Status;
			CourseStart.Date = myCourse.Start.Date;
			CourseEnd.Date = myCourse.End.Date;
			InstructorName.Text = myCourse.InstructorName.ToString();
			InstructorEmail.Text = myCourse.InstructorEmail.ToString();
			InstructorPhone.Text = myCourse.InstructorPhone.ToString();
			EditNotes.Text = myCourse.Notes;
			NotificationEdit.IsToggled = myCourse.NotificationStart;
			NotificationEnd.IsToggled = myCourse.NotificationEnd;

			foreach (var term in termList)
			{
				if (term.ID == myCourse.TermID)
				{
					TermSelect.SelectedItem = term;
					break;
				}
			}
		}

        async void SaveCourse_Clicked(object sender, EventArgs e)
        {
			Term t = (Term)TermSelect.SelectedItem;

			if (/*!validEmail(InstructorEmail.Text) && */!IsValidPhoneNumber(InstructorPhone.Text))
			{
				await DisplayAlert("Error!", "Enter a valid phone number.", "Ok");
				return;
			}
			else if (TermSelect.SelectedItem == null)
			{
				await DisplayAlert("Error!", "Please select a term.", "Ok");
				return;
			}
			else if (CourseStatus.SelectedItem == null)
			{
				await DisplayAlert("Error!", "Please select a course status.", "Ok");
				return;
			}
			else if (CourseStart.Date > CourseEnd.Date)
			{
				await DisplayAlert("Error!", "Start date cannot be greater than end date", "Ok");

				return;
			}
			else
			{
				//if (EditNotes.Text != null)
				//{
				//	var answer = await DisplayAlert("Please Choose", "Would you like to send a Notification email?", "Yes", "No");
				//	if (answer)
				//	{
				//		await SendEmail("You Have Notes", EditNotes.Text);
				//	}
				//}
				await DatabaseService.UpdateCourse(Int32.Parse(CourseId.Text), t.ID, CourseName.Text, CourseStatus.SelectedItem.ToString(),
									DateTime.Parse(CourseStart.Date.ToString()), DateTime.Parse(CourseEnd.Date.ToString()),
									InstructorName.Text, InstructorEmail.Text, InstructorPhone.Text, EditNotes.Text,
									NotificationEdit.IsToggled, NotificationEnd.IsToggled);

				await Navigation.PopAsync();
			}
		}

        async void CancelCourse_Clicked(object sender, EventArgs e)
        {
			await Navigation.PopAsync();
		}

        async void DeleteCourse_Clicked(object sender, EventArgs e)
        {
			var id = int.Parse(CourseId.Text);

			var confirmDelete = await DisplayAlert("Confirm", "Are you sure you wnat to delete this record?", "Ok", "Cancel");

			if (confirmDelete)
			{
				await DatabaseService.RemoveCourse(id);
				await Navigation.PopAsync();
			}
			else
			{
				return;
			}
		}

		//public bool validEmail(string address)
		//{
		//	EmailAddressAttribute e = new EmailAddressAttribute();
		//	if (e.IsValid(address))
		//		return true;
		//	else
		//		DisplayAlert("Error!", "Enter a valid email address.", "Ok");
		//	return false;
		//}

		// Validates that a good phone number is inputted
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
		//public async Task SendEmail(string subject, string body)
		//{
		//	try
		//	{
		//		var message = new EmailMessage
		//		{
		//			Subject = subject,
		//			Body = body,
		//			//To = recipients,

		//		};
		//		await Email.ComposeAsync(message);
		//	}
		//	catch (FeatureNotSupportedException fbsEx)
		//	{
		//		// Email is not supported on this device
		//	}
		//	catch (Exception ex)
		//	{
		//		// Some other exception occurred
		//	}
		//}

		public async void CheckAss_Clicked(object sender, EventArgs e)
		{
            var courseID = CourseId.Text;

            var assess = await DatabaseService.GetAssessment(int.Parse(courseID));

            string assessmentName = string.Join(", ", assess.Select(c => c.AssessmentName));


            await DisplayAlert("Assessments", assessmentName, "Ok");
        }

		private void CourseStart_DateSelected(object sender, DateChangedEventArgs e)
		{

		}

		private void CourseEnd_DateSelected(object sender, DateChangedEventArgs e)
		{

		}
	}
}
