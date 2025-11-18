using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    public class Notifications2
    {

    }

    public class Notificatoinsbd
    {
        private static bool _currentindex = false;
        private static readonly object _lock = new object();

        public Notificatoinsbd()
        {
            Emailbdindex().ConfigureAwait(false);
        }
        public async Task<string> Emaildb()
        {
            Form3 form = new Form3();
            string dbPath = form.GetDatabasePath();
            string Login = GlobalData.CurrentLogin;
            Notifications2PoolConnect pool = new Notifications2PoolConnect(dbPath);
            SQLiteConnection connect = null;
            try
            {
               connect = pool.Connect();
                using (var command = new SQLiteCommand("SELECT Mail FROM Usersss WHERE Login = @L", connect))
                {
                    command.Parameters.AddWithValue("@L",Login);
                    var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение:" + ex.Message);
                return "";
            }
            return "";
        }

        public async Task Emailbdindex()
        {
            Form3 form = new Form3();
            string dbPath = form.GetDatabasePath();
            Notifications2PoolConnect pool = new Notifications2PoolConnect(dbPath);
            SQLiteConnection connect = null;
            lock (_lock)
            {
                if (_currentindex) return;
                _currentindex = true;
            }

            connect = pool.Connect();
            using (var command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS Login_index ON Usersss(Login)", connect))
            { 
                await command.ExecuteScalarAsync().ConfigureAwait(false);
            }
        }

        public async Task Indexproverka()
        {
            Form3 form = new Form3();
            string dbPath = form.GetDatabasePath();
            Notifications2PoolConnect pool = new Notifications2PoolConnect(dbPath);
            SQLiteConnection connect = null;
            string Login = "fddfd";
            try
            {
                connect = pool.Connect();
                using (var command = new SQLiteCommand($"EXPLAIN QUERY PLAN SELECT Login FROM Usersss WHERE Login = '{Login}'", connect))
                {
                    await command.ExecuteScalarAsync().ConfigureAwait(false);
                    bool result = command != null;


                    MessageBox.Show(result ? $"✅ Индекс '{result.ToString()}' создан успешно!" : "❌ Индекс не создан");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение" + ex.Message);
            }
        }
    }
}
