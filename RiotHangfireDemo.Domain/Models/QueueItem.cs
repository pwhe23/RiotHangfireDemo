using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiotHangfireDemo.Domain
{
    [Table(nameof(QueueItem))]
    internal class QueueItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100), Required]
        public string Name { get; set; }

        [StringLength(25), Required]
        public string Status { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Completed { get; set; }

        [StringLength(100), Required]
        public string Type { get; set; }

        [MaxLength]
        public string Data { get; set; }

        [MaxLength]
        public string Log { get; set; }

        public const string QUEUED = "Queued";
        public const string RUNNING = "Running";
        public const string ERROR = "Error";
        public const string COMPLETED = "Completed";
    };
}
