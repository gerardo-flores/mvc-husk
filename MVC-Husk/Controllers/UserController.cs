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
using MVC_Husk.Models;

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
        public ActionResult Index()
        {

            if (!IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _users.Paged();

            var viewModel = new UsersViewModel
            {
                People = users.Items,
                TotalRecords = users.TotalRecords,
                TotalPages = users.TotalPages
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(string[] ids, List<string> deactivate)
        {
            // Loop through all the records that come back and 
            // set the Active/Inactive value correctly.
            foreach (var id in ids)
            {
                if (deactivate.Contains(id))
                    _users.Update(new { IsActive = 0 }, id);
                else
                    _users.Update(new { IsActive = 1 }, id);
            }

            return RedirectToAction("Index");
        }




        ////
        //// POST: /Admin/Create

        //[HttpPost]
        //public ActionResult CreateUser(string email, string password, string isAdmin)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /Admin/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Admin/Edit/5


        ////
        //// GET: /Admin/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Admin/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
