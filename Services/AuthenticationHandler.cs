using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
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
            Debug.WriteLine($"Handler: Processing {request.Method} {request.RequestUri}");
            Debug.WriteLine($"Auth State: IsAuth={_authService.IsAuthenticated}, HasToken={!string.IsNullOrEmpty(_authService.CurrentToken)}");


            // Check if the auth service is authenticated and has a token
            // include them in the request headers if so
            if (_authService.IsAuthenticated && !string.IsNullOrEmpty(_authService.CurrentToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.CurrentToken);
                Debug.WriteLine($"Added bearer to token to request");
            }
            else
            {
                Debug.WriteLine("Did not add bearer to request");
            }

            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                Debug.WriteLine($"Response: {response.StatusCode}");

                // check if the server response is unauthorized while auth service still thinks we are authorized
                if (response.StatusCode == HttpStatusCode.Unauthorized && _authService.IsAuthenticated)
                {
                    Debug.WriteLine("401 received - calling Handle Unauthorized");
                    _authService.HandleUnauthorized();
                }

                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Handler Exception: {ex.Message}");
                throw;
            }
        }
    }
}
