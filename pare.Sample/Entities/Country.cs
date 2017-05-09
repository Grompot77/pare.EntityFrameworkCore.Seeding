using pare.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pare.Sample.Entities
{
    [Table(nameof(Country))]
    public class Country : BaseDomainObject<Int16>
    {
        public string Description { get; set; }
        public string Alpha2Code { get; set; }
    }
}
