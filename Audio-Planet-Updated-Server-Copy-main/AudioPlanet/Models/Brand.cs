using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AudioPlanet.Models
{
    public class Brand
    {
        private bool _isActive = true;
        //[Required]
        public int ID { get; set; }

        [DisplayName("Brand Name")]
        [Required]
        public string BrandName { get; set; }

        [DisplayName("Brand URL")]
        [Required]
        public string BrandUrl { get; set; }

        public string Title { get; set; }

        public string Heading { get; set; }

        public string Description { get; set; }

        [DisplayName("Meta Keyword")]
        public string MetaKeyword { get; set; }

        [DisplayName("Meta Description")]
        public string MetaDescription { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
    }
}