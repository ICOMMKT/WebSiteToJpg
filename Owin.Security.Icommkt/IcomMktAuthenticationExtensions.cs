using System;
using Microsoft.Owin.Security;
using jrilheu.Owin.Security.Icommkt;

namespace Owin
{
    public static class IcommktAuthenticationExtensions
    {
        public static IAppBuilder UseIcommktAuthentication(this IAppBuilder app, IcommktAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(IcommktAuthenticationMiddleware), app, options);
            return app;
        }

        public static IAppBuilder UseIcommktAuthentication(
            this IAppBuilder app,
            string clientId,
            string clientSecret)
        {
            return UseIcommktAuthentication(
                app,
                new IcommktAuthenticationOptions
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                });
        }
    }
}
