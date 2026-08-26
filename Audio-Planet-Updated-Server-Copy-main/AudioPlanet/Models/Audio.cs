using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AudioPlanet.Areas.Enquiry.Models;
using AudioPlanet.Areas.SEM.Models;

namespace AudioPlanet.Models
{
    public class Audio : DbContext
    {
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageHistory> PagesHistory { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Enquiry> Enquirys { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Review> Reviews { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            dbModelBuilder.Entity<Page>().HasOptional(parent => parent.ParentPage)
                .WithMany(child => child.ChildPages)
                .HasForeignKey(pk => pk.ParentId);
            //dbModelBuilder.Entity<Enquiry>().HasOptional<Visitor>((Enquiry v) => v.Visitor);
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<LandingPage> LandingPages { get; set; }
    }
}