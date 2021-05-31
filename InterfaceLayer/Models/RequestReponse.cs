using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceLayer.Models
{
    public class RequestReponse
    {
        public string URL {get; set;}
        public bool Error { get; set; }
        public string FriendlyMessage { get; set; }

    }
}