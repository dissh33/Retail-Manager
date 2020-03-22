using Caliburn.Micro;
using RMDesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUI.Helpers
{
    public class DialogWindowHelper : IDialogWindowHelper
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        public DialogWindowHelper(StatusInfoViewModel status, IWindowManager window)
        {
            _status = status;
            _window = window;
        }

        public async Task ShowSystemError(Exception ex, string viewName)
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.Title = "System Error";

            if (ex.Message.Equals("Unauthorized"))
            {
                _status.UpdateMessage("Access is denied", $"You do not have the permission to interact with {viewName}.");
                await _window.ShowDialogAsync(_status, null, settings);
            }
            else
            {
                _status.UpdateMessage("Fatal Exception", ex.Message);
                await _window.ShowDialogAsync(_status, null, settings);
            }
        }
    }
}
