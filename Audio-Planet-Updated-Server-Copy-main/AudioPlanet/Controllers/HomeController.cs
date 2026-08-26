using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AudioPlanet.Models;
using System.Collections;

namespace AudioPlanet.Controllers
{
    public class HomeController : Controller
    {
        private readonly Audio _db = new Audio();

        List<Product> prdlist = new List<Product>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string url)
        {
            string cUrl = null;
            bool IsRouteUrl = false;
            switch (url)
            {
                case "Amplifier":
                    cUrl = _db.Categories.Any(c => c.FullPath == "hi-fi/amplifiers" && c.IsActive == true) ? "hi-fi/amplifiers" : null;
                    IsRouteUrl = true;
                    break;
                case "Source":
                    cUrl = _db.Categories.Any(c => c.FullPath == "hi-fi/source" && c.IsActive == true) ? "hi-fi/source" : null;
                    IsRouteUrl = true;
                    break;
                case "Speakers":
                    cUrl = _db.Categories.Any(c => c.FullPath == "hi-fi/speakers" && c.IsActive == true) ? "hi-fi/speakers" : null;
                    IsRouteUrl = true;
                    break;
                case "Turntables":
                    cUrl = _db.Categories.Any(c => c.FullPath == "hi-fi/source/turn-tables" && c.IsActive == true) ? "hi-fi/source/turn-tables" : null;
                    IsRouteUrl = true;
                    break;
                case "Aktimate-Micro":
                case "Products-and-Brands":
                    cUrl = null;
                    IsRouteUrl = true;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(cUrl) && IsRouteUrl == true)
            {
                return RedirectPermanent("/products/" + cUrl + "/");
            }
            else if(string.IsNullOrEmpty(cUrl) && IsRouteUrl == true)
            {
                return RedirectPermanent("/products/");
            }

            //if (url == "Amplifier") { return RedirectPermanent("/hi-fi/Amplifiers"); }
            //if (url == "Source") { return RedirectPermanent("/hi-fi/Source"); }
            //if (url == "Speakers") { return RedirectPermanent("/hi-fi/Speakers"); }
            //if (url == "Aktimate-Micro") { return RedirectPermanent("/hi-fi/Aktimate-Micro"); }
            //if (url == "Turntables") { return RedirectPermanent("/hi-fi/TurnTables"); }

            Page requestedPage = _db.Pages.FirstOrDefault(p => p.Url == url);
            if (requestedPage != null)
            {
                var fileAbsPath = string.Format("/Content/Uploads/Page/{0}.jpg", requestedPage.PageCode);
                var file = Server.MapPath(fileAbsPath);
                ViewBag.FilePath = System.IO.File.Exists(file) ? fileAbsPath : "/Content/Public/images/speaker.jpg";
                ViewBag.ParentId = requestedPage.ParentId;

                if (requestedPage.PageCode.Equals("ExpertSpeaks"))
                {
                    ViewBag.Categories = _db.Pages.SqlQuery("SELECT DISTINCT [Page].* FROM [Page] INNER JOIN [Article] ON [Article].[CategoryID]=[Page].[ID]").ToList();
                    ViewBag.FeaturedArticles = _db.Articles.Where(a => a.IsFeatured && a.IsActive);
                    ViewBag.LatestArticles = _db.Articles.Where(a => a.IsActive).OrderByDescending(o => o.ID);
                    return View("Articles", requestedPage);
                }

                if (requestedPage.PageCode.Equals("ProductReview90"))
                {
                    //ViewBag.Categories = _db.Pages.SqlQuery("SELECT DISTINCT [Page].* FROM [Page] INNER JOIN [Article] ON [Article].[CategoryID]=[Page].[ID]").ToList();
                    //ViewBag.FeaturedArticles = _db.Articles.Where(a => a.IsFeatured && a.IsActive);
                    //ViewBag.LatestArticles = _db.Articles.Where(a => a.IsActive).OrderByDescending(o => o.ID);
                    ViewBag.productReviews = _db.Reviews.Where(a => a.IsActive).OrderByDescending(o => o.ID);
                    return View("ProductReview", requestedPage);
                }

                if (requestedPage.PageCode.Equals("ProductsAndBrands") || requestedPage.PageGroup.Contains(PageGroup.Product.ToString()))
                {
                    //return View("Details2", requestedPage);
                    var products = _db.Products.Take(30);
                    ViewBag.Title = "Audio Planet - Bangalore | India";
                    return View("CategoryDetail", products);
                }
                return View(requestedPage);
            }
            return RedirectToRoute("Pages", new { Url = "Page_Not_Found" });
        }

