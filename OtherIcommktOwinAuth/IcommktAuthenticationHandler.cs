using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IcommktOwinAuth
{
    // Created by the factory in the DummyAuthenticationMiddleware class.
    class IcommktAuthenticationHandler : AuthenticationHandler<IcommktAuthenticationOptions>
    {
        private const string TokenEndpoint = "http://jrbsag-thinkpc/acme/oauth2/access_token";
        private const string GraphApiEndpoint = "http://jrbsag-thinkpc/acme/users";
        private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";

        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public IcommktAuthenticationHandler(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var error = Request.Query["error"];
            var properties = Options.StateDataFormat.Unprotect(Request.Query["state"]);
            var identity = new ClaimsIdentity(Options.SignInAsAuthenticationType);
            AuthenticationTicket authTicket = null; 

            if (string.IsNullOrEmpty(error))
            {
                // ASP.Net Identity requires the NameIdentitifer field to be set or it won't  
                // accept the external login (AuthenticationManagerExtensions.GetExternalLoginInfo)
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Options.UserId, null, Options.AuthenticationType));
                identity.AddClaim(new Claim(ClaimTypes.Name, Options.UserName));
                authTicket = new AuthenticationTicket(identity, properties);
            }
            else
            {
                Options.Error = error;
                var redirect = properties.RedirectUri;
                redirect = WebUtilities.AddQueryString(redirect, "error", "access_denied");

                Response.Redirect(redirect);
            }

            return Task.FromResult(authTicket);
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            _logger.WriteVerbose("ApplyResponseChallenge");

            if (Response.StatusCode == 401)
            {
                var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

                // Only react to 401 if there is an authentication challenge for the authentication 
                // type of this handler.
                if (challenge != null)
                {
                    string baseUri = Request.Scheme + Uri.SchemeDelimiter + Request.Host + Request.PathBase;

                    string currentUri = baseUri + Request.Path + Request.QueryString;

                    string redirectUri = baseUri + Options.CallbackPath;

                    var state = challenge.Properties;

                    if (string.IsNullOrEmpty(state.RedirectUri))
                    {
                        //state.RedirectUri = Request.Uri.ToString();
                        state.RedirectUri = currentUri;
                    }

                    var stateString = Options.StateDataFormat.Protect(state);

                    //var  callBackPath = WebUtilities.AddQueryString(Options.CallbackPath.Value, "state", stateString);
                    //Response.Redirect(callBackPath);
                    string authorizationEndpoint = "http://jrbsag-thinkpc/acme/Account/authenticate" +
                         "?client_id=" + Uri.EscapeDataString(Options.ClientId) +
                         "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
                        " &state=" + Uri.EscapeDataString(stateString);

                    Response.StatusCode = 302;
                    Response.Headers.Set("Location", authorizationEndpoint);
                }
            }

            return Task.FromResult<object>(null);
        }

        public override async Task<bool> InvokeAsync()
        {
            // This is always invoked on each request. For passive middleware, only do anything if this is
            // for our callback path when the user is redirected back from the authentication provider.
            if (Options.CallbackPath.HasValue && Options.CallbackPath.Value == Request.Path.Value)
            {
                var ticket = await AuthenticateAsync();

                if (ticket != null)
                {
                    Context.Authentication.SignIn(ticket.Properties, ticket.Identity);

                    Response.Redirect(ticket.Properties.RedirectUri);
                }
                // Prevent further processing by the owin pipeline.
                return true;
            }
            // Let the rest of the pipeline run.
            return false;
        }
    }
}
