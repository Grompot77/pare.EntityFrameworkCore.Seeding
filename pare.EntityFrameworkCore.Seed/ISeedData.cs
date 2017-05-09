using System.Threading.Tasks;

namespace pare.EntityFrameworkCore.Seed
{
    public interface ISeedData
    {
        Task Seed(string environmentName);
        void SeedEnums(string environmentName);
    }
}
