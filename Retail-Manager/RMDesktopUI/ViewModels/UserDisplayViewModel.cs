using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndpoint _userEndpoint;

        private BindingList<UserModel> _users;
        private BindingList<string> _usersRoles;
        private BindingList<string> _availableRoles = new BindingList<string>();

        private UserModel _selectedUser;
        private string _selectedUserName;
        
        private string _selectedUsersRoleToRemove;
        private string _selectedAvailableRoleToAdd;

        public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndpoint userEndpoint)
        {
            _status = status;
            _window = window;
            _userEndpoint = userEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message.Equals("Unauthorized"))
                {
                    _status.UpdateMessage("Access is denied", "You do not have the permission to interact with the Users Form.");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }

                await TryCloseAsync();
            }
        }

        public BindingList<UserModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                SelectedUserName = value.UserName;
                UsersRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                _ = LoadRoles();        //This needs to be awaited 

                NotifyOfPropertyChange(() => SelectedUser);
            }
        }
        public string SelectedUsersRoleToRemove
        {
            get => _selectedUsersRoleToRemove;
            set
            {
                _selectedUsersRoleToRemove = value;
                NotifyOfPropertyChange(() => SelectedUsersRoleToRemove);
                NotifyOfPropertyChange(() => Users);
            }
        }
        public string SelectedAvailableRoleToAdd
        {
            get => _selectedAvailableRoleToAdd;
            set
            {
                _selectedAvailableRoleToAdd = value;
                NotifyOfPropertyChange(() => SelectedAvailableRoleToAdd);
                NotifyOfPropertyChange(() => Users);
            }
        }
        public string SelectedUserName
        {
            get => _selectedUserName;
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }
        public BindingList<string> UsersRoles
        {
            get => _usersRoles;
            set
            {
                _usersRoles = value;
                NotifyOfPropertyChange(() => UsersRoles);
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }
        public BindingList<string> AvailableRoles
        {
            get => _availableRoles;
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        public async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }

        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();
            _availableRoles.Clear();

            foreach (var role in roles)
            {
                if (!UsersRoles.Contains(role.Value))
                {
                    _availableRoles.Add(role.Value);
                }
            }
        }

        public async void AddSelectedRole()
        {
            UserRolePairModel pairing = new UserRolePairModel(SelectedUser.Id, SelectedAvailableRoleToAdd);
            string role = SelectedAvailableRoleToAdd;

            await _userEndpoint.AddUserToRole(pairing);

            UsersRoles.Add(role);
            AvailableRoles.Remove(role);
        }

        public async void RemoveSelectedRole()
        {
            UserRolePairModel pairing = new UserRolePairModel(SelectedUser.Id, SelectedUsersRoleToRemove);
            string role = SelectedUsersRoleToRemove;

            await _userEndpoint.RemoveUserFromRole(pairing);

            UsersRoles.Remove(role);
            AvailableRoles.Add(role);
        }
    }
}
