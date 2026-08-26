using System;
using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Areas.Enquiry.Models
{
    public class Enquiry
    {
        public Enquiry()
        {
            Date = DateTime.Now;
            Status = "New";
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "*")]
        public string Phone { get; set; }

        public string Company { get; set; }

        public string PostalAddress { get; set; }

        public int? ProductID { get; set; }

        [Required]
        public string Query { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public string TrafficSource { get; set; }

        public string Channel { get; set; }

        public string Keywords { get; set; }

        public string CampaignName { get; set; }

        public string TrafficType { get; set; }

        public string ReferralUrl { get; set; }

        public string LandingUrl { get; set; }

        public string IpAddress { get; set; }

        [Required]
        public string Captcha { get; set; }
    }
}