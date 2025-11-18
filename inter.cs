using System.Text;
using System.Text.Json;

namespace WinFormsApp4
{
    public class inter
    {
        private readonly Gred _greeder;

        public inter(Gred greeter)
        {
            _greeder = greeter ?? throw new ArgumentNullException(nameof(greeter));
        }

        public async Task<string> valutapros(string valute)
        {
            if (string.IsNullOrWhiteSpace(valute))
            {
                return string.Empty;
            }

            return await _greeder.valutezapros(valute).ConfigureAwait(false);
        }
    }



    // класс интервейс(использование паттерна Dependency Injection)
    public interface Gred
    {
        public Task<string> valutezapros(string valute);
    }

    // реализации вариации использования класса(разные данные, разная реакция на них Dependency Injection)


    //реализация для рубля
    public class Rubvalute : Gred
    {
        public async Task<string> valutezapros(string valute)
        {
            var pool = new PoolObhectHttpiNTER();
            HttpClient client = null;
            var pool2 = new PoolObhectJsonInter1();
            JsonDocument document = null;
            string Apikey = "cf64a04e84d8235680fdfa09";
            string[] targetCurrencies = { $"{valute}" };
            if (valute == "RUB")
            {
                string[] baseCurrencies = { "EUR", "USD" };
                StringBuilder builder = new StringBuilder();

                try
                {
                    client = pool.Connect();
                    foreach (string basecyrens in baseCurrencies)
                    {
                        string URL = $"https://v6.exchangerate-api.com/v6/{Apikey}/latest/{basecyrens}";
                        HttpResponseMessage recpon = await client.GetAsync(URL).ConfigureAwait(false);
                        recpon.EnsureSuccessStatusCode();
                        string jsconcl = await recpon.Content.ReadAsStringAsync().ConfigureAwait(false);
                        try
                        {
                            document = pool2.Connect(jsconcl);
                                JsonElement root = document.RootElement;
                                if (root.TryGetProperty("result", out JsonElement result) && result.GetString() == "success")
                                {
                                    JsonElement jsom = root.GetProperty("conversion_rates");
                                    builder.AppendLine($"Курс {basecyrens}");
                                    foreach (string currentt in targetCurrencies)
                                    {
                                        if (jsom.TryGetProperty(currentt, out JsonElement rateElement))
                                        {
                                            double rate = rateElement.GetDouble();
                                            builder.AppendLine($"1 {basecyrens} = {rate} {currentt}");
                                        }
                                        builder.AppendLine();
                                        return builder.ToString();
                                    }
                                }
                                else
                                {
                                    builder.AppendLine($"Ошибка для {basecyrens}: {root.GetProperty("error-type").GetString()}");
                                }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка парсинга" + ex.Message);
                        }
                        finally
                        {
                            pool2.Close(document);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка");
                    return "";
                }
                finally
                {
                    pool.Close(client);
                }
                return builder.ToString();
            }
            return "";
        }

    }



    // реализация для доллара
    public class Usdvalute : Gred
    {
        public async Task<string> valutezapros(string valute)
        {
            var pool2 = new PoolObhectJsonInter2();
            JsonDocument document = null;
            var pool = new PoolObhectHttpiNTER();
            HttpClient client = null;
            string Apikey = "cf64a04e84d8235680fdfa09";
            string[] targetCurrencies = { $"{valute}" };
            if (valute == "USD")
            {
                StringBuilder builder = new StringBuilder();
                string[] baseCurrencies = { "EUR", "RUB" };
                try
                {
                    client = pool.Connect();
                    foreach (string basecyrens in baseCurrencies)
                    {
                        string URL = $"https://v6.exchangerate-api.com/v6/{Apikey}/latest/{basecyrens}";
                        HttpResponseMessage recpon = await client.GetAsync(URL).ConfigureAwait(false);
                        recpon.EnsureSuccessStatusCode();
                        string jsconcl = await recpon.Content.ReadAsStringAsync().ConfigureAwait(false);
                        try
                        {
                            document = pool2.Connect(jsconcl);
                                JsonElement root = document.RootElement;
                                if (root.TryGetProperty("result", out JsonElement result) && result.GetString() == "success")
                                {
                                    JsonElement jsom = root.GetProperty("conversion_rates");
                                    builder.AppendLine($"Курс {basecyrens}");
                                    foreach (string currentt in targetCurrencies)
                                    {
                                        if (jsom.TryGetProperty(currentt, out JsonElement rateElement))
                                        {
                                            double rate = rateElement.GetDouble();
                                            builder.AppendLine($"1 {basecyrens} = {rate} {currentt}");
                                        }
                                        builder.AppendLine();
                                    }
                                }
                                else
                                {
                                    builder.AppendLine($"Ошибка для {basecyrens}: {root.GetProperty("error-type").GetString()}");
                                }
                           
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка парсинга JSON: {ex.Message}", "Ошибка");
                        }
                        finally
                        {
                            pool2.close(document);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка");
                }
                finally
                {
                    pool.Close(client);
                }
                return builder.ToString();
            }
            return "";
        }

    }


    // реализация для евро
    public class Eurvalute : Gred
    {

        public async Task<string> valutezapros(string valute)
        {
            var pool2 = new PoolObhectJsonInter1();
            JsonDocument document = null;
            var pool = new PoolObhectHttpiNTER();
            HttpClient client = null;
            string Apikey = "cf64a04e84d8235680fdfa09";
            string[] targetCurrencies = { $"{valute}" };
            if (valute == "EUR")
            {
                StringBuilder builder = new StringBuilder();
                string[] baseCurrencies = { "RUB", "USD" };
                try
                {
                    client = pool.Connect();
                    foreach (string basecyrens in baseCurrencies)
                    {
                        string URL = $"https://v6.exchangerate-api.com/v6/{Apikey}/latest/{basecyrens}";
                        HttpResponseMessage recpon = await client.GetAsync(URL).ConfigureAwait(false);
                        recpon.EnsureSuccessStatusCode();
                        string jsconcl = await recpon.Content.ReadAsStringAsync().ConfigureAwait(false);
                        try
                        {
                            document = pool2.Connect(jsconcl);
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("result", out JsonElement result) && result.GetString() == "success")
                            {
                                JsonElement jsom = root.GetProperty("conversion_rates");
                                builder.AppendLine($"Курс {basecyrens}");
                                foreach (string currentt in targetCurrencies)
                                {
                                    if (jsom.TryGetProperty(currentt, out JsonElement rateElement))
                                    {
                                        double rate = rateElement.GetDouble();
                                        builder.AppendLine($"1 {basecyrens} = {rate} {currentt}");
                                    }
                                    builder.AppendLine();
                                }
                            }
                            else
                            {
                                builder.AppendLine($"Ошибка для {basecyrens}: {root.GetProperty("error-type").GetString()}");
                            }
                        }
                        catch (JsonException ex)
                        {
                            MessageBox.Show($"Ошибка парсинга JSON: {ex.Message}", "Ошибка");
                        }
                        finally
                        {
                            pool2.Close(document);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка");
                }
                finally
                {
                    pool.Close(client);
                }
                return builder.ToString();
            }
            return "";
        }

    }
}
