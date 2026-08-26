using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Models
{
    public class Testimonial
    {
        private bool _isActive = true, _isArchived = true;

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Designation { get; set; }

        [StringLength(200)]
        public string Organization { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        [Required]
        [DisplayName("Is Archived")]
        public bool IsArchived
        {
            get { return _isArchived; }
            set { _isArchived = value; }
        }
    }
}