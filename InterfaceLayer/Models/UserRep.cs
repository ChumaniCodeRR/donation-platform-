using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer.Models
{
    public class UserRep
    {
        public string Id { get; set; }
        public string NameIdentifier { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
