using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAvalonia.Models
{
    public enum AuthChangeReason
    {
        Login,
        Logout,
        TokenExpired,
        TokenRefreshed,
        NetworkError,
        ApiError
    }
}
