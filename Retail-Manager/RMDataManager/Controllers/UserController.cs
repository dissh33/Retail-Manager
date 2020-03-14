using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccessLogic;
using RMDataManager.Library.Models;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using RMDataManager.Models;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {

        // GET: User/Details/5
        [HttpGet]
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData data = new UserData();

            return data.GetUserById(userId).First();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        UserName = user.UserName
                    };

                    foreach (var r in user.Roles)
                    {
                        u.Roles.Add(r.RoleId, roles.First(x => x.Id == r.RoleId).Name);
                    }

                    output.Add(u);
                }
            }

            return output;
        }
    }

}
