using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerUsersOwinAuth.Logic
{
    public static class Utils
    {
        /// <summary>
        /// Checks For token expire date 
        /// </summary>
        /// <param name="createdOn">Token Created date</param>
        /// <returns>Valid or not valid token</returns>
        public static bool IsTokenValid(DateTime? createdOn)
        {
            bool b = true;
            DateTime when = (DateTime)createdOn;
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                b = false;
            }
            return b;
        }
    }
}