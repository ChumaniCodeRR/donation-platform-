using DataLayer.Model;
using DataLayer.Core;
using InterfaceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceLayer.Service
{
    public class UserService
    {
        public List<UserRep> GetList(bool IsAClient, string Key, string Type = null)
        {
            return new UserRepo().GetList(Type);
        }
        public List<UserAssignRep> GetAssignmntList(bool IsAClient, string Key)
        {
            return new UserRepo().GetAssignment();
        }

        public bool SaveAssignment(UserAssignModel jsondata, string UserId, string type = null, string name = null)
        {
            return new UserRepo().SaveAssignment(jsondata, UserId, type,name);
        }

        public List<RoleRep> GetRoleList(bool IsAClient, string Key)
        {
            return new UserRepo().GetRoleList();
            
        }

        public List<UserAssignRep> GetUserRoleAssignmntList(bool IsAClient, string Key)
        {
            return new UserRepo().GetRoleAssignment();
        }
        public bool LastActivity(string Key, string IP, String ActivityName)
        {
            try
            {
                return new UserRepo().SetLastActivity(Key, ActivityName, IP);
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            }catch(Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }
    }
}