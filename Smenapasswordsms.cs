using Aspose.Cells;
using Aspose.Words.Themes;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace WinFormsApp4
{
    public interface DIpatt
    {
        public  Task<int> senamessage(string Login, string dbPath);
    }

    // отправка смс на емайл
    public class Smenapasswordsms
    {
        // тип почты
        private readonly string _smtpServer = "smtp.gmail.com";
        // порт безопасности 
        private readonly int _port = 587;
        // почта с которой отправляем 
        private readonly string _gmail = "artem2007yannurow@gmail.com";
        // пароль от почты
        private readonly string _passwordmail = "mdyh mrza nlki drry";

        public async Task SendVerificationCodeAsync(string email, int code)
        {
            // создание и настройка smpt клиента 
            using var smptclient = new SmtpClient(_smtpServer, _port)
            {
                // Создаем "удостоверение" с email и паролем для авторизации на почтовом сервере
                Credentials = new NetworkCredential(_gmail,_passwordmail),
                // включаем защищенное соединение
                EnableSsl = true
            };
            // создание объекта  письма
            var smtpmessage = new MailMessage
            {
                // с какого адреса
                From = new MailAddress(_gmail),
                // тема
                Subject = "Ваш код потверждения",
                // содержимое пиьсма
                Body = $"Код потверждения: {code}",
                // поддержка html
                IsBodyHtml = false
            };
            // добавили email на который отправляем письмо 
            smtpmessage.To.Add(email);
            // асинхронно отправили 
            await smptclient.SendMailAsync(smtpmessage).ConfigureAwait(false);
        }
    }

    // основной класс
    public class SendMessageemail:DIpatt
    {
        public int _currentVerificationCode;
        public async Task<int> senamessage(string Login, string dbPath)
        {
            try
            {
                string email = "";
                // генерация кода 
                try
                {
                    var codemethod = new Codegeneration();
                    _currentVerificationCode = codemethod.codegenerate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка генерации кода"  + ex.Message);
                    return 0;
                }
                // получение email по логину
                try
                {
                    var Emailmethod = new EmailSQLitezapros();
                    email = await Emailmethod.SQLite(Login, dbPath);
                    if (string.IsNullOrEmpty(email))
                    {
                        MessageBox.Show("Не удалось получить email");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка получения почты" + ex.Message);
                    return 0;
                }
                // отправка письма на полученный маил
                try
                {
                    var Smenapasswordsmsmethod = new Smenapasswordsms();
                    await  Smenapasswordsmsmethod.SendVerificationCodeAsync(email, _currentVerificationCode);
                    MessageBox.Show($"Код  отправлен на {email}");
                    return _currentVerificationCode;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка отправки кода" + ex.Message);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка" + ex.Message);
                return 0;
            }
        }
    }
    // запрос мэйла по логину из бд
    public class EmailSQLitezapros
    {
        private string _dbPath;
        private static bool _currentindex = false;
        private static readonly object _lock = new object();
        public EmailSQLitezapros()
        {
            DBpath(); // Инициализируем путь один раз при создании объекта
            SQLiteindex().ConfigureAwait(false);
            SQLiteindexproverka().ConfigureAwait(false);
        }
        public async Task<string> SQLite(string Login, string dbPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(dbPath))
                {
                    var DbPath = dbPath;
                    using (var connectionSqlite = new SQLiteConnection($"Data Source={DbPath}"))
                    {
                        await connectionSqlite.OpenAsync().ConfigureAwait(false);

                        string command = "SELECT Mail FROM Usersss WHERE Login = @L";
                        using (var newSqlitecommand = new SQLiteCommand(command, connectionSqlite))
                        {
                            newSqlitecommand.Parameters.AddWithValue("@L", Login);
                            var result = await newSqlitecommand.ExecuteScalarAsync().ConfigureAwait(false);
                            if (result != null)
                            {
                                return result.ToString();
                            }
                            else
                            {
                                MessageBox.Show("Пользователь с таким логином не найден");
                                return "";
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля");
                    return "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eror" + ex.Message, "EROR" + MessageBoxIcon.Error, MessageBoxButtons.OK);
                return "";
            }
        }

        public void DBpath()
        { 
            _dbPath = Form3.GetDatabasePath();
        }

        public async Task SQLiteindex()
        {
            if (_currentindex) return;

            lock (_lock)
            {
                if (_currentindex) return;
                _currentindex = true;
            }
                try
                {
                    using (var connect = new SQLiteConnection($"Data Source={_dbPath}"))
                    {
                        await connect.OpenAsync().ConfigureAwait(false);
                        using (var command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS IX_Usersss_Login_Mail ON Usersss(Login, Mail)", connect))
                        {
                            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Возникло исключение1" + ex.Message);
                }
            
        }

        public async Task<bool> SQLiteindexproverka()
        {
            try
            {
                using (var connect = new SQLiteConnection($"Data Source={_dbPath}"))
                {
                    await connect.OpenAsync().ConfigureAwait(false);

                    using (var command = new SQLiteCommand("EXPLAIN QUERY PLAN SELECT Mail FROM Usersss WHERE Login = @Login", connect))
                    {
                        command.Parameters.AddWithValue("@Login", "test_login");
                        var rresult = await command.ExecuteScalarAsync().ConfigureAwait(false);
                        bool result = rresult != null;

                        MessageBox.Show(result ? $"✅ Индекс '{result.ToString()}' создан успешно!" : "❌ Индекс не создан");

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение2" + ex.Message);
                return false;
            }
        }


    }
    // генерация рандомного кода
    public class Codegeneration
    {
        public int codegenerate()
        {
            try
            {
                var random = new Random();
                var code = random.Next(100000, 999999);
                return code;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка генерации кода" + ex.Message);
                throw;
            }
        }
    }
}
