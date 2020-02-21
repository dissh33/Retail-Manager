using System.Threading.Tasks;
using RMDesktopUI.Models;

namespace RMDesktopUI.Helpers
{
    internal interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}