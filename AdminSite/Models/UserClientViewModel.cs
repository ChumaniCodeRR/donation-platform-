using InterfaceLayer.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class UserClientViewModel
    {
        public List<SelectListItem> Clients = new List<SelectListItem>();
        public List<SelectListItem> Users = new List<SelectListItem>();
        public List<SelectListItem> Roles = new List<SelectListItem>();

        public List<UserAssignRep> UserAssignmnt { get; set; }
    }
}