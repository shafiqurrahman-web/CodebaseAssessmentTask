using System.ComponentModel.DataAnnotations;

namespace CodebaseAssessmentTask.Models
{
    public class MigrationStartViewModel
    {
        public string WelcomeMessage { get; set; }
    }

    public class MigrationVerifyOtpViewModel
    {
        [Required]
        [Display(Name = "OTP")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "OTP must be 4-6 digits.")]
        public string Otp { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }

    public class MigrationPrivacyPolicyViewModel
    {
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the privacy policy.")]
        public bool Accepted { get; set; }
    }

    public class MigrationSetPinViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "PIN must be 4-6 digits.")]
        public string Pin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Pin", ErrorMessage = "PINs do not match.")]
        public string ConfirmPin { get; set; }
    }
} 