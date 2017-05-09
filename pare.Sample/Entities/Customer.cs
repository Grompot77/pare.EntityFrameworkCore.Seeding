using pare.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace pare.Sample.Entities
{
    [Table(nameof(Customer))]
    public class Customer : BaseDomainObject<int>
    {
        public TitleEnum Title { get; set; }

        [MaxLength(100)]
        public string Firstname { get; set; }
        [MaxLength(100)]
        public string Surname { get; set; }

        [Required]
        public Int16 CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public IList<Order> Orders { get; set; }
    }
}
