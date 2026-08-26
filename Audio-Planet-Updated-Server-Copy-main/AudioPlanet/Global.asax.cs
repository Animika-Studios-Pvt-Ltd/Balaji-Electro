using System.Web.Mvc;
using System.Web.Routing;


namespace AudioPlanet
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new
            {
                favicon = "(.*/)?favicon.ico(/.*)?"
            });

            
            routes.MapRoute("Pages", "systems/{url}", new
            {
                controller = "Home",
                action = "Details"
            });

            routes.MapRoute("Products", "products/{*url}", new
            {
                controller = "Home",
                action = "FetchProducts",
                url = UrlParameter.Optional
            });

            

            //routes.MapRoute("Category", "hi-fi/Products/{url}", new
            //{
            //    controller = "Home",
            //    action = "CategoryDetail"
            //});

            //routes.MapRoute("brds", "hi-fi/Brand/{url}", new
            //{
            //    controller = "Home",
            //    action = "brandsTag"
            //});

            //routes.MapRoute("CategoryBrand", "hi-fi/{*url}", new
            //{
            //    controller = "Home",
            //    action = "CategoryDetaill"
            //});

            ////routes.MapRoute("products", "hi-fi/Products", new
            ////{
            ////    controller = "Home",
            ////    action = "CategoryDetail"
            ////});

            ////routes.MapRoute("Product", "hi-fi/ProductDetail/{name}/{id}", new
            ////{
            ////    controller = "Home",
            ////    action = "ProductDeatils",
            ////    name = UrlParameter.Optional,
            ////    id = UrlParameter.Optional
            ////    //url = UrlParameter.Optional
            ////});

            //routes.MapRoute("NSubCategory", "hi-fi/{category}/{brand}", new
            //{
            //    controller = "Home",
            //    action = "CategoryAndBrand",
            //    category = UrlParameter.Optional,
            //    brand = UrlParameter.Optional
            //});

            routes.MapRoute("noSubCategory", "hi-fi/{category}/{brand}/{name}", new
            {
                controller = "Home",
                action = "CategoryAndBrandAndItem",
                category = UrlParameter.Optional,
                brand = UrlParameter.Optional,
                name = UrlParameter.Optional
            });

            //routes.MapRoute("subBrand", "hi-fi/{category}/{brand}/{subCategory}/{name}", new
            //{
            //    controller = "Home",
            //    action = "CategoryAndBrandItem",
            //    category = UrlParameter.Optional,
            //    brand = UrlParameter.Optional,
            //    subcategory = UrlParameter.Optional,
            //    name = UrlParameter.Optional
            //});

           
           
            //routes.MapRoute("CategoryBrandSubCat", "hi-fi/{category}/{brand}/{subcategory}", new
            //{
            //    controller = "Home",
            //    action = "CategoryAndBrand",
            //    //action = "CategoryBrandSubcategory",
            //    category = UrlParameter.Optional,
            //    brand = UrlParameter.Optional,
            //    subcategory = UrlParameter.Optional
            //});

           

            routes.MapRoute("Article", "articles/{url}", new
            {
                controller = "Home",
                action = "ArticleDetails"
            });
            routes.MapRoute("Campaign", "Campaign/{url}", new
            {
                controller = "Campaign",
                action = "Index",
                url = UrlParameter.Optional
            });
            //routes.MapRoute("System", "Systems/{url}", new
            //{
            //    //url: "web/news/Default.aspx",
            //    //defaults: new { controller = "Redirect", action = "News" }
            //    controller= "Home",
            //    action = "redirect"
            //});

            //routes.MapRoute("Show", "{controller}/{action}/{url}", new
            //{
            //    controller = "Home",
            //    action = "show",
            //    url = UrlParameter.Optional,
                //name = UrlParameter.Optional
            //});

            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            });
        }

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        //    routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

        //    routes.MapRoute(
        //        "Pages",                                           // Route name
        //        "Systems/{url}",                            // URL with parameters
        //        new { controller = "Home", action = "Details" }  // Parameter defaults
        //    );

        //    routes.MapRoute(
        //        "Article",                                           // Route name
        //        "Articles/{url}",                            // URL with parameters
        //        new { controller = "Home", action = "ArticleDetails" }  // Parameter defaults
        //    );

        //    routes.MapRoute(
        //       "Campaign",                                           // Route name
        //       "Campaign/{url}",                            // URL with parameters
        //       new { controller = "Campaign", action = "Index", url = UrlParameter.Optional }  // Parameter defaults
        //   );

        //    routes.MapRoute(
        //        "Default", // Route name
        //        "{controller}/{action}/{id}", // URL with parameters
        //        new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
        //    );

        //}

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}