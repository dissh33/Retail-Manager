using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccessLogic
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sql;

        public UserData(ISqlDataAccess sqlDataAccess)
        {
            _sql = sqlDataAccess;
        }

        public List<UserModel> GetUserById(string id)
        {
            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", new { Id = id }, "RMData");

            return output;
        }
    }
}