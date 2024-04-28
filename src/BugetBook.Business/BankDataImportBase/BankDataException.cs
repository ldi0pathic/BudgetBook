namespace BankDataImportBase
{
    public class BankDataException : Exception
    {
        public BankDataException(string msg) : base(msg)
        {
        }

        public BankDataException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
    }
}