using System.Data.SQLite;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace WinFormsApp4
{
    //класс для отображения текущей валюты пользователя
    public class CurrencyService
    {
        private readonly string _databasePath;
        private static bool _indexCreated = false;
        private static readonly object _lockObject = new object();
        public CurrencyService(string databasePath)
        {
            _databasePath = databasePath ?? throw new ArgumentNullException(nameof(databasePath));
            GetUserCurrencyAsyncindex().ConfigureAwait(false);
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

        private async Task GetUserCurrencyAsyncindex()
        {
            if (_indexCreated) return;

            lock (_lockObject)
            {
                if (_indexCreated) return;

                try
                {
                    using (var connection = new SQLiteConnection($"Data Source={_databasePath}"))
                    {
                        connection.Open();

                        using (var command = new SQLiteCommand(
                            "CREATE INDEX IF NOT EXISTS IX_Usersss_Login_Valute ON Usersss(Login) INCLUDE (Valute)",
                            connection))
                        {
                            command.ExecuteNonQuery();
                            _indexCreated = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания индекса: {ex.Message}");
                }
            }
        }

        public async Task<bool> GetUserCurrencyAsyncindexprovarka()
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={_databasePath}"))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    await GetUserCurrencyAsyncindex().ConfigureAwait(false);
                    using (var command = new SQLiteCommand("EXPLAIN QUERY PLAN SELECT Valute FROM Usersss WHERE Login = @Login", connection))
                    {
                        var result1 = await command.ExecuteScalarAsync().ConfigureAwait(false);
                        var result2 = result1 != null;

                        MessageBox.Show(result2 ? $"✅ Индекс '{result1.ToString()}' создан успешно!" : "❌ Индекс не создан");

                        return result2;
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
}

