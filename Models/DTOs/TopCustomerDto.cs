namespace EmLock.API.Models.DTOs
{
    public class TopCustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal TotalPaid { get; set; }
    }
}