        public ActionResult ProductDeatils(int id)
        {
            var product = _db.Products.Find(id);
            var redirectString = "/products/" + product.ProductFullUrl;
            return RedirectPermanent(redirectString);
            //return RedirectPermanent(redirectString);
        }

        public ActionResult ArticleDetails(string url)
        {
            Article requestedArticle = _db.Articles.FirstOrDefault(a => a.Url == url);
            if (requestedArticle != null)
            {
                requestedArticle.TotalViews += 1;
                _db.Entry(requestedArticle).State = EntityState.Modified;
                _db.SaveChanges();

                ViewBag.Categories = _db.Pages.SqlQuery("SELECT DISTINCT [Page].* FROM [Page] INNER JOIN [Article] ON [Article].[CategoryID]=[Page].[ID]").ToList();
                ViewBag.LatestArticles = _db.Articles.Where(a => a.IsActive).OrderByDescending(o => o.ID);
                return View(requestedArticle);
            }
            return RedirectToRoute("Pages", new { Url = "Page_Not_Found" });
        }

        public ActionResult ProductReviewDetails(string url)
        {
            Review requestedProductReview = _db.Reviews.FirstOrDefault(a => a.Url == url);
            if (requestedProductReview != null)
            {
                //requestedArticle.TotalViews += 1;
                _db.Entry(requestedProductReview).State = EntityState.Modified;
                _db.SaveChanges();
                ViewBag.productReviews = _db.Reviews.Where(a => a.IsActive).OrderByDescending(o => o.ID);
                return View(requestedProductReview);
            }
            return RedirectToRoute("Pages", new { Url = "Page_Not_Found" });
        }

        public ActionResult GetArticlesByCategory(string category)
        {
            var categoryId = Convert.ToInt16(category);
            var articles = _db.Articles.Where(a => a.CategoryID == categoryId);
            return PartialView("Widgets/FeaturedArticles", articles.ToList());
        }

        public ActionResult GetArticlesByKeyword(string keyword)
        {
            var articles = _db.Articles.Where(a => a.Title.Contains(keyword) || a.Content.Contains(keyword));
            return PartialView("Widgets/FeaturedArticles", articles.ToList());
        }

        public ActionResult GetProductReviewsByKeyword(string keyword)
        {
            var productReviews = _db.Reviews.Where(a => a.Title.Contains(keyword) || a.ProductReview.Contains(keyword) || a.Description.Contains(keyword));
            return PartialView("Widgets/FeaturedProductReviews", productReviews.ToList());
        }

        public ActionResult GetProducts(string category)
        {
            var products = _db.Products.Where(p => p.PageID == _db.Pages.FirstOrDefault(pg => pg.PageCode.Equals(category)).ID);
            ViewBag.ProductList = products.ToList();
            return PartialView("_ProductList", products.ToList());
        }

        public ActionResult GetFeaturedProducts(string url)
        {
            Page page = _db.Pages.SingleOrDefault(p => p.PageCode.Equals(url));

            var products = (
                _db.Products.Where(product => (_db.Pages.Where(level2Pages => (_db.Pages.Where(
                level1Pages => level1Pages.ParentId ==
                               page.ID).Select(level1Pages => new
                               {
                                   level1Pages.ID
                               })).Contains(new { ID = (Int32)level2Pages.ParentId })).
                                                              Select(level2Pages => new { level2Pages.ID })).Contains(
                                                                  new { ID = product.PageID })).Select(
                                                                      product => product)
                           ).Union(_db.Products.Where(product => (_db.Pages.Where(
                               level1Pages => level1Pages.ParentId == page.ID).Select(level1Pages => new
                               {
                                   level1Pages.ID
                               })).Contains(new { ID = product.PageID })).Select(product => product)
                );
            if (products.Count() < 1)
            {
                products = _db.Products.Where(pd => pd.PageID == page.ID);
            }

            return PartialView("_ProductList", products.OfType<Product>());
        }


        public ActionResult CategoryDetails(string url)
        {
            Page requestedPage = _db.Pages.FirstOrDefault(p => p.Url == url);
            var productName = _db.Products.Where(p => p.Name == url && p.IsActive); //pavan
            if (requestedPage != null)
            {
                var fileAbsPath = string.Format("/Content/Uploads/Page/{0}.jpg", requestedPage.PageCode);
                var file = Server.MapPath(fileAbsPath);
                ViewBag.FilePath = System.IO.File.Exists(file) ? fileAbsPath : "/Content/Public/images/speaker.jpg";
                ViewBag.ParentId = requestedPage.ParentId;
            }

            else if (productName != null)
            {
                var products = _db.Products.Where(p => p.Name == url && p.IsActive); //pavan
                return View(products);
            }
            return RedirectToRoute("Pages", new { Url = "Page_Not_Found" });
        }

