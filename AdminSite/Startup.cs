using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ServiceLayer.Models;

[assembly: OwinStartupAttribute("AdminWebsiteOWin",typeof(WebApplication.Startup))]
namespace WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }
        // In this method we will create default User roles and Admin user for login   
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var user = UserManager.FindByEmail("gilbertnganduk@gmail.com");
            var user = UserManager.FindByEmail("chumani@vetro.co.za");

            if (user != null)
            {
                var result1 = UserManager.AddToRole(user.Id, "Admin");

            }


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  
                //user = UserManager.FindByEmail("gilbertnganduk@gmail.com");
                user = UserManager.FindByEmail("chumani@vetro.co.za");

                if (user != null)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
               
                /*var user = new ApplicationUser();
                user.UserName = "Gilbert";
                user.Email = "gilbertnganduk@gmail.com";

                string userPWD = "P@ssword2828";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }*/
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Client"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Client";
                roleManager.Create(role);

            }
        }
    }
}
