using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

            events.SubscribeOnUIThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>(), CancellationToken.None);
        }

        public sealed override Task ActivateItemAsync(object item, CancellationToken cancellationToken)
        {
            return base.ActivateItemAsync(item, cancellationToken);
        }

        public async void UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), CancellationToken.None);
        }


        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel, cancellationToken);
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

        public async void LogOut()
        {
            _user.ResetUserModel();
            _apiClientInitializer.ClearHeaders();

            await ActivateItemAsync(IoC.Get<LoginViewModel>(), CancellationToken.None);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async void ExitApplication()
        {
            await TryCloseAsync();
        }
    }
}
