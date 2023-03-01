using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Essentials;
using CourseScheduleApp.Models;
using Plugin.LocalNotifications;

namespace CourseScheduleApp.Services
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _db;
        private static SQLiteConnection _dbConnection;

        private static readonly string dbFileName = "wgu_c971.db3";
        private static readonly string dbFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string dbPath = Path.Combine(dbFolderPath, dbFileName);

        #region Start
        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            // Get path to database file
            //var databasePath = Path.Combine(FileSystem.AppDataDirectory, "wgu-C971.db");

            _db = new SQLiteAsyncConnection(dbPath);
            _dbConnection = new SQLiteConnection(dbPath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();

            // Notifications //
            var courseList = await GetCourses();
            var assessmentList = await GetAssessments();

            // The following code block adds id's for notifications, id's cannot be shared between notifications, so assessments start at a
            // much higher number than courses
            int c = 0;
            foreach (var course in courseList)
            {
                if (course.NotificationStart == true && course.Start == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course Update!", course.Name.ToString() + " begins today!", c++);
                }
                if (course.NotificationEnd == true && course.End == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course Update!", course.Name.ToString() + " ends today!", c++);
                }
            }

            int a = 1000;
            foreach (var assessment in assessmentList)
            {
                if (assessment.Type == "Objective Assessment" && assessment.NotificationStart == true && assessment.StartDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Update!", assessment.AssessmentName.ToString() + " begins today!", a++);
                }
                if (assessment.Type == "Objective Assessment" && assessment.NotificationEnd == true && assessment.DueDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Update!", assessment.AssessmentName.ToString() + " is due today!", a++);
                }
                if (assessment.Type == "Performance Assessment" && assessment.NotificationStart == true && assessment.StartDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Update!", assessment.AssessmentName.ToString() + " begins today!", a++);
                }
                if (assessment.Type == "Performance Assessment" && assessment.NotificationEnd == true && assessment.DueDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Update!", assessment.AssessmentName.ToString() + " is due today!", a++);
                }
            }
        }
        #endregion

        #region Terms

        public static async Task AddTerm(string name, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term()
            {
                TermName = name,
                StartDate = start,
                EndDate = end,
                Status = 0,
                CreationDate = DateTime.Now
            };

            await _db.InsertAsync(term);
            var ID = term.ID;
        }
        public static async Task RemoveTerm(int id)
        {
            await Init();

            await _db.DeleteAsync<Term>(id);
        }
        public static async Task<IEnumerable<Term>> GetTerm()
        {
            await Init();

            var terms = await _db.Table<Term>().ToListAsync();
            return terms;
        }
        public static async Task UpdateTerm(int id, string name, DateTime start, DateTime end)
        {
            await Init();

            var termQuery = await _db.Table<Term>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.TermName = name;
                termQuery.StartDate = start;
                termQuery.EndDate = end;

                await _db.UpdateAsync(termQuery);
            }
        }

        #endregion

        #region Course Methods

        public static async Task AddCourse(int termId, string courseName, string courseStatus,
                    DateTime courseStart, DateTime courseEnd, string instName, string instEmail,
                    string instPhone, string notes, bool notifiyStart, bool notifiyEnd)
        {
            await Init();
            var course = new Course
            {
                TermID = termId,
                Name = courseName,
                Status = courseStatus,
                Start = courseStart,
                End = courseEnd,
                InstructorName = instName,
                InstructorEmail = instEmail,
                InstructorPhone = instPhone,
                Notes = notes,
                NotificationStart = notifiyStart,
                NotificationEnd = notifiyEnd,
            };

            await _db.InsertAsync(course);

            var id = course.ID;
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            await _db.DeleteAsync<Course>(id);
        }

        public static async Task<IEnumerable<Course>> GetCourse(int termId)
        {
            await Init();

            var courses = await _db.Table<Course>().Where(i => i.TermID == termId).ToListAsync();

            return courses;
        }

        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();

            var courses = await _db.Table<Course>().ToListAsync();

            return courses;
        }

        public static async Task UpdateCourse(int id, int termId, string courseName, string courseStatus,
                    DateTime courseStart, DateTime courseEnd, string instName, string instEmail,
                    string instPhone, string notes, bool notifiyStart, bool notifiyEnd)
        {
            await Init();

            var courseQuery = await _db.Table<Course>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.TermID = termId;
                courseQuery.Name = courseName;
                courseQuery.Status = courseStatus;
                courseQuery.Start = courseStart;
                courseQuery.End = courseEnd;
                courseQuery.InstructorName = instName;
                courseQuery.InstructorEmail = instEmail;
                courseQuery.InstructorPhone = instPhone;
                courseQuery.Notes = notes;
                courseQuery.NotificationStart = notifiyStart;
                courseQuery.NotificationEnd = notifiyEnd;

                await _db.UpdateAsync(courseQuery);
            }
        }

        #endregion

        #region Assessment Methods
        public static async Task AddAssessment(int courseId, string asessType, string assessmentName, DateTime startDate, DateTime dueDate, bool startNotify, bool endNotify)
        {
            await Init();

            var assessment = new Assessment
            {
                CourseID = courseId,
                Type = asessType,
                AssessmentName = assessmentName,
                StartDate = startDate,
                DueDate = dueDate,
                NotificationStart = startNotify,
                NotificationEnd = endNotify,
            };

            await _db.InsertAsync(assessment);

            //var id = assessment.ID;
        }

        public static async Task RemoveAssessment(int id)
        {
            await Init();

            await _db.DeleteAsync<Assessment>(id);
        }

        public static async Task<IEnumerable<Assessment>> GetAssessment(int courseId)
        {
            await Init();

            var assessment = await _db.Table<Assessment>().Where(i => i.CourseID == courseId).ToListAsync();

            return assessment;
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments()
        {
            await Init();

            var assessments = await _db.Table<Assessment>().ToListAsync();

            return assessments;
        }

        public static async Task UpdateAssessment(int id, int courseId, string assessmentType, string assessmentName, DateTime startDate, DateTime dueDate, bool startNotify, bool endNotify)
        {
            await Init();

            var assessQuery = await _db.Table<Assessment>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (assessQuery != null)
            {
                assessQuery.CourseID = courseId;
                assessQuery.AssessmentName = assessmentName;
                assessQuery.Type = assessmentType;
                assessQuery.StartDate = startDate;
                assessQuery.DueDate = dueDate;
                assessQuery.NotificationStart = startNotify;
                assessQuery.NotificationEnd = endNotify;

                await _db.UpdateAsync(assessQuery);
            }
        }

        #endregion

        #region Demo Data
        public static async Task LoadSampleData()
        {
            await Init();

            var terms = await _db.Table<Term>().ToListAsync();
            var courses = await _db.Table<Course>().ToListAsync();
            var assessments = await _db.Table<Assessment>().ToListAsync();


            if (terms.Count > 0 || courses.Count > 0 || assessments.Count > 0)
            {
                return;
            }

            Term demoTerm = new Term
            {
                TermName = "Demo Intro Term",
                StartDate = new DateTime(2023, 01, 01),
                EndDate = new DateTime(2023, 05, 06),
            };
            await _db.InsertAsync(demoTerm);

            Course demoCourse = new Course
            {
                TermID = demoTerm.ID,
                Name = "Demo Course",
                Status = "Planned",
                Start = new DateTime(2023, 02, 20),
                End = new DateTime(2023, 03, 10),
                InstructorName = "Cameron Stapp",
                InstructorEmail = "cstapp5@wgu.edu",
                InstructorPhone = "417-880-3074",
                Notes = " ",
                NotificationStart = false,
                NotificationEnd = false,
            };
            await _db.InsertAsync(demoCourse);

            Assessment demoAssessmentPA = new Assessment
            {
                CourseID = demoCourse.ID,
                AssessmentName = $"DEMO-PA0001",
                Type = "Performance Assessment",
                StartDate = new DateTime(2023, 03, 01),
                DueDate = new DateTime(2023, 03, 23),
                NotificationStart= false,
                NotificationEnd = false,
            };
            await _db.InsertAsync(demoAssessmentPA);

            Assessment demoAssessmentOA = new Assessment
            {
                CourseID = demoCourse.ID,
                AssessmentName = "DEMO-OA0001",
                Type = "Objective Assessment",
                StartDate = new DateTime(2023, 03, 01),
                DueDate = new DateTime(2023, 03, 23),
                NotificationStart = true,
                NotificationEnd = true,
            };
            await _db.InsertAsync(demoAssessmentOA);

        }

        public static async void ClearSampleData()
        {
            await Init();

            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            await _db.DropTableAsync<Assessment>();

            _db = null;
        }

        #endregion
    }
}
