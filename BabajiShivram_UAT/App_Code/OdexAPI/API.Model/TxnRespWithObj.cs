using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdexConnect.API.Model
{
    class TxnRespWithObj<T>
    {
        public bool isSuccess { get; set; }
        public string TxnOutcome { get; set; }
        public string RawData { get; set; }

    }
}
