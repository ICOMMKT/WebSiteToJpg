using Owin;


namespace IcommktOwinAuth
{
    public static class IcommkAuthenticationExtensions
    {
        public static IAppBuilder UseIcommkAuthentication(this IAppBuilder app, IcommktAuthenticationOptions options)
        {
            return app.Use(typeof(IcommktAuthenticationMiddleware), app, options);
        }
    }
}
