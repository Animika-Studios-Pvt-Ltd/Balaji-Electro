using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AudioPlanet.Models
{
    public class Item
    {
        private bool _isActive = true;

        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
    }
}