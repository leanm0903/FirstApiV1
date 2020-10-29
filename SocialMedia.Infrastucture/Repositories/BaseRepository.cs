using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T:BaseEntiy
    {
        public readonly SocialMediaContext _context;
        public DbSet<T> _entities;
        public BaseRepository(SocialMediaContext context)
        {
            _context = context; 
            _entities = context.Set<T>();//se genera  un dbSet
        }
        public async Task Add(T Entity)
        {
           await  _entities.AddAsync(Entity);
        }

        public async Task Delete(int id)
        {
            var entities = await GetById(id);
            _entities.Remove(entities);
        }

        public IEnumerable<T> GetAll()
        {
            return  _entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);

        }
    }
}
