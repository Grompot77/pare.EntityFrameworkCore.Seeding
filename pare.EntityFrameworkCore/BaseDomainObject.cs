using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace pare.EntityFrameworkCore
{
    public class BaseDomainObject<TKey> : IDomainObject<TKey>, IModifyObject where TKey : IEquatable<TKey>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }
        public DateTime Modified { get; set; }
    }
}
