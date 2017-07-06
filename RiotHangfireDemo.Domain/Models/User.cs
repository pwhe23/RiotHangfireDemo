using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiotHangfireDemo.Domain
{
    [Table(nameof(User))]
    internal class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 5), Required, EmailAddress]
        public string Email { get; set; }

        [StringLength(200, MinimumLength = 8), Required]
        public string Password { get; set; }

        [StringLength(25)]
        public string Role { get; set; }

        public const string ADMIN = "Admin";
        public const string USER = "User";
    };
}
