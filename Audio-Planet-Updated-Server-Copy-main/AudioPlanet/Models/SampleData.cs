using System;
using System.Collections.Generic;
using System.Data.Entity;
using AudioPlanet.Areas.Enquiry.Models;

namespace AudioPlanet.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<Audio>
        //public class SampleData : DropCreateDatabaseAlways<Audio>
    {
        protected override void Seed(Audio context)
        {
            var pages = new List<Page>
                {
                    new Page
                        {
                            PageCode = "Home",
                            Title = "Home",
                            Name = "Home",
                            Description = "Home",
                            Content = "<p>Home</p>",
                            Keyword = "Home",
                            Url = "Home",
                            Order = 1,
                            IsCmsPage = false,
                            IsParent = true,
                            IsActive = true,
                            IsItShowInMenu = true,
                            CreatedAt = DateTime.Now
                        },
                    new Page
                        {
                            PageCode = "Sitemap",
                            Title = "Sitemap",
                            Name = "Sitemap",
                            Description = "Sitemap",
                            Content =
                                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p>",
                            Keyword = "Sitemap",
                            Url = "Sitemap",
                            Order = 1,
                            IsCmsPage = false,
                            IsParent = false,
                            IsActive = true,
                            IsItShowInMenu = false,
                            CreatedAt = DateTime.Now
                        },
                    new Page
                        {
                            PageCode = "AudioPlanet",
                            Title = "Audio Planet",
                            Name = "Audio Planet",
                            Description = "Audio Planet",
                            Content =
                                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p>",
                            Keyword = "Audio Planet",
                            Url = "Audio Planet",
                            Order = 2,
                            IsCmsPage = false,
                            IsParent = false,
                            IsActive = true,
                            IsItShowInMenu = true,
                            CreatedAt = DateTime.Now
                        },
                    new Page
                        {
                            PageCode = "AudioConsulting",
                            Title = "Audio Consulting",
                            Name = "Audio Consulting",
                            Description = "Audio Consulting",
                            Content =
                                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p>",
                            Keyword = "About Us",
                            Url = "About Us",
                            Order = 3,
                            IsCmsPage = false,
                            IsParent = false,
                            IsActive = true,
                            IsItShowInMenu = true,
                            CreatedAt = DateTime.Now
                        },
                    new Page
                        {
                            PageCode = "ProductsAndBrands",
                            Title = "Products & Brands",
                            Name = "Products & Brands",
                            Description = "Products & Brands",
                            Content =
                                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p>",
                            Keyword = "Products and Brands",
                            Url = "Products and Brands",
                            Order = 4,
                            IsCmsPage = false,
                            IsParent = false,
                            IsActive = true,
                            IsItShowInMenu = true,
                            CreatedAt = DateTime.Now
                        },
                    new Page
                        {
                            PageCode = "Services",
                            Title = "Services",
                            Name = "Services",
                            Description = "Services",
                            Content =
                                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p>",
                            Keyword = "Services",
                            Url = "Services",
                            Order = 5,
                            IsCmsPage = false,
                            IsParent = false,
                            IsActive = true,
                            IsItShowInMenu = true,
                            CreatedAt = DateTime.Now
                        },
                    new Page
                        {
                            PageCode = "Clients",
                            Title = "Clients",
                            Name = "Clients",
                            Description = "Clients",
                            Content =
                                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p>",
                            Keyword = "Clients",
                            Url = "Clients",
                            Order = 6,
                            IsCmsPage = false,
                            IsParent = false,
                            IsActive = true,
                            IsItShowInMenu = true,
                            CreatedAt = DateTime.Now
                        },
                    new Page
                        {
                            PageCode = "ExpertSpeaks",
                            Title = "Expert Speaks",
                            Name = "Expert Speaks",
                            Description = "Expert Speaks",
                            Content =
                                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum</p>",
                            Keyword = "Expert Speaks",
                            Url = "Expert Speaks",
                            Order = 7,
                            IsCmsPage = false,
                            IsParent = false,
                            IsActive = true,
                            IsItShowInMenu = true,
                            CreatedAt = DateTime.Now
                        }
                };
            pages.ForEach(p => context.Pages.Add(p));
            context.SaveChanges();

            for (int i = 1; i <= 6; i++)
            {
                pages[i].ParentPage = pages[0];
            }
            context.SaveChanges();

            var sections = new List<Section>
                {
                    new Section {Name = "CMS", Description = "Content Management System"},
                    new Section
                        {Name = "Features", Description = "Features in Content Management System"},
                    new Section
                        {Name = "Utilities", Description = "Utilities in Content Management System"},
                };
            sections.ForEach(s => context.Sections.Add(s));
            context.SaveChanges();

            var items = new List<Item>
                {
                    new Item
                        {
                            Name = "Page Management",
                            Description = "Page Management, Create, Update, Delete CMS pages",
                            Section = sections[0]
                        },
                    new Item
                        {
                            Name = "CSS Switcher",
                            Description = "Change CSS of the Website on Fly",
                            Section = sections[0]
                        },
                    new Item
                        {
                            Name = "Error Log",
                            Description = "See what's going on Application!",
                            Section = sections[2]
                        },
                };
            items.ForEach(s => context.Items.Add(s));
            context.SaveChanges();
        }
    }
}