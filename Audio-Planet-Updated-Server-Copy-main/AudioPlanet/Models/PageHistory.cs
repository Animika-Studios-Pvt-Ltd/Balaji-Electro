using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Models
{
    public class PageHistory
    {
        public PageHistory()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
            //PageGroup = "Page";
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

        public int? ParentId { get; set; }
    }
}