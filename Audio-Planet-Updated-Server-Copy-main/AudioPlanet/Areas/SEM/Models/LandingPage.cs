using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Areas.SEM.Models
{
    public class LandingPage
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Keywords { get; set; }

        [Required]
        public string Url { get; set; }
    }
}