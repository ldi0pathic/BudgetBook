using BankDataImportBase;
using CsvHelper.Configuration.Attributes;
using ScalableCapitalPlugin.enums;


namespace ScalableCapitalPlugin.model
{
    public record ScalableCapitalCsvRecords
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

        public static explicit operator InternalBankDataFormat(ScalableCapitalCsvRecords data)
        {
            return new InternalBankDataFormat
            {
            };
        }
    }

}
