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
    public class AudioSidebarHelper
    {
        private readonly Audio _db = new Audio();
        private readonly string _urlWord;
        private readonly Page _currentPage;
        public StringBuilder sb = new StringBuilder();
        
        /*
                private readonly IImagefunctions _imageFunction = new ImageModel();
        */

        public AudioSidebarHelper()
        {
            _urlWord = Convert.ToString(HttpContext.Current.Request.RequestContext.RouteData.Values["url"]);
            _currentPage = _db.Pages.FirstOrDefault(a => a.Url == _urlWord);
        }

        public MvcHtmlString Submenu()
        {
            sb.Append("<div class=\"nestedsidemenu\">");
            getCategory(0);
            sb.Append("</div>");
            return new MvcHtmlString(sb.ToString());
        }

        public void getCategory(int categoryId)
        {
            //var sb = new StringBuilder();
            List<Category> parentCategories = new List<Category>();
            parentCategories = _db.Categories.Where(p => p.ParentCategoryId == categoryId).ToList();
            if (parentCategories.Count > 0)
            {
                sb.AppendFormat("<ul>");
                foreach (var parentCategory in parentCategories)
                {
                    sb.AppendFormat("<li><a href=\"/products/{1}\" title=\"{0}\">{0}</a>", parentCategory.CategoryName, parentCategory.FullPath);
                    getCategory(parentCategory.ID);
                    sb.AppendFormat("</li>");
                }
                sb.AppendFormat("</ul>");
            }
        }

        ////Commented 07Mar17 - Recursive categorical modification
        //public MvcHtmlString Submenu()
        //{
        //    var products = _db.Products;
        //    var sb = new StringBuilder();

        //        sb.Append("<div class=\"list-group panel\">");
        //    //sb.Append("<h4>Categories</h4><hr style=\"color:#c96; border:1px solid #c96\"/>");
        //        sb.Append("<ul>");
        //        //var mainmenu = products.GroupBy(p => p.MainCategory).Select(p => p.FirstOrDefault());
        //        var Categories = products.Where(p => p.IsActive == true).Select(n => n.MainCategory).Distinct().ToArray();
        //        if (Categories != null)
        //        {
        //            int j = 0;
        //            foreach (var Category in Categories)
        //            {
        //                j++;
        //                //sb.AppendFormat("<li><a  class=\"main-link\" href=\"/hi-fi/{0}\">{0}</a>\n", Category);
        //                //sb.AppendFormat("<li><a href=\"/hi-fi/{1}\" class=\"list-group-item list-group-item-success colordiv\">{0}<div href=\"#demo{2}\" data-toggle=\"collapse\" data-parent=\"#MainMenu\" class=\"glyphicon glyphicon-plus plus\"></div></a>\n", Category, Category.Replace(" ", "-"), j);
        //                sb.AppendFormat("<li><a href=\"/hi-fi/{1}\" class=\"list-group-item list-group-item-success colordiv\">{0}</a><a href=\"#demo{2}\" data-toggle=\"collapse\" data-parent=\"#MainMenu\" class=\"glyphicon glyphicon-plus plus\"></a>\n", Category, Category.Replace(" ", "-"), j);
        //                var brands = products.Where(p => p.IsActive == true && p.MainCategory == Category && p.Brand != null && p.Brand != string.Empty).Select(n => n.Brand).Distinct().ToArray();
        //                if (brands != null)
        //                {
        //                    int k = j*10;
        //                    sb.AppendFormat("<div class=\"collapse\" id=\"demo{0}\">", j);
        //                    sb.AppendFormat("<ul>");
        //                    foreach (var brand in brands)
        //                    {
        //                        k++;
        //                        //sb.AppendFormat("<li><a  class=\"main-link\" href=\"/hi-fi/{0}/{1}\">{1}</a>\n", Category,brand);
        //                        //sb.AppendFormat("<li><a href=\"/hi-fi/{2}/{3}\" class=\"list-group-item subdiv\">{1}<div href=\"#SubMenu{4}\" data-toggle=\"collapse\" data-parent=\"#SubMenu{4}\" class=\"glyphicon glyphicon-chevron-down\"></div></a> \n", Category, brand, Category.Replace(" ", "-"), brand.Replace(" ", "-"), k);
        //                        sb.AppendFormat("<li><a href=\"/hi-fi/{2}/{3}\" class=\"list-group-item subdiv\">{1}</a><a href=\"#SubMenu{4}\" data-toggle=\"collapse\" data-parent=\"#SubMenu{4}\" class=\"glyphicon glyphicon-minus\"></a></li> \n", Category, brand, Category.Replace(" ", "-"), brand.Replace(" ", "-"), k);
                                
                                

        //                    }
        //                    sb.Append("</ul>");
        //                    sb.Append("</div>");
        //                }
        //                sb.Append("</li>");
        //            }
        //        }
        //        sb.Append("</ul>");
        //        sb.Append("</div>");
        //    return new MvcHtmlString(sb.ToString());
        //}

        public MvcHtmlString BrandTag()
        {
            var sb = new StringBuilder();
            var brands = _db.Brands.Where(p => p.IsActive == true).ToList();
            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    sb.AppendFormat("<a  class=\"tagtext list-group-item custom-item\" href=\"/products/{1}\" title=\"{0}\">{0}</a>\n",brand.BrandName,brand.BrandUrl);
                }
            }
            return new MvcHtmlString(sb.ToString());
        }

        ////Need to reworked - 07Mar17
        //public MvcHtmlString DropDown()
        //{
        //    var dropdown = _db.Products;
        //    var sb = new StringBuilder();
        //    sb.Append("<div class=\"list-group panel\">");
        //    sb.Append("<ul>");
        //    var categories = dropdown.Where(p => p.IsActive == true).Select(n => n.MainCategory).Distinct().ToArray();
        //    if (categories != null)
        //    {
        //        int j = 0;
        //        sb.AppendFormat("<li><a  class=\"main-link\" href=\"#\">Category<div href=\"#demo1\" data-toggle=\"collapse\" data-parent=\"#MainMenu\" class=\"glyphicon glyphicon-plus plus\"></div></a>\n", j);
        //        sb.AppendFormat("<div class=\"collapse\" id=\"demo1\">");
        //        foreach (var Category in categories)
        //        {
        //            j++;
        //            sb.AppendFormat("<li><a href=\"/hi-fi/{1}\" class=\"list-group-item subdiv\">{0}</a> \n", Category, Category.Replace(" ", "-"), j);
        //        }
        //        sb.AppendFormat("</div>");
        //        sb.Append("</li>");
        //    }
        //    sb.Append("</ul>");
        //    sb.Append("</div>");
        //    return new MvcHtmlString(sb.ToString());
        //}
    }
}