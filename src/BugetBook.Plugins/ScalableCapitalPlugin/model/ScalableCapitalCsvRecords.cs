using BankDataImportBase;
using CsvHelper.Configuration.Attributes;
using ScalableCapitalPlugin.enums;
using Currency = ScalableCapitalPlugin.enums.Currency;
using DataType = ScalableCapitalPlugin.enums.DataType;


namespace ScalableCapitalPlugin.model
{
    public record ScalableCapitalCsvRecord
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public State Status { get; set; }
        public string Reference { get; set; }
        public string? Description { get; set; }
        public AssetType AssetType { get; set; }
        public DataType Type { get; set; }
        public string? Isin { get; set; }
        public int? Shares { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
        public Currency Currency { get; set; }

        [Ignore]
        public DateTime DateTime => new(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second);

        public static explicit operator InternalBankDataRecord(ScalableCapitalCsvRecord data)
        {
            BankDataImportBase.Currency currency = data.Currency switch
            {
                Currency.EUR => BankDataImportBase.Currency.EUR
            };

            BankDataImportBase.DataType dataType = data.Type switch
            {
                DataType.Buy => BankDataImportBase.DataType.Buy,
                DataType.CorporateAction => BankDataImportBase.DataType.CorporateAction,
                DataType.Deposit => BankDataImportBase.DataType.Deposit,
                DataType.Distribution => BankDataImportBase.DataType.Distribution,
                DataType.Fee => BankDataImportBase.DataType.Fee,
                DataType.Interest => BankDataImportBase.DataType.Interest,
                DataType.SavingPlan => BankDataImportBase.DataType.SavingPlan,
                DataType.SecurityTransfer => BankDataImportBase.DataType.SecurityTransfer,
                DataType.Sell => BankDataImportBase.DataType.Sell,
                DataType.Taxes => BankDataImportBase.DataType.Taxes,
            };

            return new InternalBankDataRecord
            {
                Type = dataType,
                Currency = currency,    
                DateTime = data.DateTime,
                Reference = data.Reference,
                Description = data.Description,
                Price = data.Price,
                Amount = data.Amount,
                Fee = data.Fee,
                Tax = data.Tax,
            };
        }
    }
}
