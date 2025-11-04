using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace WinFormsApp4
{
    public partial class Smenaparolyasms : Form
    {
        public string _Login;
        private int _verificationCode;
        public Smenaparolyasms(int verificationCode)
        {
            InitializeComponent();
            _verificationCode = verificationCode;
        }
        // основной метод для передачи данных и использования DI
        private async void button2_Click(object sender, EventArgs e)
        {
            _Login = textBox1.Text;
            if (string.IsNullOrEmpty(_Login))
            {
                MessageBox.Show("Заполните поле Логина для смены пароля");
                return;
            }
            string dbPath = "";
            try
            {
                // получили путь
                try
                {
                    Form3 form3 = new Form3();
                    var dbpat = form3.GetDatabasePath();
                    dbPath = dbpat;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка получения пути к бд" + ex.Message);
                    return;
                }
                if (string.IsNullOrEmpty(dbPath))
                {
                    MessageBox.Show("Не удалось получить путь к базе данных");
                    return;
                }
                try
                {
                    SendMessageemail smenapassworda = new SendMessageemail();
                    // отправляем почту с полученным логином _Login и получаем возращаемый код
                    int generatedCode = await smenapassworda.senamessage(_Login, dbPath);

                    _verificationCode = generatedCode;

                    //MessageBox.Show($"Код сохранен: {_verificationCode}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Возникла ошибка во время отправки кода" + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:" + ex.Message);
                return;
            }
            groupBox1.Visible = false;
            groupBox3.Visible = true;

        }

        // хэширование пароля
        public string Haspass(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
                throw new Exception("Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка хэширования пароля" + ex.Message);
                throw;
            }
        }
        // метод обновления пароля  в базе данных
        public async Task<bool> updatepassword(string password)
        {
            try
            {
                var form3 = new Form3();
                var formm3 = form3.GetDatabasePath();
                string dbPath = formm3;
                string Login = _Login;
                try
                {
                    using (var connection = new SQLiteConnection($"Data Source={dbPath}"))
                    {
                        await connection.OpenAsync().ConfigureAwait(false);
                        //MessageBox.Show($"Обновляем пароль для: {Login}");
                        //MessageBox.Show($"Путь к БД: {dbPath}");
                        string newcommand = "UPDATE Usersss SET Password = @NewPass WHERE Login = @L";
                        using (var sqliitecommand = new SQLiteCommand(newcommand, connection))
                        {
                            sqliitecommand.Parameters.AddWithValue("@L", Login);
                            sqliitecommand.Parameters.AddWithValue("@NewPass", password);
                            await sqliitecommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка cохранения пароля" + ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
                return false;
            }
        }
        // основной метод смены пароля 
        public async Task smenaparolia2()
        { // получили данные
            string password = textBox2.Text;
            string repeatpassword = textBox3.Text;
            var formvalipassword = new validpassword();
            var valid = formvalipassword.Passwortd(password);
            try
            {
                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(repeatpassword))
                {
                    if (password == repeatpassword)
                    {
                        if (valid)
                        {
                            try
                            {
                                // хэшируем пароль с помощью метода хэша
                                string hashpass = Haspass(password);
                                //MessageBox.Show($"Хэш пароля: {hashpass}");
                                try
                                {
                                    // обновляем хэш пароля в базе  и открываем след форму
                                    await updatepassword(hashpass);
                                    MessageBox.Show("Пароль успешно изменен");
                                    Form2 form2 = new Form2();
                                    form2.Show();
                                    this.Hide();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Ошибка сохранения пароля" + ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка хэширования пароля" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пароли не совпадают");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }
        }
        //метод проверки кода доступа
        public void validcode()
        {
            try
            {
                // получили данные
                int codeuser = (int)numericUpDown1.Value;
                int codegen1 = _verificationCode;

                //MessageBox.Show($"Введенный код: {codeuser}, Ожидаемый: {codegen1}");
                // сравнили  и открыли  основной метод смены пароля
                if (codeuser == codegen1)
                {
                    MessageBox.Show("Код верный!");
                    groupBox3.Visible = false;
                    groupBox2.Visible = true;
                }
                else
                {
                    MessageBox.Show("Код не верный!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            validcode();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
           await smenaparolia2();
        }
    }
}
