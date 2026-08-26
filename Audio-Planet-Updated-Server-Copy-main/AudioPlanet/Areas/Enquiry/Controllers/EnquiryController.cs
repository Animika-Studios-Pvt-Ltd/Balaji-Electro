using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AudioPlanet.Models;
using AudioPlanet.Controllers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace AudioPlanet.Areas.Enquiry.Controllers
{

    public class EnquiryController : Controller
    {
        private readonly Audio _db = new Audio();
        private static readonly TimeZoneInfo IndianZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        [Authorize]
        public ActionResult Index()
        {
            var enquiry = _db.Enquirys.ToList();
            return View(enquiry);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Enquiry objEnquiry, FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var enquiryCookie = Request.Cookies["_vinfoCnsl"];
                    //if (enquiryCookie != null)
                    //{
                    //    var visitor = _db.Visitors.Find(Convert.ToInt32(enquiryCookie.Value));
                    //    if (visitor != null)
                    //    {
                    //        objEnquiry.VisitorID = Convert.ToInt32(enquiryCookie.Value);
                    //        objEnquiry.Visitor = visitor;
                    //    }
                    //    enquiryCookie.Expires = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianZone).AddDays(-1d);
                    //    Response.Cookies.Add(enquiryCookie);
                    //}
                    //validate captcha 
                    if (Session["Captcha"] == null || Session["Captcha"].ToString() != objEnquiry.Captcha)
                    {
                        ModelState.AddModelError("Captcha", "Wrong value of sum, please try again.");
                        //dispay error and generate a new captcha 
                        return Redirect("/");
                    }
                    else
                    {
                        _db.Enquirys.Add(objEnquiry);
                        _db.SaveChanges();
                        //new MailController().NewEnquiry(objEnquiry).Deliver();
                        new MailController().NewEnquiry(objEnquiry).Deliver();
                        if (frm["source"] != null)
                        {
                            switch (frm["source"])
                            {
                                case "main":
                                    return RedirectToRoute("Pages", new { Url = "Thanks_for_your_inquiry" });
                                case "communication":
                                    return RedirectToRoute("Pages", new { Url = "Thanks_for_your_interest" });
                            }
                        }
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Models.Enquiry objEnquiry = _db.Enquirys.Find(id);
            return View(objEnquiry);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Models.Enquiry objEnquire)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(objEnquire).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error in Updating the record, Please try after sometime.");
                return View(objEnquire);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message.ToString(CultureInfo.InvariantCulture));
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            Models.Enquiry objEnquiry = _db.Enquirys.Find(id);
            try
            {
                _db.Enquirys.Remove(objEnquiry);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message.ToString(CultureInfo.InvariantCulture));
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + small_alphabets + numbers;
            int length = 6;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            var captcha = string.Format("{0}", otp);

            //store answer 
            //Session["Captcha"] = a + b;
            Session["Captcha" + prefix] = otp;

            //image stream 
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise 
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, (x - r), (y - r), r, r);
                    }
                }

                //add question 
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }
    }
}