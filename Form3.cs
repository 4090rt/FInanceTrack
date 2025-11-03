using Aspose.Pdf.Operators;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using static WinFormsApp4.DIcreatebdusertest;

namespace WinFormsApp4
{
    public partial class Form3 : Form
    {
        private reposit.IUser _userRepository;
        public Form3()
        {
            InitializeComponent();
            massiv();
            pictures();
        }



        //массив валют
        public void massiv()
        {
            string[] valute = { "EUR", "USD", "RUB" };
            comboBox2.Items.AddRange(valute);

        }



        // работа с элементами формы
        public void pictures()
        {
            pictureBox2.Image = Properties.Resources._2;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            button1.FlatStyle = FlatStyle.Popup;
            button2.FlatStyle = FlatStyle.Popup;
        }

        //валидация(авторизация юзера)
        public async Task<bool> vakidateuser(string Login, string Password)
        {

            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return false;
            }

            string dbPath = GetDatabasePath();
            if (!File.Exists(dbPath))
            {
                MessageBox.Show("База данных не найдена!");
                MessageBox.Show("Подождите.. База данных создается");
                return true;
            }

            string hashpass;
            try
            {
                hashpass = hashpqpass(Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обработке пароля: " + ex.Message);
                return false;
            }

            using (var das = new SQLiteConnection($"Data Source={dbPath}"))
            {
                await das.OpenAsync().ConfigureAwait(false);
                try
                {
                    using (var gg = new SQLiteCommand("SELECT Password FROM Usersss WHERE Login = @L LIMIT 1", das))
                    {
                        gg.Parameters.AddWithValue("@L", Login);
                        var value = await gg.ExecuteScalarAsync().ConfigureAwait(false);
                        if (value == null || value == DBNull.Value)
                        {
                            MessageBox.Show("Пользователь с таким логином не найден! Зарегестрируйтесь!");
                            return false;
                        }
                        string pasd = Convert.ToString(value);
                        bool isValid = string.Equals(pasd, hashpass, StringComparison.Ordinal);

                        if (!isValid)
                        {
                            MessageBox.Show("Неверный пароль!");
                        }

                        return isValid;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при проверке пользователя: " + ex.Message);
                }
            }
            return false;
        }





        ////Проверка логина на уникальность
        //public async Task<bool> Loginproverka()
        //{
        //    string login = textBox1.Text;

        //    if (string.IsNullOrEmpty(login))
        //    {
        //        MessageBox.Show("Введите логин!");
        //        return false;
        //    }
        //    try
        //    {
        //        string dbPath = GetDatabasePath();
        //        using (var das = new SQLiteConnection($"Data Source={dbPath}"))
        //        {
        //            das.OpenAsync().ConfigureAwait(false);
        //            using (var commandsql = new SQLiteCommand($"SELECT COUNT(*) FROM Usersss WHERE Login = @L", das))
        //            {
        //                commandsql.Parameters.AddWithValue("@L", login);
        //                var result = await commandsql.ExecuteScalarAsync().ConfigureAwait(false);

        //                if (result == null && result == DBNull.Value)
        //                {
        //                    return false;
        //                }
        //                int count = Convert.ToInt32(result);

        //                if (count == 0)
        //                {
        //                    return true;
        //                }

        //                if (count <= 1)
        //                {
        //                    return false;
        //                }
        //                return false;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка проверки пароля {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //}


        //хэширование пароля
        public string hashpqpass(string Password)
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



        ////сохранение юзера в базу данных
        //public async Task<bool> saveUser()
        //{
        //    string Login = textBox1.Text;
        //    string Password = textBox2.Text;
        //    string valute = comboBox2.Text;

        //    if (await Loginproverka().ConfigureAwait(false))
        //    {
        //        if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(valute))
        //        {
        //            try
        //            {
        //                string hashPassword = hashpqpass(Password);
        //                try
        //                {
        //                    string dbPath = GetDatabasePath();
        //                    using (var das = new SQLiteConnection($"Data Source={dbPath}"))
        //                    {
        //                        await das.OpenAsync().ConfigureAwait(false);
        //                        var dass = new SQLiteCommand($"INSERT INTO [Usersss] (Login,Password,Valute) VALUES (@L,@P,@V)", das);
        //                        dass.Parameters.AddWithValue("@L", Login);
        //                        dass.Parameters.AddWithValue("@P", hashPassword);
        //                        dass.Parameters.AddWithValue("@V", valute);
        //                        await dass.ExecuteNonQueryAsync().ConfigureAwait(false);
        //                        MessageBox.Show("Данные сохранены");
        //                        return true;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show($"Ошибка сохранения данных {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return false;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Ошибка при хэшировании данных: " + ex.Message);
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Такой логин уже существует! Выберите другой логин.");
        //        return false;
        //    }
        //}



        // получение все возможных путей расположения бд
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


        // создание бд
        public async Task createbduser()
        {
            try
            {
                var dbcon = new getdbph();
                string dbPath = dbcon.getdbpath();

                //MessageBox.Show($"Путь к БД Users: {dbPath}", "Отладка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var dbopen = new getdbopen($"Data Source={dbPath}");
                await dbopen.openbd();

                var command = new getCommand();
                await command.Newcom();

            }
            catch (Exception ex)
            {
                string dbPath = GetDatabasePath();
                MessageBox.Show($"EROR Users: {ex.Message}\nПуть к БД: {dbPath}", "SQLite", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //регистрация(сохранения юзера)
        private async void button1_Click(object sender, EventArgs e)
        {
            createbduser();
            string dbPath = GetDatabasePath();
            var hashService = new reposit.HashService();
            _userRepository = new reposit.Realiz(dbPath, hashService);
            string Login = textBox1.Text;
            string Password = textBox2.Text;
            string Valute = comboBox2.Text;
            string Mail = textBox3.Text;
            validpassword validpass = new validpassword();
            var proverpapass = validpass.Passwortd(Password);
            if (proverpapass)
            {
                try
                {
                    bool result = await _userRepository.SaveUserAsync(Login, Password, Valute,Mail);

                        if (result)
                        {
                            MessageBox.Show("Успешно Сохранено");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка сохранения");
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Возникло исключение" + ex.Message, "Ошибка" + MessageBoxIcon.Error + MessageBoxButtons.OK);
                }
            }
            else
            {
                return;
            }
        }



        //авторизация юзера
        private async void button2_Click(object sender, EventArgs e)
        {
            string Login = textBox1.Text;
            string Password = textBox2.Text;
            bool isValid = await vakidateuser(Login, Password);
            if (isValid)
            {
                // Сохраняем логин и пароль в глобальную переменную
                GlobalData.SetCurrentUser(Login, Password);
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Smenaparolyasms form = new Smenaparolyasms();
            form.Show();
            this.Hide();
        }
    }


    // Глобальный класс для хранения данных приложения
    public static class GlobalData
    {
        public static string CurrentLogin { get; set; } = string.Empty;
        public static string CurrentPassword { get; set; } = string.Empty;

        public static void SetCurrentUser(string login, string password)
        {
            CurrentLogin = login ?? string.Empty;
            CurrentPassword = password ?? string.Empty;
        }

        public static void ClearCurrentUser()
        {
            CurrentLogin = string.Empty;
            CurrentPassword = string.Empty;
        }

        public static bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(CurrentLogin);
        }

        public static void SetCurrentUserPas(string password)
        {
            CurrentPassword = password ?? string.Empty;
        }

        public static void ClearCurrentUserPas()
        {
            CurrentPassword = string.Empty;
        }

        public static bool IsUserLoggedInPas()
        {
            return !string.IsNullOrEmpty(CurrentPassword);
        }
    }
}