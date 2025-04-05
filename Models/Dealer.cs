namespace EmLock.API.Models
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal SharePercentage { get; set; } // e.g., 30.00 for 30%
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public List<User> Shopkeepers { get; set; }
    }
}
