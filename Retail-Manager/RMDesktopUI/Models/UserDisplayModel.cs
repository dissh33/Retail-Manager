using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;

namespace RMDesktopUI.Models
{
    public class UserDisplayModel : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();

        private string _roleList;

        public string RoleList
        {
            get
            {
                if (_roleList == null)
                {
                    return string.Join(", ", Roles.Select(x => x.Value));
                }
                else
                {
                    return _roleList;
                }
            }
            set
            {
                _roleList = value;
                OnPropertyChanged(nameof(RoleList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
