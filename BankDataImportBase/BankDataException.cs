using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDataImportBase
{
    public class BankDataException : Exception
    {
       public BankDataException(string msg, Exception innerException): base (msg, innerException) { } 
    }
}
