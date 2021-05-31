using System;
using System.Collections.Generic;


namespace InterfaceLayer.Models
{
    public class DashboardViewModel
    {
        public int OrderDelivered { get; set; }
        public int OrdersOnRoute { get; set; }
        public decimal Open500ppm { get; set; }
        public decimal Open50ppm { get; set; }
        public decimal ULP93 { get; set; }
        public int OpenOrders { get; set; }
        public decimal TotalLiters { get; set; }
        public decimal Swapped { get; set; }
        public int Sent { get; set; }
        public int Received { get; set; }
    }

    

}