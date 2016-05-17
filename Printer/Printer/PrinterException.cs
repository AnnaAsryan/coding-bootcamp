using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer
{
    class PrinterException:Exception
    {
        public PrinterException(string ex) : base(ex)
        {

        }
    }
}
