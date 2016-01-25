using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace jrilheu.Owin.Security.Icommkt
{
    public class IcommktAuthenticationOptions : AuthenticationOptions
    {
        public const string Scheme = "Icommkt";

        public IcommktAuthenticationOptions()
            : base(Scheme)
        {
            Caption = Scheme;
            CallbackPath = "/signin-icommkt";
            AuthenticationMode = AuthenticationMode.Passive;
            BackchannelTimeout = TimeSpan.FromSeconds(60);
            Scope = new List<string>();
        }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public ICertificateValidator BackchannelCertificateValidator { get; set; }
        public TimeSpan BackchannelTimeout { get; set; }
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        public string CallbackPath { get; set; }

        public string SignInAsAuthenticationType { get; set; }

        public IIcommktAuthenticationProvider Provider { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        public IList<string> Scope { get; private set; }

    }
}
