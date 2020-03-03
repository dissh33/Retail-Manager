using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesViewModel;

        public ShellViewModel(IEventAggregator events , SalesViewModel salesViewModel)
        {
            _salesViewModel = salesViewModel;

            events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public sealed override void ActivateItem(object item)
        {
            base.ActivateItem(item);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
        }
    }
}
