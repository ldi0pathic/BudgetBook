using BondoraPlugin;
using BondoraPlugin.mapper;
using BondoraPlugin.model;
using ExcelDataReader;
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
            Assert.Equal(1,data.Count(s => s.Type.Equals(BankDataImportBase.DataType.SavingPlan)));
            Assert.Equal(15,data.Count(s => s.Type.Equals(BankDataImportBase.DataType.Interest)));

            //todo
        }
    }
}