using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer.Models
{
    public class RoleRep
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime>InsertDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
