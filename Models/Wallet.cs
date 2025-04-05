namespace EmLock.API.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int? DealerId { get; set; }   // Nullable for Admin Wallet
        public decimal Balance { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public User Dealer { get; set; }
    }
}
