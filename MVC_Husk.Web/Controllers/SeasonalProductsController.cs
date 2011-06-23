//
//  Copyright Info
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Husk.Infrastructure.IdStore;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Model;
using MVC_Husk.Infrastructure.BackgroundJobs;

namespace MVC_Husk.Controllers
{
    public class SeasonalProductsController : ApplicationController
    {
        SeasonalProducts _product;
        IBackgroundJobManager _jobManager;

        public SeasonalProductsController(IBackgroundJobManager jobManager, IIdStore idStore, ILogger logger)
            : base(idStore, logger)
        {
            _product = new SeasonalProducts();
            _jobManager = jobManager;
        }

        #region File Upload and Display

        /// <summary>
        /// Will accept a file, offload it to Quartz to process, then return to the Index page
        //  where a Grid of the data is shown
        /// </summary>
        /// <param name="file">The File Location</param>
        /// <returns>A Redirect to the Index View</returns>
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);

                Logger.LogDebug("file was saved locally to " + path);

                _jobManager.LoadDataJob("SeasonalProducts", path);
                Logger.LogDebug("The background load of SeasonalProducts was successfully queued");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// The basic jqGrid Query and Pagination
        /// </summary>
        /// <param name="sidx">The column to sort by, default is the order the values were entered</param>
        /// <param name="sord">Ascending or Decending sort order, default is ASC</param>
        /// <param name="page">The page number, default is 1</param>
        /// <param name="rows">The page size, default is 10</param>
        /// <returns></returns>
        public JsonResult GridData(string sidx = "ID", string sord = "asc", int page = 1, int rows = 10)
        {
            var products = _product.Paged(orderBy: sidx + " " + sord, currentPage: page, pageSize: rows);

            Logger.LogDebug("Seasonal grid data requested with page number " + page.ToString() + ", pageSize " + rows.ToString() + " and totalRecords " + products.TotalRecords);

            List<dynamic> items = new List<dynamic>();
            foreach (var item in products.Items)
            {
                items.Add(new {item.ID, item.Week, item.Category, item.SeasonalityIndex});
            }

            var jsonData = new
            {
                total = products.TotalPages,
                page = page,
                records = products.TotalRecords,
                rows = items.ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Used to Plot the JSON via jQuery
        /// </summary>
        /// <returns>Just grab the most recent 10 items from the DB</returns>
        public JsonResult PlotData()
        {
            var products = _product.All(limit: 10, orderBy: "ID desc");

            Logger.LogDebug("grabbed 10 items from db for plot");

            var jsonData =
                (from product in products
                 select new
                 {
                     product.Week,
                     product.SeasonalityIndex
                 }).ToArray();

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        #endregion

        [Authorize]
        public ActionResult Index()
        {
            var products = _product.All();

            SeasonalProductsViewModel model = new SeasonalProductsViewModel
            {
                Products = products
            };

            return View(model);
        }

        //
        // GET: /SeasonalProducts/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string week, string category, string seasonalityIndex)
        {
            var result = _product.CreateSeasonalProduct(week, category, seasonalityIndex);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Message = result.Message;            
            return View();
        }

        [Authorize]    
        public ActionResult Edit(int id)
        {
            var product = _product.Single(id);

            var model = new SeasonalProductViewModel
            {
                Product = product
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Int64 id, string week, string category, string seasonalityIndex)
        {
            var result = _product.UpdateSeasonalProduct(id, week, category, seasonalityIndex);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Message = result.Message;            
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(Int64 id)
        {
            _product.Delete(id);
            Logger.LogInfo("User " + CurrentUser.ID + " deleted Seasonal Product ID: " + id);
            return RedirectToAction("Index");
        }
    }
}
