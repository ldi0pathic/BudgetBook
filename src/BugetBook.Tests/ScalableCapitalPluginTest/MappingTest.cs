using ScalableCapitalPlugin;
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

        [Theory]
        [InlineData(".\\testdata\\test.csv")]
        public void TestPlugin(string path)
        {
            var plugin = new ScalableCapitalBankDataImport();

            Assert.True(plugin.SetAndCheck(path));

            var data = plugin.GetBankData().ToBlockingEnumerable().ToList();

            Assert.Equal(4, data.Count);
            Assert.Equal((decimal)36.58, data.Sum(s => s.Amount));

            var data1 = data.Where(w => w.DateTime.Equals(new DateTime(2023, 12, 28, 11, 26, 51))).Single();

            Assert.NotNull(data1);
            Assert.Equal(new DateTime(2023, 12, 28, 11, 26, 51), data1.DateTime);
            Assert.Equal("GZHGJNJKLU", data1.Reference);
            Assert.Equal("ZH9097 | CTEST | x1", data1.Description);
            Assert.Equal(BankDataImportBase.DataType.Sell, data1.Type);
            Assert.Equal(0, data1.Price);
            Assert.Equal(0, data1.Amount);
            Assert.Equal(0, data1.Fee);
            Assert.Equal(0, data1.Tax);
            Assert.Equal(BankDataImportBase.Currency.EUR, data1.Currency);

            var data2 = data.Where(w => w.DateTime.Equals(new DateTime(2024, 04, 05, 02, 0, 0))).Single();

            Assert.NotNull(data2);
            Assert.Equal(new DateTime(2024, 04, 05, 02, 0, 0), data2.DateTime);
            Assert.Equal("fwefew 26511873", data2.Reference);
            Assert.Equal("GH89857989 | AAA | x0", data2.Description);
            Assert.Equal(BankDataImportBase.DataType.Distribution, data2.Type);
            Assert.Equal(0, data2.Price);
            Assert.Equal((decimal)41.57, data2.Amount);
            Assert.Equal(0, data2.Fee);
            Assert.Equal(0, data2.Tax);
            Assert.Equal(BankDataImportBase.Currency.EUR, data2.Currency);

            var data3 = data.Where(w => w.DateTime.Equals(new DateTime(1990, 04, 04, 19, 57, 34))).Single();

            Assert.NotNull(data3);
            Assert.Equal(new DateTime(1990, 04, 04, 19, 57, 34), data3.DateTime);
            Assert.Equal("NHkchbk89", data3.Reference);
            Assert.Equal("Ge7890H9 | Bsl Post | x0", data3.Description);
            Assert.Equal(BankDataImportBase.DataType.Sell, data3.Type);
            Assert.Equal((decimal)2, data3.Price);
            Assert.Equal(0, data3.Amount);
            Assert.Equal(0, data3.Fee);
            Assert.Equal(0, data3.Tax);
            Assert.Equal(BankDataImportBase.Currency.EUR, data3.Currency);

            var data4 = data.Where(w => w.DateTime.Equals(new DateTime(2024, 03, 28, 01, 00, 00))).Single();

            Assert.NotNull(data4);
            Assert.Equal(new DateTime(2024, 03, 28, 01, 00, 00), data4.DateTime);
            Assert.Equal("jifewfew", data4.Reference);
            Assert.Equal(" | Entgelt Gebühren | x0", data4.Description);
            Assert.Equal(BankDataImportBase.DataType.Fee, data4.Type);
            Assert.Equal(0, data4.Price);
            Assert.Equal((decimal)-4.99, data4.Amount);
            Assert.Equal(0, data4.Fee);
            Assert.Equal(0, data4.Tax);
            Assert.Equal(BankDataImportBase.Currency.EUR, data4.Currency);
        }
    }
}