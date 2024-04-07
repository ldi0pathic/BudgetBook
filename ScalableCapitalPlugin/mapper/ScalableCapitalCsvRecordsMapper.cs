using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ScalableCapitalPlugin.enums;
using ScalableCapitalPlugin.model;


namespace ScalableCapitalPlugin.mapper
{
    public class ScalableCapitalCsvRecordsMapper : ClassMap<ScalableCapitalCsvRecords>
    {
        public ScalableCapitalCsvRecordsMapper()
        {
            Map(m => m.Date).Name("date").TypeConverter(new DateOnlyConverter());
            Map(m => m.Time).Name("time").TypeConverter(new TimeOnlyConverter());
            Map(m => m.Status).Name("status").TypeConverter(new EnumConverter(typeof(State)));
            Map(m => m.Reference).Name("reference").TypeConverter(new StringConverter());
            Map(m => m.Description).Name("description").TypeConverter(new StringConverter()).Default("");
            Map(m => m.AssetType).Name("assetType").TypeConverter(new EnumConverter(typeof(AssetType)));
            Map(m => m.Type).Name("type").TypeConverter(new EnumConverter(typeof(DataType)));
            Map(m => m.Isin).Name("isin").TypeConverter(new StringConverter()).Default("");
            Map(m => m.Shares).Name("shares").TypeConverter(new Int32Converter()).Default(0);
            Map(m => m.Price).Name("price").TypeConverter(new DecimalConverter()).Default((decimal)0.00);
            Map(m => m.Amount).Name("amount").TypeConverter(new DecimalConverter()).Default((decimal)0.00);
            Map(m => m.Fee).Name("fee").TypeConverter(new DecimalConverter()).Default((decimal)0.00);
            Map(m => m.Tax).Name("tax").TypeConverter(new DecimalConverter()).Default((decimal)0.00);
            Map(m => m.Currency).Name("currency").TypeConverter(new EnumConverter(typeof(Currency)));
        }
    }
}