        public ActionResult FetchProducts(string url)
        {
            url = string.IsNullOrEmpty(url) ? null : url.TrimEnd();
            url = string.IsNullOrEmpty(url) ? null : url.TrimEnd('/');
            string[] hierarchy = (!string.IsNullOrEmpty(url)) ? url.Split('/') : null;
            if (hierarchy != null && hierarchy.Length > 0)
            {
                string tmp = hierarchy.Last();
                if (!string.IsNullOrEmpty(url) && _db.Products.Any(p => p.ProductFullUrl == url && p.IsActive == true))
                {
                    var prd = _db.Products.FirstOrDefault(p => p.ProductFullUrl.Equals(url) && p.IsActive); //pavan
                    ViewBag.Title = string.IsNullOrEmpty(prd.Title)?(prd.Name + " in Bangalore, india"):prd.Title;
                    ViewBag.Description = prd.Description;
                    ViewBag.MetaDescription = string.IsNullOrEmpty(prd.MetaDescription)?(prd.Description.Length > 160 ? prd.Description.Substring(0, 160) : prd.Description):prd.MetaDescription;
                    ViewBag.Keywords = string.IsNullOrEmpty(prd.MetaKeyword)?(prd.Name + ", " + prd.Name + " Dealers, " + prd.Name + " Dealers in Bengaluru, " + prd.Name + " Dealers in Bangalroe, " + prd.Name + " Dealers in India"):prd.MetaKeyword;
                    ViewBag.PgTitle = string.IsNullOrEmpty(prd.Heading) ? prd.Name : prd.Heading;
                    return PartialView("_product", prd);
                }
                else if (hierarchy.Length == 1 && _db.Brands.Any(b => b.BrandUrl == url && b.IsActive == true))
                {
                    var brnd = _db.Brands.Where(b => b.BrandUrl.Equals(url)).SingleOrDefault();
                    ViewBag.Title = string.IsNullOrEmpty(brnd.Title)?(brnd.BrandName + " in Bengaluru, India"):brnd.Title;
                    ViewBag.Description = brnd.Description;
                    ViewBag.MetaDescription = string.IsNullOrEmpty(brnd.MetaDescription) ? (brnd.Description.Length > 160 ? brnd.Description.Substring(0, 160) : brnd.Description) : brnd.MetaDescription;
                    ViewBag.Keywords = string.IsNullOrEmpty(brnd.MetaKeyword)?(brnd.BrandName + ", " + brnd.BrandName + " Dealers, " + brnd.BrandName + " Dealers in Bengaluru, " + brnd.BrandName + " Dealers in Bangalroe, " + brnd.BrandName + " Dealers in India"):brnd.MetaKeyword;
                    ViewBag.PgTitle = string.IsNullOrEmpty(brnd.Heading)?brnd.BrandName:brnd.Heading;
                    prdlist.AddRange(_db.Products.Where(p => p.BrandID == brnd.ID && p.IsActive == true).ToList());
                    return View("CategoryDetail", prdlist);
                }
                else if (_db.Categories.Any(c => c.FullPath == url && c.IsActive == true))
                {
                    var cat = _db.Categories.Where(c => c.FullPath.Equals(url)).SingleOrDefault();
                    ViewBag.Title = string.IsNullOrEmpty(cat.Title)?(cat.CategoryName + " in Bengaluru, India"):cat.Title;
                    ViewBag.Description = cat.Description;
                    ViewBag.MetaDescription = string.IsNullOrEmpty(cat.MetaDescription) ? (cat.Description.Length > 160 ? cat.Description.Substring(0, 160) : cat.Description) : cat.MetaDescription;
                    ViewBag.Keywords = string.IsNullOrEmpty(cat.MetaKeyword)?(cat.CategoryName + ", " + cat.CategoryName + " Dealers, " + cat.CategoryName + " Dealers in Bengaluru, " + cat.CategoryName + " Dealers in Bangalroe, " + cat.CategoryName + " Dealers in India"):cat.MetaKeyword;
                    ViewBag.PgTitle = string.IsNullOrEmpty(cat.Heading)?cat.CategoryName:cat.Heading;
                    prdlist.AddRange(_db.Products.Where(p => p.CategoryID == cat.ID && p.IsActive == true).ToList());
                    getCategoryProducts(cat.ID);
                    return View("CategoryDetail", prdlist);
                }
                else
                {
                    return RedirectToRoute("Pages", new { Url = "Page_Not_Found" });
                }
            }

            ViewBag.Title = "Products | Audio Systems in Bengaluru, India";
            //ViewBag.Description = "Our disparate assortment of products with boundless options will leave little to the imagination of even an ardent sound fanatic. Our products comprise a conglomeration of Amplifiers, Source, and Systems. &nbsp;Bryston, Cambridge Audio, Musical Fidelity, ATC are some of the time-tested products Planet Audio showcases. If you are looking for an 13.2 auro 3D system or Dolby atmos 5.2.2,7.2.4 or DTS X surround sound system in Bangalore, Audio Planet is the place to visit.  We offer a consortium of brands/products based on your budget without compromising on the sound quality.";
            ViewBag.Description = "Bryston, Cambridge Audio, ATC, Monitor Audio, MK Sound, Quadral, Marantz, Denon, Triangle, Q-Acoustics, Oppo, Panasonic, Epson, Sony, Optoma, Paradigm,Sunfire";
            ViewBag.MetaDescription = "Audio Planet is a one stop solution for some of the world’s best sound systems in Bengaluru, India";
            ViewBag.Keywords = "Audio Systems, Audio System Dealers, Audio Sytem Dealers in Bengaluru, Audio System Dealers in Bangalroe, Audio System  Dealers in India";
            ViewBag.PgTitle = "Products";
            var products = _db.Products.Where(p => p.IsActive == true ).Take(30);
            return View("CategoryDetail", products);
        }

