using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDBInitializer> _Logger;

        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }
        public void Initialize()
        {
            _Logger.LogInformation("Инициализация БД...");
            var timer = Stopwatch.StartNew();
            //_db.Database.EnsureDeleted();
            //_db.Database.EnsureCreated();
            if (_db.Database.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Миграция БД...");
                _db.Database.Migrate();
                _Logger.LogInformation("Миграция БД выполнена за {0} c", timer.Elapsed.TotalSeconds);
            }
            else
                _Logger.LogInformation("Миграция БД не требуется. {0} c", timer.Elapsed.TotalSeconds);

            try
            {
                InitializeProducts();
            }
            catch (Exception e)
            {
                _Logger.LogError("Ошибка при инициализации товаров в БД");
                throw;
            }


            _Logger.LogInformation("Инициализация БД завершена за {0} c", timer.Elapsed.TotalSeconds);
        }

        private void InitializeProducts()
        {
            if (_db.Products.Any())
            {
                _Logger.LogInformation("БД не нуждается в инициализации товаров");
                return;
            }
            _Logger.LogInformation("Инициализация секций...");
            var timer = Stopwatch.StartNew();

            var sections_pool = TestData.Sections.ToDictionary(section => section.Id);

            //foreach (var section in TestData.Sections.Where(s => s.ParentId != null))
            //{
            //    section.Parent = sections_pool[(int)section.ParentId!];
            //}

            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);
                
                _db.SaveChanges(); //именно тут формируется SQL-запрос в БД
               
                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Инициализация товаров выполнена за {0} c", timer.Elapsed.TotalSeconds);
        }

    }
}
