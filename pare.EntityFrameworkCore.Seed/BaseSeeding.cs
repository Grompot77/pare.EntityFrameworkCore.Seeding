using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace pare.EntityFrameworkCore.Seed
{
    public abstract class BaseSeeding<TContext> : ISeedData where TContext : DbContext, IDbContext
    {
        public IServiceProvider Provider { get; }

        public BaseSeeding(IServiceProvider provider)
        {
            Provider = provider;
        }

        public async Task AddOrUpdateAsync<TEntity>(IEnumerable<TEntity> entities, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            await InternalAddOrUpdateAsync(entities);
        }

        public async Task AddOrUpdateAsync<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            await InternalAddOrUpdateAsync(entities, action, true, propertiesToMatch);
        }

        public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            await InternalAddOrUpdateAsync(entities, false, propertiesToMatch);
        }

        public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            await InternalAddOrUpdateAsync(entities, action, false, propertiesToMatch);
        }

        public void AddOrUpdate<TEntity>(IEnumerable<TEntity> entities, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            InternalAddOrUpdate(entities, true, propertiesToMatch);
        }

        public void AddOrUpdate<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            InternalAddOrUpdate(entities, action, true, propertiesToMatch);
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            InternalAddOrUpdate(entities, false, propertiesToMatch);
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action, params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            InternalAddOrUpdate(entities, action, false, propertiesToMatch);
        }

        private void InternalAddOrUpdate<TEntity>(IEnumerable<TEntity> entities, bool update = true,
            params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            InternalAddOrUpdate(entities, null, update, propertiesToMatch);
        }

        private void InternalAddOrUpdate<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action, bool update = true,
            params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            using (var serviceScope = Provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TContext>();
                InternalEntityState(entities, action, update, propertiesToMatch, context);
                context.SaveChanges();
            }
        }

        private async Task InternalAddOrUpdateAsync<TEntity>(IEnumerable<TEntity> entities, bool update = true,
            params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            await InternalAddOrUpdateAsync(entities, null, update, propertiesToMatch);
        }

        private async Task InternalAddOrUpdateAsync<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action, bool update = true,
            params Func<TEntity, object>[] propertiesToMatch)
            where TEntity : class
        {
            using (var serviceScope = Provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TContext>();
                InternalEntityState(entities, action, update, propertiesToMatch, context);
                await context.SaveChangesAsync();
            }
        }

        private static void InternalEntityState<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action, bool update,
            Func<TEntity, object>[] propertiesToMatch, TContext context) where TEntity : class
        {
            var existing = context.Set<TEntity>().AsNoTracking().ToList(); //ajb: do as no tracking to avoid errors
            foreach (var item in entities)
            {
                var match = FindMatch(existing, item, propertiesToMatch);
                var citem = context.Entry(item);
                if (match != null)
                    citem.Property("Id").CurrentValue = context.Entry(match).Property("Id").CurrentValue;

                context.Entry(item).State = update
                    ? (match != null ? EntityState.Modified : EntityState.Added)
                    : (match != null ? EntityState.Unchanged : EntityState.Added);

                if (item is IModifyObject)
                {
                    if (((IModifyObject)item).Modified == DateTime.MinValue)
                        ((IModifyObject)item).Modified = DateTime.Now;
                }

                action?.Invoke(citem.Entity);
            }
        }

        private static TEntity FindMatch<TEntity>(List<TEntity> existing, TEntity item, params Func<TEntity, object>[] propertiesToMatch)
        {
            return existing.FirstOrDefault(g =>
            {
                var r = true;
                foreach (var ptm in propertiesToMatch)
                {
                    var rptm = ptm(g);
                    if (rptm != null)
                        r &= ptm(g).Equals(ptm(item));
                }
                return r;
            });
        }

        public Type[] GetDbEnums()
        {
            var types = Provider.GetService<TContext>().Model.GetEntityTypes().Select(t => t.ClrType);
            var result = types.Where(l => typeof(IDbEnum).GetTypeInfo().IsAssignableFrom(l));
            return result.ToArray();
        }

        public void SeedEnum(params Type[] types)
        {
            types.ToList().ForEach(t => InternalTypedSeedEnum(t));
        }

        public void SeedEnum<TClass>(bool update = true) where TClass : class, IDbEnum
        {
            InternalSeedEnum<TClass>(update);
        }

        private void InternalSeedEnum<TClass>(bool update = true) where TClass : class, IDbEnum
        {
            var type = typeof(TClass).GetTypeInfo();
            while (type.BaseType != null)
            {
                type = type.BaseType.GetTypeInfo();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DbEnum<>))
                {
                    var arg = (type.GetGenericArguments()[0]).GetTypeInfo();
                    var instance = typeof(TClass).GetTypeInfo().GetConstructor(new Type[] { arg.AsType() });
                    var list = new List<TClass>();
                    if (arg.IsEnum)
                    {
                        var values = Enum.GetValues(arg.AsType());
                        foreach (var val in values)
                        {
                            if ((int)Convert.ChangeType(val, typeof(int)) == 0) //this is to exclude the 0 index
                                continue;
                            var obj = instance.Invoke(new[] { val });
                            list.Add((TClass)obj);
                        }
                        InternalAddOrUpdate<TClass>(list, update, i => i.Id);
                    }
                }
            }
        }

        private void InternalTypedSeedEnum(Type type, bool update = true)
        {
            var mi = GetType().GetTypeInfo().BaseType.GetTypeInfo().GetMethod("InternalSeedEnum", BindingFlags.NonPublic | BindingFlags.Instance);
            var gm = mi.MakeGenericMethod(type);
            gm.Invoke(this, new object[] { update });
        }

        public abstract Task Seed(string environmentName);

        public virtual void SeedEnums(string environmentName)
        {
            SeedEnum(GetDbEnums());
        }
    }
}
