using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.BLL
{
    public class BusinessMessage
    {
        public string Message { get; set; }
        public string ViewName { get; set; }
        public object ViewModel { get; set; }
    }
}
