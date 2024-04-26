using BankDataImportBase;
using BondoraPlugin.mapper;
using BondoraPlugin.model;
using ExcelDataReader;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace BondoraPlugin
{
    public class BondoraBankDataImport : BankDataImport
    {
        private readonly string[] rowOfInterrest;

        [SetsRequiredMembers]
        public BondoraBankDataImport()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Name = "Bondaora";
            SupportedFormats = [".XLSX"];
            rowOfInterrest = ["Go & Grow Zinsen".ToUpper(), "SEPA-Banküberweisung".ToUpper(), "Überweisen".ToUpper()];
        }
       
        public override async IAsyncEnumerable<InternalBankDataRecord> GetBankData(CancellationToken cancellationToken = default)
        {
            if (_path is null)
                throw new ArgumentNullException(nameof(_path));

            using (var stream = File.Open(_path, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var tables = reader.AsDataSet().Tables;
                reader.Close();

                foreach (DataTable table in tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        if (rowOfInterrest.Contains((row.ItemArray[1] ?? "").ToString().ToUpper()))
                        {
                            var hasDate = DateTime.TryParse(row.ItemArray[0].ToString(), out DateTime date);
                            var data = new BondoraXslRecord
                            {
                                Date = hasDate ? date : DateTime.MinValue,
                                Type = row.ItemArray[1].ToString().ToDataType(),
                                Amount = decimal.Parse(row.ItemArray[2].ToString().Replace('.',',')),
                                Description = row.ItemArray[1].ToString(),
                            };

                            yield return (InternalBankDataRecord)data;
                        }
                    }
                }

            }
        }

        public override bool IsValid()
        {
            //todo
            return true;
        }
    }
}
