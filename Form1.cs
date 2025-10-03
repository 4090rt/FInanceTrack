using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            createbdtransaction();
            createbduser();
        }

        private string hashpqpass(string Password)
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
        public async Task saveUser()
        {
            string Login = textBox1.Text;
            string Password = textBox2.Text;
            if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    string hashPassword = hashpqpass(Password);
                    try
                    {
                        string dbPath = GetDatabasePath();
                        using (var das = new SQLiteConnection($"Data Source={dbPath}"))
                        {
                            await das.OpenAsync().ConfigureAwait(false);
                            var dass = new SQLiteCommand($"INSERT INTO [USERS] (Login,Password) VALUES (@L,@P)", das);
                            dass.Parameters.AddWithValue("@L", Login);
                            dass.Parameters.AddWithValue("@P", hashPassword);
                            await dass.ExecuteNonQueryAsync().ConfigureAwait(false);
                            MessageBox.Show("Данные сохранены");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сохранения данных {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при хэшировании данных: " + ex.Message);
                }
            }
            else 
            {
                MessageBox.Show("Заполните все поля","Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public string GetDatabasePath()
        {
            try
            {
                // Получаем папку Documents с дополнительными проверками
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                //MessageBox.Show($"Documents path: '{documentsPath}'", "Отладка GetDatabasePath", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Если Documents path пустой или null, используем альтернативы
                if (string.IsNullOrEmpty(documentsPath))
                {
                    //MessageBox.Show("Documents path пустой, пробуем UserProfile", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                    if (string.IsNullOrEmpty(documentsPath))
                    {
                        //MessageBox.Show("UserProfile тоже пустой, используем C:\\", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        documentsPath = @"C:\";
                    }
                }

                string appFolder = null;
                try
                {
                    appFolder = System.IO.Path.Combine(documentsPath, "WinFormsApp4");
                    //MessageBox.Show($"App folder: '{appFolder}'", "Отладка GetDatabasePath", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании пути к папке приложения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    appFolder = documentsPath; // Используем папку Documents напрямую
                }

                // Проверяем, что appFolder не null
                if (string.IsNullOrEmpty(appFolder))
                {
                    throw new InvalidOperationException("Не удалось определить папку приложения");
                }

                // Создаем папку если её нет
                try
                {
                    if (!System.IO.Directory.Exists(appFolder))
                    {
                        System.IO.Directory.CreateDirectory(appFolder);
                        //MessageBox.Show($"Создана папка: {appFolder}", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось создать папку {appFolder}: {ex.Message}", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                string dbPath = null;
                try
                {
                    dbPath = System.IO.Path.Combine(appFolder, "UserBase.db");
                    //MessageBox.Show($"Database path: '{dbPath}'", "Отладка GetDatabasePath", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании пути к БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dbPath = appFolder + "\\UserBase.db"; // Принудительное создание пути
                }

                // Проверяем, что путь валидный
                if (!string.IsNullOrEmpty(dbPath))
                {
                    return dbPath;
                }
                else
                {
                    throw new InvalidOperationException("Путь к базе данных получился пустым");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Общая ошибка в GetDatabasePath: {ex.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // В крайнем случае используем абсолютный путь
                try
                {
                    string fallbackPath = @"C:\Temp\WinFormsApp4_UserBase.db";
                    //MessageBox.Show($"Используется резервный путь: {fallbackPath}", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return fallbackPath;
                }
                catch
                {
                    // Последний резерв - просто имя файла
                    return "UserBase.db";
                }
            }
        }
        public async Task createbduser()
        {
            try
            {
                string dbPath = GetDatabasePath();

                //MessageBox.Show($"Путь к БД Users: {dbPath}", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                using (var das = new SQLiteConnection($"Data Source={dbPath}"))
                {
                    await das.OpenAsync().ConfigureAwait(false);
                    string createTableCommand = @"CREATE TABLE IF NOT EXISTS [Users] (
                                 [ID] INTEGER PRIMARY KEY AUTOINCREMENT,
                                 [Login] TEXT NOT NULL UNIQUE,
                                 [Password] TEXT NOT NULL
                             );";

                    using (var command = new SQLiteCommand(createTableCommand, das))
                    {
                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                string dbPath = GetDatabasePath();
                MessageBox.Show($"EROR Users: {ex.Message}\nПуть к БД: {dbPath}", "SQLite", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async Task createbdtransaction()
        {
            try
            {
                string dbPath = GetDatabasePath();
                //MessageBox.Show($"Путь к БД Transaction: {dbPath}", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Проверяем, что путь не null и не пустой
                if (string.IsNullOrEmpty(dbPath))
                {
                    MessageBox.Show("Ошибка: путь к базе данных пустой!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //MessageBox.Show("Создаем соединение с БД...", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Дополнительные проверки перед созданием соединения
                //MessageBox.Show($"Проверяем dbPath: '{dbPath}' (Length: {dbPath?.Length ?? 0})", "Детальная отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (string.IsNullOrEmpty(dbPath))
                {
                    MessageBox.Show("КРИТИЧЕСКАЯ ОШИБКА: dbPath пустой!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Попробуем создать строку подключения пошагово
                string connectionString;
                try
                {
                    connectionString = $"Data Source={dbPath}";
                    //MessageBox.Show($"Строка подключения создана: '{connectionString}'", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании строки подключения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Попробуем создать соединение с дополнительными параметрами
                try
                {
                    //MessageBox.Show("Пытаемся создать SQLiteConnection...", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Альтернативная строка подключения с дополнительными параметрами
                    string enhancedConnectionString = $"Data Source={dbPath};Version=3;New=True;Compress=True;";
                    //MessageBox.Show($"Используем расширенную строку: {enhancedConnectionString}", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (var das = new SQLiteConnection(enhancedConnectionString))
                    {
                        //MessageBox.Show("Открываем соединение...", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await das.OpenAsync().ConfigureAwait(false);

                        //MessageBox.Show("Соединение открыто, создаем команду...", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        string createTableCommand =
                            @"CREATE TABLE IF NOT EXISTS [Tranzaction](
                        [ID] INTEGER PRIMARY KEY AUTOINCREMENT,
                        [Name] TEXT NOT NULL,
                        [Summ] TEXT NOT NULL,
                        [What] TEXT NOT NULL
                         )";

                        //MessageBox.Show($"SQL команда: {createTableCommand}", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        using (var command = new SQLiteCommand(createTableCommand, das))
                        {
                            //MessageBox.Show("Выполняем команду...", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                            //MessageBox.Show("Команда выполнена успешно!", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception connEx)
                {
                    MessageBox.Show($"Ошибка при создании/использовании соединения: {connEx.Message}\nStackTrace: {connEx.StackTrace}", "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Попробуем упрощенную строку подключения
                    try
                    {
                        MessageBox.Show("Пробуем упрощенную строку подключения...", "Резервный вариант", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        using (var das = new SQLiteConnection($"Data Source={dbPath}"))
                        {
                            await das.OpenAsync().ConfigureAwait(false);

                            string createTableCommand =
                                @"CREATE TABLE IF NOT EXISTS [Tranzaction](
                            [ID] INTEGER PRIMARY KEY AUTOINCREMENT,
                            [Name] TEXT NOT NULL,
                            [Summ] TEXT NOT NULL,
                            [What] TEXT NOT NULL
                             )";

                            using (var command = new SQLiteCommand(createTableCommand, das))
                            {
                                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                                MessageBox.Show("Таблица создана с упрощенной строкой подключения!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception fallbackEx)
                    {
                        MessageBox.Show($"И резервный вариант не сработал: {fallbackEx.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {

            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await saveUser();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
            
        }
    }
}
