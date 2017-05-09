using pare.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pare.Sample.Entities
{
    [Table(nameof(Product))]
    public class Product : BaseDomainObject<int>
    {
        [MaxLength(100)]
        public string Description { get; set; }
        [Range(1, 9999999)]
        public decimal Cost { get; set; }
    }
}
