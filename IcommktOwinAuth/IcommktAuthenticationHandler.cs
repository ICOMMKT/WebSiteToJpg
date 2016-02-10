using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;
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
        private const string TokenEndpoint = "http://jrbsag-thinkpc/ServerUsersOwinAuth/oauth2/access_token";
        private const string GraphApiEndpoint = "http://jrbsag-thinkpc/ServerUsersOwinAuth/users";
        private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";

        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public IcommktAuthenticationHandler(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationTicket authTicket = null;
            try
            {
                _logger.WriteVerbose("AuthenticateCoreAsync");
                string error = null, token = null, state = null;

                IReadableStringCollection query = Request.Query;

                IList<string> values = query.GetValues("error");
                if (values != null && values.Count == 1)
                {
                    error = values[0];
                }
                values = query.GetValues("state");
                if (values != null && values.Count == 1)
                {
                    state = values[0];
                }
                var properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return null;
                }
                values = query.GetValues("token");
                if (values != null && values.Count == 1)
                {
                    token = values[0];
                }
                var graphUri = GraphApiEndpoint + "/" + Uri.EscapeDataString(token);
                HttpResponseMessage graphResponse = await _httpClient.GetAsync(
                    graphUri,  Request.CallCancelled);
                graphResponse.EnsureSuccessStatusCode();
                string accountString = await graphResponse.Content.ReadAsStringAsync();
                JObject accountInformation = JObject.Parse(accountString);
                var userman = accountInformation.Children()["id"];//["response"]["user"];
                var user = accountInformation;
                var context = new IcommktAuthenticatedContext(Context, user, token);
                
                var identity = new ClaimsIdentity(Options.SignInAsAuthenticationType);
                
                if (string.IsNullOrEmpty(error))
                {
                    // ASP.Net Identity requires the NameIdentitifer field to be set or it won't  
                    // accept the external login (AuthenticationManagerExtensions.GetExternalLoginInfo)
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.Id, null, Options.AuthenticationType));
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Email, context.Email));
                    authTicket = new AuthenticationTicket(identity, properties);
                }
                else
                {
                    Options.Error = error;
                    var redirect = properties.RedirectUri;
                    redirect = WebUtilities.AddQueryString(redirect, "error", "access_denied");

                    Response.Redirect(redirect);
                }
            }
            catch(Exception ex)
            {
                _logger.WriteWarning("Authentication failed", ex);
            }
           
            return authTicket;
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
                        state.RedirectUri = currentUri;
                    }

                    var stateString = Options.StateDataFormat.Protect(state);

                    string authorizationEndpoint = "http://jrbsag-thinkpc/ServerUsersOwinAuth/Account/authenticate" +
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
