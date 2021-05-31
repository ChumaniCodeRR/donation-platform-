using System;

namespace InterfaceLayer.Models
{
  
        public interface IRepository<T> where T : EntityBase

        {
            object GetContext();   
            T GetById(Int64 id);

            void Create(T entity);

            void Delete(T entity);

            void Update(T entity);

            void Save();

        }
    
}
