using MeteoApp.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Services
{
    public class DatabaseService
    {

        private SQLiteAsyncConnection _database;
        private readonly string _dbPath;
        private readonly string _dbPassword;

        public DatabaseService(string dbPath, string dbPassword)
        {
            _dbPath = dbPath;
            _dbPassword = dbPassword;
        }

        private async Task Init()
        {
            if (_database != null) return;
            try
            {
                SQLitePCL.Batteries_V2.Init();
                SQLiteConnectionString options = new SQLiteConnectionString(_dbPath, true, key: _dbPassword);
                _database = new SQLiteAsyncConnection(options);
                await _database.CreateTableAsync<Entry>();

            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\n\n ---> ERRORE GRAVE DATABASE: {ex.Message} \n\n");
                throw; // Rilanciamo l'errore
            }


        }

        public async Task<List<Entry>> GetEntriesAsync()
        {
            await Init();
            return await _database.Table<Entry>().ToListAsync();
        }

        public async Task<int> SaveEntryAsync(Entry entry)
        {
            //cambia id poi
            await Init();
            if (entry.Id != 0)
                return await _database.UpdateAsync(entry);
            else
                return await _database.InsertAsync(entry);
        }

        public async Task<int> DeleteEntryAsync(Entry entry)
        {
            await Init();
            return await _database.DeleteAsync(entry);
        }

    }
}
