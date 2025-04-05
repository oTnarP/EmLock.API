using System;

namespace EmLock.API.Models
{
    public class WithdrawalRequest
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public User Dealer { get; set; }
    }
}
