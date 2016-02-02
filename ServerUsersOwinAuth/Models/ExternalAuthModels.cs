using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace ServerUsersOwinAuth.Models
{
    public class ExternalAuthClients
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UsersAppsAccessGranted> UsersAppsAccessGranted { get; set; }
    }
    public class AuthTokens
    {
        [Key]
        public string Id { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        public string ExternalAuthClientsID { get; set; }

        public virtual ExternalAuthClients ExternalAuthClients { get; set; }
    }

    public class UsersAppsAccessGranted {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string ExternalAuthClientsID { get; set; }
        public bool AccessGranted { get; set; }

        public virtual ICollection<ExternalAuthClients> ExternalAuthClients { get; set; }
    }

    public class AppDBContext : DbContext
    {
        public AppDBContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer<AppDBContext>(null);
        }
        public DbSet<ExternalAuthClients> ExternalAuthClients { get; set; }

        public DbSet<AuthTokens> AuthTokens { get; set; }

        public DbSet<UsersAppsAccessGranted> UsersAppsAccessGranted { get; set; }
    }
}