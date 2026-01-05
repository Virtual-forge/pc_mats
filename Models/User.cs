

using System.ComponentModel.DataAnnotations;

namespace pc_mats.Models
{
    public enum UserType
    {
        Client,
        Admin
    }
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public UserType UserType { get; set; }

        public User() { }
            

    }
}
