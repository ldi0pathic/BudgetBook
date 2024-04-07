using System.Diagnostics;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ScalableCapitalPlugin.enums;
using ScalableCapitalPlugin.mapper;
using ScalableCapitalPlugin.model;
using Xunit.Abstractions;

namespace ScalableCapitalPluginTest
{
    public class ScalableCapitalPluginTest
    {
        private readonly ITestOutputHelper _log;

        public ScalableCapitalPluginTest(ITestOutputHelper testOutputHelper)
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

            using (var reader = new StringReader(s.ToString()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", InjectionCharacters = ['=', '@', '+', '-', '\t', '\r', '\"'] }))
            {
                csv.Context.RegisterClassMap<ScalableCapitalCsvRecordsMapper>();
                var records = csv.GetRecords<ScalableCapitalCsvRecords>().ToList();

                Assert.Equal(2, records.Count);

                Assert.Equal(new DateTime(2023, 12, 28, 11, 26, 51), records[0].DateTime);
                Assert.Equal(State.Pending, records[0].Status);
                Assert.Equal("GZHGJNJKLU", records[0].Reference);
                Assert.Equal("CTEST", records[0].Description);
                Assert.Equal(AssetType.Security, records[0].AssetType);
                Assert.Equal(DataType.Sell, records[0].Type);
                Assert.Equal("ZH9097", records[0].Isin);
                Assert.Equal(0, records[0].Shares);
                Assert.Equal(0, records[0].Price);
                Assert.Equal(0, records[0].Amount);
                Assert.Equal(0, records[0].Fee);
                Assert.Equal(0, records[0].Tax);
                Assert.Equal(Currency.EUR, records[0].Currency);

                Assert.Equal(new DateTime(2024, 04, 05, 02, 0, 0), records[1].DateTime);
                Assert.Equal(State.Executed, records[1].Status);
                Assert.Equal("fwefew 26511873", records[1].Reference);
                Assert.Equal("AAA", records[1].Description);
                Assert.Equal(AssetType.Cash, records[1].AssetType);
                Assert.Equal(DataType.Distribution, records[1].Type);
                Assert.Equal("GH89857989", records[1].Isin);
                Assert.Equal(0, records[1].Shares);
                Assert.Equal(0, records[1].Price);
                Assert.Equal((decimal)41.57, records[1].Amount);
                Assert.Equal(0, records[1].Fee);
                Assert.Equal(0, records[1].Tax);
                Assert.Equal(Currency.EUR, records[1].Currency);
            }
        }
      
    }
}