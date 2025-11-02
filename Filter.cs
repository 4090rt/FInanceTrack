using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinFormsApp4.Filter;
using static WinFormsApp4.reposit;

namespace WinFormsApp4
{
    internal class Filter
    {

        public interface FIltred
        {
            List<expense> Filter(List<expense> expenses, DateTime now);
            void DisplayResults(List<expense> filtered, ListView listView, FormsPlot plot);
        }



        public class DayFiltered: FIltred
        {
            private readonly string _selfilter;


            public DayFiltered(string selfilter)
            {
                _selfilter = selfilter;
            }

            public List<expense> Filter(List<expense> expenses, DateTime now)
            {
                if (expenses==null || !expenses.Any())
                    return new List<expense>();
                IEnumerable<expense> filtered = Enumerable.Empty<expense>();

                        filtered = expenses
                        .Where(e => e.date.Date == now.Date)
                        .ToList();
                return _selfilter == "По возрастанию затрат"
                ? filtered.OrderBy(r => r.count).ToList()
                : filtered.OrderByDescending(r => r.count).ToList();

            }
            public void DisplayResults(List<expense> filtered, ListView listView, FormsPlot plot)
            {
                if (filtered == null || !filtered.Any())
                { 
                    plot.Visible =false;
                    return;
                }
                    listView.Items.Clear();

                foreach (var exp in filtered)
                {
                    var item = new ListViewItem(exp.category);
                    item.SubItems.Add(exp.count.ToString("0.##"));
                    item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                    listView.Items.Add(item);
                }

                plot.Visible = true;
                plot.Plot.Clear();
                double[] values = filtered.Select(e => (double)e.count).ToArray();
                string[] labels = filtered.Select(e => e.category).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                var bars = plot.Plot.Add.Bars(values);
                plot.Plot.Axes.Margins(bottom: 0);
                plot.Plot.Title("Расходы по категориям");
                plot.Plot.YLabel("Сумма");
                plot.Plot.XLabel("Категории");
                plot.Refresh();
            }
        }


        public class WeekFiLTERED : FIltred
        {
            private readonly string _selfilter;


            public WeekFiLTERED(string selfilter)
            {
                _selfilter = selfilter;
            }
            public List<expense> Filter(List<expense> expenses, DateTime now)
            {
                if (expenses == null || !expenses.Any())
                    return new List<expense>();
                IEnumerable<expense> filtered = Enumerable.Empty<expense>();
                DateTime periodStart = now.Date.AddDays(-6);
                filtered = expenses
                     .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                     .ToList();
                return _selfilter == "По возрастанию затрат"
                    ? filtered.OrderBy(r => r.count).ToList()
                    : filtered.OrderByDescending(r => r.count).ToList();
            }

            public void DisplayResults(List<expense> filtered, ListView listView, FormsPlot plot)
            {
                if (filtered == null || !filtered.Any())
                {
                    plot.Visible = false;
                    return;
                }
                listView.Clear();
                foreach (var exp in filtered)
                {
                    var item = new ListViewItem(exp.category);
                    item.SubItems.Add(exp.count.ToString("0.##"));
                    item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                    listView.Items.Add(item);
                }
                plot.Visible = true;
                plot.Plot.Clear();
                double[] values = filtered.Select(e => (double)e.count).ToArray();
                string[] labels = filtered.Select(e => e.category).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                var bars = plot.Plot.Add.Bars(values);
                plot.Plot.Axes.Margins(bottom: 0);
                plot.Plot.Title("Расходы по категориям");
                plot.Plot.YLabel("Сумма");
                plot.Plot.XLabel("Категории");
                plot.Refresh();
            }
        }

        public class MesFiLTERED : FIltred
        {
            private readonly string _selfilter;

            public MesFiLTERED(string selfilter)
            {
                _selfilter = selfilter;
            }

            public List<expense> Filter(List<expense> expenses, DateTime now)
            {
                if (expenses == null || !expenses.Any())
                    return new List<expense>();
                IEnumerable<expense> filtered = Enumerable.Empty<expense>();
                DateTime periodStart = new DateTime(now.Year, now.Month, 1);
                filtered = expenses
                       .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                       .ToList();
                return _selfilter == "По возрастанию затрат"
                    ? filtered.OrderBy(r => r.count).ToList()
                    : filtered.OrderByDescending(r => r.count).ToList();
            }

            public void DisplayResults(List<expense> filtered, ListView listView, FormsPlot plot)
            {
                if (filtered == null || !filtered.Any())
                {
                    plot.Visible = false;
                    return;
                }
               listView.Items.Clear();
                foreach (var exp in filtered)
                {
                    var item = new ListViewItem(exp.category);
                    item.SubItems.Add(exp.count.ToString("0.##"));
                    item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                    listView.Items.Add(item);
                }

                plot.Visible = true;
                plot.Plot.Clear();
                double[] values = filtered.Select(e => (double)e.count).ToArray();
                string[] labels = filtered.Select(e => e.category).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                var bars = plot.Plot.Add.Bars(values);
                plot.Plot.Axes.Margins(bottom: 0);
                plot.Plot.Title("Расходы по категориям");
                plot.Plot.YLabel("Сумма");
                plot.Plot.XLabel("Категории");
                plot.Refresh();
            }
        }


        public class YearFiLTERED : FIltred
        {
            private readonly string _selfilter;

            public YearFiLTERED(string selfilter)
            {
                _selfilter = selfilter;
            }

            public List<expense> Filter(List<expense> expenses, DateTime now)
            {
                if (expenses == null || !expenses.Any())
                    return new List<expense>();
                DateTime periodStart = new DateTime(now.Year, 1, 1);
                IEnumerable<expense> filtered = Enumerable.Empty<expense>();
                filtered = expenses
                        .Where(e => e.date.Date >= periodStart && e.date.Date <= now.Date)
                        .ToList();
                return _selfilter == "По возрастанию затрат"
                ? filtered.OrderBy(r => r.count).ToList()
                : filtered.OrderByDescending(r => r.count).ToList();
            }

            public void DisplayResults(List<expense> filtered, ListView listView, FormsPlot plot)
            {
                if (filtered == null || !filtered.Any())
                {
                    plot.Visible = false;
                    return;
                }
                listView.Items.Clear();
                foreach (var exp in filtered)
                {
                    var item = new ListViewItem(exp.category);
                    item.SubItems.Add(exp.count.ToString("0.##"));
                    item.SubItems.Add(exp.date.ToString("dd.MM.yyyy"));
                    listView.Items.Add(item);
                }

                plot.Visible = true;
                plot.Plot.Clear();
                double[] values = filtered.Select(e => (double)e.count).ToArray();
                string[] labels = filtered.Select(e => e.category).ToArray();
                double[] positions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
                var bars = plot.Plot.Add.Bars(values);
                plot.Plot.Axes.Margins(bottom: 0);
                plot.Plot.Title("Расходы по категориям");
                plot.Plot.YLabel("Сумма");
                plot.Plot.XLabel("Категории");
                plot.Refresh();
            }
        }


        public static class FilterStrategyFactory
        {
            public static FIltred CreateFilterStrategy(string period, string sortOrder)
            {
                return period switch
                {
                    "День" => new DayFiltered(sortOrder),
                    "Неделя" => new WeekFiLTERED(sortOrder),
                    "Месяц" => new MesFiLTERED(sortOrder), 
                    "Год" => new YearFiLTERED(sortOrder),    
                    _ => throw new ArgumentException($"Неизвестный период: {period}")
                };
            }
        }
    }
}
