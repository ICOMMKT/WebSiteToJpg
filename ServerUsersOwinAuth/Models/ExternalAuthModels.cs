using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace ServerUsersOwinAuth.Models
{
    public partial class ExternalAuthClients
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UsersAppsAccessGranted> UsersAppsAccessGranted { get; set; }
    }

    public partial class UsersAppsAccessGranted
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string ExternalAuthClientsID { get; set; }
        public bool AccessGranted { get; set; }
        public string Token { get; set; }
        public string Key { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual ICollection<ExternalAuthClients> ExternalAuthClients { get; set; }
    }

    public partial class TokenGranted
    {
        public bool GrantedAccess { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }

    public class AppDBContext : DbContext
    {
        public AppDBContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer<AppDBContext>(null);
        }
        //public static AppDBContext Create()
       // {
       //     return new AppDBContext();
       // }
        public DbSet<ExternalAuthClients> ExternalAuthClients { get; set; }

        public DbSet<UsersAppsAccessGranted> UsersAppsAccessGranted { get; set; }
    }

    public class AppDBInitializer : DropCreateDatabaseIfModelChanges<AppDBContext> { }
}