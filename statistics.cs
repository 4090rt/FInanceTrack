using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Globalization;

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
        }

        //массивы для комбобоксов
        public void massivdays()
        {
            string[] days = { "День", "Неделя", "Месяц", "Год" };
            comboBox1.Items.AddRange(days);
            string[] bolshemenshe = {"По возрастанию затрат", "По убыванию затрат"};
            comboBox2.Items.AddRange(bolshemenshe);
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
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();

            string selectedFilter = comboBox1.Text?.Trim();
            string selectedfilter2 = comboBox2.Text?.Trim();

            if (string.IsNullOrWhiteSpace(selectedFilter) || string.IsNullOrWhiteSpace(selectedfilter2))
            {
                MessageBox.Show("Выберите период и тип сортировки");
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
                        listView2.Items.Add(item);
                    }
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
                        listView2.Items.Add(item);
                    }
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
                        listView3.Items.Add(item);
                    }
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
                        listView3.Items.Add(item);
                    }
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
                        listView4.Items.Add(item);
                    }
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
                        listView4.Items.Add(item);
                    }
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
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("Категория", 150);
                listView1.Columns.Add("Сумма", 100, HorizontalAlignment.Right);
                listView1.Columns.Add("Дата", 150);
            }

            listView2.View = View.Details;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;
            if (listView2.Columns.Count == 0)
            {
                listView2.Columns.Add("Категория", 150);
                listView2.Columns.Add("Сумма", 100, HorizontalAlignment.Right);
                listView2.Columns.Add("Дата", 150);
            }

            listView3.View = View.Details;
            listView3.FullRowSelect = true;
            listView3.GridLines = true;
            if (listView3.Columns.Count == 0)
            {
                listView3.Columns.Add("Категория", 150);
                listView3.Columns.Add("Сумма", 100, HorizontalAlignment.Right);
                listView3.Columns.Add("Дата", 150);
            }

            listView4.View = View.Details;
            listView4.FullRowSelect = true;
            listView4.GridLines = true;
            if (listView4.Columns.Count == 0)
            {
                listView4.Columns.Add("Категория", 150);
                listView4.Columns.Add("Сумма", 100, HorizontalAlignment.Right);
                listView4.Columns.Add("Дата", 150);
            }
        }
    }

}
    

