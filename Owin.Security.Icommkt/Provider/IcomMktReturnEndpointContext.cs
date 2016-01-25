using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using System.Collections.Generic;

namespace jrilheu.Owin.Security.Icommkt
{
    public class IcommktReturnEndpointContext : ReturnEndpointContext
    {
        public IcommktReturnEndpointContext(
            IOwinContext context,
            AuthenticationTicket ticket)
            : base(context, ticket)
        {
        }
    }
}
