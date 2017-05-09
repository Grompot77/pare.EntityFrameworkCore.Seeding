using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pare.EntityFrameworkCore.Seed;
using pare.Sample.Entities;
using pare.Sample.Domain;

namespace pare.Sample.Seeding
{
public class SeedSampleContext : BaseSeeding<SampleContext>
{
    private string[] _allowed = new[] { "Development", "Staging" };

    public SeedSampleContext(IServiceProvider provider) : base(provider) { }

    public override Task Seed(string environmentName)
    {
        //see that on the second parameter we are updating the modified date
        AddOrUpdate(Data.Countries.Select(c => c.Value),
            action: c => c.Modified = DateTime.Now, propertiesToMatch: c => c.Alpha2Code);
        //one property only
        Add(Data.Products.Select(p => p.Value), p => p.Description);

        if (!_allowed.Contains(environmentName))
            return null;
        //two properties will be checked for uniqueness
        Add(Data.Customers.Select(c => c.Value), c => c.Firstname, c => c.Surname);
        Add(Data.Orders.Select(o => o.Value), o => o.Number);
        Add(Data.GetSomeOrderItems(), oi => oi.ProductId, oi => oi.OrderId);
        return null;
    }
}
}
