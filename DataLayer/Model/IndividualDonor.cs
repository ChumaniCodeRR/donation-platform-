
namespace DataLayer.Model
{
    public class IndividualDonor:DonorBase
    {
        public string Gender { get; set; }
        public long TaxNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }

    }
}