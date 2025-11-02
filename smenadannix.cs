using System.Data.SQLite;

namespace WinFormsApp4
{
    public partial class smenadannix : Form
    {
        private string _currentCurrency;
        public smenadannix()
        {
            InitializeComponent();
            massivoperaciy();
            viewLogin();
        }

        // Получение логина из бд для отображения
        public async Task<string> pokazLogin()
        {
            var form3 = new Form3();
            string dppath = form3.GetDatabasePath();
            try
            {
                using (var connect = new SQLiteConnection($"Data Source={dppath}"))
                {
                    await connect.OpenAsync().ConfigureAwait(false);

                    string Login = GlobalData.CurrentLogin;

                    if (!string.IsNullOrEmpty(Login))
                    {
                        try
                        {
                            using (var newcommand = new SQLiteCommand("SELECT Login FROM Usersss WHERE Login = @L", connect))
                            {
                                newcommand.Parameters.AddWithValue("@L", Login);
                                var result = await newcommand.ExecuteScalarAsync().ConfigureAwait(false);
                                string result2 = result.ToString();
                                return result2;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка создания подключения или получения логина" + ex.Message);
                            return false.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невохможно отобразить логин");
                        return false.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка" + ex.Message);
                return false.ToString();
            }
        }

        //Отображение логина на экране
        public async Task<bool> viewLogin()
        {
            try
            {
                var Login = await pokazLogin();
                string Loginconvert = Login.ToString();

                if (!string.IsNullOrEmpty(Loginconvert))
                {
                    label5.Text = Loginconvert;
                    return true;
                }
                else
                {
                    label5.Text = ("Не вохможно отобразить текст");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить логин для отображения" + ex.Message);
                return false;
            }
        }
        // массив с доступными валютами
        public void massivoperaciy()
        {
            string[] massivvalute = { "RUB", "EUR", "USD" };
            comboBox1.Items.AddRange(massivvalute);
        }

        // метод показывающий текущую валоюту 
        public async Task<bool> valutelocal()
        {
            try
            {
                Form3 form = new Form3();
                string dbPath = form.GetDatabasePath();


                if (string.IsNullOrEmpty(dbPath))
                {
                    MessageBox.Show("ОШИБКА: Путь к БД равен null или пустой строке!");
                    return false;
                }


                string directory = Path.GetDirectoryName(dbPath);
                if (Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    var currencyService = new CurrencyService(dbPath);
                    string userCurrency = await currencyService.GetUserCurrencyAsync(GlobalData.CurrentLogin);

                    var currencyInter = await CurrencyFactory.CreateCurrencyServiceAsync(userCurrency);
                    string currencyRates = await currencyInter.valutapros(userCurrency);
                    _currentCurrency = await currencyInter.valutapros(userCurrency);
                    return true;
                }
                else 
                {
                    string directory1 = Path.GetDirectoryName(dbPath);
                    MessageBox.Show("Не удалось найти директорию");
                    await Task.Delay(1000);
                    MessageBox.Show("Создаем директорию...");
                    await Task.Delay(2000);
                    Directory.CreateDirectory(directory1);
                    MessageBox.Show("Директория успешно создана!");
                    return true;
                }
                throw new Exception("Не удается создать директорию");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось определить текущую валюту: {ex.Message}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }



        // метод смены валюты
        private async Task<bool> smenavalute()
        {
            string Valute = comboBox1.Text;
            string Login = GlobalData.CurrentLogin;
            string Password = GlobalData.CurrentPassword;
            Form3 form = new Form3();
            var userproverka = form.vakidateuser(Login, Password);
            string newvalute = comboBox1.Text;
            string dbPath = form.GetDatabasePath();


            if (await userproverka)
            {
                if (_currentCurrency == newvalute)
                {
                    if (!string.IsNullOrEmpty(newvalute))
                    {
                        try
                        {
                            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbPath}"))
                            {
                                await connection.OpenAsync().ConfigureAwait(false);
                                using (var command = new SQLiteCommand($"UPDATE Usersss SET Valute = @NEWvalute WHERE Login = @L", connection))
                                {
                                    command.Parameters.AddWithValue("@L", GlobalData.CurrentLogin);
                                    command.Parameters.AddWithValue("@NEWvalute", newvalute);
                                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                                    MessageBox.Show("Вы успешно сменили валюту!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Не удалось сменить валюту! {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите валюту", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("У вас уже выбрана данная валюта", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Войдите в аккаунт", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }



        // метод смены пароля
        private async Task<bool> smenaparolia()
        {
            string Passwordd = textBox1.Text;
            string parolrepeat = textBox2.Text;
            string Login = GlobalData.CurrentLogin;
            string Password = GlobalData.CurrentPassword;
            Form3 form = new Form3();
            validpassword validpass = new validpassword();
            var proverpapass = validpass.Passwortd(Passwordd);
            string dbPath = form.GetDatabasePath();
            var userproverka = form.vakidateuser(Login, Password);
            string hashpas = form.hashpqpass(Passwordd);

            if (await userproverka)
            {
                if (proverpapass)
                {
                    if (!string.IsNullOrEmpty(Passwordd) && !string.IsNullOrEmpty(parolrepeat))
                    {
                        if (Passwordd == parolrepeat)
                        {
                            try
                            {
                                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbPath}"))
                                {
                                    await connection.OpenAsync().ConfigureAwait(false);
                                    using (var command = new SQLiteCommand($"UPDATE Usersss SET Password = @newPassword WHERE Login = @L", connection))
                                    {
                                        command.Parameters.AddWithValue("@L", Login);
                                        command.Parameters.AddWithValue("@newPassword", hashpas);
                                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                                        MessageBox.Show("Вы успешно сменили пароль", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return true;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Не удалось сменить пароль! {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Пароли не совпадают!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Войдите в аккаунт", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //КНОПКА СМЕНЫ валюты
        private async void button3_Click(object sender, EventArgs e)
        {
            string Valute = comboBox1.Text;
            string Password = GlobalData.CurrentPassword;
            string Login = GlobalData.CurrentLogin;
            await smenavalute();
        }



        // кнопка смены пароля
        private async void button1_Click(object sender, EventArgs e)
        {
            await smenaparolia();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}
