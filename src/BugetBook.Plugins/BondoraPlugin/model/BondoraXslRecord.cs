using BankDataImportBase;
using DataType = BondoraPlugin.enums.DataType;

namespace BondoraPlugin.model
{
    public record BondoraXslRecord
    {
        public DateTime Date { get; set; }
        public DataType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public static explicit operator InternalBankDataRecord(BondoraXslRecord data)
        {

            BankDataImportBase.DataType dataType = data.Type switch
            {
                DataType.Buy => BankDataImportBase.DataType.Buy,
                DataType.Interest => BankDataImportBase.DataType.Interest,
                DataType.SavingPlan => BankDataImportBase.DataType.SavingPlan,
            };

            return new InternalBankDataRecord
            {
                Type = dataType,
                Currency = Currency.EUR,
                DateTime = data.Date,
                Description = data.Description,
                Amount = data.Amount,
            };
        }
    }
}