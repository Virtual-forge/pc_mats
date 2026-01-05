namespace pc_mats.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; } // Using 'int' to match your Product Model Price

        // Calculated property for the subtotal of this item
        public int TotalPrice => Price * Quantity;
        public string Category { get; set; }
    }
}