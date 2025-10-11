using System;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    public static class CurrencyFactory
    {
        public static Task<inter> CreateCurrencyServiceAsync(string userCurrency)
        {
            if (string.IsNullOrWhiteSpace(userCurrency))
            {
                userCurrency = "RUB"; // Валюта по умолчанию
            }

            Gred currencyProvider = userCurrency.ToUpper() switch
            {
                "RUB" => new Rubvalute(),
                "USD" => new Usdvalute(),
                "EUR" => new Eurvalute(),
                _ => new Rubvalute() // По умолчанию используем рублевый сервис
            };

            return Task.FromResult(new inter(currencyProvider));
        }
    }
}
