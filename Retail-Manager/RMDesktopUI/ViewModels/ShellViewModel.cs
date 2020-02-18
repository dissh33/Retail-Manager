using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        public ShellViewModel(LoginViewModel loginViewModel)
        {
            ActivateItem(loginViewModel);
        }

        public sealed override void ActivateItem(object item)
        {
            base.ActivateItem(item);
        }
    }
}
