using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AudioPlanet.Models
{
    public class Category
    {
        private bool _isActive = true;

        //[Required]
        public int ID { get; set; }

        [DisplayName("Category Name")]
        [Required]
        public string CategoryName { get; set; }

        [DisplayName("Category URL")]
        [Required]
        public string CategoryUrl { get; set; }

        [DisplayName("Parent Category")]
        public int ParentCategoryId { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        [DisplayName("Is Brand")]
        public bool IsBrand { get; set; }

        public int Depth { get; set; }

        [DisplayName("Full Path")]
        public string FullPath { get; set; }

        public string Title { get; set; }

        public string Heading { get; set; }

        public string Description { get; set; }

        [DisplayName("Meta Keyword")]
        public string MetaKeyword { get; set; }

        [DisplayName("Meta Description")]
        public string MetaDescription { get; set; }
        //public bool Checked { get; set; }
    }
}