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
    public class AudioUrl
    {
    //    private readonly Audio _db = new Audio();
    //    private readonly string _urlWord;

    //    public AudioUrl()
    //    {
    //        _urlWord = Convert.ToString(HttpContext.Current.Request.RequestContext.RouteData.Values["url"]);

    //    }

    //    public MvcHtmlString urlName()
    //    {
           
    //        IQueryable<Product> prds = _db.Products.FirstOrDefault(p => p.Name == _urlWord);
    //        var category = _db.Categories;
    //        var sb = new StringBuilder();
    //        sb.Clear();
    //        foreach(Product product in prds)
    //        {
    //        sb.AppendFormat("<a style=\"color: #333333\" data-fancybox-type=\"ajax\" class=\"fancybox\" title=\"prods\" href=\"\"><img class=\"thumbImage\" src=\"\" data-original=\"\" width=\"220px\" height=\"220px\" alt=\"\"/> </a>", GetUrl("Pages", product.Name));
    //        }
    //        return new MvcHtmlString(sb.ToString());
    //    }

    //    public string GetUrl(string routeName, string ser)
    //    {
    //        switch (routeName)
    //        {
    //            case "Pages":
    //                var page = _db.Products.FirstOrDefault(p => p.Name == ser);
    //                if (page != null)
    //                {
    //                    return MapUrl(routeName, new { url = page.Name});
    //                }
    //                return "#";
                
    //            default:
    //                return MapUrl(routeName, new { url = routeName });
    //        }
    //    }

    //    private string MapUrl(string routeName, object routeParameters)
    //    {
    //        var directory = new RouteValueDictionary(routeParameters);
    //        VirtualPathData pathData = RouteTable.Routes.GetVirtualPath(HttpContext.Current.Request.RequestContext, routeName, directory);
    //        return pathData != null ? pathData.VirtualPath : "#";
    //    }
    }
}