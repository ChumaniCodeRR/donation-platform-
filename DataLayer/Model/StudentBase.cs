using InterfaceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DataLayer.Model
{
    public class StudentBase : IStudent
    {
        public int Id { get; set; }

        public string StudentID { get; set; }
    }
}