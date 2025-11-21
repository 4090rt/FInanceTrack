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
    public class WeatherHttp
    {
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
                    PropertyInfo[] prop = type.GetProperties();

                    foreach (PropertyInfo pop in prop)
                    { 
                        object value = pop.GetValue(ser);
                        MessageBox.Show($"- {pop.Name} ({pop.PropertyType.Name}): {value ?? "null"}");
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
}
