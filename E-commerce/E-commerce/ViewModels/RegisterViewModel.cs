using System.ComponentModel.DataAnnotations;

namespace E_commerce.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(250)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // Populated after successful temp image upload
        [Required(ErrorMessage = "Please upload an image.")]
        public Guid UploadToken { get; set; }

        // Used only during registration UI
        public IFormFile? Image { get; set; }
    }
}
