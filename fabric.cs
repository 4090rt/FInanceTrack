using Aspose.Cells;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp4
{
    public class fabric
    {

        public interface Fabricinter
        {
             Task<string> Exportcustompath(string txtFilePath);
        }

        public enum Exportformat
        {
            PDF,
            Excel,
            Word,
            HTML
        }

        // Экспорт в html
        public class ExportHTML : Fabricinter
        {
            // 1 запись байтов из файла в лист 
            private async Task<List<expense>> ReadExpensesFromTxt(string txtFilePath)
            { 
              var expenses = new List<expense>();
                if (File.Exists(txtFilePath))
                {
                    string[] alllines = await File.ReadAllLinesAsync(txtFilePath).ConfigureAwait(false);
                    var linesnotnull = alllines.Where(lines => !string.IsNullOrWhiteSpace(lines)).ToArray();

                    for (int i = 0; i < linesnotnull.Length; i += 3)
                    {
                        if (i + 2 >= linesnotnull.Length)
                            break;

                        string category = linesnotnull[i].Trim();
                        string count = linesnotnull[i + 1].Trim();
                        string date = linesnotnull[i + 2].Trim();

                        try
                        {
                            var newitemsadd = new expense
                            {
                                category = category,
                                count = decimal.Parse(count),
                                date = DateTime.Parse(date)
                            };
                            expenses.Add(newitemsadd);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка: {ex.Message}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Файл для экспорта не найден");
                    return null;
                }
                return expenses;
            }
            //2 метод экспорта в html
            public async Task<string> ExportToHTML(string txtFilePath, string outputPath)
            {
                try
                {
                    var expenses = await ReadExpensesFromTxt(txtFilePath);

                    Workbook workbook = new Workbook();
                    Worksheet worksheet = workbook.Worksheets[0];
                    worksheet.Name = "Расходы";

                    // Заголовки
                    worksheet.Cells["A1"].PutValue("Категория");
                    worksheet.Cells["B1"].PutValue("Сумма");
                    worksheet.Cells["C1"].PutValue("Дата");

                    //данные
                    int row = 2;
                    foreach (var expense in expenses)
                    {
                        worksheet.Cells[$"A{row}"].PutValue(expense.category);
                        worksheet.Cells[$"B{row}"].PutValue(expense.count);
                        worksheet.Cells[$"C{row}"].PutValue(expense.date.ToString("dd.MM.yyyy"));
                        row++;
                    }
                    // Стиль заголовков
                    Aspose.Cells.Style headerStyle = workbook.CreateStyle();
                    headerStyle.Font.IsBold = true;
                    headerStyle.ForegroundColor = System.Drawing.Color.LightBlue;
                    headerStyle.Pattern = BackgroundType.Solid;

                    worksheet.Cells["A1"].SetStyle(headerStyle);
                    worksheet.Cells["B1"].SetStyle(headerStyle);
                    worksheet.Cells["C1"].SetStyle(headerStyle);

                    worksheet.AutoFitColumns();
                    workbook.Save(outputPath, Aspose.Cells.SaveFormat.Html);
                    var a = Exportnotification.ExportHTML;
                    var not = new notificationexport();
                    var exporter = not.Fabricexports(a, outputPath);
                    exporter.Notif();
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}");
                    return "";
                }
            }
            // 3. Главный метод экспорта сохранение
            public async Task<string> Exportcustompath(string txtFilePath)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";
                    saveFileDialog.Title = "Экспрот в HTML";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var outputh = saveFileDialog.FileName;
                        ExportToHTML(txtFilePath, outputh);
                        return outputh;
                    }
                    else
                    {
                        MessageBox.Show($"Export1 вызван с: {txtFilePath}");
                        MessageBox.Show("Отменено пользователем");
                        return "";
                    }
                }
            }
        }

        public class ExportExcel : Fabricinter
        {
            // 1. Чтение из TXT
            private async Task<List<expense>> ReadExpensesFromTxt(string txtFilePath)
            {
                MessageBox.Show(txtFilePath);
                var expenses = new List<expense>();
                if (File.Exists(txtFilePath))
                {
                    try
                    {
                        string[] allLines = await File.ReadAllLinesAsync(txtFilePath).ConfigureAwait(false);
                        var lines = allLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                        //MessageBox.Show($"Найдено непустых строк: {lines.Length}");

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

                        //MessageBox.Show($"Прочитано записей: {expenses.Count}");
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
            public async Task<string> ExportToExcel(string txtFilePath, string outputPath)
            {
                try
                {
                    //MessageBox.Show("Начало экспорта в Excel");

                    List<expense> expenses = await ReadExpensesFromTxt(txtFilePath);

 
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
                    Aspose.Cells.Style headerStyle = workbook.CreateStyle();
                    headerStyle.Font.IsBold = true;
                    headerStyle.ForegroundColor = System.Drawing.Color.LightBlue;
                    headerStyle.Pattern = BackgroundType.Solid;

                    worksheet.Cells["A1"].SetStyle(headerStyle);
                    worksheet.Cells["B1"].SetStyle(headerStyle);
                    worksheet.Cells["C1"].SetStyle(headerStyle);

                    worksheet.AutoFitColumns();
                    workbook.Save(outputPath, Aspose.Cells.SaveFormat.Xlsx);

                    var a = Exportnotification.ExportExcel;
                    var not = new notificationexport();
                    var exporter = not.Fabricexports(a, outputPath);
                    exporter.Notif();
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}");
                    return "";
                }
            }

            // 3. Главный метод экспорта сохранение
            public async Task<string> Exportcustompath(string txtFilePath)
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.Title = "Сохранить Excel файл";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outpath = saveDialog.FileName;
                        ExportToExcel(txtFilePath, outpath);
                        return outpath;
                    }
                    else
                    {
                        MessageBox.Show($"Export1 вызван с: {txtFilePath}");
                        MessageBox.Show("Отменено пользователем");
                        return "";
                    }
                }
            }
        }

        public class ExportPdf : Fabricinter
        {
            // экспорт в PDF
            public async Task<string> ExportToPDF(string txtFilePath,string outputPath)
            {

                try
                {
                    List<expense> expenses = await ReadExpensesFromTxt(txtFilePath);
                    //создание нового документа и страницы
                    Aspose.Pdf.Document doc = new Aspose.Pdf.Document();
                    Aspose.Pdf.Page page = doc.Pages.Add();
                    // настройка страницы
                    TextFragment text = new TextFragment("Расходы");
                    text.TextState.Font = FontRepository.FindFont("Arial");
                    text.TextState.FontSize = 14;
                    page.Paragraphs.Add(text);
                    //создание таблицы
                    Table table = new Table();
                    table.Border = new BorderInfo(BorderSide.All, 1f);
                    Aspose.Pdf.Row row = table.Rows.Add();
                    row.Cells.Add("Категория");
                    row.Cells.Add("Сумма");
                    row.Cells.Add("Дата");

                    try
                    {
                        foreach (var exp in expenses)
                        {
                            Aspose.Pdf.Row datarow = table.Rows.Add();
                            datarow.Cells.Add(exp.category);
                            datarow.Cells.Add(exp.count.ToString("F2"));
                            datarow.Cells.Add(exp.date.ToString("dd.MM.yyyy"));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось создать таблицу для экспорта" + ex.Message);
                    }
                    //добавление таблицы на страницу
                    page.Paragraphs.Add(table);

                    //сохранение файла
                    doc.Save(outputPath, Aspose.Pdf.SaveFormat.Pdf);

                    var a = Exportnotification.ExportPDF;
                    var not = new notificationexport();
                    var exporter = not.Fabricexports(a, outputPath);
                    exporter.Notif();
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось экспортировать в PDF" + ex.Message);
                    return "";
                }
            }

            // чтение из файла в лист
            public async Task<List<expense>> ReadExpensesFromTxt(string txtFilePath)
            {
                MessageBox.Show(txtFilePath);
                var expenses = new List<expense>();
                if (File.Exists(txtFilePath))
                {
                    try
                    {
                        string[] allines = await File.ReadAllLinesAsync(txtFilePath).ConfigureAwait(false);
                        var lines = allines.Where(lines => !string.IsNullOrWhiteSpace(lines)).ToArray();
                        for (int i = 0; i < lines.Length; i += 3)
                        {
                            if (i + 2 >= lines.Length)
                                break;
                            string category = lines[i].Trim();
                            string count = lines[i + 1].Trim();
                            string date = lines[i + 2].Trim();
                            try
                            {
                                var Listadd = new expense
                                {
                                    category = category,
                                    count = decimal.Parse(count),
                                    date = DateTime.Parse(date)
                                };
                                expenses.Add(Listadd);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка в записи {i / 3 + 1}: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show($"Не удалось найти файл для экспорта по пути {txtFilePath}");
                }
                return expenses;
            }

            //3. Главный метод экспорта сохранение
            public async Task<string> Exportcustompath(string txtFilePath)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files|*.PDF";
                    saveFileDialog.Title = "Сохранить PDF файл";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string outputPath = saveFileDialog.FileName;
                        ExportToPDF(txtFilePath, outputPath);
                        return outputPath;
                    }
                    else
                    {
                        MessageBox.Show($"Export1 вызван с: {txtFilePath}");
                        MessageBox.Show("Отменено пользователем");
                        return "";
                    }
                }
            }
        }



        public class ExportWord : Fabricinter
        {
            // экспорт в Word
            public async Task<string> ExportToWord(string txtFilePath, string outputPath)
            {
                try
                {
                    List<expense> expenses = await ReadExpensesFromTxt(txtFilePath);

                    Aspose.Words.Document document = new Aspose.Words.Document();
                    DocumentBuilder builder = new DocumentBuilder(document);

                    builder.Font.Size = 16;
                    builder.Font.Bold = true;
                    builder.Writeln("Отчет о расходах");
                    builder.Writeln();

                    // Таблица
                    builder.Font.Size = 12;
                    builder.Font.Bold = false;

                    Aspose.Words.Tables.Table table = builder.StartTable();
                    builder.InsertCell();
                    builder.Write("Категория");
                    builder.InsertCell();
                    builder.Write("Cумма");
                    builder.InsertCell();
                    builder.Write("Дата");
                    builder.EndRow();

                    foreach (var expense in expenses)
                    {
                        builder.InsertCell();
                        builder.Write(expense.category);
                        builder.InsertCell();
                        builder.Write(expense.count.ToString("F2"));
                        builder.InsertCell();
                        builder.Write(expense.date.ToString("dd.MM.yyyy"));
                        builder.EndRow();
                    }
                    builder.EndTable();
                    document.Save(outputPath);

                    var a = Exportnotification.ExportWord;
                    var not = new notificationexport();
                    var exporter = not.Fabricexports(a, outputPath);
                    exporter.Notif();
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось экспортировать в PDF" + ex.Message);
                    return "";
                }
            }

            // чтение из файла в лист
            public async Task<List<expense>> ReadExpensesFromTxt(string txtFilePath)
            {
                var expenses = new List<expense>();
                if (File.Exists(txtFilePath))
                {
                    try
                    {
                        string[] alllines = await File.ReadAllLinesAsync(txtFilePath).ConfigureAwait(false);
                        var lines = alllines.Where(lines => !string.IsNullOrEmpty(lines)).ToArray();
                        for (int i = 0; i < lines.Length; i += 3)
                        {
                            if (i + 2 >= lines.Length)
                                break;

                            string category = lines[i].Trim();
                            string count = lines[i + 1].Trim();
                            string date = lines[i + 2].Trim();

                            try
                            {
                                var ListAdd = new expense
                                {
                                    category = category,
                                    count = Decimal.Parse(count),
                                    date = DateTime.Parse(date)
                                };
                                expenses.Add(ListAdd);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка в записи {i / 3 + 1}: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show($"Не удалось найти файл для экспорта по пути {txtFilePath}");
                }
                return expenses;
            }

            //3. Главный метод экспорта сохранение
            public async Task<string>  Exportcustompath(string txtFilePath)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Word Documents|*.docx";
                    saveFileDialog.Title = "Сохранить Word файл";
                    try
                    {
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string outputPath = saveFileDialog.FileName;
                            ExportToWord(txtFilePath, outputPath);
                            return outputPath;
                        }
                        else
                        {
                            MessageBox.Show($"Export1 вызван с: {txtFilePath}");
                            MessageBox.Show("Отменено пользователем");
                            return "";
                        }
                    }
                    catch(Exception ex)
                    { 
                        MessageBox.Show(ex.Message + "Возникло в  ExportWord => ExportExcelcustompath ");
                        return "";
                    }
                }
            }
        }
        public class Exports
        {
          public Fabricinter Fabricexports(Exportformat exportformat)
          {
                return exportformat switch
                {
                    Exportformat.Excel => new ExportExcel(),
                    Exportformat.Word => new ExportWord(),
                    Exportformat.PDF => new ExportPdf(),
                    Exportformat.HTML => new ExportHTML(),
                    _ => throw new ArgumentException($"Неизвестный формат {exportformat}")
                };
          }
        }
        
    }
}
