using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Core;
using InterfaceLayer.Models;

namespace ServiceLayer.BLL
{
    public class BusinessRule
    {
        PPSDonationEntities db = new PPSDonationEntities();

        

        ///<summary>
        ///This function will check if a user is approved or not
        ///</summary>
        ///<remarks>
        ///When a client login, someone at Belton Park will have to link the client to one or multiple companies in our database
        ///</remarks>
        public BusinessMessage IsAllowedToLogin(String Email)
        {

            AspNetUser Currentuser = db.AspNetUsers.Where(x => x.Email == Email).FirstOrDefault();
            if (Currentuser==null)
            {
                return null;
            }
            else if (!Currentuser.IsApproved.Value){
                return new BusinessMessage
                {
                    Message = "",
                    ViewName = "NotApproved"
                };
            }

            return null;
        }
       
        ///<summary>
        ///This function will get the list of all the clients that the user is allowed to see
        ///</summary>
       
        
    }
}