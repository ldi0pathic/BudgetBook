using System;


namespace BankDataImportBase
{
    public record InternalBankDataRecord
    {
        public DataType Type { get; set; }
        public Currency Currency { get; set; }

        public DateTime DateTime { get; set; }

        public string? Reference { get; set; }
        public string? Description { get; set; }


        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
    }

    public enum Currency
    {
        EUR
    }

    public enum DataType
    {
        Buy,
        CorporateAction,
        Deposit,
        Distribution,
        Fee,
        Interest,
        SavingPlan,
        SecurityTransfer,
        Sell,
        Taxes
    }
}
