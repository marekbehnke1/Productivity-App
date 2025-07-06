using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAvalonia.Exceptions
{
    internal class NetworkException : Exception
    {
        public NetworkException() : base() { }

        public NetworkException(string message) : base(message) { }

        public NetworkException(string message, Exception innerException) : base(message, innerException) { }
    }
}
