using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp4
{
    public partial class smenadannix : Form
    {
        private string _currentCurrency;
        public smenadannix()
        {
            InitializeComponent();
            massivoperaciy();
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

                var currencyService = new CurrencyService(dbPath);
                string userCurrency = await currencyService.GetUserCurrencyAsync(GlobalData.CurrentLogin);

                var currencyInter = await CurrencyFactory.CreateCurrencyServiceAsync(userCurrency);
                string currencyRates = await currencyInter.valutapros(userCurrency);
                _currentCurrency = await currencyInter.valutapros(userCurrency);
                return true;
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
            string dbPath = form.GetDatabasePath();
            var userproverka = form.vakidateuser(Login, Password);
            string hashpas = form.hashpqpass(Passwordd);

            if (await userproverka)
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
