using System;
using System.Collections.Generic;

namespace CurrencySample
{
    public sealed record Currency
    {
        public string Code { get; }

        private static readonly HashSet<string> ValidCodes = new()
        {
            "USD", "JPY", "EUR"
        };

        private Currency(string code)
        {
            Code = code;
        }

        public static Currency Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("通貨コードは必須です");

            code = code.ToUpperInvariant();

            if (!ValidCodes.Contains(code))
                throw new ArgumentException($"未対応の通貨コードです: {code}");

            return new Currency(code);
        }

        public override string ToString() => Code;

        // 定数的インスタンス
        public static Currency USD => new("USD");
        public static Currency JPY => new("JPY");
        public static Currency EUR => new("EUR");
    }


    public sealed class Money
    {
        public Currency Currency { get; }
        public decimal Amount { get; }

        public Money(Currency currency, decimal amount)
        {
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));

            if (amount < 0)
                throw new ArgumentException("金額は0以上である必要があります");

            Amount = amount;
        }
    }

    public sealed class ExchangeRate
    {
        public Currency BaseCurrency { get; }
        public Currency TargetCurrency { get; }
        public decimal Rate { get; }

        public ExchangeRate(Currency baseCurrency, Currency targetCurrency, decimal rate)
        {
            BaseCurrency = baseCurrency ?? throw new ArgumentNullException(nameof(baseCurrency));
            TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));

            if (rate <= 0)
                throw new ArgumentException("レートは正の値である必要があります");

            Rate = rate;
        }

        public Money Convert(Money money)
        {
            if (money.Currency == BaseCurrency)
            {
                return new Money(TargetCurrency, money.Amount * Rate);
            }

            if (money.Currency == TargetCurrency)
            {
                return new Money(BaseCurrency, money.Amount / Rate);
            }

            throw new InvalidOperationException("このレートでは変換できません");
        }
    }
}