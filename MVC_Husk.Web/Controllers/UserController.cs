//
//  Copyright Info
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Husk.Infrastructure.IdStore;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Model;
using MVC_Husk.Infrastructure.Attributes;

namespace MVC_Husk.Controllers
{
    public class UserController : ApplicationController
    {
        Users _users;

        /// <summary>
        /// The overloaded constructor for the Account Controller
        /// </summary>
        /// <param name="idStore">The Forms Authentication Abstraction</param>
        /// <param name="logger">The Logging Abstraction</param>
        public UserController(IIdStore idStore, ILogger logger)
            : base(idStore, logger)
        {
            _users = new Users();
        }

        [Authorize]
        public ActionResult Index(int pageNum = 1, string searchTerms = "")
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _users.Paged(where: "Email like '%" + searchTerms + "%'", currentPage: pageNum, pageSize: 10);

            var viewModel = new UsersViewModel
            {
                People = users.Items,
                CurrentUser = this.CurrentUser,
                CurrentPage = pageNum,
                TotalRecords = users.TotalRecords,
                TotalPages = users.TotalPages
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Search(string search = "")
        {
            return RedirectToAction("Index", new { searchTerms = search });
        }

        [Authorize]
        public ActionResult Create()
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string email)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = _users.Register(email, email, email, true);

            ViewBag.Message = result.Message;

            return View();
        }

        [Authorize]
        public ActionResult Suspend(string id)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            _users.Update(new { IsActive = 0 }, id);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Activate(string id)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            _users.Update(new { IsActive = 1 }, id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            _users.Delete(id);
            Logger.LogInfo("User " + CurrentUser.ID + " deleted user " + id);
            return RedirectToAction("Index");
        }
    }
}
