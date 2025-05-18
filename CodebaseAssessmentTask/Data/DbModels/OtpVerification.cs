namespace CodebaseAssessmentTask.Data.DbModels
{
    public class OtpVerification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? OtpCode { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
