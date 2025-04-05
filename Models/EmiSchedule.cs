using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmLock.API.Models
{
    public class EmiSchedule
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        [JsonIgnore]
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime? PaymentDate { get; set; }
    }
}
