using Microsoft.EntityFrameworkCore;
using SmallFirmApp.Data;
using System.Collections.Generic;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SmallFirmApp.Repositories
{
    public class SmallFirmAppRepository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext context { get; set; }
        private DbSet<T> dbSet { get; set; }
        public SmallFirmAppRepository(ApplicationDbContext ctx)
        {
            context = ctx;
            dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var dbRecord = await dbSet.FindAsync(id);
            if (dbRecord == null)
            {
                throw new KeyNotFoundException();
            }
            dbSet.Remove(dbRecord);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllActiveAsync()
        {
            var propertyInfo = typeof(T).GetProperty("isActive", BindingFlags.Public | BindingFlags.Instance);
            var dbList = await dbSet.ToListAsync();
            List<T> filter = new List<T>();

            foreach (var db in dbList)
            {
                if (propertyInfo != null && propertyInfo.CanRead)
                {
                    var value = propertyInfo.GetValue(db);
                    if (value != null && !value.Equals(0))
                    {
                        filter.Add(db);
                    }
                }
            }
            return filter;
        }
 
        public async Task<T> GetByIdAsync(int id)
        {
            var dbReckord = await dbSet.FindAsync(id);
            if (dbReckord == null)
            {
                throw new KeyNotFoundException();
            }
            return dbReckord;
        }

        //public async Task<T> GetByDateAsync(DateTime d)
        //{
        //    var dbReckord = await dbSet.FirstOrDefaultAsync(v => v.Day == d);
        //    if (dbReckord == null)
        //    {
        //        throw new KeyNotFoundException();
        //    }
        //    return dbReckord;
        //}

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await context.SaveChangesAsync();
        }

        //public async Task<T> ConsInUse(int consId)
        //{
        //    var del = context.Deliveries.Where(c => c.ConsumativeId == consId).FirstOrDefault();
        //    return del;
        //}

        //public async Task ExtraInUse(int extraId)
        //{

        //}
    }
}
