using System;
using System.ComponentModel.DataAnnotations;

namespace CodebaseAssessmentTask.Models
{
    public class RegistrationStartViewModel
    {
        public string WelcomeMessage { get; set; }
    }

    public class RegistrationVerifyOtpViewModel
    {
        [Required]
        [Display(Name = "OTP")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "OTP must be 4-6 digits.")]
        public string Otp { get; set; }

        public string PhoneNumber { get; set; }

        public string ICNumber { get; set; }
        public string Email { get; set; }
        public bool IsEmailOtp { get; set; }
    }

    public class RegistrationPrivacyPolicyViewModel
    {
        [Required(ErrorMessage = "You must accept the privacy policy.")]
        [MustBeTrue(ErrorMessage = "You must accept the privacy policy.")]      
        public bool Accepted { get; set; }
    }

    public class RegistrationSetPinViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "PIN must be 6 digits.")]
        public string Pin { get; set; }

        [DataType(DataType.Password)]
        [Compare("Pin", ErrorMessage = "PINs do not match.")]
        public string ConfirmPin { get; set; }
    }

    public class RegistrationCreatePinViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "PIN must be 6 digits.")]
        public string Pin { get; set; }
    }

    public class RegistrationConfirmPinViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "PIN must be 6 digits.")]
        public string ConfirmPin { get; set; }
    }

    public class RegistrationCreateAccountViewModel
    {
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "IC Number")]
        public string ICNumber { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }
} 