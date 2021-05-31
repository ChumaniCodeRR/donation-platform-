using InterfaceLayer;
using InterfaceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Core;
using System.Web;

namespace DataLayer.Model
{
    public class UserRepo
    {
        
        public List<UserRep> GetList(string type = null)
        {
            PPSDonationEntities db = new PPSDonationEntities();
            if (type == null)
            {
                var UserRepList = db.AspNetUsers.Where(x => x.AspNetRoles.FirstOrDefault() != null ? x.AspNetRoles.FirstOrDefault().Name.Equals("Client") : false).Select(v => new UserRep
                {
                    Id = v.Id,
                    IsApproved = v.IsApproved,
                    EmailConfirmed = v.EmailConfirmed,
                    NameIdentifier = v.NameIdentifier,
                    PhoneNumber = v.PhoneNumber,
                    PhoneNumberConfirmed = v.PhoneNumberConfirmed,
                    CompanyName = v.CompanyName

                })
              .OrderBy(z => z.NameIdentifier)
              .ToList();
                return UserRepList;
            }
            else if (type.Equals("All"))
            {
                var UserRepList = db.AspNetUsers.Select(v => new UserRep
                {
                    Id = v.Id,
                    IsApproved = v.IsApproved,
                    EmailConfirmed = v.EmailConfirmed,
                    NameIdentifier = v.NameIdentifier,
                    PhoneNumber = v.PhoneNumber,
                    PhoneNumberConfirmed = v.PhoneNumberConfirmed,
                    CompanyName = v.CompanyName

                })
              .OrderBy(z => z.NameIdentifier)
              .ToList();
                return UserRepList;
            }

            return null;
            
        }

        public bool SetLastActivity(string key, string activity, string ip)
        {
            PPSDonationEntities db = new PPSDonationEntities();
           /*
           Log User Activities
            */
            db.SaveChanges();
            return true;
        }

        public List<UserAssignRep> GetRoleAssignment()
        {
            PPSDonationEntities db = new PPSDonationEntities();
            List<UserAssignRep> userAssign = new List<UserAssignRep>();
            var users = db.AspNetUsers.ToList();
            foreach (var u in users)
            {
                UserAssignRep uu = new UserAssignRep
                {
                    NameIdentifier = u.NameIdentifier,
                    RoleName = u.AspNetRoles.FirstOrDefault() !=null ? u.AspNetRoles.FirstOrDefault().Name : "",
                    IsApproved = u.IsApproved,
                    EmailConfirmed = u.EmailConfirmed,
                    CompanyName = u.CompanyName,
                    Email = u.Email
                };

                userAssign.Add(uu);
            }

            return userAssign;
        }

        public List<RoleRep> GetRoleList()
        {
            PPSDonationEntities db = new PPSDonationEntities();
            return db.AspNetRoles.Select(v => new RoleRep {
                RoleName = v.Name,
                RoleId = v.Id
            }).ToList();
        }

        public bool SaveAssignment(UserAssignModel jsondata, string userId, string type = null, string approvedby=null)
        {
            try
            {
                
                PPSDonationEntities db = new PPSDonationEntities();

                if(type == "Role")
                {

                    List<AspNetUser> assignmtList = db.AspNetUsers.Where(u => u.Id == jsondata.user).ToList();
                    assignmtList.ForEach(v =>
                    {
                        try
                        {
                            v.AspNetRoles.Remove(v.AspNetRoles.FirstOrDefault());
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        }catch(NullReferenceException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Ignore this 
                        }
                    });
                    db.SaveChanges();
                    string roleselected = jsondata.list[0];
                    AspNetRole roletoadd = db.AspNetRoles.Where(v => v.Id.Equals(roleselected)).FirstOrDefault();
                    AspNetUser usertobeChanged = assignmtList.FirstOrDefault();
                    usertobeChanged.AspNetRoles.Add(roletoadd);
                    usertobeChanged.IsApproved = true;
                    usertobeChanged.ApprovalDate = DateTime.Now;
                    usertobeChanged.ApprovedBy = approvedby;
                    db.SaveChanges();
                    
                }
                return true;
            }
#pragma warning disable CS0168 // The variable 'exs' is declared but never used
            catch(Exception exs)
#pragma warning restore CS0168 // The variable 'exs' is declared but never used
            {
                return false;
            }
            
        }

        public List<UserAssignRep> GetAssignment()
        {
            PPSDonationEntities db = new PPSDonationEntities();
            var UserAssignmntList = 
            (from user in db.AspNetUsers
             select  new UserAssignRep
            {
                IsApproved = user.IsApproved,
                EmailConfirmed = user.EmailConfirmed,
                NameIdentifier = user.NameIdentifier,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                CompanyName = user.CompanyName,
                
            })
          .ToList();
            return UserAssignmntList;
        }
    }
}