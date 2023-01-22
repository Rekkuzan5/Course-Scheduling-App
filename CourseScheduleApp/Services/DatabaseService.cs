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
        public static async Task AddGadget(string name, string color, int inStock, decimal price, DateTime creationDate)
        {
            await Init();
            var gadget = new Gadget()
            {
                Name = name,
                Color = color,
                InStock = inStock,
                Price = price,
                CreationDate = creationDate
            };

            await _db.InsertAsync(gadget);

            var id = gadget.ID;
        }
        public static async Task RemoveGadget(int id)
        {
            await Init();

            await _db.DeleteAsync<Gadget>(id);
        }
        public static async Task<IEnumerable<Gadget>> GetGadgets()
        {
            await Init();

            var gadgets = await _db.Table<Gadget>().ToListAsync();
            return gadgets;
        }
        public static async Task UpdateGadget(int id, string name, string color, int inStock, decimal price, DateTime creationDate)
        {
            await Init();

            var gadgetQuery = await _db.Table<Gadget>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (gadgetQuery != null)
            {
                gadgetQuery.Name = name;
                gadgetQuery.Color = color;
                gadgetQuery.InStock = inStock;
                gadgetQuery.Price = price;
                gadgetQuery.CreationDate = creationDate;

                await _db.UpdateAsync(gadgetQuery);
            }
        }
        #endregion

        #region Widgets methods
        public static async Task AddWidget(int gadgetId, string name, string color, int inStock, decimal price, DateTime creationDate, bool notificationStart, string notes)
        {
            await Init();
            var widget = new Widget
            {
                GadgetId = gadgetId,
                Name = name,
                Color = color,
                InStock = inStock,
                Price = price,
                CreationDate = creationDate,
                StartNotification = notificationStart,
                Notes = notes
            };

            await _db.InsertAsync(widget);

            var id = widget.ID;
        }
        public static async Task RemoveWidget(int id)
        {
            await Init();

            await _db.DeleteAsync<Widget>(id);
        }
        public static async Task<IEnumerable<Widget>> GetWidgets(int gadgetID)
        {
            await Init();

            var widgets = await _db.Table<Widget>().Where(i => i.GadgetId == gadgetID).ToListAsync();

            return widgets;
        }
        public static async Task<IEnumerable<Widget>> GetWidgets()
        {
            await Init();
            var widgets = await _db.Table<Widget>().ToListAsync();

            return widgets;
        }
        public static async Task UpdateWidget(int id, string name, string color, int inStock, decimal price, DateTime creationDate, bool notificationStart, string notes)
        {
            await Init();

            var widgetQuery = await _db.Table<Widget>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();
            
            if (widgetQuery != null)
            {
                widgetQuery.Name = name;
                widgetQuery.Color = color;
                widgetQuery.InStock = inStock;
                widgetQuery.Price = price;
                widgetQuery.StartNotification = notificationStart;
                widgetQuery.Notes = notes;
                widgetQuery.CreationDate = creationDate;

                await _db.UpdateAsync(widgetQuery);
            }
        }

        #endregion

        #region DemoData
        public static async void LoadSampleData()
        {
            await Init();

            Gadget gadget = new Gadget()
            {
                Name = "Gadget 1",
                Color = "Blue",
                InStock = 255,
                Price = 25m,
                CreationDate = DateTime.Today.Date
            };

            await _db.InsertAsync(gadget);

            Widget widget = new Widget()
            {
                Name = "Widget 1",
                Color = "Teal",
                InStock = 25,
                Price = 22.59m,
                CreationDate = DateTime.Now.Date,
                StartNotification = true,
                GadgetId = gadget.ID
            };

            await _db.InsertAsync(widget);

            Widget widget2 = new Widget()
            {
                Name = "Widget 2",
                Color = "Green",
                InStock = 55,
                Price = 22.59m,
                CreationDate = DateTime.Now.Date,
                StartNotification = true,
                GadgetId = gadget.ID
            };

            await _db.InsertAsync(widget2);

            Gadget gadget2 = new Gadget()
            {
                Name = "Gadget 2",
                Color = "Black",
                InStock = 155,
                Price = 250m,
                CreationDate = DateTime.Today.Date
            };

            await _db.InsertAsync(gadget2);

            Widget widget3 = new Widget()
            {
                Name = "Widget 3",
                Color = "Teal",
                InStock = 25,
                Price = 22.59m,
                CreationDate = DateTime.Now.Date,
                StartNotification = true,
                GadgetId = gadget2.ID
            };

            await _db.InsertAsync(widget3);

            Widget widget4 = new Widget()
            {
                Name = "Widget 4",
                Color = "Green",
                InStock = 55,
                Price = 22.59m,
                CreationDate = DateTime.Now.Date,
                StartNotification = true,
                GadgetId = gadget2.ID
            };

            await _db.InsertAsync(widget4);

            Widget widget5 = new Widget()
            {
                Name = "Widget 5",
                Color = "Orange",
                InStock = 55,
                Price = 22.59m,
                CreationDate = DateTime.Now.Date,
                StartNotification = true,
                GadgetId = gadget2.ID
            };

            await _db.InsertAsync(widget5);

        }
        
        public static async Task ClearSampleData()
        {
            await Init();

            await _db.DropTableAsync<Widget>();
            await _db.DropTableAsync<Gadget>();
            _db = null;
            _dbConnection = null;
        }

        public static async void LoadSampleDataSql()
        {

        }
        #endregion
        // check end of webinar 5 for more ideas on sqlLite queries

    }
}
