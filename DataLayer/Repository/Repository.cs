using InterfaceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase

    {

        public void Create(T entity)

        {

            //Write your logic here to persist the entity

        }

        public void Delete(T entity)

        {

            //Write your logic here to delete an entity

        }

        public T GetById(long id)

        {

            //Write your logic here to retrieve an entity by Id

            throw new NotImplementedException();

        }

        public object GetContext()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)

        {

            //Write your logic here to update an entity

        }

    }
}