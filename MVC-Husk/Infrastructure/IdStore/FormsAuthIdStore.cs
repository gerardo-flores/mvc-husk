using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MVC_Husk.Infrastructure.IdStore
{
    public class FormsAuthIdStore : IIdStore
    {
        /// <summary>
        /// Sets the value of the Forms Authentication Cookie via a Token
        /// </summary>
        /// <param name="token">The Token of the user Session</param>
        public void SetClientAccess(string userId)
        {
            FormsAuthentication.SetAuthCookie(userId, true);
        }

        /// <summary>
        /// Signs out the user
        /// </summary>
        public void RemoveClientAccess()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Fetches the Token from the Cookie
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            return FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        }
    }
}