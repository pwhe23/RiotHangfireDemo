using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiotHangfireDemo
{
    [Table(nameof(Task))]
    public class Task
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100), Required]
        public string Name { get; set; }

        public bool Completed { get; set; }
    };
}
