using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using InterfaceLayer.Models;
using DataLayer.Core;
using System.Data.Entity;

namespace DataLayer.Repository
{
    public class DonationRepository : IRepository<DonationBase>
    {
        PPSDonationEntities database;
        public DonationRepository(DbContext myDatabase)
        {
            database = (PPSDonationEntities)myDatabase;
        }
        public void Create(DonationBase entity)
        {
            string TransactionID = Guid.NewGuid().ToString();
            Donation donation = new Donation
            {
                DonationName = entity.DonationName,
                Amount = entity.Amount,
                //DonationStatus = database.DonationTy.Where(x => x.)
            };

            return;
        }

        public void Delete(DonationBase entity)
        {
            throw new NotImplementedException();
        }

        public DonationBase GetById(long id)
        {
            throw new NotImplementedException();
        }

        public object GetContext()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            database.SaveChanges();
        }

        public void Update(DonationBase entity)
        {
            throw new NotImplementedException();
        }

      
    }
    
}