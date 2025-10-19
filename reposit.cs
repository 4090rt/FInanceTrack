using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    internal class reposit
    {

        public class User
        {
            public string Login { get; set; }
            public string Password { get; set; }

            public string Valute { get; set; }
        }


        public interface IUser
        {
            Task<bool> SaveUserAsync(string Login, string Password, string Valute);
        }


        public interface IHashService
        {
            string HashPassword(string password);
        }

        // Реализация
        public class HashService : IHashService
        {
            public string HashPassword(string Password)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Password))
                    {
                        using (SHA256 sHA256 = SHA256.Create())
                        {
                            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                            StringBuilder buider = new StringBuilder();
                            for (int i = 0; i < bytes.Length; i++)
                            {
                                buider.Append(bytes[i].ToString("x2"));
                            }
                            return buider.ToString();
                        }
                    }
                    throw new Exception("Ошибка");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка" + ex.Message);
                    throw;
                }
            }
        }

        public class Realiz : IUser
        {
            private readonly string _dbPath;
            private readonly IHashService _hashService;

            public Realiz(string dbPath, IHashService hashService)
            {
                _dbPath = dbPath;
                _hashService = hashService;
            }

            public async Task<bool> SaveUserAsync(string login, string password, string valute)
            {
                try
                {
                    if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(valute))
                    {
                        return false;
                    }
                    string hashpass = _hashService.HashPassword(password);

                    if (!await IsLoginExistsAsync(login).ConfigureAwait(false))
                    {
                        using var connect = new SQLiteConnection($"Data Source={_dbPath}");
                        {
                            await connect.OpenAsync();

                            using var command = new SQLiteCommand("INSERT INTO [Usersss] (Login, Password, Valute) VALUES (@L, @HP, @V)", connect);
                            {
                                command.Parameters.AddWithValue("@L", login);
                                command.Parameters.AddWithValue("@HP", hashpass);
                                command.Parameters.AddWithValue("@V", valute);

                                int result = await command.ExecuteNonQueryAsync();
                                return result > 0;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Такой логин уже существует");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения пользователя: {ex.Message}");
                    return false;
                }
                return false;
            }

            public async Task<bool> IsLoginExistsAsync(string login)
            {
                try
                {
                    using var connect = new SQLiteConnection($"Data Source={_dbPath}");
                    await connect.OpenAsync().ConfigureAwait(false);

                    using var command = new SQLiteCommand(
                        "SELECT COUNT(1) FROM [Usersss] WHERE Login = @L",
                        connect);

                    command.Parameters.AddWithValue("@L", login);
                    var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    return Convert.ToInt32(result) > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка проверки логина: {ex.Message}");
                    return false;
                }
            }
        }


    }
}
