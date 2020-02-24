using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccessLogic;
using RMDataManager.Library.Models;
using System.Linq;
using System.Web.Http;

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

    }

}
