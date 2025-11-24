using System.Text;
using System.Text.RegularExpressions;

namespace WinFormsApp4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            UpdateUserInterface();
            massivoperaciy();
            notific();
            weather();
            weather2();
            //MessageBox.Show($"Min: {numericUpDown1.Minimum}, Max: {numericUpDown1.Maximum}");
        }


        //массивы для numeric
        public void massivoperaciy()
        {
            string[] massivoperation = { "Переводы", "Мед-услуги", "Магазины", "Транспорт", "Равзлечения", "Подписки", "Прочее", "Показать всё" };
            comboBox1.Items.AddRange(massivoperation);
        }


        public async Task notific()
        {
            
            main m = new main();
            await m.Maulmethod1().ConfigureAwait(false);
        }

        public async Task weather()
        { 
            WeatherHttp weat = new WeatherHttp();
            await weat.Weather().ConfigureAwait(false);
        }

        public async Task weather2()
        {
            Weather2 weat = new Weather2();
            await weat.WEATHER22().ConfigureAwait(false);
        }
        //информация о текущем пользователе
        private void UpdateUserInterface()
        {
            if (GlobalData.IsUserLoggedIn() &&GlobalData.IsUserLoggedInPas())
            {
                this.Text = $"Транзакции - Пользователь: {GlobalData.CurrentLogin}";
            }
            else
            {
                this.Text = "Транзакции - Не авторизован";
            }
        }



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
                label1.Text = time;
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
                            //MessageBox.Show($"Файл записан: {filePath}\nРазмер: {size} байт");
                            MessageBox.Show("Транзакция успешно сохранена!");
                            label1.Text = "";
                            numericUpDown1.Value = 0;
                            comboBox1.SelectedIndex = -1;
                        }
                        else
                        {
                            File.Create(filePath).Dispose();
                            MessageBox.Show("файл Создан", "Успех", MessageBoxButtons.OK);
                            string content = string.Join(Environment.NewLine, new[] { Name, Summ, time }) + Environment.NewLine + Environment.NewLine;
                            await File.AppendAllTextAsync(filePath, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
                            long size = new FileInfo(filePath).Length;
                            //MessageBox.Show($"Файл записан: {filePath}\nРазмер: {size} байт");
                            MessageBox.Show("Транзакция успешно сохранена!");
                            label1.Text = "";
                            numericUpDown1.Value = 0;
                            comboBox1.SelectedIndex = -1;

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



        //выход на основную форму
        private void Logout()
        {
            GlobalData.ClearCurrentUser();
            GlobalData.ClearCurrentUserPas();

            Form3 form3 = new Form3();
            form3.Show();
            this.Close();
        }






        // настройка элементов формы
        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources._6;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Properties.Resources._5;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            linkLabel1.Text = "Сменить данные";
            linkLabel2.Text = "Текущий курс валют";
            numericUpDown1.Maximum = 10000000;
            numericUpDown1.Minimum = 0;
            numericUpDown1.Increment = 1;
            button2.FlatStyle = FlatStyle.Popup;
            button3.FlatStyle = FlatStyle.Popup;
            button1.FlatStyle = FlatStyle.Popup;
        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            await saveTranzaction();
        }



        private async void button2_Click_1(object sender, EventArgs e)
        {
            statistics stat = new statistics();
            stat.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            smenadannix form = new smenadannix();
            form.Show();
            this.Hide();
        }



        private async void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {
                string dbPath = Form3.GetDatabasePath();
                // Получаем валюту пользователя
                var currencyService = new CurrencyService(dbPath);
                string userCurrency = await currencyService.GetUserCurrencyAsync(GlobalData.CurrentLogin);

                // Получаем курсы валют
                try
                {
                    Valuteformat format = new Valuteformat();
                    var currencyInter = await CurrencyFactory.CreateCurrencyServiceAsync(userCurrency);
                    string currencyRates = await currencyInter.valutapros(userCurrency);
                    if (userCurrency == "USD")
                    {
                        if (!string.IsNullOrEmpty(currencyRates))
                        {
                            var forma = Valutessscrypt.USD;
                            var cryptoService = format.Valutecrypt(forma);
                            var result = await cryptoService.Courcevalutecrypt();

                            MessageBox.Show(result);
                            MessageBox.Show(currencyRates, $"Курсы валют для {userCurrency}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось получить курсы валют", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (userCurrency == "EUR")
                    {
                        if (!string.IsNullOrEmpty(currencyRates))
                        {
                            var forma = Valutessscrypt.EUR;
                            var cryptoservice = format.Valutecrypt(forma);
                            var result = await cryptoservice.Courcevalutecrypt();
                            MessageBox.Show(currencyRates, $"Курсы валют для {userCurrency}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show(result.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Не удалось получить курсы валют", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (userCurrency == "RUB")
                    {
                        if (!string.IsNullOrEmpty(currencyRates))
                        {
                            var forma = Valutessscrypt.RUB;
                            var cryptoservice = format.Valutecrypt(forma);
                            var result = await cryptoservice.Courcevalutecrypt();
                            MessageBox.Show(currencyRates, $"Курсы валют для {userCurrency}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show(result.ToString() + $"умножьте на {currencyRates}");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось получить курсы валют", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка получения курсов валют: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения курсов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
