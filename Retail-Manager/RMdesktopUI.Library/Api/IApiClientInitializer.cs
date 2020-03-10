using System.Net.Http;

namespace RMDesktopUI.Library.Api
{
    public interface IApiClientInitializer
    {
        HttpClient ApiClient { get; }
        
        void ClearHeaders();
    }
}