using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CodebaseAssessmentTask.Data.DbModels
{


    [Index(nameof(ICNumber), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "IC Number")]
        public string ICNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        public string PasswordHash { get; set; }
        public bool IsMigrated { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
