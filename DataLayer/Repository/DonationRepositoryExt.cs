using DataLayer.Model;
using InterfaceLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DataLayer.Core;
using System.Data.Entity.Validation;

namespace DataLayer.Repository
{
    public class DonationRepositoryExt : DonationRepository
    {
        private PPSDonationEntities db;
        public DonationRepositoryExt(DbContext dbDatabase) : base(dbDatabase)
        {
            db = (PPSDonationEntities)dbDatabase;
        }

        public string CreateDonation(DonationBase entity)
        {
            Guid TransactionID = Guid.NewGuid();
            var donor = ((MainDonor)entity.DonorData);
            var student = ((StudentBase)entity.StudentData);
            string donorType = donor.DonorType;
            
            // TODO: Use ninject to get the class
            /*
            switch(donorType){
                case "Individual":
                    donor = ((IndividualDonor)entity.DonorData);
                    break;
                case "Organization":
                        donor = ((OrganizationDonor)entity.DonorData);
                        break;
            };*/
            Donor newDonor = new Donor
            {
                DonorType = db.DonorTypes.Where(x => x.DonorTypeName.Equals(donor.DonorType)).FirstOrDefault(),
                ContactName = donor.ContactPersonName,
                ContactEmail = donor.ContactPersonEmail,
                ContactPhoneNumber = donor.ContactPersonPhoneNumber,
                TaxNumber = donor.TaxNumber,
                FirstName = donor.FirstName,
                LastName = donor.Surname,
                IsMember = donor.IsMember,
                MembershipNumber = donor.MembershipNumber,
                State = donor.State,
                Country = donor.Country,
                City = donor.City,
                PostCode = donor.PostCode,
                Address1 = donor.Address1,
                Address2 = donor.Address2,
                Gender = donor.Gender,
                RegistrationNumber = donor.RegistrationNumber,
                OrganizationEmail = donor.OrganizationEmail,
                OrganizationName = donor.CompanyName,
                InsertDate = DateTime.Now
            };

            Donation newDonation = new Donation
            {
                Amount = entity.Amount,
                DonationName = entity.DonationName,
                DonationStatu = db.DonationStatus.Where(x => x.StatusName.Equals("CREATED")).FirstOrDefault(),
                Donor = newDonor,
                AspNetStudent = db.AspNetStudents.SingleOrDefault(x => x.Id == student.Id),
                InsertDate = DateTime.Now,
                TransactionDate = DateTime.Now,
                GUID = TransactionID,
                DonatorType = entity.DonatorType
            };
            //db.Donors.Add(newDonor);
            db.Donations.Add(newDonation);
            

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    error = error + " Entity Type:" + eve.Entry.Entity.GetType().Name + " Entity State:" + eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        error = error + " ve.PropertyName:" + ve.PropertyName + " ve.ErrorMessage:" + ve.ErrorMessage;
                    }
                }
                throw new Exception(error);
            }
            db.Entry(newDonation).Reload();            //entity.donationdata;
            return newDonation.DonationID.ToString();
        }

        public bool DeleteDonation(DonationBase entity)
        {
            throw new NotImplementedException();
        }

        public bool GetByIdDonation(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateDonation(DonationBase entity)
        {
            throw new NotImplementedException();
        }



    }
}