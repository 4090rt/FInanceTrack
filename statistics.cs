using ScottPlot;
using System.Data;
using System.Globalization;
using System.Text;
using static WinFormsApp4.fabric;
using static WinFormsApp4.Filter;

namespace WinFormsApp4
{
    public partial class statistics : Form
    {
        public statistics()
        {
            InitializeComponent();
            massivdays();
            // Запрет ввода произвольного текста, только выбор из списка
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndexChanged += async (s, e) => { await filtretranzaction(); };
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndexChanged += async (s, e) => { await filtretranzaction(); };
            filtretranzaction();
            filtretranzaction2();
            massivcombfabric();
        }





        //массивы для комбобоксов
        public void massivdays()
        {
            string[] days = { "День", "Неделя", "Месяц", "Год" };
            comboBox1.Items.AddRange(days);
            string[] bolshemenshe = { "По возрастанию затрат", "По убыванию затрат" };
            comboBox2.Items.AddRange(bolshemenshe);
        }





        //основной метод для отображения статистики
        public async Task filtretranzaction2()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
                MessageBox.Show("Файл данных создан. Данных для фильтрации нет.");
                return;
            }






            string[] massiv1 = await File.ReadAllLinesAsync(filePath);
            string[] nolines = massiv1.Where(l => !string.IsNullOrEmpty(l)).ToArray();

            List<expense> expenses = new List<expense>();
            for (int i = 0; i < nolines.Length; i += 3)
            {
                if (i + 2 < nolines.Length && !string.IsNullOrWhiteSpace(nolines[i]))
                {
                    string category = nolines[i].Trim();
                    decimal amount = decimal.Parse(nolines[i + 1].Trim());
                    DateTime dateValue = DateTime.Parse(nolines[i + 2].Trim());

                    expenses.Add(new expense
                    {
                        category = category,
                        count = amount,
                        date = dateValue,
                    });
                }
            }
            IEnumerable<expense> filtered = Enumerable.Empty<expense>();






            decimal MAX = expenses.Max(x => x.count);
            var maxExpense = expenses.FirstOrDefault(x => x.count == MAX);

            if (maxExpense != null)
            {
                label5.Text = "Максимальная трата\n" +
                              $"Категория: {maxExpense.category}\n" +
                              $"Сумма: {maxExpense.count.ToString("0.##")}\n" +
                              $"Дата: {maxExpense.date.ToString("dd.MM.yyyy")}";
            }
            else
            {
                label5.Text = "Расходы не найдены";
            }











            decimal min = expenses.Min(x => x.count);
            var minExpense = expenses.FirstOrDefault(x => x.count == min);

            if (minExpense != null)
            {
                label4.Text = "Минимальная трата\n" +
                              $"Категория: {minExpense.category}\n" +
                              $"Сумма: {minExpense.count.ToString("0.##")}\n" +
                              $"Дата: {minExpense.date.ToString("dd.MM.yyyy")}";
            }
            else
            {
                label4.Text = "Расходы не найдены";
            }







            var popular = expenses
                .GroupBy(e => e.category)
                .Select(x => new
                {
                    Categore = x.Key,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            if (popular.Any())
            {
                var mostPopular = popular.First();
                label1.Text = $"{mostPopular.Categore} (использована {mostPopular.Count} раз)";


            }
            else
            {
                label1.Text = "Нет данных о расходах";
            }





            List<PieSlice> slices = new()
            {
                new PieSlice() {
                    Value = (double)min,
                    FillColor = Colors.Red,
                    Label = $"Min: {minExpense?.category}"
                },
                new PieSlice() {
                    Value = (double)MAX,
                    FillColor = Colors.Blue,
                    Label = $"Max: {maxExpense?.category}"
                },
            };

            formsPlot2.Plot.Clear();
            formsPlot2.Visible = true;
            var pie = formsPlot2.Plot.Add.Pie(slices);
            pie.DonutFraction = .5;

            formsPlot2.Plot.ShowLegend();

            formsPlot2.Plot.Axes.Frameless();
            formsPlot2.Plot.HideGrid();

            formsPlot2.Refresh();
        }





        //основной метод для отображения и фильтрации статистики по давности/убыванию и возрастанию расходов
        public async Task filtretranzaction()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");

            //MessageBox.Show($"Файл существует: {File.Exists(filePath)}");
            //MessageBox.Show($"Ожидаемый путь: {filePath}");
            //MessageBox.Show($"CurrentLogin: '{GlobalData.CurrentLogin}'");

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
                MessageBox.Show("Файл данных создан. Данных для фильтрации нет.");
                return;
            }
            FileInfo fileinfo = new FileInfo(filePath);
            if (fileinfo.Length == 0)
            {
                MessageBox.Show("Файл существует, но пуст");
            }

