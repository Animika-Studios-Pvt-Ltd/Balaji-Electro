using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AudioPlanet.Models
{
    public class Review
    {
        public Review()
        {
            IsActive = true;
            IsFeatured = false;
            PostedTime = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        public int ProductID { get; set; }

        [DataType(DataType.Text)]
        public string ProductName { get; set; }

        public int? CategoryID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Html)]
        public string ProductReview { get; set; }

        public DateTime PostedTime { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }

        public virtual Category categories { get; set; }

        public virtual Product products { get; set; }
    }
}