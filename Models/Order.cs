namespace pc_mats.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Matches your Session UserId
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public int TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Navigation property
        public List<OrderItem> OrderItems { get; set; }
    }
}
