using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Essentials;
using CourseScheduleApp.Models;

namespace CourseScheduleApp.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _db;
        private static SQLiteConnection _dbConnection;

        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            // Get path to database file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Gadgets.db");

            _db = new SQLiteAsyncConnection(databasePath);
            _dbConnection = new SQLiteConnection(databasePath);

            await _db.CreateTableAsync<Gadget>();
            await _db.CreateTableAsync<Widget>();

        }
        #region Gadgets methods
        // 8:47 on part 5 webinar


        #endregion

        #region Widgets methods


        #endregion
    }
}
