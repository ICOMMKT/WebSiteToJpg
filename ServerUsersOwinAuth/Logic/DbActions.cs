using ServerUsersOwinAuth.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServerUsersOwinAuth.Logic
{
    public class DbActions : IDisposable
    {
        private AppDBContext _db = new AppDBContext();
        private UsersDbContext _usrDb = new UsersDbContext();

        /// <summary>
        /// Save a new granted access
        /// </summary>
        /// <param name="usrApps">data of the new access</param>
        public void AddUserAppsAccessGranted(UsersAppsAccessGranted usrApps)
        {
            _db.UsersAppsAccessGranted.Add(usrApps);
            _db.SaveChanges();
        }

        /// <summary>
        /// Checks for token validation asyncronous
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <param name="token">Id string, indentifies and valid the currently permission request</param>
        /// <returns>Token Granted Object</returns>
        public async Task<TokenGranted> AreGrantedPermissionsAsync(string userid)
        {             
            //bool b = false;
            TokenGranted tokenGranted = null;
            IQueryable<UsersAppsAccessGranted> query = null;

            if (userid != String.Empty)
            {
                query = _db.UsersAppsAccessGranted.Where(x => x.Userid == userid);
            }
            var usrAppsGranted = await query.SingleOrDefaultAsync();
            if (usrAppsGranted != null)
            {
                tokenGranted = new TokenGranted();
                if (usrAppsGranted.AccessGranted == true)
                {
                    if (!Utils.IsTokenValid(usrAppsGranted.CreatedOn))
                    {
                        usrAppsGranted.AccessGranted = false;
                        await _db.SaveChangesAsync();
                    }
                }
                tokenGranted.GrantedAccess = usrAppsGranted.AccessGranted; 
                tokenGranted.Token = usrAppsGranted.Token;
                tokenGranted.UserId = usrAppsGranted.Userid;
            }
            return tokenGranted;
        }

        /// <summary>
        /// Checks for token validation
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <param name="token">Id string, indentifies and valid the currently permission request</param>
        /// <returns>Token Granted Object</returns>
        public TokenGranted AreGrantedPermissions(string token)
        {
            //bool b = false;
            TokenGranted tokenGranted = null;
            IQueryable<UsersAppsAccessGranted> query = null;

            if (token != String.Empty)
            {
                query = _db.UsersAppsAccessGranted.Where(x => x.Token == token);
            }
            var usrAppsGranted = query.SingleOrDefault();
            if (usrAppsGranted != null)
            {
                tokenGranted = new TokenGranted();
                if (usrAppsGranted.AccessGranted == true)
                {
                    if (!Utils.IsTokenValid(usrAppsGranted.CreatedOn))
                    {
                        usrAppsGranted.AccessGranted = false;
                       _db.SaveChanges();
                    }
                }
                tokenGranted.GrantedAccess = usrAppsGranted.AccessGranted;
                tokenGranted.Token = usrAppsGranted.Token;
                tokenGranted.UserId = usrAppsGranted.Userid;
            }
            return tokenGranted;
        }

        /// <summary>
        /// Gets external app name
        /// </summary>
        /// <param name="clientid">Client Id</param>
        /// <returns>App Name</returns>
        public async Task<string> GetExternalAppName(string clientid)
        {
            string externalAppName = null;
            externalAppName = await _db.ExternalAuthClients.Where(x => x.Id == clientid).Select(x => x.Name).SingleOrDefaultAsync();
            return externalAppName;
        }

        /// <summary>
        /// Get User Data
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>User Data</returns>
        public  ApplicationUser UserData(string userid)
        {
            if(string.IsNullOrEmpty(userid))
            {
                return null;
            }

            var user = _usrDb.Users.Where(x => x.Id == userid).SingleOrDefault();
            return user;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            if (_usrDb != null)
            {
                _usrDb.Dispose();
                _usrDb = null;
            }
        }
    }
}