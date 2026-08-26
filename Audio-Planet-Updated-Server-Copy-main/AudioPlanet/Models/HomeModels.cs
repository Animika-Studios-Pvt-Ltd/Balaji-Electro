using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(12, MinimumLength = 10,ErrorMessage = "*")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Query")]
        public string Query { get; set; }
    }
}