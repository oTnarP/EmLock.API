using System;

namespace EmLock.API.Models
{
    public class WalletTransaction
    {
        public int Id { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public decimal Amount { get; set; }
        public string Type { get; set; } // "Credit" or "Debit"
        public string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
