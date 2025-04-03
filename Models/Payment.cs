namespace EmLock.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public int EMIPlanId { get; set; }
        public EMIPlan EMIPlan { get; set; }
    }
}
