namespace pc_mats.Models
{
    public enum Category
    {
        CPU, GPU, PSU, RAM, SDD, Accesoir, Clavier, Souris, Routeur
    }

    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } // Standard 'int' is fine for Int32
        public int Quantity { get; set; }
        public Category Category { get; set; }

        public string? ImageUrl { get; set; }

        public Product() { }
    }
}