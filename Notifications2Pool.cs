using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WinFormsApp4
{

    public class Notifications2PoolConnect
    { 
        private readonly Stack<SQLiteConnection> _pool = new Stack<SQLiteConnection>();
        private readonly string _connectionString;


        public Notifications2PoolConnect(string DBpath)
        {
            _connectionString = $"Data Source={DBpath}";
        }

        public SQLiteConnection Connect()
        {
            lock (_pool)
            {
                if (_pool.Count > 0)
                {
                    _pool.Pop();
                }
            }
            return DB();
        }

        public SQLiteConnection DB()
        { 
            var connect = new SQLiteConnection(_connectionString);
            connect.Open();
            return connect;
        }

        public void Close(SQLiteConnection connect)
        {
            if (connect.State == ConnectionState.Broken)
            {
                connect.Dispose();
                return;
            }
            lock (_pool)
            {
                _pool.Push(connect);
            }
        }
    }
}
