using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AudioPlanet.Helpers;
using AudioPlanet.Models;

namespace AudioPlanet.Controllers
{
    public class AccountController : Controller
    {
        private readonly Audio _db = new Audio();
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string encodedPassword = Utilities.EncodePassword(model.Password);
                IQueryable<AdminUser> user =
                    _db.AdminUsers.Where(p => p.Email == model.Email && p.Password == encodedPassword);

                if (user.Any())
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                    AdminUser loggedInUser = user.FirstOrDefault();
                    if (loggedInUser != null) loggedInUser.LastSeen = DateTime.Now;
                    _db.Entry(loggedInUser).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null,
                                      out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    string encodedPassword = Utilities.EncodePassword(model.OldPassword);
                    IQueryable<AdminUser> user =
                        _db.AdminUsers.Where(p => p.Email == User.Identity.Name && p.Password == encodedPassword);

                    AdminUser loggedInUser = user.FirstOrDefault();
                    if (loggedInUser != null) loggedInUser.Password = Utilities.EncodePassword(model.NewPassword);
                    _db.Entry(loggedInUser).State = EntityState.Modified;
                    _db.SaveChanges();
                    changePasswordSucceeded = true;
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // passwordSetSucceeded will throw an exception rather
                // than return false in certain failure scenarios.
                bool passwordSetSucceeded;
                try
                {
                    IQueryable<AdminUser> user = _db.AdminUsers.Where(p => p.Email == model.Email);
                    AdminUser loggedInUser = user.FirstOrDefault();
                    //if (loggedInUser != null && Helpers.SendMail())
                    //{
                    //    passwordSetSucceeded = true;
                    //}
                    //else
                    //{
                    //    passwordSetSucceeded = false;
                    //}
                    passwordSetSucceeded = true;
                }
                catch (Exception)
                {
                    passwordSetSucceeded = false;
                }

                if (passwordSetSucceeded)
                {
                    return RedirectToAction("ForgotPasswordSuccess");
                }
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }
            return View(model);
        }

        public ActionResult ForgotPasswordSuccess()
        {
            return View();
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion
    }
}