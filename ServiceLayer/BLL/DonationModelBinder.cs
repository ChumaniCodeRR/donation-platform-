using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using DataLayer.Model;
namespace ServiceLayer.BLL
{
    public class DonationModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(DonationViewModel))
            {
                return false;
            }
            
            var content = actionContext.Request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            JObject jObj = JObject.Parse(jsonContent);
            var donorDetails = jObj["DonorDetails"];
            var studentDetails = jObj["StudentDetails"];
            var donorType = (jObj["DonorType"]).ToString();

            DonationViewModel donation = new DonationViewModel();
            JsonConvert.PopulateObject(jsonContent, donation);
            DonorBase donor = null;
            StudentBase student = null;
            /*switch(donorType)
            {
                //TODO: Removed to reduce uneccessary complexity
                case "Individual":
                    donor = new IndividualDonor();
                    JsonConvert.PopulateObject(donorDetails.ToString(), donor);
                    donation.DonorDetails = donor;
                    break;
                case "Organization":
                    donor = new OrganizationDonor();
                    JsonConvert.PopulateObject(donorDetails.ToString(), donor);
                    donation.DonorDetails = donor;
                    
                    break;
                default:
                    return false;
                    
            }*/
            donor = new MainDonor();
            student = new StudentBase();
            JsonConvert.PopulateObject(donorDetails.ToString(), donor);
            JsonConvert.PopulateObject(studentDetails.ToString(), student);
            donation.DonorDetails = donor;
            donation.StudentDetails = student;
            bindingContext.Model = donation;

            return true;
            
        }
    }
}