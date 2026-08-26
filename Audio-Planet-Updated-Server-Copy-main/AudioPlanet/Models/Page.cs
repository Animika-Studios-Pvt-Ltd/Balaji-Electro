using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AudioPlanet.Models
{
    public class Page : IDisposable
    {
        public Page()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
            PageGroup = "Page";
            IsCmsPage = true;
            IsItShowInMenu = false;
            IsPublished = false;
            IsActive = true;
            IsParent = false;
        }

        public int ID { get; set; }

        public string PageCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string SubTitle { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        public string Keyword { get; set; }

        [Required]
        [MaxLength(255)]
        [Remote("CheckDuplicate", "Page", ErrorMessage = "Url already taken", AdditionalFields = "initialUrl")]
        public string Url { get; set; }

        [Required]
        public int Order { get; set; }

        [DisplayName("Is CMS Pgae")]
        public bool IsCmsPage { get; set; }

        [DisplayName("Is Parent Page")]
        public bool IsParent { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Show on Main Menu")]
        public bool IsItShowInMenu { get; set; }

        [DisplayName("Is Published")]
        public bool IsPublished { get; set; }

        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Page Group")]
        public string PageGroup { get; set; }

        public Page ParentPage { get; set; }

        public int? ParentId { get; set; }

        public virtual ICollection<Page> ChildPages { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    public enum PageGroup
    {
        Header,
        Footer,
        Page,
        Microsite,
        Tips,
        News,
        Article,
        Product
    }
}