using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnAvalonia.Services;

namespace LearnAvalonia.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        public EventHandler? RegisterSucceeded;
        public EventHandler? NavigateToLogin;

        public RegisterViewModel(IAuthenticationService authService)
        {
            _authService = authService;
        }

        // TODO: Add registration Logic
    }
}
