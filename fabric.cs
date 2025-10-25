using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public class fabric
    {

        public interface Fabricinter
        {
            void Export(List<expense> expenses, string filePath); 
            void Export1(string txtFilePath);
        }

        public enum Exportformat
        {
            PDF,
            Excel,
            Word
        }

        public class ExportExcel : Fabricinter
        {
            // 1. Чтение из TXT
            private async Task<List<expense>> ReadExpensesFromTxt(string txtFilePath)
            {
                var expenses = new List<expense>();
                if (File.Exists(txtFilePath))
                {
                    try
                    {
                        string[] allLines = await File.ReadAllLinesAsync(txtFilePath).ConfigureAwait(false);
                        var lines = allLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                        MessageBox.Show($"Найдено непустых строк: {lines.Length}");

                        for (int i = 0; i < lines.Length; i += 3)
                        {
                            if (i + 2 >= lines.Length)
                                break;

                            string category = lines[i].Trim();
                            string countStr = lines[i + 1].Trim();
                            string dateStr = lines[i + 2].Trim();

                            try
                            {
                                var expenseItem = new expense
                                {
                                    category = category,
                                    count = decimal.Parse(countStr),
                                    date = DateTime.Parse(dateStr)
                                };
                                expenses.Add(expenseItem);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка в записи {i / 3 + 1}: {ex.Message}");
                            }
                        }

                        MessageBox.Show($"Прочитано записей: {expenses.Count}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл для экспорта не найден");
                    return null;
                }

                return expenses;
            }

            // 2. Экспорт в Excel
            public async Task ExportToExcel(string txtFilePath, string outputPath)
            {
                try
                {
                    MessageBox.Show("Начало экспорта в Excel");

                    List<expense> expenses = await ReadExpensesFromTxt(txtFilePath);

                    if (expenses.Count == 0)
                    {
                        MessageBox.Show("Нет данных для экспорта!");
                        return;
                    }

                    Workbook workbook = new Workbook();
                    Worksheet worksheet = workbook.Worksheets[0];
                    worksheet.Name = "Расходы";

                    // Заголовки
                    worksheet.Cells["A1"].PutValue("Категория");
                    worksheet.Cells["B1"].PutValue("Сумма");
                    worksheet.Cells["C1"].PutValue("Дата");

                    // Данные
                    int row = 2;
                    foreach (var expense in expenses)
                    {
                        worksheet.Cells[$"A{row}"].PutValue(expense.category);
                        worksheet.Cells[$"B{row}"].PutValue(expense.count);
                        worksheet.Cells[$"C{row}"].PutValue(expense.date.ToString("dd.MM.yyyy"));
                        row++;
                    }

                    // Стиль заголовков
                    Style headerStyle = workbook.CreateStyle();
                    headerStyle.Font.IsBold = true;
                    headerStyle.ForegroundColor = System.Drawing.Color.LightBlue;
                    headerStyle.Pattern = BackgroundType.Solid;

                    worksheet.Cells["A1"].SetStyle(headerStyle);
                    worksheet.Cells["B1"].SetStyle(headerStyle);
                    worksheet.Cells["C1"].SetStyle(headerStyle);

                    worksheet.AutoFitColumns();
                    workbook.Save(outputPath, SaveFormat.Xlsx);
                    

                    MessageBox.Show($"Файл сохранен: {outputPath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}");
                }
            }

            // 3. Главный метод экспорта
            public void Export1(string txtFilePath)
            {
                MessageBox.Show($"Export1 вызван с: {txtFilePath}");

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.Title = "Сохранить Excel файл";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToExcel(txtFilePath, saveDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Отменено пользователем");
                    }
                }
            }

            // Не используется
            public void Export(List<expense> expenses, string filePath)
            {
            }
        }

        //public class ExportPdf : Fabricinter
        //{
        //    public void Export(List<expense> expenses, string filePath)
        //    {

        //    }
        //}

        //public class ExportWord : Fabricinter
        //{
        //    public void Export(List<expense> expenses, string filePath)
        //    {

        //    }
        //}

        public class Exports
        {
            public Fabricinter Favricexports(Exportformat exportformat)
            {
                return exportformat switch
                {
                    Exportformat.Excel => new ExportExcel(),
                    //Exportformat.Word => new ExportWord(),
                    //Exportformat.PDF => new ExportPdf()
                };
            }
        }
    }
}
