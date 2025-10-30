using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinFormsApp4.DIcreatebdusertest;

namespace WinFormsApp4
{
    internal class DIcreatebdusertest
    {
        //получения пути к бд
        public interface DBPath
        {
            string getdbpath();
        }
        // открытие подключения к бд
        public interface DBOPEN
        {
            Task openbd();
        }
        // новый команда
        public interface DBnewCom
        {
            Task Newcom();
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

            public async Task openbd()
            {
                using (_connection = new SQLiteConnection(_connectionString))
                {
                    await _connection.OpenAsync().ConfigureAwait(false);
                }
            }
            public SQLiteConnection GetConnection() => _connection;
        }

        // новый команда
        public class getCommand : DBnewCom
        {
            public async Task Newcom()
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
                             [Valute]
                         );";

                    using(var command = new SQLiteCommand(createTableCommand,connection))
                    {
                       await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
        }
            //получения пути к бд
        public class getdbph : DBPath
        {
            public string getdbpath()
            {
                var form3 = new Form3();
                var resultpath = form3.GetDatabasePath();
                return resultpath;
            }
        }
    }

