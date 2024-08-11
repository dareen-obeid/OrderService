using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class OrderStatus
	{
        [Key]
        public int OrderStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatusName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

