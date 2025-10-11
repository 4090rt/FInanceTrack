# Схема работы системы валют

## Поток данных:

```
1. Пользователь заходит в Form2
   ↓
2. Form2.valutelocal() вызывается автоматически
   ↓
3. CurrencyService.GetUserCurrencyAsync(GlobalData.CurrentLogin)
   - Подключается к базе данных
   - Выполняет SQL запрос: SELECT Valute FROM Usersss WHERE Login = @Login
   - Возвращает валюту пользователя (RUB/USD/EUR)
   ↓
4. CurrencyFactory.CreateCurrencyServiceAsync(userCurrency)
   - Получает валюту как параметр
   - Switch определяет какой сервис создать:
     * "RUB" → new Rubvalute()
     * "USD" → new Usdvalute() 
     * "EUR" → new Eurvalute()
   - Создает экземпляр inter(currencyProvider)
   ↓
5. inter.valutapros(userCurrency)
   - Вызывает API для получения курсов валют
   - Возвращает строку с курсами
   ↓
6. Form2 отображает результат в label1 и/или MessageBox
```

## Ключевые моменты:

- **CurrencyService** - получает валюту из БД по логину
- **CurrencyFactory** - создает правильный валютный сервис
- **inter** - работает с API валют
- **userCurrency** передается как параметр между сервисами

## Связь компонентов:

```
CurrencyService (получает валюту) 
    ↓ передает userCurrency ↓
CurrencyFactory (создает сервис)
    ↓ создает inter ↓
inter (получает курсы)
```
