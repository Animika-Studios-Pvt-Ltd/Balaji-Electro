using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AudioPlanet.Models
{
    public class AdminUser
    {
        private bool _gender = true;
        private bool _isActive = true;
        private DateTime _lastSeen = DateTime.Now;

        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required (we promise not to spam you)")]
        [StringLength(200, MinimumLength = 3)]
        [Remote("CheckDuplicate", "AdminUser", ErrorMessage = "Email already taken", AdditionalFields = "InitialTitle")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public DateTime LastSeen
        {
            get { return _lastSeen; }
            set { _lastSeen = value; }
        }

        public bool Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
    }
}