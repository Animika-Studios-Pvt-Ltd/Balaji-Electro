using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AudioPlanet.Areas.SEM.Models;
using AudioPlanet.Models;
using Ionic.Zip;

namespace AudioPlanet.Areas.SEM.Controllers
{
    public class LandingPageController : Controller
    {
        private readonly Audio _db = new Audio();

        //
        // GET: /SEM/LandingPage/

        public ViewResult Index()
        {
            return View(_db.LandingPages.ToList());
        }

        //
        // GET: /SEM/LandingPage/Details/5

        public ViewResult Details(int id)
        {
            LandingPage landingpage = _db.LandingPages.Find(id);
            return View(landingpage);
        }

        //
        // GET: /SEM/LandingPage/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SEM/LandingPage/Create

        [HttpPost]
        public ActionResult Create(LandingPage landingpage, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                _db.LandingPages.Add(landingpage);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(landingpage);
        }

        //
        // GET: /SEM/LandingPage/Edit/5

        public ActionResult Edit(int id)
        {
            LandingPage landingpage = _db.LandingPages.Find(id);
            return View(landingpage);
        }

        //
        // POST: /SEM/LandingPage/Edit/5

        [HttpPost]
        public ActionResult Edit(LandingPage landingpage)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(landingpage).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(landingpage);
        }

        //
        // GET: /SEM/LandingPage/Delete/5

        public ActionResult Delete(int id)
        {
            LandingPage landingpage = _db.LandingPages.Find(id);
            return View(landingpage);
        }

        //
        // POST: /SEM/LandingPage/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            LandingPage landingpage = _db.LandingPages.Find(id);
            _db.LandingPages.Remove(landingpage);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Upload(int id)
        {
            ViewBag.LandingPageID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, FormCollection frm)
        {
            try
            {
                var id = Convert.ToInt16(frm["LandingPageID"]);
                LandingPage landingpage = _db.LandingPages.Find(id);
                if (file != null && file.ContentLength > 0 && landingpage != null)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var dir =
                            new DirectoryInfo(
                                Server.MapPath(string.Format("~/Content/Uploads/Campaigns/{0}/", landingpage.ID)));
                        if (dir.Exists)
                        {
                            FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
                            foreach (FileInfo fileInfo in files)
                            {
                                fileInfo.Delete();
                            }
                        }
                        else
                        {
                            dir.Create();
                        }

                        //fileName = landingpage.Name.Replace(" ", string.Empty) + Path.GetExtension(fileName);
                        string path = Path.Combine(dir.FullName, fileName);

                        // Save the File
                        file.SaveAs(path);

                        // Extract the File Content in same folder
                        using (ZipFile zip1 = ZipFile.Read(path))
                        {
                            // here, we extract every entry, but we could extract conditionally
                            // based on entry name, size, date, checkbox status, etc.  
                            foreach (ZipEntry e in zip1)
                            {
                                e.Extract(dir.FullName, ExtractExistingFileAction.OverwriteSilently);
                            }
                        }

                        // Read HTML file and Replace the Values
                        using (var clientw = new WebClient())
                        {
                            var htmlCode =
                                clientw.DownloadString(
                                    System.Web.HttpContext.Current.Server.MapPath(
                                        string.Format("/Content/Uploads/Campaigns/{0}/{1}/Index.html", landingpage.ID,
                                                      Path.GetFileNameWithoutExtension(fileName))));

                            htmlCode = htmlCode.Replace("css/",
                                                       string.Format("/Content/Uploads/Campaigns/{0}/{1}/css/",
                                                                     landingpage.ID,
                                                                     Path.GetFileNameWithoutExtension(fileName)));
                            htmlCode = htmlCode.Replace("js/",
                                                       string.Format("/Content/Uploads/Campaigns/{0}/{1}/js/",
                                                                     landingpage.ID,
                                                                     Path.GetFileNameWithoutExtension(fileName)));

                            htmlCode = htmlCode.Replace("images/",
                                                        string.Format("/Content/Uploads/Campaigns/{0}/{1}/images/",
                                                                      landingpage.ID,
                                                                      Path.GetFileNameWithoutExtension(fileName)));

                            const string layoutCode = "@{Layout = null;}";

                            //Create a new File in Campaign Views
                            using (var fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(string.Format("/Views/Campaign/{0}.cshtml", landingpage.Url.Replace("/Campaign/", ""))), FileMode.Create))
                            {
                                using (var w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    w.WriteLine(layoutCode + "\n" + htmlCode);
                                }
                            }
                        }

                    }
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("Upload");
        }
    }
}