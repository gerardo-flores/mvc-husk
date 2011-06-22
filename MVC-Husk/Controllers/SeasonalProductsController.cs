using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Husk.Models;
using System.IO;
using MVC_Husk.Infrastructure.BackgroundJobs;
using MVC_Husk.Infrastructure.Logging;

namespace MVC_Husk.Controllers
{ 
    public class SeasonalProductsController : Controller
    {
        private SeasonalProductDBContext db = new SeasonalProductDBContext();
        private ILogger _logger;
        private IBackgroundJobManager _jobManager;

        public SeasonalProductsController(ILogger logger, IBackgroundJobManager manager)
        {
            _logger = logger;
            _jobManager = manager;
        }

        //
        // GET: /SeasonalProducts/

        public ViewResult Index()
        {
            return View(db.SeasonalProducts.ToList());
        }

        //
        // GET: /SeasonalProducts/Details/5

        public ViewResult Details(int id)
        {
            SeasonalProduct seasonalproduct = db.SeasonalProducts.Find(id);
            return View(seasonalproduct);
        }

        //
        // GET: /SeasonalProducts/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /SeasonalProducts/Create

        [HttpPost]
        public ActionResult Create(SeasonalProduct seasonalproduct)
        {
            if (ModelState.IsValid)
            {
                db.SeasonalProducts.Add(seasonalproduct);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(seasonalproduct);
        }
        
        //
        // GET: /SeasonalProducts/Edit/5
 
        public ActionResult Edit(int id)
        {
            SeasonalProduct seasonalproduct = db.SeasonalProducts.Find(id);
            return View(seasonalproduct);
        }

        //
        // POST: /SeasonalProducts/Edit/5

        [HttpPost]
        public ActionResult Edit(SeasonalProduct seasonalproduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seasonalproduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seasonalproduct);
        }

        //
        // GET: /SeasonalProducts/Delete/5
 
        public ActionResult Delete(int id)
        {
            SeasonalProduct seasonalproduct = db.SeasonalProducts.Find(id);
            return View(seasonalproduct);
        }

        //
        // POST: /SeasonalProducts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            SeasonalProduct seasonalproduct = db.SeasonalProducts.Find(id);
            db.SeasonalProducts.Remove(seasonalproduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //method that will accept a file, offload it to Quartz to process, then return to the Index page
        //where a Grid of the data is shown
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

                _logger.LogDebug("file was saved locally to " + path);

                //TaskFactory factory = HttpRuntime.Cache.Get("BackgroundTaskScheduler") as TaskFactory;
                //factory.StartNew( () => SeasonalProduct.LoadFileData(path));

                _jobManager.LoadDataJob("SeasonalProducts",path);
                _logger.LogDebug("The background load of SeasonalProducts was successfully queued");
            }

            return RedirectToAction("Index", "SeasonalProducts");
        }

        //method that supports the basic jqGrid query and pagination
        //TODO: still need to do sorting
        public JsonResult GridData(string sidx, string sord, int page = 1, int rows = 10)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = db.SeasonalProducts.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var products = db.SeasonalProducts.OrderBy(p => p.id).Skip(pageIndex * pageSize).Take(pageSize);

            _logger.LogDebug("Seasonal grid data requested with pageIndex " + pageIndex + ", pageSize "+pageSize +" and totalRecords " + totalRecords);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from product in products
                        select new
                        {
                           product.id, product.Week, product.Category, product.IDX
                        }).ToArray()

            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PlotData()
        {
            var products = db.SeasonalProducts.Take(10);

            _logger.LogDebug("grabbed 10 items from db for plot");

            var jsonData =
                (from product in products
                 select new
                 {
                     product.Week,
                     product.IDX
                 }).ToArray();

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}