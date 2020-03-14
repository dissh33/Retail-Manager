using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesViewModel;
        private readonly ILoggedInUserModel _user;
        private readonly IApiClientInitializer _apiClientInitializer;

        public ShellViewModel(IEventAggregator events , SalesViewModel salesViewModel, 
            ILoggedInUserModel user, IApiClientInitializer apiClientInitializer)
        {
            _salesViewModel = salesViewModel;
            _user = user;
            _apiClientInitializer = apiClientInitializer;

            events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }
        public void UserManagement()
        {
            ActivateItem(IoC.Get<UserDisplayViewModel>());
        }

        public sealed override void ActivateItem(object item)
        {
            base.ActivateItem(item);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = (string.IsNullOrWhiteSpace(_user.Token) == false);

                return output;
            }
        }

        public void LogOut()
        {
            _user.ResetUserModel();
            _apiClientInitializer.ClearHeaders();

            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        }
    }
}
