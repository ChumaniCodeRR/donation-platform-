using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer.Models
{
    public class UserAssignRep
    {
        public int Id { get; set; }
        public string NameIdentifier { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string CompanyName { get; set; }
        public Nullable<System.DateTime> AssignmentDate { get; set; }
        public string AssignedBy { get; set; }
        public bool? IsApproved { get; set; }
        public string RoleName { get; set; }
    }
}
