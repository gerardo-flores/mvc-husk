//
//  Copyright Info
//

using System.Web.Mvc;
using MVC_Husk.Infrastructure.IdStore;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Model;

namespace MVC_Husk.Controllers
{
    public class ApplicationController : Controller
    {   
        private dynamic _currentUser;

        /// <summary>
        /// Used for abstract out Forms Authentications
        /// </summary>
        public IIdStore IdStore;
        
        /// <summary>
        /// Used to Log
        /// </summary>
        public ILogger Logger;

        /// <summary>
        /// Empty detault constructor
        /// </summary>
        public ApplicationController() { }

        /// <summary>
        /// Overloaded constructor to the Application Controller
        /// </summary>
        /// <param name="idStore">The Forms Authentication Abstraction</param>
        /// <param name="logger">The Logging Abstraction</param>
        public ApplicationController(IIdStore idStore, ILogger logger)
        {
            IdStore = idStore;
            Logger = logger;
        }

        /// <summary>
        /// Returns the Current User object from the Users based on the ID
        /// </summary>
        public dynamic CurrentUser
        {
            get
            {
                var id = IdStore.GetUserId();

                if (!string.IsNullOrEmpty(id))
                {
                    _currentUser = new Users().Single(id);
                }
                else
                {
                    _currentUser = null;
                }

                // User has a Cookie, but are not in the database, remove the Forms Auth Cookie.
                if (_currentUser == null)
                {
                    IdStore.RemoveClientAccess();
                }

                return _currentUser;
            }
        }

        /// <summary>
        /// Returns if there is a Current User
        /// </summary>
        public bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }

        /// <summary>
        /// Returns if the Current User is an Admin
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                if (CurrentUser != null)
                    return CurrentUser.IsAdmin;

                return false;
            }
        }
    }
}
