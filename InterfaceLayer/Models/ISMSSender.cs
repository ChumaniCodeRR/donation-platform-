using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer
{
    public interface ISMSSender
    {
        bool SendSMS(List<string> cellnumbers, string msg);
    }
}
