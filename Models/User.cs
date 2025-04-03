namespace EmLock.API.Models
{
    public class User
    {
        public int Id { get; set; }  // Primary Key
        public string FullName { get; set; }
        public string Role { get; set; } // Admin / Shopkeeper / Customer
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Relationships (Navigation Properties)
        public List<Device> Devices { get; set; }
    }
}
