using Microsoft.EntityFrameworkCore;
using pc_mats.Data;
using pc_mats.Models;

namespace pc_mats.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any products already in the database
                if (context.Products.Any())
                {
                    return;   // DB has been seeded
                }

                context.Products.AddRange(
    // CPUs
    new Product { ProductName = "Intel Core i9-14900K", Description = "24-Core 6.0 GHz Unlocked Desktop Processor", Price = 7200, Quantity = 8, Category = Category.CPU, ImageUrl = "cpu_placeholder.jpg" },
    new Product { ProductName = "AMD Ryzen 7 7800X3D", Description = "8-Core 16-Thread with 3D V-Cache Technology", Price = 4900, Quantity = 12, Category = Category.CPU, ImageUrl = "cpu_placeholder.jpg" },
    new Product { ProductName = "Intel Core i5-13600K", Description = "14-Core High Performance Mid-Range CPU", Price = 3800, Quantity = 15, Category = Category.CPU, ImageUrl = "cpu_placeholder.jpg" },

    // GPUs
    new Product { ProductName = "NVIDIA RTX 4090", Description = "24GB GDDR6X - The Ultimate Gaming GPU", Price = 24000, Quantity = 3, Category = Category.GPU, ImageUrl = "gpu_placeholder.jpg" },
    new Product { ProductName = "NVIDIA RTX 4070 Super", Description = "12GB GDDR6X - Perfect for 1440p Gaming", Price = 8500, Quantity = 10, Category = Category.GPU, ImageUrl = "gpu_placeholder.jpg" },
    new Product { ProductName = "AMD Radeon RX 7800 XT", Description = "16GB GDDR6 - High performance AMD Graphics", Price = 6900, Quantity = 7, Category = Category.GPU, ImageUrl = "gpu_placeholder.jpg" },

    // RAM & SSD
    new Product { ProductName = "Corsair Vengeance 32GB DDR5", Description = "6000MHz C36 RGB Memory Kit", Price = 1850, Quantity = 25, Category = Category.RAM, ImageUrl = "ram_placeholder.jpg" },
    new Product { ProductName = "Kingston FURY Beast 16GB", Description = "DDR4 3200MHz Desktop RAM", Price = 550, Quantity = 40, Category = Category.RAM, ImageUrl = "ram_placeholder.jpg" },
    new Product { ProductName = "Samsung 990 Pro 2TB", Description = "NVMe M.2 Gen5 Ready SSD", Price = 2400, Quantity = 20, Category = Category.SDD, ImageUrl = "ssd_placeholder.jpg" },
    new Product { ProductName = "Crucial P3 1TB", Description = "NVMe PCIe 3.0 M.2 SSD", Price = 850, Quantity = 30, Category = Category.SDD, ImageUrl = "ssd_placeholder.jpg" },

    // PSU & Peripherals
    new Product { ProductName = "ASUS ROG Thor 1000W", Description = "80+ Platinum Fully Modular PSU", Price = 3200, Quantity = 5, Category = Category.PSU, ImageUrl = "psu_placeholder.jpg" },
    new Product { ProductName = "Corsair RM750e", Description = "750W 80+ Gold Quiet Power Supply", Price = 1300, Quantity = 14, Category = Category.PSU, ImageUrl = "psu_placeholder.jpg" },
    new Product { ProductName = "Logitech G502 X Plus", Description = "Wireless RGB Gaming Mouse", Price = 1500, Quantity = 20, Category = Category.Souris, ImageUrl = "mouse_placeholder.jpg" },
    new Product { ProductName = "SteelSeries Apex Pro", Description = "Mechanical Keyboard with OmniPoint Switches", Price = 2100, Quantity = 10, Category = Category.Clavier, ImageUrl = "kb_placeholder.jpg" },

    // Networking & Accessories
    new Product { ProductName = "ASUS RT-AX88U Pro", Description = "Dual Band WiFi 6 Gaming Router", Price = 3500, Quantity = 6, Category = Category.Routeur, ImageUrl = "router_placeholder.jpg" },
    new Product { ProductName = "DeepCool LT720", Description = "360mm High-Performance Liquid CPU Cooler", Price = 1750, Quantity = 12, Category = Category.Accesoir, ImageUrl = "cooler_placeholder.jpg" }
);

                context.SaveChanges();
            }
        }
        public static void SeedOrders(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Only run if the Orders table is empty
                if (context.Orders.Any()) return;

                var productList = context.Products.ToList();

                // Helper to find the ID by name keywords (e.g., "4090" or "i9")
                int GetId(string keyword) =>
                    productList.FirstOrDefault(p => p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase))?.Id ?? 0;

                var seedOrders = new List<Order>
        {
            // Dec 28: A high-end gamer purchase
            new Order {
                UserId = "SeedUser",
                FullName = "Ahmed Alami", Address = "Casablanca", PaymentMethod = "Card",
                OrderDate = new DateTime(2025, 12, 28), TotalAmount = 31200,
                OrderItems = new List<OrderItem> {
                    new OrderItem { ProductId = GetId("14900K"), ProductName = "Intel Core i9-14900K", Category = "CPU", Quantity = 1, Price = 7200 },
                    new OrderItem { ProductId = GetId("4090"), ProductName = "NVIDIA RTX 4090", Category = "GPU", Quantity = 1, Price = 24000 }
                }
            },
            // Dec 30: A mid-range setup
            new Order {
                UserId = "SeedUser",
                FullName = "Sami Tazi", Address = "Rabat", PaymentMethod = "COD",
                OrderDate = new DateTime(2025, 12, 30), TotalAmount = 14150,
                OrderItems = new List<OrderItem> {
                    new OrderItem { ProductId = GetId("13600K"), ProductName = "Intel Core i5-13600K", Category = "CPU", Quantity = 1, Price = 3800 },
                    new OrderItem { ProductId = GetId("4070"), ProductName = "NVIDIA RTX 4070 Super", Category = "GPU", Quantity = 1, Price = 8500 },
                    new OrderItem { ProductId = GetId("P3 1TB"), ProductName = "Crucial P3 1TB", Category = "SDD", Quantity = 2, Price = 850 }
                }
            },
            // Jan 1: New Year Peripherals
            new Order {UserId = "SeedUser",
                FullName = "Yassine Benani", Address = "Marrakech", PaymentMethod = "Card",
                OrderDate = new DateTime(2026, 1, 1), TotalAmount = 3600,
                OrderItems = new List<OrderItem> {
                    new OrderItem { ProductId = GetId("G502"), ProductName = "Logitech G502 X Plus", Category = "Souris", Quantity = 1, Price = 1500 },
                    new OrderItem { ProductId = GetId("BlackWidow"), ProductName = "Razer BlackWidow V4", Category = "Clavier", Quantity = 1, Price = 2100 }
                }
            },
            // Jan 2: Bulk RAM Upgrade for a studio
            new Order {UserId = "SeedUser",
                FullName = "Creative Studio", Address = "Tanger", PaymentMethod = "Card",
                OrderDate = new DateTime(2026, 1, 2), TotalAmount = 7400,
                OrderItems = new List<OrderItem> {
                    new OrderItem { ProductId = GetId("Vengeance"), ProductName = "Corsair Vengeance 32GB DDR5", Category = "RAM", Quantity = 4, Price = 1850 }
                }
            },
            new Order {
                UserId = "SeedUser",
        FullName = "Omar Mansouri", Address = "Agdal, Rabat", PaymentMethod = "Card",
        OrderDate = new DateTime(2026, 1, 2), TotalAmount = 8500,
        OrderItems = new List<OrderItem> {
            new OrderItem { ProductId = GetId("4070"), ProductName = "NVIDIA RTX 4070 Super", Category = "GPU", Quantity = 1, Price = 8500 }
        }
    },

    // Jan 3: Networking Upgrade Day (Spike for the Router line)
    new Order {UserId = "SeedUser",
        FullName = "Internet Cafe Plus", Address = "Fez Medina", PaymentMethod = "COD",
        OrderDate = new DateTime(2026, 1, 3), TotalAmount = 10500,
        OrderItems = new List<OrderItem> {
            new OrderItem { ProductId = GetId("RT-AX88U"), ProductName = "ASUS RT-AX88U Pro", Category = "Routeur", Quantity = 3, Price = 3500 }
        }
    },

    // Jan 3: The "Full Setup" Order
    new Order {UserId = "SeedUser",
        FullName = "Laila Haddad", Address = "Hay Riad, Rabat", PaymentMethod = "Card",
        OrderDate = new DateTime(2026, 1, 3), TotalAmount = 14450,
        OrderItems = new List<OrderItem> {
            new OrderItem { ProductId = GetId("7800X3D"), ProductName = "AMD Ryzen 7 7800X3D", Category = "CPU", Quantity = 1, Price = 4900 },
            new OrderItem { ProductId = GetId("7800 XT"), ProductName = "AMD Radeon RX 7800 XT", Category = "GPU", Quantity = 1, Price = 6900 },
            new OrderItem { ProductId = GetId("LT720"), ProductName = "DeepCool LT720", Category = "Accesoir", Quantity = 1, Price = 1750 },
            new OrderItem { ProductId = GetId("990 Pro"), ProductName = "Samsung 990 Pro 2TB", Category = "SDD", Quantity = 1, Price = 2400 }
        }
    },

    // Jan 3: Last minute peripherals
    new Order {UserId = "SeedUser",
        FullName = "Karim Bennani", Address = "Oujda", PaymentMethod = "COD",
        OrderDate = new DateTime(2026, 1, 3), TotalAmount = 1950,
        OrderItems = new List<OrderItem> {
            new OrderItem { ProductId = GetId("RM750e"), ProductName = "Corsair RM750e", Category = "PSU", Quantity = 1, Price = 1300 },
            new OrderItem { ProductId = GetId("FURY"), ProductName = "Kingston FURY Beast 16GB", Category = "RAM", Quantity = 1, Price = 650 }
        } }

        };

                context.Orders.AddRange(seedOrders);
                context.SaveChanges();
            }
        }
    }
}