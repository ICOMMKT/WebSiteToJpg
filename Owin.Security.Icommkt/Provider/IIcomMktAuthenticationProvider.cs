using System.Threading.Tasks;

namespace jrilheu.Owin.Security.Icommkt
{
    public interface IIcommktAuthenticationProvider
    {
        Task Authenticated(IcommktAuthenticatedContext context);
        Task ReturnEndpoint(IcommktReturnEndpointContext context);
    }
}
