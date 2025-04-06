namespace EmLock.API.Models.DTOs
{
    public class UserRegisterDto
    {
        public string FullName { get; set; }
        public string Role { get; set; } // Admin / Shopkeeper / Customer
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? DealerId { get; set; }

        // Only relevant for Shopkeepers
        public DateTime? LicenseStartDate { get; set; }
        public DateTime? LicenseEndDate { get; set; }
    }
}
