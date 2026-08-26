using System;
using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Areas.Enquiry.Models
{

    public class Visitor
    {
        private static readonly TimeZoneInfo IndianZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public Visitor()
        {
            Host = string.Empty;
            OperatingSystem = string.Empty;
            Browser = string.Empty;
            SourceType = "Direct";
            Source = "Direct";
            Keywords = "Direct";
            CampaignName = "N/A";
            ReferralUrl = "N/A";
            VisitedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianZone);
        }

        [Key]
        public int ID { get; set; }

        public string Host { get; set; }

        public string OperatingSystem { get; set; }

        public string Browser { get; set; }

        public string IpAddress { get; set; }

        public string SourceType { get; set; }

        public string Source { get; set; }

        public string Keywords { get; set; }

        public string CampaignName { get; set; }

        [MaxLength]
        public string ReferralUrl { get; set; }

        [MaxLength]
        public string LandingUrl { get; set; }

        [MaxLength]
        public string QueryString { get; set; }

        public DateTime VisitedDate { get; set; }
    }
}
