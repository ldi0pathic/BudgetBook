using BondoraPlugin;
using System.Data;
using Xunit.Abstractions;


namespace BondoraPluginTest
{
    public class MappingTest
    {
        private readonly ITestOutputHelper _log;

        public MappingTest(ITestOutputHelper testOutputHelper)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _log = testOutputHelper;
        }

        [Theory]
        [InlineData(".\\testdata\\test_new.xlsx")]
        public void TestPlugin(string path)
        {
            var plugin = new BondoraBankDataImport();

            Assert.True(plugin.SetAndCheckPath(path));
            Assert.True(plugin.IsValid());

            var data = plugin.GetBankData().ToBlockingEnumerable().ToList();

            Assert.NotEmpty(data);
            Assert.Equal(16, data.Count);
            Assert.Equal(1, data.Count(s => s.Type.Equals(BankDataImportBase.DataType.SavingPlan)));
            Assert.Equal(15, data.Count(s => s.Type.Equals(BankDataImportBase.DataType.Interest)));
            Assert.Equal((decimal)117.85, data.Sum(s => s.Amount));


            var plan = data.Where(w => w.Type.Equals(BankDataImportBase.DataType.SavingPlan)).Single();

            Assert.NotNull(plan);
            Assert.Equal(DateTime.MinValue, plan.DateTime);
            Assert.Equal(BankDataImportBase.Currency.EUR, plan.Currency);
            Assert.Equal("SEPA-Banküberweisung", plan.Description);
            Assert.Equal(100, plan.Amount);

            var data1 = data.Where(w => w.DateTime.Equals(new DateTime(2023, 12, 19))).Single();

            Assert.NotNull(data1);
            Assert.Equal(BankDataImportBase.DataType.Interest, data1.Type);
            Assert.Equal(BankDataImportBase.Currency.EUR, data1.Currency);
            Assert.Equal("Go & Grow Zinsen", data1.Description);
            Assert.Equal((decimal)1.18, data1.Amount);

            var data2 = data.Where(w => w.DateTime.Equals(new DateTime(2024, 01, 01))).Single();

            Assert.NotNull(data2);
            Assert.Equal(BankDataImportBase.DataType.Interest, data2.Type);
            Assert.Equal(BankDataImportBase.Currency.EUR, data2.Currency);
            Assert.Equal("Go & Grow Zinsen", data2.Description);
            Assert.Equal((decimal)1.29, data2.Amount);
        }
    }
}