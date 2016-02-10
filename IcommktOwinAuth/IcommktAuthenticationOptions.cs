using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IcommktOwinAuth
{
    public class IcommktAuthenticationOptions : AuthenticationOptions
    {
        public IcommktAuthenticationOptions(string clientId)
            : base(Constants.DefaultAuthenticationType)
        {
            Description.Caption = Constants.DefaultAuthenticationType;
            CallbackPath = new PathString("/signin-icommkt");
            AuthenticationMode = AuthenticationMode.Passive;
            ClientId = clientId;
            BackchannelTimeout = TimeSpan.FromSeconds(60);
        }

        public PathString CallbackPath { get; set; }

        public string ClientId { get; set; }

        public string Error { get; set; }

        //public string UserName { get; set; }

       // public string UserId { get; set; }

        

        public string SignInAsAuthenticationType { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        public TimeSpan BackchannelTimeout { get; set; }
        public HttpMessageHandler BackchannelHttpHandler { get; set; }
    }
}
