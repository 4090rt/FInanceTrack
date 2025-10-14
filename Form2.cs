using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
            massivoperaciy();
            comboBox2.SelectedIndexChanged += async (s, e) => await filtretranzaction();
            filtretranzaction();
            valutelocal();
            //MessageBox.Show($"Min: {numericUpDown1.Minimum}, Max: {numericUpDown1.Maximum}");
        }


        //отображение текущей валюты
        public async Task<bool> valutelocal()
        {
            try
            {
                Form3 form = new Form3();
                string dbPath = form.GetDatabasePath();

                var currencyService = new CurrencyService(dbPath);
                string userCurrency = await currencyService.GetUserCurrencyAsync(GlobalData.CurrentLogin);

                label1.Text = $"Текущая валюта: {userCurrency}";

                var currencyInter = await CurrencyFactory.CreateCurrencyServiceAsync(userCurrency);
                string currencyRates = await currencyInter.valutapros(userCurrency);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось определить текущую валюту: {ex.Message}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }




        //основной метод фильтрации
        public async Task filtretranzaction()
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                    return;
                }

                string[] lines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
                string[] nonEmptyLines = lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
                List<expense> expenses = new List<expense>();

                for (int i = 0; i < nonEmptyLines.Length; i += 3)
                {
                    if (i + 2 < nonEmptyLines.Length && !string.IsNullOrWhiteSpace(nonEmptyLines[i]))
                    {
                        expenses.Add(new expense
                        {
                            category = nonEmptyLines[i].Trim(),
                            count = decimal.Parse(nonEmptyLines[i + 1].Trim()),
                            date = DateTime.Parse(nonEmptyLines[i + 2].Trim()),
                        });
                    }
                }


                // фильтр 1
                listView1.Items.Clear();
                string selectedFilter = comboBox2.Text?.Trim();
                if (selectedFilter == "Показать только Транспорт")
                {
                    var filteredTransport = expenses.Where(e => e.category == "Транспорт");
                    foreach (var exp in filteredTransport)
                    {
                        var item65 = new ListViewItem(exp.category);
                        item65.SubItems.Add(exp.count.ToString("0.##"));
                        item65.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item65);
                    }
                }


                // фильтр 2
                else if (selectedFilter == "Показать только Переводы")
                {
                    var filteredperevod = expenses.Where(e => e.category == "Переводы");
                    foreach (var exp in filteredperevod)
                    {
                        var item74 = new ListViewItem(exp.category);
                        item74.SubItems.Add(exp.count.ToString("0.##"));
                        item74.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item74);
                    }
                }



                // фильтр 3
                else if (selectedFilter == "Показать только Мед-услуги")
                {
                    var filteredperevod = expenses.Where(e => e.category == "Мед-услуги");
                    foreach (var exp in filteredperevod)
                    {
                        var item83 = new ListViewItem(exp.category);
                        item83.SubItems.Add(exp.count.ToString("0.##"));
                        item83.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item83);
                    }
                }


                // фильтр 4
                else if (selectedFilter == "Показать только Магазины")
                {
                    var filteredperevod = expenses.Where(e => e.category == "Магазины");
                    foreach (var exp in filteredperevod)
                    {
                        var item92 = new ListViewItem(exp.category);
                        item92.SubItems.Add(exp.count.ToString("0.##"));
                        item92.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item92);
                    }
                }


                // фильтр 5
                else if (selectedFilter == "Показать только Равзлечения")
                {
                    var filteredperevod = expenses.Where(e => e.category == "Развлечения");
                    foreach (var exp in filteredperevod)
                    {
                        var item101 = new ListViewItem(exp.category);
                        item101.SubItems.Add(exp.count.ToString("0.##"));
                        item101.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item101);
                    }
                }



                // фильтр 6
                else if (selectedFilter == "Показать только Подписки")
                {
                    var filteredperevod = expenses.Where(e => e.category == "Подписки");
                    foreach (var exp in filteredperevod)
                    {
                        var item110 = new ListViewItem(exp.category);
                        item110.SubItems.Add(exp.count.ToString("0.##"));
                        item110.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item110);
                    }
                }



                // фильтр 7
                else if (selectedFilter == "Показать только Прочее")
                {
                    var filteredperevod = expenses.Where(e => e.category == "Прочее");
                    foreach (var exp in filteredperevod)
                    {
                        var item119 = new ListViewItem(exp.category);
                        item119.SubItems.Add(exp.count.ToString("0.##"));
                        item119.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item119);
                    }
                }


                // фильтр 8
                else if (selectedFilter == "Показать всё")
                {
                    var filteredperevod = expenses;
                    foreach (var exp in filteredperevod)
                    {
                        var item128 = new ListViewItem(exp.category);
                        item128.SubItems.Add(exp.count.ToString("0.##"));
                        item128.SubItems.Add(exp.date.ToString("g"));
                        listView1.Items.Add(item128);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //информация о текущем пользователе
        private void UpdateUserInterface()
        {
            if (GlobalData.IsUserLoggedIn() && GlobalData.IsUserLoggedInPas())
            {
                this.Text = $"Транзакции - Пользователь: {GlobalData.CurrentLogin}";
            }
            else
            {
                this.Text = "Транзакции - Не авторизован";
            }
        }




        //public async Task<bool> log()
        //{
        //    try
        //    {
        //        var mc = new Form1();
        //        string dbPath = mc.GetDatabasePath();
        //        using (var das = new SQLiteConnection($"Data Source={dbPath}"))
        //        {
        //            await das.OpenAsync().ConfigureAwait(false);
        //            var dass = new SQLiteCommand($"SELECT Name, Summ, What FROM Usersss", das);
        //            var reader = await dass.ExecuteReaderAsync().ConfigureAwait(false);

        //            listBox1.Items.Clear();
        //            while (await reader.ReadAsync())
        //            {
        //                string name = reader["Name"].ToString();
        //                string summ = reader["Summ"].ToString();
        //                string what = reader["What"].ToString();
        //                listBox1.Items.Add($"{name} - {summ} - {what}");
        //            }
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка загрузки данных {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //}


        // сохранение картинки
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




        //массивы для numeric
        public void massivoperaciy()
        {
            string[] massivoperation = { "Переводы", "Мед-услуги", "Магазины", "Транспорт", "Равзлечения", "Подписки", "Прочее", "Показать всё" };
            comboBox1.Items.AddRange(massivoperation);
            string[] massivfilter = { "Показать всё", "Показать только Переводы", "Показать только Мед-услуги", "Показать только Магазины", "Показать только Транспорт", "Показать только Равзлечения", "Показать только Подписки", "Показать только Прочее" };
            comboBox2.Items.AddRange(massivfilter);
        }




        //public async Task saveTranzaction()
        //{
        //    // Проверяем, что пользователь авторизован
        //    if (!GlobalData.IsUserLoggedIn())
        //    {
        //        MessageBox.Show("Вы не авторизованы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    string Name = textBox1.Text;
        //    string Summ = textBox2.Text;
        //    string What = textBox3.Text;

        //    if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Summ) && !string.IsNullOrEmpty(What))
        //    {
        //        try
        //        {
        //            var mc = new Form1();
        //            string dbPath = mc.GetDatabasePath();
        //             using (var das = new SQLiteConnection($"Data Source={dbPath}"))
        //            {
        //                await das.OpenAsync().ConfigureAwait(false);
        //                var sqlcommand = new SQLiteCommand(
        //                "UPDATE Usersss SET Name = @N, Summ = @S, What = @W WHERE Login = @L", das);
        //                sqlcommand.Parameters.AddWithValue("@N", Name);
        //                sqlcommand.Parameters.AddWithValue("@S", Summ);
        //                sqlcommand.Parameters.AddWithValue("@W", What);
        //                sqlcommand.Parameters.AddWithValue("@L", GlobalData.CurrentLogin);
        //                await sqlcommand.ExecuteNonQueryAsync().ConfigureAwait(false);
        //                MessageBox.Show($"Данные сохранены для пользователя: {GlobalData.CurrentLogin}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка сохранения данных {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}




        //сохранение информации о транзакции
        public async Task saveTranzaction()
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");
                DateTime selected = monthCalendar1.SelectionStart;
                string Name = comboBox1.Text;
                string time = selected.ToString("yyyy-MM-dd");
                label2.Text = time;
                Regex regex = new Regex(@"\p{L}");
                string Summ = ((int)numericUpDown1.Value).ToString();
                bool regexx = regex.IsMatch(Summ);
                if (regexx == false)
                {
                    if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Summ) && !string.IsNullOrWhiteSpace(time))
                    {
                        if (File.Exists(filePath))
                        {
                            string content = string.Join(Environment.NewLine, new[] { Name, Summ, time }) + Environment.NewLine + Environment.NewLine;
                            await File.AppendAllTextAsync(filePath, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
                            long size = new FileInfo(filePath).Length;
                            MessageBox.Show($"Файл записан: {filePath}\nРазмер: {size} байт");
                        }
                        else
                        {
                            File.Create(filePath).Dispose();
                            MessageBox.Show("файл Создан", "Успех", MessageBoxButtons.OK);
                            string content = string.Join(Environment.NewLine, new[] { Name, Summ, time }) + Environment.NewLine + Environment.NewLine;
                            await File.AppendAllTextAsync(filePath, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
                            long size = new FileInfo(filePath).Length;
                            MessageBox.Show($"Файл записан: {filePath}\nРазмер: {size} байт");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все строки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите число", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eror" + ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        //показать транзакции
        public async Task pokazTranzaction()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");

            if (File.Exists(filePath))
            {
                string a = await File.ReadAllTextAsync(filePath);
                listView1.Items.Clear();
                string[] lines = a.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i + 2 < lines.Length; i += 3)
                {
                    string category = lines[i].Trim();
                    if (!decimal.TryParse(lines[i + 1].Trim(), out var amount)) continue;
                    if (!DateTime.TryParse(lines[i + 2].Trim(), out var dt)) continue;

                    var item = new ListViewItem(category);
                    item.SubItems.Add(amount.ToString("0.##"));
                    item.SubItems.Add(dt.ToString("g"));
                    listView1.Items.Add(item);
                }
            }
        }

        //обработчик показать/скрыть календарь
        int i = 0;
        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            i++;
            if (i % 2 != 0)
                monthCalendar1.Visible = true;
            if (i % 2 == 0)
                monthCalendar1.Visible = false;
        }


        //сохранение картинки транзакции по нажатию
        private void button1_Click(object sender, EventArgs e)
        {
            saveTranzactiewonimage();
        }

        //отображение транзакции по нажатию
        private async void button2_Click(object sender, EventArgs e)
        {
            await pokazTranzaction();
        }



        //выход на основную форму
        private void Logout()
        {
            GlobalData.ClearCurrentUser();
            GlobalData.ClearCurrentUserPas();

            Form3 form3 = new Form3();
            form3.Show();
            this.Close();
        }



        // выход по нажатию
        private void button3_Click(object sender, EventArgs e)
        {
            Logout();
        }




        //очистка информации о юзере
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            GlobalData.ClearCurrentUser();
            GlobalData.ClearCurrentUserPas();
            base.OnFormClosing(e);
        }




        //кнопка очистки listbox
        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }


        // настройка элементов формы
        private void Form2_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = 10000000;
            numericUpDown1.Minimum = 0;
            numericUpDown1.Increment = 1;
            // Настройка ListView на режим таблицы из 3 столбцов
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("Категория", 150);
                listView1.Columns.Add("Сумма", 100, HorizontalAlignment.Right);
                listView1.Columns.Add("Дата", 150);
            }
        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            await saveTranzaction();
        }

        //обработчик кнопки курсов валют
        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Form3 form = new Form3();
                string dbPath = form.GetDatabasePath();

                // Получаем валюту пользователя
                var currencyService = new CurrencyService(dbPath);
                string userCurrency = await currencyService.GetUserCurrencyAsync(GlobalData.CurrentLogin);

                // Получаем курсы валют
                var currencyInter = await CurrencyFactory.CreateCurrencyServiceAsync(userCurrency);
                string currencyRates = await currencyInter.valutapros(userCurrency);

                if (!string.IsNullOrEmpty(currencyRates))
                {
                    MessageBox.Show(currencyRates, $"Курсы валют для {userCurrency}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось получить курсы валют", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения курсов валют: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            smenadannix form = new smenadannix();
            form.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            statistics stat = new statistics();
            stat.Show();
            this.Hide();
        }
    }
}
