using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp4
{
    //класс для отображения текущей валюты пользователя
    public class CurrencyService
    {
        private readonly string _databasePath;

        public CurrencyService(string databasePath)
        {
            _databasePath = databasePath ?? throw new ArgumentNullException(nameof(databasePath));
        }

        public async Task<string> GetUserCurrencyAsync(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                return "RUB";
            }

            try
            {
                using (var connection = new SQLiteConnection($"Data Source={_databasePath}"))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    using (var command = new SQLiteCommand("SELECT Valute FROM Usersss WHERE Login = @Login", connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                        
                        if (result != null && result != DBNull.Value)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения валюты пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return "RUB";
        }
    }
}

