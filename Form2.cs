using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            UpdateUserInterface();
        }
        
        private void UpdateUserInterface()
        {
            if (GlobalData.IsUserLoggedIn())
            {
                this.Text = $"Транзакции - Пользователь: {GlobalData.CurrentLogin}";
            }
            else
            {
                this.Text = "Транзакции - Не авторизован";
            }
        }
        public async Task<bool> log()
        {
            try
            {
                var mc = new Form1();
                string dbPath = mc.GetDatabasePath();
                using (var das = new SQLiteConnection($"Data Source={dbPath}"))
                {
                    await das.OpenAsync().ConfigureAwait(false);
                    var dass = new SQLiteCommand($"SELECT Name, Summ, What FROM Usersss", das);
                    var reader = await dass.ExecuteReaderAsync().ConfigureAwait(false);
                    
                    listBox1.Items.Clear();
                    while (await reader.ReadAsync())
                    {
                        string name = reader["Name"].ToString();
                        string summ = reader["Summ"].ToString();
                        string what = reader["What"].ToString();
                        listBox1.Items.Add($"{name} - {summ} - {what}");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task saveTranzactiewonimage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            dialog.Title = "Выберите изображение";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                Image image = Image.FromFile(filePath);
                pictureBox2.Image = image;
            }
            else
            {
                MessageBox.Show("Ошибка выбора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async Task Selectalltranzaction()
        {

        }

        public async Task saveTranzaction()
        {
            // Проверяем, что пользователь авторизован
            if (!GlobalData.IsUserLoggedIn())
            {
                MessageBox.Show("Вы не авторизованы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string Name = textBox1.Text;
            string Summ = textBox2.Text;
            string What = textBox3.Text;
            
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Summ) && !string.IsNullOrEmpty(What))
            {
                try
                {
                    var mc = new Form1();
                    string dbPath = mc.GetDatabasePath();
                    using (var das = new SQLiteConnection($"Data Source={dbPath}"))
                    {
                        await das.OpenAsync().ConfigureAwait(false);
                        var sqlcommand = new SQLiteCommand(
                        "UPDATE Usersss SET Name = @N, Summ = @S, What = @W WHERE Login = @L", das);
                        sqlcommand.Parameters.AddWithValue("@N", Name);
                        sqlcommand.Parameters.AddWithValue("@S", Summ);
                        sqlcommand.Parameters.AddWithValue("@W", What);
                        sqlcommand.Parameters.AddWithValue("@L", GlobalData.CurrentLogin);
                        await sqlcommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                        MessageBox.Show($"Данные сохранены для пользователя: {GlobalData.CurrentLogin}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения данных {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await saveTranzaction();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveTranzactiewonimage();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await log();
        }
        
        private void Logout()
        {
            GlobalData.ClearCurrentUser();
            
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            Logout();
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            GlobalData.ClearCurrentUser();
            base.OnFormClosing(e);
        }
    }
}
