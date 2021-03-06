using System.ComponentModel.DataAnnotations;

namespace HGT6.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarImage { get; set; }
        public int CapthaId { get; set; }
        [Required]
        public string Captha { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string Town { get; set; }

        public int Age { get; set; }
    }
}