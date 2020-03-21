using System.Collections.Generic;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccessLogic
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string id);
    }
}