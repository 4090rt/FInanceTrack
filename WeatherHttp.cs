using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    public class TimezoneResponse
    {
        [JsonPropertyName("date_time_txt")]
        public string Timezone { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("Time")]
        public string Time { get; set; }

    }

    public class Weathers
    {
        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }
    }

    public class Weather
    {
        [JsonPropertyName("main")]
        public string main { get; set; }
        [JsonPropertyName("description")]
        public string description { get; set; }
    }
    public class WeatherHttp
    {
        private readonly object _lock = new object();
        public async Task Weather()
        {
            string APIKEY = "818684b83cb44c9f87e6a189bf48bf83";
            string City = "Ekaterinburg";
            string Country = "Russia";
            string URL = $"https://api.ipgeolocation.io/timezone?apiKey={APIKEY}&location={Uri.EscapeDataString(City)},%20{Uri.EscapeDataString(Country)}";
            PoolObjectHTTP poolObjectHTTP = new PoolObjectHTTP();
            HttpClient client = null;
            try
            {
                client = poolObjectHTTP.Connect();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Language", "ru");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ry-Ru,ry;q=0.9,en;q=0.8");

                HttpResponseMessage responseMessage = await client.GetAsync(URL);
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.IsSuccessStatusCode)
                { 
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    var ser = JsonSerializer.Deserialize<TimezoneResponse>(result);

                    Type type = ser.GetType();
                    PropertyInfo[] prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    var sb =new StringBuilder();
                    Parallel.ForEach(prop, pro =>
                    {
                        object value = pro.GetValue(ser);
                        lock (_lock)
                        {
                            sb.AppendLine($"{pro.Name}: {value}");
                        }
                    });
                    MessageBox.Show(sb.ToString());
                    if (ser?.Timezone != null)
                    {
                        MessageBox.Show($"{ser.Timezone}");
                    }
                    else
                    {
                        MessageBox.Show("Данные о времени отсутствуют");
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                poolObjectHTTP.CloseConnect(client);
            }
        }
    }

    public class Weather2
    {
        private readonly object _lock = new object();
        public async Task WEATHER22()
        {
            string city = "Ekaterinburg";
            string APIKey = "6f7b4977c06cf7032b4f49790617fc3d";
            string URL = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={APIKey}&units=metric&lang=ru";
            PoolObjectHTTP poolObjectHTTP = new PoolObjectHTTP();
            HttpClient client = new HttpClient();
            try
            {
                client = poolObjectHTTP.Connect();

                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Language", "ru");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ry-Ru,ry;q=0.9,en;q=0.8");

                HttpResponseMessage recpon = await client.GetAsync(URL);
                recpon.EnsureSuccessStatusCode();
                if (recpon.IsSuccessStatusCode)
                { 
                    var result = await recpon.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var ser = JsonSerializer.Deserialize<Weathers>(result);
                    MessageBox.Show(result);

                    Type type = ser.GetType();
                    PropertyInfo[] prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    var sb = new StringBuilder();
                    Parallel.ForEach(prop, pro =>
                    {
                        var value = pro.GetValue(ser);
                        lock (_lock)
                        {
                            sb.AppendLine($"{pro.Name}: {value}");
                        }
                    });
                    MessageBox.Show(sb.ToString());
                    if (ser?.Weather != null && ser.Weather.Count > 0)
                    {
                        MessageBox.Show($"{ser.Weather[0].main} {ser.Weather[0].description}");
                    }
                    else
                    {
                        MessageBox.Show("Данные о погоде отсутствуют");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                poolObjectHTTP.CloseConnect(client);
            }
        }
    }
}
