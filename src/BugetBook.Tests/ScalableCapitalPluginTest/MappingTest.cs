using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using ScalableCapitalPlugin.enums;
using ScalableCapitalPlugin.mapper;
using ScalableCapitalPlugin.model;
using Xunit.Abstractions;

namespace ScalableCapitalPluginTest
{
    public class MappingTest
    {
        private readonly ITestOutputHelper _log;

        public MappingTest(ITestOutputHelper testOutputHelper)
        {
            _log = testOutputHelper;
        }

        [Fact]
        public void Test()
        {
            var s = new StringBuilder();
            s.AppendLine("date;time;status;reference;description;assetType;type;isin;shares;price;amount;fee;tax;currency");  
            s.AppendLine("2023-12-28;11:26:51;Pending;\"GZHGJNJKLU\";\"CTEST\";Security;Sell;ZH9097;0;0,00;0,00;0,00;0,00;EUR");
            s.AppendLine("2024-04-05;02:00:00;Executed;\"fwefew 26511873\";\"AAA\";Cash;Distribution;GH89857989;;;41,57;0,00;;EUR");
            s.AppendLine("1990-04-04;19:57:34;Cancelled;\"NHkchbk89\";\"Bsl Post\";Security;Sell;Ge7890ß9;0;0,00;0,00;0,00;0,00;EUR");
            s.AppendLine("2024-03-28;01:00:00;Executed;\"jifewfew\";\"Entgelt Gebühren\";Cash;Fee;;;;-4,99;0,00;;EUR");

            using (var reader = new StringReader(s.ToString()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", InjectionCharacters = ['=', '@', '+', '-', '\t', '\r', '\"'] }))
            {
                int index = 0;
                csv.Context.RegisterClassMap<ScalableCapitalCsvRecordsMapper>();
                var records = csv.GetRecords<ScalableCapitalCsvRecord>().ToList();

                Assert.Equal(4, records.Count);

                //"2023-12-28;11:26:51;Pending;\"GZHGJNJKLU\";\"CTEST\";Security;Sell;ZH9097;0;0,00;0,00;0,00;0,00;EUR"
                index = 0;
                Assert.Equal(new DateTime(2023, 12, 28, 11, 26, 51), records[index].DateTime);
                Assert.Equal(State.Pending, records[index].Status);
                Assert.Equal("GZHGJNJKLU", records[index].Reference);
                Assert.Equal("CTEST", records[index].Description);
                Assert.Equal(AssetType.Security, records[index].AssetType);
                Assert.Equal(DataType.Sell, records[index].Type);
                Assert.Equal("ZH9097", records[index].Isin);
                Assert.Equal(0, records[index].Shares);
                Assert.Equal(0, records[index].Price);
                Assert.Equal(0, records[index].Amount);
                Assert.Equal(0, records[index].Fee);
                Assert.Equal(0, records[index].Tax);
                Assert.Equal(Currency.EUR, records[index].Currency);

                //"2024-04-05;02:00:00;Executed;\"fwefew 26511873\";\"AAA\";Cash;Distribution;GH89857989;;;41,57;0,00;;EUR"
                index = 1;
                Assert.Equal(new DateTime(2024, 04, 05, 02, 0, 0), records[index].DateTime);
                Assert.Equal(State.Executed, records[index].Status);
                Assert.Equal("fwefew 26511873", records[index].Reference);
                Assert.Equal("AAA", records[index].Description);
                Assert.Equal(AssetType.Cash, records[index].AssetType);
                Assert.Equal(DataType.Distribution, records[index].Type);
                Assert.Equal("GH89857989", records[index].Isin);
                Assert.Equal(0, records[index].Shares);
                Assert.Equal(0, records[index].Price);
                Assert.Equal((decimal)41.57, records[index].Amount);
                Assert.Equal(0, records[index].Fee);
                Assert.Equal(0, records[index].Tax);
                Assert.Equal(Currency.EUR, records[index].Currency);

                //"1990-04-04;19:57:34;Cancelled;\"NHkchbk89\";\"Bsl Post\";Security;Sell;Ge7890ß9;0;0,00;0,00;0,00;0,00;EUR"
                index = 2;
                Assert.Equal(new DateTime(1990, 04, 04, 19, 57, 34), records[index].DateTime);
                Assert.Equal(State.Cancelled, records[index].Status);
                Assert.Equal("NHkchbk89", records[index].Reference);
                Assert.Equal("Bsl Post", records[index].Description);
                Assert.Equal(AssetType.Security, records[index].AssetType);
                Assert.Equal(DataType.Sell, records[index].Type);
                Assert.Equal("Ge7890ß9", records[index].Isin);
                Assert.Equal(0, records[index].Shares);
                Assert.Equal(0, records[index].Price);
                Assert.Equal(0, records[index].Amount);
                Assert.Equal(0, records[index].Fee);
                Assert.Equal(0, records[index].Tax);
                Assert.Equal(Currency.EUR, records[index].Currency);

                //"2024-03-28;01:00:00;Executed;\"jifewfew\";\"Entgelt Gebühren\";Cash;Fee;;;;-4,99;0,00;;EUR"
                index = 3;
                Assert.Equal(new DateTime(2024, 03, 28, 01, 00, 00), records[index].DateTime);
                Assert.Equal(State.Executed, records[index].Status);
                Assert.Equal("jifewfew", records[index].Reference);
                Assert.Equal("Entgelt Gebühren", records[index].Description);
                Assert.Equal(AssetType.Cash, records[index].AssetType);
                Assert.Equal(DataType.Fee, records[index].Type);
                Assert.Equal("", records[index].Isin);
                Assert.Equal(0, records[index].Shares);
                Assert.Equal(0, records[index].Price);
                Assert.Equal((decimal)-4.99, records[index].Amount);
                Assert.Equal(0, records[index].Fee);
                Assert.Equal(0, records[index].Tax);
                Assert.Equal(Currency.EUR, records[index].Currency);
            }
        }    
    }
}