using ExcelDataReader;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Xunit.Abstractions;
using BondoraPlugin.mapper;
using BondoraPlugin.model;


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
        public void Test1(string path)
        {
            string[] rowOfInterrest = ["Go & Grow Zinsen".ToUpper(), "SEPA-Banküberweisung".ToUpper(), "Überweisen".ToUpper()];

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))  
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var tables = reader.AsDataSet().Tables;
                reader.Close();

                foreach (DataTable table in tables)
                {
                    foreach(DataRow row in table.Rows)
                    {
                        if (rowOfInterrest.Contains(row.ItemArray[1]?.ToString().ToUpper()))
                        {
                            var data = new BondoraXslRecord
                            {
                                Date = DateTime.Parse(row.ItemArray[0].ToString()),
                                Type = row.ItemArray[1].ToString().ToDataType(),
                                Amount = decimal.Parse(row.ItemArray[2].ToString().Replace('.',',')),
                            };
                            var _ = data;
                            _log.WriteLine(data.ToString());
                        }
                    }
                }
            }    
        }
    }
}