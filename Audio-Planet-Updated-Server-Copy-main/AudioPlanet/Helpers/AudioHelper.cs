using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AudioPlanet.Models;

namespace AudioPlanet.Helpers
{
    public class AudioHelper
    {
        private readonly Audio _db = new Audio();
        private readonly string _urlWord;
        private readonly Page _currentPage;
        private StringBuilder ssb = new StringBuilder();
            
        /*
                private readonly IImagefunctions _imageFunction = new ImageModel();
        */

        public AudioHelper()
        {
            _urlWord = Convert.ToString(HttpContext.Current.Request.RequestContext.RouteData.Values["url"]);
            _currentPage = _db.Pages.FirstOrDefault(a => a.Url == _urlWord);
        }

        // Main menu
        public MvcHtmlString MainMenu()
        {
            //string group = PageGroup.Header.ToString();
            //IQueryable<Page> primaryPages = _db.Pages.Where(p => p.PageGroup.Contains(group));
            //var sb = new StringBuilder();
            //int i = 0;
            //sb.Clear();
            ////sb.Append("<ul>");Sunil
            //sb.Append("<ul  class=\"nav navbar-nav\">");

            //foreach (Page page in primaryPages)
            //{
            //    i++;
            //    if (page.PageCode == "Home")
            //    {
            //        //sb.AppendFormat(
            //        //    "<li class=\"logo\"><a href=\"/\"><img src=\"/Content/Public/images/AudioPlanet_Logo.png\" /></a></li>");Sunil
            //    }
            //    else
            //    {
            //        if (_currentPage != null)
            //        {
            //            Page parentPage = _db.Pages.Find(_currentPage.ParentId);
            //            if (parentPage != null)
            //            {
            //                sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);

            //                //sb.AppendFormat(
            //                //    page.PageCode == parentPage.PageCode || page.PageCode == _currentPage.PageCode
            //                //        ? "<li class=\"{2} {2}active active\"><a href=\"{0}\">{1}</a></li>"
            //                //        : "<li class=\"{2}\"><a href=\"{0}\">{1}</a></li>", GetUrl("Pages", page.PageCode),
            //                //    page.Name, page.PageCode);Sunil Make it dynamic current page elment white
            //            }
            //            else
            //            {
            //                sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);

            //                //sb.AppendFormat("<li class=\"{2}\"><a href=\"{0}\">{1}</a></li>", GetUrl("Pages", page.PageCode),
            //                //                page.Name, page.PageCode);Sunil
            //            }
            //        }
            //        else
            //        {
            //            sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                            page.Name, page.PageCode);
            //            //sb.AppendFormat("<li class=\"{2}\"><a href=\"{0}\">{1}</a></li>", GetUrl("Pages", page.PageCode),
            //            //                page.Name, page.PageCode);
            //        }

            //        if (i == primaryPages.Count()) continue;
            //        sb.AppendFormat("<li class=\"new_menudivider\"><img src=\"/images/Menu/menudivider.png\" class=\"img-responsive\" alt=\"divider\"></li>");
            //        //sb.AppendFormat("<li class=\"new_menudivider\"><img src=\"images/Menu/menudivider.png\" class=\"img-responsive\"></li>");Sunil
            //        //sb.AppendFormat("<li><img src=\"/Content/Public/images/Menu/menudivider.png\" /></li>");Sunil
            //    }
            //}
            //sb.Append("</ul>");
            //return new MvcHtmlString(sb.ToString());

            //=======================second Day-------------------

            //string group = PageGroup.Header.ToString();
            //IQueryable<Page> primaryPages = _db.Pages.Where(p => p.PageGroup.Contains(group));
            //var sb = new StringBuilder();
            //int i = 0;
            //sb.Clear();
            ////sb.Append("<ul>");Sunil
            //sb.Append("<ul  class=\"nav navbar-nav\">");

            //foreach (Page page in primaryPages)
            //{
            //    i++;
            //    if (page.PageCode == "Home")
            //    {
            //        //sb.AppendFormat(
            //        //    "<li class=\"logo\"><a href=\"/\"><img src=\"/Content/Public/images/AudioPlanet_Logo.png\" /></a></li>");Sunil
            //    }
            //    else
            //    {
            //        if (_currentPage != null)
            //        {
            //            Page parentPage = _db.Pages.Find(page.ID);
            //            int pageId = parentPage.ID;
            //            if (parentPage != null)
            //            {
            //                if (page.PageCode != "ProductsAndBrands")
            //                {
            //                    var childPages = _db.Pages.Where(p => p.ParentId == parentPage.ID && p.IsActive);
            //                    if (childPages.Any())
            //                    {
            //                        sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                    page.Name, page.PageCode);
            //                        int? parentId = parentPage.ID;
            //                        sb.Append("<ul class=\"dropdown-menu\">");
            //                        //sb.Append("<li><img src=\"/images/submenudivider.jpg\" alt=\"divider\"></li>");
            //                        //sb.Append("<li><a href=\"/\">Home</a></li>");
            //                        //sb.Append("<li><a href=\"/\"><img src=\"/Content/Public/images/home.png\" alt=\"home\"></a></li>");Sunil
            //                        if (parentId == parentPage.ID)
            //                        {
            //                            IOrderedEnumerable<Page> childs = _db.Pages.Find(parentId).ChildPages.OrderBy(p => p.Order);
            //                            if (childs.Any())
            //                            {
            //                                foreach (Page pagee in childs.Where(c => c.IsActive).AsQueryable())
            //                                {
            //                                    //sb.Append("<li>");
            //                                    //sb.Append("<img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/>");
            //                                    //sb.Append("</li>");

            //                                    string cssClass = pagee.Title.Length >= 18
            //                                                          ? "selected secondaryMenuTwoLines"
            //                                                          : "secondaryMenuTwoLines";

            //                                    sb.AppendFormat(
            //                                        pagee.PageCode == page.PageCode
            //                                            ? "<li><a class=\"" + cssClass + "\" href=\"{0}\">{1}</a> </li>"
            //                                            : "<li><a href=\"{0}\">{1}</a> </li>",
            //                                        GetUrl("Pages", pagee.PageCode), pagee.Title);
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            Page thisPage = _db.Pages.Find(pageId);
            //                            if (thisPage != null)
            //                            {
            //                                sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");

            //                                sb.AppendFormat(
            //                                    "<li><a class=\"selected secondaryMenuTwoLines\"  href=\"{0}\">{1}</a></li>",
            //                                    GetUrl("Pages", page.PageCode),
            //                                    thisPage.Title);
            //                            }
            //                        }
            //                        //sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");
            //                        sb.Append("</ul>");
            //                        sb.Append("</li>");
            //                    }
            //                    else
            //                    {
            //                        sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);
            //                    }
            //                }
            //                else
            //                {
            //                    sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);
            //                }

            //            }
            //            else
            //            {
            //                if (page.PageCode != "ProductsAndBrands")
            //                {
            //                    var childPages = _db.Pages.Where(p => p.ParentId == parentPage.ID && p.IsActive);
            //                    if (childPages.Any())
            //                    {
            //                        sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                    page.Name, page.PageCode);
            //                        int? parentId = parentPage.ID;
            //                        sb.Append("<ul class=\"dropdown-menu\">");
            //                        //sb.Append("<li><img src=\"/images/submenudivider.jpg\" alt=\"divider\"></li>");
            //                        //sb.Append("<li><a href=\"/\">Home</a></li>");
            //                        //sb.Append("<li><a href=\"/\"><img src=\"/Content/Public/images/home.png\" alt=\"home\"></a></li>");Sunil
            //                        if (parentId == parentPage.ID)
            //                        {
            //                            IOrderedEnumerable<Page> childs = _db.Pages.Find(parentId).ChildPages.OrderBy(p => p.Order);
            //                            if (childs.Any())
            //                            {
            //                                foreach (Page pagee in childs.Where(c => c.IsActive).AsQueryable())
            //                                {
            //                                    //sb.Append("<li>");
            //                                    //sb.Append("<img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/>");
            //                                    //sb.Append("</li>");

            //                                    string cssClass = pagee.Title.Length >= 18
            //                                                          ? "selected secondaryMenuTwoLines"
            //                                                          : "secondaryMenuTwoLines";

            //                                    sb.AppendFormat(
            //                                        pagee.PageCode == page.PageCode
            //                                            ? "<li><a class=\"" + cssClass + "\" href=\"{0}\">{1}</a> </li>"
            //                                            : "<li><a href=\"{0}\">{1}</a> </li>",
            //                                        GetUrl("Pages", pagee.PageCode), pagee.Title);
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            Page thisPage = _db.Pages.Find(pageId);
            //                            if (thisPage != null)
            //                            {
            //                                sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");

            //                                sb.AppendFormat(
            //                                    "<li><a class=\"selected secondaryMenuTwoLines\"  href=\"{0}\">{1}</a></li>",
            //                                    GetUrl("Pages", thisPage.PageCode),
            //                                    thisPage.Title);
            //                            }
            //                        }
            //                        //sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");
            //                        sb.Append("</ul>");
            //                        sb.Append("</li>");
            //                    }
            //                    else
            //                    {
            //                        sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);
            //                    }
            //                }
            //                else
            //                {
            //                    sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Page parentPage = _db.Pages.Find(page.ID);
            //            int pageId = page.ID;
            //            if (page.PageCode != "ProductsAndBrands")
            //            {
            //                var childPages = _db.Pages.Where(p => p.ParentId == parentPage.ID && p.IsActive);
            //                if (childPages.Any())
            //                {
            //                    sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);
            //                    int? parentId = parentPage.ID;
            //                    sb.Append("<ul class=\"dropdown-menu\">");
            //                    //sb.Append("<li><img src=\"/images/submenudivider.jpg\" alt=\"divider\"></li>");
            //                    //sb.Append("<li><a href=\"/\">Home</a></li>");
            //                    //sb.Append("<li><a href=\"/\"><img src=\"/Content/Public/images/home.png\" alt=\"home\"></a></li>");Sunil
            //                    if (parentId == parentPage.ID)
            //                    {
            //                        IOrderedEnumerable<Page> childs = _db.Pages.Find(parentId).ChildPages.OrderBy(p => p.Order);
            //                        if (childs.Any())
            //                        {
            //                            foreach (Page pagee in childs.Where(c => c.IsActive).Where(c => c.IsActive).AsQueryable())
            //                            {
            //                                //sb.Append("<li>");
            //                                //sb.Append("<img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/>");
            //                                //sb.Append("</li>");

            //                                string cssClass = pagee.Title.Length >= 18
            //                                                      ? "selected secondaryMenuTwoLines"
            //                                                      : "secondaryMenuTwoLines";

            //                                sb.AppendFormat(
            //                                    pagee.PageCode == page.PageCode
            //                                        ? "<li><a class=\"" + cssClass + "\" href=\"{0}\">{1}</a> </li>"
            //                                        : "<li><a href=\"{0}\">{1}</a> </li>",
            //                                    GetUrl("Pages", pagee.PageCode), pagee.Title);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Page thisPage = _db.Pages.Find(pageId);
            //                        if (thisPage != null)
            //                        {
            //                            sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");

            //                            sb.AppendFormat(
            //                                "<li><a class=\"selected secondaryMenuTwoLines\"  href=\"{0}\">{1}</a></li>",
            //                                GetUrl("Pages", page.PageCode),
            //                                thisPage.Title);
            //                        }
            //                    }
            //                    //sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");
            //                    sb.Append("</ul>");
            //                    sb.Append("</li>");
            //                }
            //                else
            //                {
            //                    sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                                page.Name, page.PageCode);
            //                }
            //            }
            //            else
            //            {
            //                sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
            //                            page.Name, page.PageCode);
            //            }
            //        }

            //        if (i == primaryPages.Count()) continue;
            //        sb.AppendFormat("<li class=\"new_menudivider\"><img src=\"/images/Menu/menudivider.png\" class=\"img-responsive\" alt=\"divider\"></li>");
            //        //sb.AppendFormat("<li class=\"new_menudivider\"><img src=\"images/Menu/menudivider.png\" class=\"img-responsive\"></li>");Sunil
            //        //sb.AppendFormat("<li><img src=\"/Content/Public/images/Menu/menudivider.png\" /></li>");Sunil
            //    }
            //}
            //sb.Append("</ul>");

            //=================================second day=====================


            string group = PageGroup.Header.ToString();
            IQueryable<Page> primaryPages = _db.Pages.Where(p => p.PageGroup.Contains(group));
            var sb = new StringBuilder();
            int i = 0;
            sb.Clear();
            sb.Append("<ul class=\"nav navbar-nav\">");
            foreach (Page page in primaryPages.OrderBy(p=>p.Order).Where(p=>p.ID != 1))
            {
                i++;
                if (page.PageCode == "Home")
                {
                    //sb.AppendFormat(
                    //    "<li class=\"logo\"><a href=\"/\"><img src=\"/Content/Public/images/AudioPlanet_Logo.png\" /></a></li>");Sunil
                }
                if (_currentPage != null)
                {
                    //if (_currentPage == null)
                    //{
                    //    _currentPage = _db.Pages.FirstOrDefault(p => p.ID == 1);
                    //}
                    if (_currentPage != null)
                    {
                        if (page.PageCode != "ProductsAndBrands")
                        {
                            var childPages = _db.Pages.Where(p => p.ParentId == page.ID && p.IsActive);
                            if (childPages.Any())
                            {
                                sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
                                                page.Name, page.PageCode);
                                IOrderedEnumerable<Page> childs = _db.Pages.Find(page.ID).ChildPages.OrderBy(p => p.Order);

                                sb.Append("<ul class=\"dropdown-menu\">");
                                foreach (Page pagee in childs.Where(c => c.IsActive).AsQueryable())
                                {
                                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a> </li>", GetUrl("Pages", pagee.PageCode), pagee.Title);
                                }
                                sb.Append("</ul>");


                                sb.AppendFormat("</li>");
                            }
                            else
                            {
                                sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
                                            page.Name, page.PageCode);
                            }
                        }
                        else
                        {
                            sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
                                            page.Name, page.PageCode);
                        }
                    }
                }
                else
                {
                    if (page.PageCode == "Home")
                    {
                        //sb.AppendFormat(
                        //    "<li class=\"logo\"><a href=\"/\"><img src=\"/Content/Public/images/AudioPlanet_Logo.png\" /></a></li>");Sunil
                    }
                    if (page.PageCode != "ProductsAndBrands")
                    {
                        var childPages = _db.Pages.Where(p => p.ParentId == page.ID && p.IsActive);
                        if (childPages.Any())
                        {
                            sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
                                            page.Name, page.PageCode);
                            IOrderedEnumerable<Page> childs = _db.Pages.Find(page.ID).ChildPages.OrderBy(p => p.Order);

                            sb.Append("<ul class=\"dropdown-menu\">");
                            foreach (Page pagee in childs.Where(c => c.IsActive).AsQueryable())
                            {
                                sb.AppendFormat("<li><a href=\"{0}\">{1}</a> </li>", GetUrl("Pages", pagee.PageCode), pagee.Title);
                            }
                            sb.Append("</ul>");


                            sb.AppendFormat("</li>");
                        }
                        else
                        {
                            sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
                                        page.Name, page.PageCode);
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<li class=\"dropdown\"><a href=\"{0}\"><img src=\"/images/Menu/{2}.png\" class=\"img-responsive\" alt=\"{2}\"><p>{1}</p></a></li>", page.PageCode == "ProductsAndBrands" ? "/products/" : GetUrl("Pages", page.PageCode),
                                        page.Name, page.PageCode);
                    }
                }
                sb.AppendFormat("<li class=\"new_menudivider\"><img src=\"/images/Menu/menudivider.png\" class=\"img-responsive\"></li>");
            }
            sb.AppendFormat("</ul>");



            return new MvcHtmlString(sb.ToString());
        }

         //Inner Page Submenu
        public MvcHtmlString Submenu()
        {
            var sb = new StringBuilder();

            if (_currentPage != null)
            {
                int pageId = _currentPage.ID;
                int? parentId = _currentPage.ParentId;

                if (parentId == 1)
                {
                    Page firstOrDefault = _currentPage.ChildPages.OrderBy(p => p.Order).FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        HttpContext.Current.Response.Redirect(string.Format("{0}", GetUrl("Pages", firstOrDefault.PageCode)));
                    }
                }
                else
                {
                    parentId = _currentPage.ParentId;
                }

                sb.Clear();
                sb.Append("<ul class=\"dropdown-menu\">");
                sb.Append("<li><img src=\"/images/submenudivider.jpg\" alt=\"divider\"></li>");
                sb.Append("<li><a href=\"/\">Home</a></li>");
                //sb.Append("<li><a href=\"/\"><img src=\"/Content/Public/images/home.png\" alt=\"home\"></a></li>");Sunil
                if (parentId != 1)
                {
                    IOrderedEnumerable<Page> childs = _db.Pages.Find(parentId).ChildPages.OrderBy(p => p.Order);
                    if (childs.Any())
                    {
                        foreach (Page page in childs.Where(c => c.IsActive).AsQueryable())
                        {
                            sb.Append("<li>");
                            sb.Append("<img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/>");
                            sb.Append("</li>");

                            string cssClass = page.Title.Length >= 18
                                                  ? "selected secondaryMenuTwoLines"
                                                  : "secondaryMenuTwoLines";

                            sb.AppendFormat(
                                page.PageCode == _currentPage.PageCode
                                    ? "<li><a class=\"" + cssClass + "\" href=\"{0}\">{1}</a> </li>"
                                    : "<li><a href=\"{0}\">{1}</a> </li>",
                                GetUrl("Pages", page.PageCode), page.Title);
                        }
                    }
                }
                else
                {
                    Page thisPage = _db.Pages.Find(pageId);
                    if (thisPage != null)
                    {
                        sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");

                        sb.AppendFormat(
                            "<li><a class=\"selected secondaryMenuTwoLines\"  href=\"{0}\">{1}</a></li>",
                            GetUrl("Pages", thisPage.PageCode),
                            thisPage.Title);
                    }
                }
                sb.Append("<li><img src=\"/Content/Public/images/submenudivider.jpg\" alt=\"divider\"/></li>");
                sb.Append("</ul>");
            }
            return new MvcHtmlString(sb.ToString());
        }

        //Footer menu
        public MvcHtmlString FooterMenu()
        {
            var db = new Audio();
            //Get all the pages of Home
            IOrderedEnumerable<Page> primaryPages = db.Pages.Find(1).ChildPages.OrderBy(p => p.Order);
            var sb = new StringBuilder();
            var sbSingle = new StringBuilder();
            sb.Clear();
            sbSingle.Clear();
            foreach (Page page in primaryPages)
            {
                IOrderedEnumerable<Page> secondaryPages = db.Pages.Find(page.ID).ChildPages.OrderBy(p => p.Order);
                if (secondaryPages.Any())
                {
                    sb.Append("<div class=\"FooterPrimaryPanel\">");
                    sb.Append("<ul>");
                    if (page.IsPublished)
                    {
                        sb.AppendFormat("<li><a href=\"{0}\">{1}</a>", GetUrl("Pages", page.PageCode), page.Name);
                    }
                    sb.Append("<ul>");
                    foreach (Page sp in secondaryPages)
                    {
                        if (sp.IsPublished)
                        {
                            sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", GetUrl("Pages", sp.PageCode), sp.Name);
                        }
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                    sb.Append("</ul>");
                    sb.Append("</div>");
                }
                else
                {
                    //Pages which don't have child pages will be stored separately
                    if (page.IsPublished)
                    {
                        sbSingle.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", GetUrl("Pages", page.PageCode), page.Name);
                    }
                }
            }

            sb.Append("<div class=\"FooterPrimaryPanel\">");
            sb.Append("<ul>");
            sb.Append(sbSingle);
            sb.Append("</ul>");
            sb.Append("</div>");
            return new MvcHtmlString(sb.ToString());
        }

        //Footer menu
        public MvcHtmlString ProductMain()
        {
            var sb = new StringBuilder();
            if (_currentPage != null)
            {
                int pageId = _currentPage.ID;
                IOrderedEnumerable<Page> childs =
                    _db.Pages.Find(pageId).ChildPages.Where(pc => pc.PageGroup.Contains(PageGroup.Product.ToString()) && pc.IsActive).
                        OrderBy(p => p.Order);
                if (childs.Any())
                {
                    int i = 0;
                    sb.Append("<div id=\"Amplifier_menu\">");
                    sb.Append("<ul id=\"main-nav\">\n");
                    foreach (Page page in childs.AsQueryable())
                    {
                        i++;
                        IOrderedEnumerable<Page> childsOfChild =
                            _db.Pages.Find(page.ID).ChildPages.Where(pc => pc.PageGroup.Contains(PageGroup.Product.ToString()) && pc.IsActive).OrderBy(p => p.Order);
                        if (childsOfChild.Any())
                        {
                            int j = 0;
                            sb.AppendFormat("<li><a id=\"{0}\" class=\"main-link\" href=\"#\">{1}</a>\n",
                                            page.PageCode,
                                            page.Name);
                            sb.Append("<ul class=\"sub-links\">\n");
                            foreach (Page subpage in childsOfChild.AsQueryable())
                            {
                                j++;
                                sb.AppendFormat(
                                    "<li><a class=\"child-link\"  id=\"{0}\" href=\"#\">{1}</a></li>\n",
                                    subpage.PageCode, subpage.Name);
                                if (j == childsOfChild.Count()) continue;
                                sb.Append("<li><img src=\"/Content/Public/images/amp_divider.jpg\" alt=\"\" /></li>\n");
                            }
                            sb.Append("</ul>\n</li>");
                        }
                        else
                        {
                            sb.AppendFormat("<li><a id=\"{0}\" class=\"no-main-link\" href=\"#\">{1}</a></li>\n",
                                            page.PageCode, page.Name);
                        }
                        if (i == childs.Count()) continue;
                        sb.Append("<li><img src=\"/Content/Public/images/amp_divider.jpg\" alt=\"\" /></li>\n");
                    }
                    sb.Append("</ul>\n");
                    sb.Append("</div>");
                }
            }
            return new MvcHtmlString(sb.ToString());
        }

        public MvcHtmlString Sitemap()
        {
            ssb.Clear();
            var root = _db.Pages.FirstOrDefault(p => p.PageCode == "Home");
            if (root != null)
            {
                var pages = _db.Pages.Where(p => p.ParentId == root.ID && p.IsPublished);
                if (pages.Any())
                {
                    ssb.AppendFormat("<ul id=\"sitemap\">");
                    ssb.AppendFormat("<li><a href=\"/\">Home</a></li>");
                    foreach (var page in pages)
                    {
                        Page page1 = page;
                        var children = _db.Pages.Where(p => p.ParentId == page1.ID && p.IsPublished);
                        if (children.Any())
                        {
                            if (page.Name == "Products")
                            {
                                ssb.AppendFormat("<li><a href=\"{0}\">{1}</a>", GetUrl("Pages", page.PageCode), page.Name);
                                getCategory(0);
                                ssb.AppendFormat("</li>");
                            }
                            else
                            {
                                ssb.AppendFormat("<li><a href=\"{0}\">{1}</a>", GetUrl("Pages", page.PageCode), page.Name);
                                ssb.AppendFormat("<ul>");
                                foreach (var child in children)
                                {
                                    ssb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", GetUrl("Pages", child.PageCode), child.Name);
                                }
                                ssb.AppendFormat("</ul></li>");
                            }
                        }
                        else
                        {
                            ssb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", GetUrl("Pages", page.PageCode), page.Name);
                        }
                    }
                    ssb.AppendFormat("</ul>");
                }

            }
            return new MvcHtmlString(ssb.ToString());
        }

        public void getCategory(int categoryId)
        {
            //var sb = new StringBuilder();
            List<Category> parentCategories = new List<Category>();
            parentCategories = _db.Categories.Where(p => p.ParentCategoryId == categoryId).ToList();
            if (parentCategories.Count > 0)
            {
                ssb.AppendFormat("<ul>");
                foreach (var parentCategory in parentCategories)
                {
                    ssb.AppendFormat("<li><a href=\"/products/{1}\" title=\"{0}\">{0}</a>", parentCategory.CategoryName, parentCategory.FullPath);
                    getCategory(parentCategory.ID);
                    ssb.AppendFormat("</li>");
                }
                ssb.AppendFormat("</ul>");
            }
        }

        public string GetUrl(string routeName, string referenceCodeOrUrl)
        {
            switch (routeName)
            {
                case "Pages":
                    var page = _db.Pages.FirstOrDefault(p => p.PageCode == referenceCodeOrUrl);
                    if (page != null)
                    {
                        return MapUrl(routeName, new { url = page.Url.ToLower() });
                    }
                    return "#";
                case "Article":
                    var article = _db.Articles.FirstOrDefault(a => a.Url == referenceCodeOrUrl);
                    if (article != null)
                    {
                        return MapUrl(routeName, new { url = article.Url.ToLower() });
                    }
                    return "#";
                default:
                    return MapUrl(routeName, new { url = referenceCodeOrUrl.ToLower() });
            }
        }

        private string MapUrl(string routeName, object routeParameters)
        {
            var directory = new RouteValueDictionary(routeParameters);
            VirtualPathData pathData = RouteTable.Routes.GetVirtualPath(HttpContext.Current.Request.RequestContext, routeName, directory);
            return pathData != null ? pathData.VirtualPath : "#";
        }
    }
}