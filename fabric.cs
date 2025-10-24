using Aspose.Cells;
using System;
using System.Collections.Generic;
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

        // 2 экпортируем из листа в Excel создавая новую книгу, столбики и записываем туда данные из лист,настраиваем стиль, авторазмер и сохраняем
        //public class ExportExcel:Fabricinter
        //{
        //    public void ExportToExcel(string outputPath)
        //    {
        //        List<expense> expenses = ReadExpensesFromTxt();
        //        MessageBox.Show($"Прочитано записей из TXT: {expenses.Count}");
        //        Workbook exporTbook = new Workbook();
        //        Worksheet worksheet = exporTbook.Worksheets[0];
        //        worksheet.Name = "Расходы";

        //        worksheet.Cells["A1"].PutValue("Категория");
        //        worksheet.Cells["B1"].PutValue("Сумма");
        //        worksheet.Cells["C1"].PutValue("Дата");

        //        int row = 2;
        //        int addedCount = 0;
        //        foreach (var expensee in expenses)
        //        {
        //            worksheet.Cells[$"A{row}"].PutValue(expensee.category);
        //            worksheet.Cells[$"B{row}"].PutValue(expensee.count);
        //            worksheet.Cells[$"C{row}"].PutValue(expensee.date);
        //            row++;
        //            addedCount++;
        //        }
        //        MessageBox.Show($"Добавлено строк в Excel: {addedCount}"); // ← ДОБАВЬ ЭТО
        //        Style workshet = exporTbook.CreateStyle();
        //        workshet.Font.IsBold = true;
        //        workshet.ForegroundColor = System.Drawing.Color.LightBlue;
        //        workshet.Pattern = BackgroundType.Solid;


        //        for (int stl = 0; stl < 4; stl++)
        //        {
        //            worksheet.Cells[0, 4].SetStyle(workshet);
        //        }

        //        worksheet.AutoFitColumns();
        //        MessageBox.Show($"Сохраняем файл по пути: {outputPath}"); // ← ДОБАВЬ ЭТО
        //        exporTbook.Save(outputPath, SaveFormat.Xlsx);
        //        MessageBox.Show("Сохранение завершено!"); // ← ДОБАВЬ ЭТО
        //    }

        //    // 1 сначала читаем txt из в маасив и записываем в лист
        //    private List<expense> ReadExpensesFromTxt()
        //    {
        //        var expenses = new List<expense>();
        //        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        string correctFilePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");
        //        MessageBox.Show($"Пытаемся прочитать файл: {correctFilePath}"); // ← ДОБАВЬ
        //        MessageBox.Show($"Файл существует: {File.Exists(correctFilePath)}"); // ← ДОБАВЬ
        //        try
        //        {
        //            string[] lines = File.ReadAllLines(correctFilePath);
        //            MessageBox.Show($"Найдено строк в файле: {lines.Length}"); // ← ДОБАВЬ
        //            foreach (string line in lines)
        //            {

        //                if (string.IsNullOrWhiteSpace(line))
        //                    continue;
        //                MessageBox.Show($"Обрабатываем строку: {line}");
        //                string[] parts = line.Split('|');
        //                MessageBox.Show($"Разделено на частей: {parts.Length}"); // ← ДОБАВЬ
        //                if (parts.Length >= 3)
        //                {
        //                    var expenseItem = new expense
        //                    {
        //                        category = parts[0].Trim(),
        //                        count = decimal.Parse(parts[1].Trim()),
        //                        date = DateTime.Parse(parts[2].Trim())
        //                    };
        //                    expenses.Add(expenseItem);
        //                    MessageBox.Show($"Добавлена запись: {expenseItem.category}"); // ← ДОБАВЬ

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        return expenses;

        //    }

        //    //просто для сохранения реализации из интерфейса
        //    public void Export(List<expense> expenses, string txtFilePath)
        //    {

        //    }


        //    // 3 даем пользователю выбрать место сохранения и сохраняем туда excel
        //    public  void Export1()
        //    {
        //        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        string txtFilePath = Path.Combine(documentsPath, $"{GlobalData.CurrentLogin}.txt");
        //        if (string.IsNullOrEmpty(txtFilePath) || !File.Exists(txtFilePath))
        //        {
        //            MessageBox.Show($"Файл не найден или путь пустой: {txtFilePath}");
        //            return;
        //        }
        //        MessageBox.Show($"Начинаем экспорт из: {txtFilePath}");
        //        MessageBox.Show(GlobalData.CurrentLogin);
        //        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        //        {
        //            saveFileDialog.Filter = "Excel Files|*.xlsx";
        //            saveFileDialog.Title = "Сохранить Excel файл";
        //            saveFileDialog.DefaultExt = "xlsx";

        //            if (saveFileDialog.ShowDialog() == DialogResult.OK)
        //            {
        //                var outputPath = saveFileDialog.FileName;
        //                try
        //                {

        //                    ExportToExcel(outputPath);
        //                    MessageBox.Show($"Файл успешно экспортирован в Excel. Файл расположен: {outputPath}");
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show("Ошибка экспорта: " + ex.Message);
        //                    if (ex.InnerException != null)
        //                    {
        //                        MessageBox.Show($"Внутренняя ошибка: {ex.InnerException.Message}");
        //                    }
        //                }
        //            }

        //            else
        //            {
        //                MessageBox.Show("EROR!");
        //            }
        //        }
        //    }
        //}
        public class ExportExcel : Fabricinter
        {
            // 1. Чтение из TXT
            private List<expense> ReadExpensesFromTxt(string txtFilePath)
            {
                var expenses = new List<expense>();

                MessageBox.Show($"Чтение файла: {txtFilePath}");

                if (!File.Exists(txtFilePath))
                {
                    MessageBox.Show("Файл не существует!");
                    return expenses;
                }

                try
                {
                    string[] lines = File.ReadAllLines(txtFilePath);
                    MessageBox.Show($"Найдено строк: {lines.Length}");

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split('|');
                        if (parts.Length >= 3)
                        {
                            expenses.Add(new expense
                            {
                                category = parts[0].Trim(),
                                count = decimal.Parse(parts[1].Trim()),
                                date = DateTime.Parse(parts[2].Trim())
                            });
                        }
                    }

                    MessageBox.Show($"Успешно прочитано: {expenses.Count} записей");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка чтения: {ex.Message}");
                }

                return expenses;
            }

            // 2. Экспорт в Excel
            public void ExportToExcel(string txtFilePath, string outputPath)
            {
                try
                {
                    MessageBox.Show("Начало экспорта в Excel");

                    List<expense> expenses = ReadExpensesFromTxt(txtFilePath);

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
                        worksheet.Cells[$"C{row}"].PutValue(expense.date);
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
