using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer.Models
{
    public interface IEmailSender
    {
        bool SendEmail(List<string> emailTo, string body, string subject, MemoryStream file, String filename);
    }
}
