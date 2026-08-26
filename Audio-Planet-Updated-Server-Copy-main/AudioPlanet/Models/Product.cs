using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Hosting;

namespace AudioPlanet.Models
{
    public class Product
    {
        private bool _isActive = true;
        private DateTime _createdAt = DateTime.Now;

        [Key]
        public int ID { get; set; }

        public int PageID { get; set; }

        [DisplayName("Category")]
        public virtual Page Page { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("PDF")]
        [StringLength(200, MinimumLength = 3)]
        public string PdfLink { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        [DisplayName("Is Featured")]
        public bool IsFeatured { get; set; }

        [DisplayName("Created At")]
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        [Display(Name = "Category")]
        public int? CategoryID { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public int BrandID { get; set; }

        public virtual Brand Brand { get; set; }

        
        public string ProductUrl { get; set; }

        public string ProductFullUrl { get; set; }

        public string Title { get; set; }

        public string Heading { get; set; }

        [DisplayName("Meta Keyword")]
        public string MetaKeyword { get; set; }

        [DisplayName("Meta Description")]
        public string MetaDescription { get; set; }

        public static string GetProductFiles(int id, string type)
        {
            var dir = new DirectoryInfo(HostingEnvironment.MapPath(string.Format("~/Content/Uploads/Product/{0}/{1}/", id, type)));
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo fileInfo in files)
                {
                    if(fileInfo.Name.Contains("Default"))
                    {
                        return string.Format("/Content/Uploads/Product/{0}/{1}/{2}", id, type, fileInfo.Name);
                    }
                }
            }
            switch (type)
            {
                case "Image":
                    return "/Content/Uploads/NoImage.jpg";
                case "Document":
                    return string.Empty;
                case "Video":
                    break;
            }
            return string.Empty;
        }
    }
}