            string[] massivdannyx = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
            string[] nolines = massivdannyx.Where(l => !string.IsNullOrEmpty(l)).ToArray();
            //MessageBox.Show($"Прочитано строк из файла: {massivdannyx.Length}, после фильтрации: {nolines.Length}");

            List<expense> expenses = new List<expense>();
            for (int i = 0; i < nolines.Length; i += 3)
            {
                if (i + 2 < nolines.Length && !string.IsNullOrWhiteSpace(nolines[i]))
                {
                    string category = nolines[i].Trim();

                    // Безопасный парсинг
                    if (!decimal.TryParse(nolines[i + 1].Trim(), out decimal amount))
                    {
                        continue;
                    }

                    string dateRaw = nolines[i + 2].Trim();
                    DateTime dateValue;

                    if (!DateTime.TryParseExact(dateRaw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                    {
                        if (!DateTime.TryParse(dateRaw, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue))
                        {
                            continue;
                        }
                    }

                    expenses.Add(new expense
                    {
                        category = category,
                        count = amount,
                        date = dateValue,
                    });
                }
            }


            listView1.Items.Clear();

            string selectedFilter = comboBox1.Text?.Trim();
            string selectedfilter2 = comboBox2.Text?.Trim();

            if (string.IsNullOrWhiteSpace(selectedFilter) || string.IsNullOrWhiteSpace(selectedfilter2))
            {
                return;
            }

            DateTime now = DateTime.Now;
            IEnumerable<expense> filtered = Enumerable.Empty<expense>();
            MessageBox.Show($"Фильтр: {selectedFilter}, Сортировка: {selectedfilter2}, Всего записей: {expenses.Count}");



            var filteredd = FilterStrategyFactory.CreateFilterStrategy(selectedFilter, selectedfilter2);
            var filtereddd = filteredd.Filter(expenses, now);
            filteredd.DisplayResults(filtereddd, listView1, formsPlot1);
            MessageBox.Show(filtereddd.Count().ToString());


            if (!filtereddd.Any())
            {
                MessageBox.Show("Нет данных для отображения по выбранным фильтрам");
            }
        }





        // метод настройки элементов дизайна
        private void statistics_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.HorizontalAlignment alignment1 = System.Windows.Forms.HorizontalAlignment.Right;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("Категория", 150);
                listView1.Columns.Add("Сумма", 100, alignment1);
                listView1.Columns.Add("Дата", 150);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }



        public void massivcombfabric()
        {
            string[] massiv = ["Экспорт Excel", "Экспорт PDF", "Экспорт Word", "Экспорт HTML"];
            comboBox3.Items.AddRange(massiv);
        }


        private async void label6_Click(object sender, EventArgs e)
        {
            
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string txtFilePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");
            //MessageBox.Show($"Пытаемся открыть: {txtFilePath}");
            if (string.IsNullOrEmpty(comboBox3.Text))
            {
                MessageBox.Show("Выберите формат для экспорта");
                return;
            }

            if (comboBox3.Text == "Экспорт PDF")
            {
                var exportformat = Exportformat.PDF;
                var factory = new Exports();
                var exporter = factory.Fabricexports(exportformat);
                string path = await exporter.Exportcustompath(txtFilePath);
                var clas = new ExporttoPdf(path);
            }

            if (comboBox3.Text == "Экспорт Excel")
            {
                var exportformat = Exportformat.Excel;
                var factory = new Exports();
                var exporter = factory.Fabricexports(exportformat);
                string path = await exporter.Exportcustompath(txtFilePath);
                var clas = new ExporttoExcel(path);  
            }

            if (comboBox3.Text == "Экспорт Word")
            {
                var exportformat = Exportformat.Word;
                var factory = new Exports();
                var exporter = factory.Fabricexports(exportformat);
                string path = await exporter.Exportcustompath(txtFilePath);
                var clas = new ExporttoWord(path);
            }

            if (comboBox3.Text == "Экспорт HTML")
            {
                var exportformat = Exportformat.HTML;
                var factory = new Exports();
                var exporter = factory.Fabricexports(exportformat);
                string path = await exporter.Exportcustompath(txtFilePath);
                var clas = new ExporttoHTML(path);
            }
        }
    }

}


