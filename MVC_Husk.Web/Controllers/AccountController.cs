//
//  Copyright Info
//

using System.Web.Mvc;
using MVC_Husk.Infrastructure.IdStore;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Model;

namespace MVC_Husk.Controllers
{
    public class AccountController : ApplicationController
    {
        Users _users;

        /// <summary>
        /// The overloaded constructor for the Account Controller
        /// </summary>
        /// <param name="idStore">The Forms Authentication Abstraction</param>
        /// <param name="logger">The Logging Abstraction</param>
        public AccountController(IIdStore idStore, ILogger logger) : base(idStore, logger)
        {
            _users = new Users();
        }

        /// <summary>
        /// Get for the LogOn Action
        /// </summary>
        /// <returns>Redirect to the LogOn Page</returns>
        public ActionResult LogOn()
        {
            return View();
        }

        /// <summary>
        /// Post for the LogOn Action
        /// </summary>
        /// <param name="email">The Email Address of the User</param>
        /// <param name="password">The Password of the User</param>
        /// <returns>Action to either the Home Page or back to the LogOn Page</returns>
        [HttpPost]
        public ActionResult LogOn(string email, string password)
        {
            var result = _users.Login(email, password);
            if (result.Authenticated)
            {
                IdStore.SetClientAccess(result.User.ID.ToString());

                if (result.User.RequirePasswordChange)
                    return View("ChangePassword");

                if (result.User.IsFirstVisit)
                    return View("ClickThru");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = result.Message;
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        /// <summary>
        /// Get for the LogOff Action
        /// </summary>
        /// <returns>Redirect to the Home Page</returns>
        public ActionResult LogOff()
        {
            IdStore.RemoveClientAccess();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Post Register Action
        /// </summary>
        /// <param name="email">The Email Address of the User</param>
        /// <param name="password">The Password of the User</param>
        /// <param name="confirmation">The Password Confirmation</param>
        /// <returns>Action to either the Home Page or back to the LogOn Page</returns>
        [HttpPost]
        public ActionResult Register(string email, string password, string confirmation)
        {
            var result = _users.Register(email, password, confirmation);
            if (result.Success)
            {      
                IdStore.SetClientAccess(result.UserId.ToString());
                return View("ClickThru");
            }
            else
            {
                ViewBag.Message = result.Message;
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        /// <summary>
        /// Get for the Change Password Action
        /// </summary>
        /// <returns>Redirect to the Change Password or the back to the LogOn View</returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            var model = new UserViewModel
            {
                Person = CurrentUser
            };

            return View(model);
        }

        /// <summary>
        /// Post for the Change Password Action
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <param name="password">The user's current password</param>
        /// <param name="newPassword">The user's new password</param>
        /// <param name="confirmNewPassword">The confirmation of the user's new password</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(string email, string password, string newPassword, string confirmNewPassword)
        {
            var result = _users.ChangePassword(email, password, newPassword, confirmNewPassword);

            if (result.Success == true)
            {
                if (result.User.IsFirstVisit)
                    return View("ClickThru");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = result.Message;
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        /// <summary>
        /// Get for the Change Password Success Action
        /// </summary>
        /// <returns>Redirect to the Change Password Success</returns>
        [Authorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// Get for the Click Thru 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ClickThru()
        {
            if (!CurrentUser.IsFirstVisit)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ClickThru(string accept)
        {
            if (accept == null)
            {
                IdStore.RemoveClientAccess();
                return RedirectToAction("Index", "Home");
            }

            if (accept == "false")
            {
                IdStore.RemoveClientAccess();
                return RedirectToAction("Index", "Home");
            }

            _users.Update(new { IsFirstVisit = 0 }, CurrentUser.ID);

            return RedirectToAction("Index", "Home");
        }
    }
}
