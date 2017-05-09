using Microsoft.EntityFrameworkCore;
using pare.Sample.Entities;

namespace pare.Sample.Domain
{
    public partial class SampleContext
    {
        public virtual DbSet<Title> Titles { get; set; }
    }
}
