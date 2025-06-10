using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Infrastructure.Interfaces;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly Context context;
        private readonly DbSet<T> db;

        public Repo(Context context)
        {
            this.context = context;
            db = context.Set<T>();
        }
        public void Add(T entity)
        {
            db.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = db.Find(id);


            if (entity != null)
                db.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return db.ToList();
        }

        public T GetById(int id)
        {
            var entity = db.Find(id);

            return entity!;

        }


    }
}
