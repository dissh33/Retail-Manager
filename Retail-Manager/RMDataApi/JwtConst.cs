using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace RMDataApi
{
    public class JwtConst
    {
        public const string ISSUER = "SomeAuthServer"; 
        public const string AUDIENCE = "RM Api";
        public const int LIFETIME = 1;

        const string KEY = "XXXMySecretKeyIsSecretSoIsSuperSecret123XXX";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
