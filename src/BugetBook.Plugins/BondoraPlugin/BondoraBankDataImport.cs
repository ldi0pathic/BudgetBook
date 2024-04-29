using BankDataImportBase;
using BondoraPlugin.mapper;
using BondoraPlugin.model;
using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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

                foreach (DataRow row in tables[0].Rows)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (rowOfInterrest.Contains((row.ItemArray[1] ?? "").ToString().ToUpper()))
                    {
                        var data = new BondoraXslRecord
                        {
                            Date = row.ItemArray[0].ParseOrDefault(DateTime.MinValue),
                            Type = row.ItemArray[1].ToDataType(),
                            Amount = row.ItemArray[2].ToString().Replace('.', ',').ParseOrDefault(0m),
                            Description = row.ItemArray[1].ToString(),
                        };

                        yield return (InternalBankDataRecord)data;
                    }
                }
            }
        }

        private bool CheckHeaderRow(DataRow row)
        {
            if (row.HasErrors ||
               row.ItemArray.Length != 5 ||
               !row.ItemArray[0].ToString().Equals("Datum") ||
               !row.ItemArray[1].ToString().Equals("Zahlungsart") ||
               !row.ItemArray[2].ToString().Equals("Eingänge") ||
               !row.ItemArray[3].ToString().Equals("Ausgänge") ||
               !row.ItemArray[4].ToString().Equals("Guthaben")
               )
                return false;

            return true;
        }

        protected override bool IsValid()
        {
            if (_path is null)
                return false;
            bool head = false;
            using (var stream = File.Open(_path, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var tables = reader.AsDataSet().Tables;
                reader.Close();

                foreach (DataRow row in tables[0].Rows)
                {
                    if (!head)
                    {
                        if (!CheckHeaderRow(row))
                            continue;

                        head = true;
                    }
                }
            }
            return head;
        }
    }
}