using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Logging;
using System.Net.Http;

namespace IcommktOwinAuth
{
    // One instance is created when the application starts.
    public class IcommktAuthenticationMiddleware : AuthenticationMiddleware<IcommktAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public IcommktAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, IcommktAuthenticationOptions options)
            : base(next, options)
        {
            if (string.IsNullOrWhiteSpace(Options.ClientId))
            {
                throw new ArgumentException("The 'ClientId' must be provided.");
            }

            _logger = app.CreateLogger<IcommktAuthenticationMiddleware>();

            if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            }
            if(options.StateDataFormat == null)
            {
                var dataProtector = app.CreateDataProtector(typeof(IcommktAuthenticationMiddleware).FullName,
                    options.AuthenticationType);

                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }
            _httpClient = new HttpClient(ResolveHttpMessageHandler(Options));
            _httpClient.Timeout = Options.BackchannelTimeout;
            _httpClient.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
        }

        // Called for each request, to create a handler for each request.
        protected override AuthenticationHandler<IcommktAuthenticationOptions> CreateHandler()
        {
            return new IcommktAuthenticationHandler(_httpClient, _logger);
        }

        private static HttpMessageHandler ResolveHttpMessageHandler(IcommktAuthenticationOptions options)
        {
            HttpMessageHandler handler = options.BackchannelHttpHandler ?? new WebRequestHandler();

            // If they provided a validator, apply it or fail.
            //if (options.BackchannelCertificateValidator != null)
            //{
                // Set the cert validate callback
                var webRequestHandler = handler as WebRequestHandler;
                if (webRequestHandler == null)
                {
                    throw new InvalidOperationException("Validator Handler Mismatch");
                }
                //webRequestHandler.ServerCertificateValidationCallback = options.BackchannelCertificateValidator.Validate;
            //}

            return handler;
        }
    }
}
