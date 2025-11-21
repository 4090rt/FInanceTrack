using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp4;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace WinFormsApp4
{
    public class main
    {
        public async Task Maulmethod1()
        {
            try
            {
                // 1. Проверяем логин
                MessageBox.Show($"Текущий логин: '{GlobalData.CurrentLogin}'");
                // 2. Проверяем индекс
                Notificatoinsbd bd = new Notificatoinsbd();
                await bd.Indexproverka().ConfigureAwait(false);
                var result = await bd.Emaildb().ConfigureAwait(false);
                MessageBox.Show(result.ToString());
                string email = result;
                // Проверяем, что email не пустой и валиден
                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Email адрес не найден в базе данных");
                    return;
                }
                MessageBox.Show("Найден email: " + email);
                Notifications2 notification = new Notifications2();
                await notification.SendMail(email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение" + ex.Message);
            }
        }

        public async Task Maulmethod2(string Login)
        {
            try
            {
                Notificatoinsbd bd = new Notificatoinsbd();
                await bd.Indexproverka().ConfigureAwait(false);
                var result = await bd.Emaildb2(Login).ConfigureAwait(false);
                MessageBox.Show(result.ToString());
                string email = result;

                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Email адрес не найден в базе данных");
                    return;
                }
                MessageBox.Show("Найден email: " + email);

                Notifications22 notification = new Notifications22();
                await notification.SendMail(email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение" + ex.Message);
            }
        }
    }
    public class Notifications22
    {
        // тип почты
        private readonly string _smtpServer = "smtp.gmail.com";
        // порт безопасности 
        private readonly int _port = 587;
        // почта с которой отправляем 
        private readonly string _gmail = "artem2007yannurow@gmail.com";
        // пароль от почты
        private readonly string _passwordmail = "mdyh mrza nlki drry";

        public async Task SendMail(string Email)
        {
            string Login = GlobalData.CurrentLogin;
            try
            {
                using var smptclient = new SmtpClient(_smtpServer, _port)
                {
                    Credentials = new NetworkCredential(_gmail, _passwordmail),
                    EnableSsl = true
                };

                var smptmesseage = new MailMessage
                {
                    From = new MailAddress(_gmail),
                    Subject = "Кто то попытался войти в аккаунт Finance Track",
                    Body = $"Кто то попытался войти в аккаунт {Login}",
                    IsBodyHtml = false
                };
                smptmesseage.To.Add(Email);

                await smptclient.SendMailAsync(smptmesseage);
                MessageBox.Show("Сообщение отправлено");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Пойман" + ex.Message);
            }
        }
    }

    public class Notifications2
    {
        // тип почты
        private readonly string _smtpServer = "smtp.gmail.com";
        // порт безопасности 
        private readonly int _port = 587;
        // почта с которой отправляем 
        private readonly string _gmail = "artem2007yannurow@gmail.com";
        // пароль от почты
        private readonly string _passwordmail = "mdyh mrza nlki drry";

        public async Task SendMail(string Email)
        {
            string Login = GlobalData.CurrentLogin;
            try
            {
                using var smptclient = new SmtpClient(_smtpServer, _port)
                {
                    Credentials = new NetworkCredential(_gmail, _passwordmail),
                    EnableSsl = true
                };

                var smptmesseage = new MailMessage
                {
                    From = new MailAddress(_gmail),
                    Subject = "Вход в аккаунт Finance Track",
                    Body = $"Был выполнен вход в {Login}",
                    IsBodyHtml = false
                };
                smptmesseage.To.Add(Email);
                await smptclient.SendMailAsync(smptmesseage).ConfigureAwait(false);
                MessageBox.Show("Сообщение отправлено");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение" + ex.Message);
            }
        }
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
            await Emailbdindex().ConfigureAwait(false);     
            string dbPath = Form3.GetDatabasePath();
            string Login = GlobalData.CurrentLogin;
            Notifications2PoolConnect pool = new Notifications2PoolConnect(dbPath);
            SQLiteConnection connect = null;
            try
            {
                connect = pool.Connect();
                using (var command = new SQLiteCommand("SELECT Mail FROM Usersss WHERE Login = @L", connect))
                {
                    command.Parameters.AddWithValue("@L", Login);
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
            finally
            {
                pool.Close(connect);
            }
            return "";
        }

        public async Task<string> Emaildb2(string Login)
        {
            await Emailbdindex().ConfigureAwait(false);
            string dbPath = Form3.GetDatabasePath();
            Notifications2PoolConnect pool = new Notifications2PoolConnect(dbPath);
            SQLiteConnection connect = null;
            try
            {
                connect = pool.Connect();
                using (var command = new SQLiteCommand("SELECT Mail FROM Usersss WHERE Login = @L", connect))
                {
                    command.Parameters.AddWithValue("@L", Login);
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
            finally
            {
                pool.Close(connect);
            }
            return "";
        }
        public async Task Emailbdindex()
        {
            Form3 form = new Form3();
            string dbPath = Form3.GetDatabasePath();
            Notifications2PoolConnect pool = new Notifications2PoolConnect(dbPath);
            SQLiteConnection connect = null;
            lock (_lock)
            {
                if (_currentindex) return;
                _currentindex = true;
            }
            try
            {
                connect = pool.Connect();
                using (var command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS Loginindex ON Usersss(Login)", connect))
                {
                    command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                pool.Close(connect);
            }
        }

        public async Task Indexproverka()
        {
            string dbPath = Form3.GetDatabasePath();
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
            finally
            {
                pool.Close(connect);
            }
        }
    }
}


