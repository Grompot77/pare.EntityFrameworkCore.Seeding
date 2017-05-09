using System;
using System.Collections.Generic;
using System.Text;

namespace pare.EntityFrameworkCore
{
    public interface IDomainObject<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IModifyObject<TKey> : IModifyObject where TKey : IEquatable<TKey>
    {
        TKey ModifiedBy { get; set; }
    }

    public interface IModifyObject
    {
        DateTime Modified { get; set; }
    }
}
