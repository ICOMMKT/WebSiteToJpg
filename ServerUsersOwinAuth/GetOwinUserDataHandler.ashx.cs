using Newtonsoft.Json;
using ServerUsersOwinAuth.Logic;
using ServerUsersOwinAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ServerUsersOwinAuth
{
    /// <summary>
    /// Summary description for GetOwinUserData
    /// </summary>
    public class GetOwinUserDataHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public  void ProcessRequest(HttpContext context)
        {
            string json = "";
            context.Response.ContentType = "text/json";
            var tokenObject = RequestContext.RouteData.Values["token"];
            var hasContent = tokenObject == null ? false : true;
            if (hasContent)
            {
                var token = tokenObject.ToString();
                var dbAction = new DbActions();
                var tokenGrant = dbAction.AreGrantedPermissions(token);
                if(tokenGrant.GrantedAccess)
                {
                    var user = dbAction.UserData(tokenGrant.UserId);
                    var responseData = new {
                        Id = user.Id,
                        Email = user.Email
                    };

                    json = JsonConvert.SerializeObject(responseData);
                }
            }

            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class RouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new GetOwinUserDataHandler() { RequestContext = requestContext };
        }
    }
}