using System;
using System.Threading.Tasks;

namespace RMDesktopUI.Helpers
{
    public interface IDialogWindowHelper
    {
        Task ShowSystemError(Exception ex, string viewName);
    }
}