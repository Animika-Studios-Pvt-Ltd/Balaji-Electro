using System;
using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Models
{
    public class Article
    {
        public Article()
        {
            IsActive = true;
            IsFeatured = false;
            TotalViews = 0;
            PostedTime = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        public DateTime PostedTime { get; set; }

        public int TotalViews { get; set; }

        public int? CategoryID { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }
    }
}