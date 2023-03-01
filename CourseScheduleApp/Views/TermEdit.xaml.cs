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
    public partial class TermEdit : ContentPage
    {
        public TermEdit()
        {
            InitializeComponent();
        }
        public TermEdit(Term selectedTerm)
        {
            InitializeComponent();

            TermId.Text = selectedTerm.ID.ToString();
            TermName.Text = selectedTerm.TermName.ToString();
            TermStart.Date = selectedTerm.StartDate.Date;
            TermEnd.Date = selectedTerm.EndDate.Date;
        }

        async void SaveTerm_Clicked(object sender, EventArgs e)
        {
            if (TermName.Text == "")
            {
                await DisplayAlert("Error!", "Term name cannot be empty", "Ok");

                return;
            }
            if (TermStart.Date >= TermEnd.Date)
            {
                await DisplayAlert("Error!", "Start date cannot be later than end date", "Ok");

                return;
            }
            else
            {
                await DatabaseService.UpdateTerm(Int32.Parse(TermId.Text), TermName.Text,
                                            DateTime.Parse(TermStart.Date.ToString()),
                                            DateTime.Parse(TermEnd.Date.ToString()));

                await Navigation.PopAsync();
            }
        }

        async void CancelTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            var id = int.Parse(TermId.Text);

            var confirmDelete = await DisplayAlert("Confirm", "Are you sure you want to delete this term?", "Ok", "Cancel");

            if (confirmDelete)
            {
                await DatabaseService.RemoveTerm(id);
                await Navigation.PopAsync();
            }
            else
            {
                return;
            }
        }

        async void CheckCourse_Clicked(object sender, EventArgs e)
        {
            var termID = TermId.Text;

            var courses = await DatabaseService.GetCourse(int.Parse(termID));

            string courseName = string.Join(", ", courses.Select(c => c.Name));


            await DisplayAlert("Courses", courseName, "Ok");
        }
        private void TermStart_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void TermEnd_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}