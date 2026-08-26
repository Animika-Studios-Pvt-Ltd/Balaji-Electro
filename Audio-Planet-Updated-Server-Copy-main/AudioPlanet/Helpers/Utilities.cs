using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AudioPlanet.Models;

namespace AudioPlanet.Helpers
{
    public static class Utilities
    {
        public static MvcHtmlString DisplayBool(bool value)
        {
            return value
                       ? new MvcHtmlString("<span class=\"ok\">&nbsp;</span>")
                       : new MvcHtmlString("<span class=\"cancel\">&nbsp;</span>");
        }

        public static IHtmlString DisplayPublished(bool value)
        {
            return value
                       ? new MvcHtmlString("<span class=\"icon icon35\"></span>")
                       : new MvcHtmlString("<span class=\"icon icon197\"></span>");
        }

        public static string GetUrl(string routeName, object routeParameters)
        {
            var directory = new RouteValueDictionary(routeParameters);
            VirtualPathData pathData = RouteTable.Routes.GetVirtualPath(HttpContext.Current.Request.RequestContext,
                                                                        routeName, directory);
            if (pathData != null)
            {
                return pathData.VirtualPath;
            }
            return null;
        }

        public static MvcHtmlString GetProfile(string email)
        {
            var db = new Audio();
            AdminUser user = db.AdminUsers.FirstOrDefault(p => p.Email == email);
            var sb = new StringBuilder();
            sb.Clear();
            if (user != null)
            {
                sb.Append("<div>");
                sb.Append("<div class=\"thumb\">");

                var dir =
                    new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Content/Uploads/Admin/" + user.ID + "/"));
                if (dir.Exists)
                {
                    FileInfo[] files = dir.GetFiles();
                    if (files.Any())
                    {
                        foreach (FileInfo fileInfo in files)
                        {
                            if (File.Exists(fileInfo.FullName))
                            {
                                sb.AppendFormat(
                                    "<img alt=\"profile image\" src=\"/Content/Uploads/Admin/{0}/{1}\" width=\"100\" height=\"100\" />",
                                    user.ID, fileInfo.Name);
                            }
                            else
                            {
                                sb.AppendFormat(
                                    "<img src=\"/Content/Uploads/Admin/default_{0}.jpg\" width=\"100\" height=\"100\"/>",
                                    user.Gender);
                            }
                            break;
                        }
                    }
                    else
                    {
                        sb.AppendFormat(
                            "<img src=\"/Content/Uploads/Admin/default_{0}.jpg\" width=\"100\" height=\"100\"/>",
                            user.Gender);
                    }
                }
                sb.AppendFormat(
                    "<span class=\"overlaylink\"><a href=\"/Admin/ProfileImage/{0}\">Change photo</a></span>", user.ID);
                sb.Append("</div>");
                sb.AppendFormat(
                    "<div class=\"details\"><span class=\"name\">{0}</span><br /> <a href=\"mailto:{1}\" class=\"email\">{1}</a><br /><span class=\"lastseen\"><strong>Last seen :</strong>{2}</span><br /><span class=\"lastseen\"><a href=\"/Account/ChangePassword\">Change Password</a></span><br /><a href=\"/AdminUser/Edit/{3}\" class=\"button action blue\"><span class=\"label\">Edit Profile</span></a>",
                    user.Name, user.Email, user.LastSeen, user.ID);
                sb.Append("</div></div>");
            }
            return new MvcHtmlString(sb.ToString());
        }


        internal static string DecodePassword(string encodedPassword)
        {
            try
            {
                var objEncoding = new UTF8Encoding();
                Decoder objDecoder = objEncoding.GetDecoder();

                byte[] getDecodeByte = Convert.FromBase64String(encodedPassword);
                int charCount = objDecoder.GetCharCount(getDecodeByte, 0, getDecodeByte.Length);
                var getDecodedChar = new char[charCount];
                objDecoder.GetChars(getDecodeByte, 0, getDecodeByte.Length, getDecodedChar, 0);

                var result = new String(getDecodedChar);
                return result;
            }
            catch (Exception onDecodePassword)
            {
                throw new Exception("Error During Decoding. Reason: " + onDecodePassword.Message);
            }
        }

        internal static string EncodePassword(string plainPassword)
        {
            try
            {
                byte[] getEncodeByte = Encoding.UTF8.GetBytes(plainPassword);
                return Convert.ToBase64String(getEncodeByte);
            }
            catch (Exception onEncodePassword)
            {
                throw new Exception("Error During Encoding. Reason: " + onEncodePassword.Message);
            }
        }

        public static string Truncate(this HtmlHelper helper, string input, int length, string width = "auto", bool stripParagraph = false)
        {
            //string style = string.Format("style=\"width:{0}\"", width);
            string style = string.Empty;

            //input = input.Replace("<p>", string.Empty).Replace("</p>", string.Empty);
            //return input.Length <= length
            //           ? string.Format("<p {0}>" + input + "</p>", style)
            //           : string.Format("<p {0}>" + input.Substring(0, length) + "..." + "</p>", style);

            input = Regex.Replace(input, @"<[^>]*>", String.Empty);
            return !stripParagraph
                       ? input.Length <= length
                             ? string.Format("<p {0}>" + input + "</p>", style)
                             : string.Format("<p {0}>" + input.Substring(0, length) + " .." + "</p>", style)
                       : input.Length <= length ? input : input.Substring(0, length) + " ..";
        }

    }
}