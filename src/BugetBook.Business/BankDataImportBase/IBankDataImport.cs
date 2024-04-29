namespace BankDataImportBase
{
    public abstract class BankDataImport
    {
        public string Name { get; init; }
        public string[] SupportedFormats { get; init; }

        protected string? _path;

        public virtual bool SetAndCheck(string path)
        {
            if (!File.Exists(path))
                return false;

            if (!SupportedFormats.Contains(Path.GetExtension(path).ToUpper()))
                return false;

            _path = path;

            return IsValid();
        }

        protected abstract bool IsValid();

        public abstract IAsyncEnumerable<InternalBankDataRecord> GetBankData(CancellationToken token = default);
    }
}