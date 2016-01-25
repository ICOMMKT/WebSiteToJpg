using System;
using System.Threading.Tasks;

namespace jrilheu.Owin.Security.Icommkt
{
    public class IcommktAuthenticationProvider : IIcommktAuthenticationProvider
    {
        public IcommktAuthenticationProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
            OnReturnEndpoint = context => Task.FromResult<object>(null);
        }

        public Func<IcommktAuthenticatedContext, Task> OnAuthenticated { get; set; }

        public Func<IcommktReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

        public virtual Task Authenticated(IcommktAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }

        public virtual Task ReturnEndpoint(IcommktReturnEndpointContext context)
        {
            return OnReturnEndpoint(context);
        }
    }
}
