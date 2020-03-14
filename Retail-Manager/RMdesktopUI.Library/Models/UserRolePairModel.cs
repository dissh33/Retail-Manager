using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Models
{
    public class UserRolePairModel
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public UserRolePairModel(string id, string roleName)
        {
            UserId = id;
            RoleName = roleName;
        }
    }
}
