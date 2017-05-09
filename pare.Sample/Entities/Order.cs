using pare.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace pare.Sample.Entities
{
    [Table(nameof(Order))]
    public class Order : BaseDomainObject<int>
    {
        [MaxLength(8)]
        public string Number { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
