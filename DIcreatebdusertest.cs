using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinFormsApp4.DIcreatebdusertest;

namespace WinFormsApp4
{
    public class DIcreatebdusertest
    {
        //получения пути к бд
        public interface DBPath
        {
            string getdbpath();
        }
        // открытие подключения к бд
        public interface DBOPEN
        {
            Task<bool> openbd();
        }
        // новая команда
        public interface DBnewCom
        {
            Task<bool> Newcom();
        }


        // открытие подключения к бд
        public class getdbopen : DBOPEN
        {
            private readonly string _connectionString;
            private SQLiteConnection _connection;
            public getdbopen(string connectionString)
            {
                _connectionString = connectionString;
            }

            public async Task<bool> openbd()
            {
                _connection = new SQLiteConnection(_connectionString);

                await _connection.OpenAsync().ConfigureAwait(false);
                return true;
            }
            public SQLiteConnection GetConnection() => _connection;
   
        }

        // новый команда
        public class getCommand : DBnewCom
        {
            public async Task<bool> Newcom()
            {
                try
                {
                    var path = new getdbph();
                    var RESULTPATH = path.getdbpath();

                    var openconnect = new getdbopen($"Data Source={RESULTPATH}");

                    await openconnect.openbd().ConfigureAwait(false);

                    var connection = openconnect.GetConnection();

                    string createTableCommand = @"CREATE TABLE IF NOT EXISTS [Usersss] (
                             [ID] INTEGER PRIMARY KEY AUTOINCREMENT,
                             [Login]  UNIQUE,
                             [Password],
                             [Valute],
                             [Mail]
                         );";

                    var command = new SQLiteCommand(createTableCommand, connection);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    return true;

                }
                catch
                {
                    throw new Exception("Ошибка");
                }
             }
            }
        }
            //получения пути к бд
        public class getdbph : DBPath
        {
            public string getdbpath()
            {
                var resultpath = Form3.GetDatabasePath();
                return resultpath;
            }
        }
    }

