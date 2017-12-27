using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Spielzeugverleih.Models;

[assembly: OwinStartupAttribute(typeof(Spielzeugverleih.Startup))]
namespace Spielzeugverleih
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login  
        // https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97#content 
        private void createRolesandUsers()   
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


                // In Startup iam creating first Admin Role and creating a default Admin User    
                if (!roleManager.RoleExists("Admin"))
                {

                    // first we create Admin rool   
                    var role = new IdentityRole();
                    role.Name = "Admin";
                    roleManager.Create(role);

                    //Here we create a Admin super user who will maintain the website                  

                    var user = new ApplicationUser();
                    user.UserName = "admin";
                    user.Email = "admin@gmail.com";

                    string userPWD = "admin";

                    var chkUser = UserManager.Create(user, userPWD);

                    //Add default User to Role Admin   
                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user.Id, "Admin");

                    }

                    context.SaveChanges();
                }
            }     
        } 
    }
}
