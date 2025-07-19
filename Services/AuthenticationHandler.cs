using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LearnAvalonia.Models.Dtos;

namespace LearnAvalonia.Services
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationHandler(IAuthenticationService authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check if the auth service is authenticated and has a token
            // include them in the request headers if so
            if (_authService.IsAuthenticated && !string.IsNullOrEmpty(_authService.CurrentToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.CurrentToken);
            }

            
            var response = await base.SendAsync(request, cancellationToken);

            // check if the server response is unauthorized while auth service still thinks we are authorized
            if (response.StatusCode == HttpStatusCode.Unauthorized && _authService.IsAuthenticated)
            {
                _authService.HandleUnauthorized();
            }

            return response;
        }
    }
}
