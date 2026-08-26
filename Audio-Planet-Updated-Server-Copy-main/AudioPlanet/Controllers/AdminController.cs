using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly Picture _pic = new Picture();
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult UploadCss()
        {
            var folder = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/Public"));
            FileInfo[] files = folder.GetFiles("*.css");
            var list = new SelectList(files, "Name", "Name");

            HttpCookie httpCookie = ControllerContext.HttpContext.Request.Cookies["Style"];
            if (httpCookie != null)
            {
                list = new SelectList(files, "Name", "Name", httpCookie.Value);
            }

            ViewBag.FileList = list;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadCss(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                string fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                if (fileName != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Public/"), fileName);
                    file.SaveAs(path);
                }
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult SelectCss(FormCollection frm)
        {
            string fileName = Path.GetFileName(frm["Files"]);
            if (fileName != null)
            {
                var cookie = new HttpCookie("Style") { Value = fileName };
                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ErrorLog()
        {
            //Response.Redirect("~/elmah");
            return View();
        }

        [Authorize]
        public ActionResult ImageLibrary(string id = "")
        {
            List<Picture> lstFiles = _pic.GetFiles(id);
            return View(lstFiles);
        }

        public ActionResult ProfileImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProfileImage(HttpPostedFileBase file, int id)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                string fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                if (fileName != null)
                {
                    var dir = new DirectoryInfo(Server.MapPath("~/Content/Uploads/Admin/" + id + "/"));
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    else
                    {
                        FileInfo[] files = dir.GetFiles();
                        foreach (FileInfo fileInfo in files)
                        {
                            fileInfo.Delete();
                        }
                    }
                    string path = Path.Combine(dir.FullName, fileName);
                    file.SaveAs(path);
                }
            }
            // redirect back to the index action to show the form once again
            //return RedirectToAction("Index");
            return View("Index");
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    string fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var dir = new DirectoryInfo(Server.MapPath("~/Content/Uploads/General/"));
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }

                        string path = Path.Combine(dir.FullName, fileName);
                        file.SaveAs(path);
                    }
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ViewBag.ErrorMessage =
                    "Unable to save changes. Try again, and if the problem persists see your system administrator (http://support.worlditlab.com).";
            }
            return RedirectToAction("ImageLibrary", "Admin");
        }

        public ActionResult Delete(string url)
        {
            var file = new FileInfo(Server.MapPath(url));
            if (file.Exists)
            {
                file.Delete();
            }
            return RedirectToAction("ImageLibrary", "Admin");
        }

        public ActionResult ChangeType(string mediaType)
        {
            return RedirectToAction("ImageLibrary", "Admin", new {id = mediaType});
        }
    }
}