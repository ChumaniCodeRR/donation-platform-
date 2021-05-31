using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer.Models
{
    
    public interface IRepositoryWithBool<T> where T : EntityBase, IRepository<T>
    {
       
        bool Create(T entity);

        bool Delete(T entity);

        bool Update(T entity);

    }
}
