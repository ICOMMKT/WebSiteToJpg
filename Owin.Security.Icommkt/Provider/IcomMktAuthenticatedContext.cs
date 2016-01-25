using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace jrilheu.Owin.Security.Icommkt
{
    public class IcommktAuthenticatedContext : BaseContext
    {
        public IcommktAuthenticatedContext(IOwinContext context, JObject user, string accessToken)
            : base(context)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            User = user;
            AccessToken = accessToken;

            JToken userId = User["id"];
            if (userId == null)
            {
                throw new ArgumentException("The user does not have an id.", "user");
            }

            Id = TryGetValue(user, "id");
            FirstName = TryGetValue(user, "firstName");
            LastName = TryGetValue(user, "lastName");
            Name = FirstName + " " + LastName;
            Email = TryGetValue(JObject.Parse(Contact), "email");
            //Link = "http://localhost:18989/user/" + Id;
        }

        public JObject User { get; private set; }
        public string AccessToken { get; private set; }
        public string Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        //public string Link { get; private set; }
        public string Contact { get; private set; }

        public ClaimsIdentity Identity { get; set; }
        public AuthenticationProperties Properties { get; set; }

        private static string TryGetValue(JObject user, string propertyName)
        {
            JToken value;
            return user.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }

    }




}