        public void getCategoryProducts(int categoryId)
        {
            List<Category> childCategories = new List<Category>();
            childCategories = _db.Categories.Where(p => p.ParentCategoryId == categoryId).ToList();
            foreach (var childCat in childCategories)
            {
                prdlist.AddRange(_db.Products.Where(p => p.CategoryID == childCat.ID && p.IsActive).ToList()); //pavan
                getCategoryProducts(childCat.ID);
            }
        }

        public ActionResult CategoryAndBrandAndItem(string category, string brand, string name)
        {
            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(name))
            {
                int pid = 0;

                switch (name.Trim())
                {
                    case "B135":
                        pid = 28;
                        break;
                    case "Minx":
                        pid = 157;
                        break;
                    case "150 Series":
                        pid = 230;
                        break;
                    case "750 Series":
                        pid = 232;
                        break;
                    case "950 Series":
                        pid = 233;
                        break;
                    case "Magellan":
                        pid = 78;
                        break;
                    case "AURUM":
                        pid = 199;
                        break;
                    case "Gold GX":
                        pid = 84;
                        break;
                    case "Silver rx":
                        pid = 85;
                        break;
                    case "Platinum":
                        pid = 83;
                        break;
                    case "Signature":
                        pid = 191;
                        break;
                    case "Elara":
                        pid = 193;
                        break;
                    case "902":
                        pid = 82;
                        break;
                    default:
                        break;
                }

                if (pid > 0)
                {
                    //return RedirectPermanent(_db.Products.Where(p => p.ID.Equals(pid)).SingleOrDefault().ProductFullUrl);
                    return RedirectPermanent("/products/" + _db.Products.Where(p => p.ID.Equals(pid)).SingleOrDefault().ProductFullUrl);
                }
                else
                {
                    //return RedirectPermanent("/products/");
                    return RedirectPermanent("/products/");
                }
            }
            else if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(brand) && string.IsNullOrEmpty(name))
            {
                string initialUrl = category + "/" + brand;

                Hashtable ht = new Hashtable();
                ht.Add("Amplifiers/Bryston", "hi-fi/amplifiers/bryston");
                ht.Add("Amplifiers/Cambridge-Audio", "hi-fi/amplifiers/cambridge-audio");
                ht.Add("Amplifiers/Cary-Audio", "hi-fi/amplifiers/cary-audio");
                ht.Add("Amplifiers/Musical-Fidelity", "hi-fi/amplifiers/musical-fidelity");
                ht.Add("Source/Aurum", "hi-fi/source");
                ht.Add("Source/Bryston", "hi-fi/source");
                ht.Add("Source/Cambridge-Audio", "hi-fi/source");
                ht.Add("Source/Monitor-Audio", "hi-fi/source");
                ht.Add("Source/Musical-Fidelity", "hi-fi/source/cd-players/musical-fidelity");
                ht.Add("Source/Air-Stream/", "hi-fi/source");
                ht.Add("Speakers/ATC", "hi-fi/source");
                ht.Add("Speakers/Cambridge-Audio", "hi-fi/source");
                ht.Add("Speakers/MK-Sound-System", "home-cinema/speakers/mk-sound");
                ht.Add("Speakers/Monitor-Audio", "hi-fi/speakers/monitor-audio");
                ht.Add("Speakers/Mordaunt-Short", "hi-fi/speakers");
                ht.Add("Speakers/Quadral", "home-cinema/speakers/quadral");
                ht.Add("Speakers/Thiel", "hi-fi/speakers");
                ht.Add("Speakers/Triangle", "hi-fi/speakers/triangle");
                ht.Add("TurnTables/Pro-Ject", "hi-fi/source/turn-tables");

                if (ht.ContainsKey(initialUrl))
                {
                    string selectedItem = (String)ht[initialUrl];
                    if( _db.Categories.Any(c => c.FullPath == selectedItem && c.IsActive == true))
                    {
                        return RedirectPermanent("/products/" + selectedItem + "/");
                    }
                }
            }
            else if (!string.IsNullOrEmpty(category) && string.IsNullOrEmpty(brand) && string.IsNullOrEmpty(name))
            {
                string brandOrCatUrl = category.Trim();

                if (brandOrCatUrl == "ATC" || brandOrCatUrl == "Aurum" || brandOrCatUrl == "Bryston" || brandOrCatUrl == "Cambridge-Audio" || brandOrCatUrl == "Cary-Audio" ||
                    brandOrCatUrl == "MK-Sound-System" || brandOrCatUrl == "Monitor-Audio" || brandOrCatUrl == "Mordaunt-Short" || brandOrCatUrl == "Musical-Fidelity" ||
                    brandOrCatUrl == "Pro-Ject" || brandOrCatUrl == "Quadral" || brandOrCatUrl == "Thiel" || brandOrCatUrl == "Triangle" ||
                    brandOrCatUrl == "Aktimate-Micro" || brandOrCatUrl == "Amplifiers" || brandOrCatUrl == "Source" || brandOrCatUrl == "Speakers" || brandOrCatUrl == "TurnTables")
                {
                    string bcurl = null;

                    switch (brandOrCatUrl)
                    {
                        case "MK-Sound-System":
                            bcurl = "mk-sound";
                            break;
                        case "Amplifiers":
                            bcurl = _db.Categories.Any(c => c.FullPath == "hi-fi/amplifiers" && c.IsActive == true) ? "hi-fi/amplifiers" : null;
                            break;
                        case "Source":
                            bcurl = _db.Categories.Any(c => c.FullPath == "hi-fi/source" && c.IsActive == true) ? "hi-fi/source" : null;
                            break;
                        case "Speakers":
                            bcurl = _db.Categories.Any(c => c.FullPath == "hi-fi/speakers" && c.IsActive == true) ? "hi-fi/speakers" : null;
                            break;
                        case "Pro-Ject":
                        case "TurnTables":
                            bcurl = _db.Categories.Any(c => c.FullPath == "hi-fi/source/turn-tables" && c.IsActive == true) ? "hi-fi/source/turn-tables" : null;
                            break;
                        case "Bryston":
                        case "Cambridge-Audio":
                        case "Cary-Audio":
                        case "Monitor-Audio":
                        case "Musical-Fidelity":
                        case "Quadral":
                        case "Triangle":
                            bcurl = _db.Brands.Any(b => b.BrandUrl == brandOrCatUrl.ToLower() && b.IsActive == true) ? brandOrCatUrl.ToLower() : null;
                            break;
                        case "ATC":
                        case "Aurum":
                        case "Thiel":
                        case "Mordaunt-Short":
                        case "Aktimate-Micro":
                            bcurl = null;
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrEmpty(bcurl))
                    {
                        return RedirectPermanent("/products/" + bcurl + "/");
                    }
                    else
                    {
                        return RedirectPermanent("/products/");
                    }
                }
                else
                {
                    //return RedirectPermanent("/products/");
                    return RedirectPermanent("/products/");
                }
            }
            else
            {
                return RedirectPermanent("/products/hi-fi/");
            }

            return RedirectPermanent("/products/");
        }
    }
}