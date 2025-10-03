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
        }
        public async Task saveTranzactiewon()
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
        }


        public async Task saveTranzaction()
        {
            string Name = textBox1.Text;
            string Summ = textBox2.Text;
            string What = textBox3.Text;
            var mc = new Form1();
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Summ) && !string.IsNullOrEmpty(What))
            {
                try
                {
                    string dbPath = mc.GetDatabasePath();
                    using (var das = new SQLiteConnection($"Data Source={dbPath}"))
                    {
                        await das.OpenAsync().ConfigureAwait(false);
                        var sqlcommand = new SQLiteCommand($"INSERT INTO [TRANZACTION] (Name,Summ,What) VALUES (@N,@S,@W)", das);
                        sqlcommand.Parameters.AddWithValue("@N", Name);
                        sqlcommand.Parameters.AddWithValue("@S", Summ);
                        sqlcommand.Parameters.AddWithValue("@W", What);
                        await sqlcommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                        MessageBox.Show("Данные сохранены");
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
            saveTranzactiewon();
        }
    }
}
