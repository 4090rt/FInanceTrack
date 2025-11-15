using Aspose.Pdf.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static WinFormsApp4.fabric;

namespace WinFormsApp4
{
    public interface Inter
    {
        public Task<string> Courcevalutecrypt();
    }

    public enum Valutessscrypt
    { 
        USD,
        EUR,
        RUB
    }

    public class CryptoApicursUSD : Inter
    {
        public async Task<string> Courcevalutecrypt()
        {
            string URL = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,tether&vs_currencies=usd";
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("Accept", "application/json");
                    http.DefaultRequestHeaders.Add("Language", "ru");
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                    http.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ry-Ru,ry;q=0.9,en;q=0.8");
                    HttpResponseMessage responseMessage = await http.GetAsync(URL).ConfigureAwait(false);
                    responseMessage.EnsureSuccessStatusCode();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var contentjson = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (contentjson == null)
                        {
                            MessageBox.Show("Не удалос получить данные");
                            return "";
                        }
                        try
                        {
                            using (JsonDocument json = JsonDocument.Parse(contentjson))
                            {
                                JsonElement root = json.RootElement;
                                StringBuilder builder = new StringBuilder();

                                builder.AppendLine($"Bitcoin: {root.GetProperty("bitcoin").GetProperty("usd").GetDecimal()} USD");
                                builder.AppendLine($"Ethereum: {root.GetProperty("ethereum").GetProperty("usd").GetDecimal()} USD");
                                builder.AppendLine($"Tether: {root.GetProperty("tether").GetProperty("usd").GetDecimal()} USD");

                                return builder.ToString();
                            }
                        }
                        catch (Exception ex)
                        { 
                            MessageBox.Show("Ошибка парсинга" + ex.Message);
                            return "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка получения курса");
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла Ошибка получения курса:" + ex.Message);
                return "";
            }
        }
    }


    public class CryptoApicursEUR : Inter
    {
        public async Task<string> Courcevalutecrypt()
        { 
         string URL = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,tether&vs_currencies=eur";
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("Accept", "application/json");
                    http.DefaultRequestHeaders.Add("Language", "ru");
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                    http.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ry-Ru,ry;q=0.9,en;q=0.8");
                    HttpResponseMessage responseMessage = await http.GetAsync(URL).ConfigureAwait(false);
                    responseMessage.EnsureSuccessStatusCode();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        try
                        {
                            var contentjson = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                            if (contentjson == null)
                            {
                                MessageBox.Show("Не удалос получить данные");
                                return "";
                            }
                            using (JsonDocument json = JsonDocument.Parse(contentjson))
                            {
                                JsonElement root = json.RootElement;
                                StringBuilder builder = new StringBuilder();

                                builder.AppendLine($"Bitcoin: {root.GetProperty("bitcoin").GetProperty("eur").GetDecimal()} EUR");
                                builder.AppendLine($"Ethereum: {root.GetProperty("ethereum").GetProperty("eur").GetDecimal()} EUR");
                                builder.AppendLine($"Tether: {root.GetProperty("tether").GetProperty("eur").GetDecimal()} EUR");

                                return builder.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка парсинга" + ex.Message);
                            return "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка получения курса");
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла Ошибка получения курса:" + ex.Message);
                return "";
            }
        }
    }

    public class CryptoApicursRUB : Inter
    {
        public async Task<string> Courcevalutecrypt()
        {
            string URL = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,tether&vs_currencies=eur";
            try
            {
                using (HttpClient http = new HttpClient())
                { 
                    HttpResponseMessage responseMessage = await http.GetAsync(URL).ConfigureAwait(false);
                    responseMessage.EnsureSuccessStatusCode();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var contentjson = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (contentjson == null)
                        {
                            MessageBox.Show("Не удалос получить данные");
                            return "";
                        }
                        try
                        {
                            using (JsonDocument json = JsonDocument.Parse(contentjson))
                            {
                                JsonElement root = json.RootElement;
                                StringBuilder builder = new StringBuilder();

                                builder.AppendLine($"Bitcoin: {root.GetProperty("bitcoin").GetProperty("eur").GetDecimal()} RUB");
                                builder.AppendLine($"Ethereum: {root.GetProperty("ethereum").GetProperty("eur").GetDecimal()} RUB");
                                builder.AppendLine($"Tether: {root.GetProperty("tether").GetProperty("eur").GetDecimal()} RUB");

                                return builder.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка парсинга" + ex.Message);
                            return "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка получения курса");
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла Ошибка получения курса:" + ex.Message);
                return "";
            }
        }

        }


    public class Valuteformat
    {
        public Inter Valutecrypt(Valutessscrypt format)
        {
            return format switch
            {
                Valutessscrypt.USD => new CryptoApicursUSD(),
                Valutessscrypt.EUR => new CryptoApicursEUR(),
                Valutessscrypt.RUB => new CryptoApicursRUB(),
            };
        }
    }

}
