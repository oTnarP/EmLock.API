namespace EmLock.API.Models
{
    public class User
    {
        public int Id { get; set; }  // Primary Key
        public string FullName { get; set; }
        public string Role { get; set; } // Admin / Dealer / Shopkeeper / Customer
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // 🔹 Relationship with Devices (already there)
        public List<Device> Devices { get; set; }

        // 🔹 New: Optional relationship to a Dealer (only for Shopkeepers)
        public int? DealerId { get; set; }
        public Dealer Dealer { get; set; }

        // 🔹 New: Wallet tracking for profit-sharing
        public decimal WalletBalance { get; set; } = 0;
        public Wallet Wallet { get; set; }  // One-to-one relation

    }
}
