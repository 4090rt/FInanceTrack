using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;

namespace WinFormsApp4
{
    internal class inter
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




    public interface Gred
    {
        public Task<string> valutezapros(string valute);
    }
    public class Rubvalute : Gred
    {
        public async Task<string> valutezapros(string valute)
        {
            string Apikey = "cf64a04e84d8235680fdfa09";
            string[] targetCurrencies = { $"{valute}" };
            if (valute == "RUB")
            {
                string[] baseCurrencies = { "EUR", "USD" };
                StringBuilder builder = new StringBuilder();

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        foreach (string basecyrens in baseCurrencies)
                        {
                            string URL = $"https://v6.exchangerate-api.com/v6/{Apikey}/latest/{basecyrens}";
                            HttpResponseMessage recpon = await client.GetAsync(URL);
                            recpon.EnsureSuccessStatusCode();
                            string jsconcl = await recpon.Content.ReadAsStringAsync();

                            using (JsonDocument json = JsonDocument.Parse(jsconcl))
                            {
                                JsonElement root = json.RootElement;
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
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка");
                    return "";
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Ошибка парсинга JSON: {ex.Message}", "Ошибка");
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка");
                    return "";
                }
                return builder.ToString();
            }
            return "";
        }
        
    }

    public class Usdvalute : Gred
    {
        public async Task<string> valutezapros(string valute)
        {
            string Apikey = "cf64a04e84d8235680fdfa09";
            string[] targetCurrencies = { $"{valute}" };
            if (valute == "USD")
            {
                StringBuilder builder = new StringBuilder();
                string[] baseCurrencies = { "EUR", "RUB" };
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        foreach (string basecyrens in baseCurrencies)
                        {
                            string URL = $"https://v6.exchangerate-api.com/v6/{Apikey}/latest/{basecyrens}";
                            HttpResponseMessage recpon = await client.GetAsync(URL);
                            recpon.EnsureSuccessStatusCode();
                            string jsconcl = await recpon.Content.ReadAsStringAsync();

                            using (JsonDocument json = JsonDocument.Parse(jsconcl))
                            {
                                JsonElement root = json.RootElement;
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
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка");
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Ошибка парсинга JSON: {ex.Message}", "Ошибка");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка");
                }
                return builder.ToString();
            }
            return "";
        }

    }

    public class Eurvalute : Gred
    {

        public async Task<string> valutezapros(string valute)
        {
            string Apikey = "cf64a04e84d8235680fdfa09";
            string[] targetCurrencies = { $"{valute}" };
            if (valute == "EUR")
            {
                StringBuilder builder = new StringBuilder();
                string[] baseCurrencies = { "RUB", "USD" };
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        foreach (string basecyrens in baseCurrencies)
                        {
                            string URL = $"https://v6.exchangerate-api.com/v6/{Apikey}/latest/{basecyrens}";
                            HttpResponseMessage recpon = await client.GetAsync(URL);
                            recpon.EnsureSuccessStatusCode();
                            string jsconcl = await recpon.Content.ReadAsStringAsync();

                            using (JsonDocument json = JsonDocument.Parse(jsconcl))
                            {
                                JsonElement root = json.RootElement;
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
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка");
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Ошибка парсинга JSON: {ex.Message}", "Ошибка");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка");
                }
                return builder.ToString();
            }
            return "";
        }
        
    }
}
