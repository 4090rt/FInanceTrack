using ScottPlot;
using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
                MessageBox.Show("Файл данных создан. Данных для фильтрации нет.");
                return;
            }







            string[] massivdannyx = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
            string[] nolines = massivdannyx.Where(l => !string.IsNullOrEmpty(l)).ToArray();

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








            if (selectedFilter == "День")
            {
                if (selectedfilter2 == "По возрастанию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date == now.Date)
                        .OrderBy(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }
                else if (selectedfilter2 == "По убыванию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date == now.Date)
                        .OrderByDescending(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }

            }









            else if (selectedFilter == "Неделя")
            {
                DateTime periodStart = now.Date.AddDays(-6);

                if (selectedfilter2 == "По возрастанию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                        .OrderBy(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }
                else if (selectedfilter2 == "По убыванию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                        .OrderByDescending(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }

            }








            else if (selectedFilter == "Месяц")
            {
                DateTime periodStart = new DateTime(now.Year, now.Month, 1);

                if (selectedfilter2 == "По возрастанию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                        .OrderBy(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }
                else if (selectedfilter2 == "По убыванию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                        .OrderByDescending(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }
            }








            else if (selectedFilter == "Год")
            {
                DateTime periodStart = new DateTime(now.Year, 1, 1);

                if (selectedfilter2 == "По возрастанию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                        .OrderBy(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }
                else if (selectedfilter2 == "По убыванию затрат")
                {
                    filtered = expenses
                        .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                        .OrderByDescending(r => r.count)
                        .ToList();
                    foreach (var exp in filtered)
                    {
                        var item = new ListViewItem(exp.category);
                        item.SubItems.Add(exp.count.ToString("0.##"));
                        item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                        listView1.Items.Add(item);
                    }
                    formsPlot1.Visible = true;
                    formsPlot1.Plot.Clear();
                    double[] values = filtered.Select(e => (double)e.count).ToArray();
                    string[] labels = filtered.Select(e => e.category).ToArray();
                    double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                    var bars = formsPlot1.Plot.Add.Bars(values);
                    formsPlot1.Plot.Axes.Margins(bottom: 0);
                    formsPlot1.Plot.Title("Расходы по категориям");
                    formsPlot1.Plot.YLabel("Сумма");
                    formsPlot1.Plot.XLabel("Категории");
                    formsPlot1.Refresh();
                }
            }










            if (!filtered.Any())
            {
                MessageBox.Show("Нет данных для отображения по выбранным фильтрам");
            }
            else
            {
                MessageBox.Show($"Отображено {filtered.Count()} записей");
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }
    }

}
    

