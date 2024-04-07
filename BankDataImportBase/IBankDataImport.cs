namespace BankDataImportBase
{
    public interface IBankDataImport
    {
        public string  Name { get; }
        public string[] SupportedFormats { get; }

        public bool SetAndCheckPath(string path);
        public bool IsValid();
        public IAsyncEnumerable<InternalBankDataFormat> GetBankData(CancellationToken token = default);
    }
}
