﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RMDesktopUI.ViewModels
{
    class LoginViewModel : Screen
    {
        private string _userName;
            private string _password;

            public string UserName
            {
                get => _userName;
                set
                {
                    _userName = value;
                    NotifyOfPropertyChange(() => UserName);
                    NotifyOfPropertyChange(() => CanLogIn);
                }
            }

            public string Password
            {
                get => _password;
                set
                {
                    _password = value;
                    NotifyOfPropertyChange(() => Password);
                    NotifyOfPropertyChange(() => CanLogIn);
                }
            }

            public bool CanLogIn
            {
                get
                {
                    bool output = UserName?.Length > 0 && Password?.Length > 0;

                    return output;
                }
            }

            public void LogIn()
            {
                Console.WriteLine();
            }
        }
    }

