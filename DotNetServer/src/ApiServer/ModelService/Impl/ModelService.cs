using System.Linq;
using Common.Base;
using Common.Extensions;
using Common.Helpers;
using Core.ViewOnly;
using Core.Views;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiRequests.LeadForms;
using Dto.ApiRequests.NewsLetter;

namespace WebApp.ModelService.Impl
{
    public class ModelService : IModelService
    {
        private readonly IViewRepository<AppUserView> _appUserViewRepository;
        private readonly IViewRepository<NewsLetterView> _newsLetterViewRepository;

        public ModelService(IViewRepository<AppUserView> appUserViewRepository, IViewRepository<NewsLetterView> newsLetterViewRepository)
        {
            _appUserViewRepository = appUserViewRepository;
            _newsLetterViewRepository = newsLetterViewRepository;
        }

        public bool CheckAppUserRegistrationForm(RegistrationForm form, ValidationResult validationResult)
        {
            if (Formatter.EmailId(form.Email) != true)
                validationResult.AddError("Email", "should be correct format. (xxxxx@xxx.xx)");

            if (Formatter.MobileNumber(form.Mobile) != true)
                validationResult.AddError("MobileNumber", " should be correct format. (9898989898)");


            var userWithSameEmail = _appUserViewRepository.GetByKey(Property.Of<AppUserView>(entity => entity.Email), form.Email);
            if (userWithSameEmail != null)
                validationResult.AddError("Email", "A user with the same email address already exists in the system.");

            var userWithSameMobile = _appUserViewRepository.GetByKey(Property.Of<AppUserView>(entity => entity.Mobile), form.Mobile);
            if (userWithSameMobile != null)
                validationResult.AddError("Mobile", "A user with the same email address already exists in the system.");
        
            return validationResult.IsValid;
        }

        public bool CheckNewsLetterForm(AddNewsLetterForm form, ValidationResult validationResult)
        {
            var alradyAddedWithSameEmail = _newsLetterViewRepository.GetAllFor(Property.Of<LeadView>(x => x.Email), form.Email).SingleOrDefault();

            if (alradyAddedWithSameEmail != null)
                validationResult.AddError("Email ", " should not be duplicated. Already available in system.");

            if (!string.IsNullOrWhiteSpace(form.Email) && form.Email.Length > 128)
                validationResult.AddError("Email", " entered exceeds the maximum length 128");

            if (form.Email.IsEmpty())
            {
                validationResult.AddError("Email", "should not be empty");
            }
            else
            {
                if (form.Email.IsNotEmpty() && Formatter.EmailId(form.Email) != true)
                    validationResult.AddError("Email", "should be correct formate. (xxxxx@xxx.xx)");
            }

            return validationResult.IsValid;
        }

        public bool CheckAnonymousLeadForm(AddAnonymousLeadForm form, ValidationResult validationResult)
        {

            if (!string.IsNullOrWhiteSpace(form.Name) && form.Name.Length > 128)
                validationResult.AddError("Name", " entered exceeds the maximum length 128");

            if (string.IsNullOrWhiteSpace(form.Name))
                validationResult.AddError("Name", "should not empty.");

            if (!string.IsNullOrWhiteSpace(form.Email) && form.Email.Length > 128)
                validationResult.AddError("Email", " entered exceeds the maximum length 128");

            if (form.Email.IsEmpty())
                validationResult.AddError("Email", "should not be empty");
            else
            {
                if (form.Email.IsNotEmpty() && Formatter.EmailId(form.Email) != true)
                    validationResult.AddError("Email", "should be correct formate. (xxxxx@xxx.xx)");
            }

            if (string.IsNullOrWhiteSpace(form.Phone))
                validationResult.AddError("Mobile No.", " should not empty.");

            if (!string.IsNullOrWhiteSpace(form.Description) && form.Description.Length > 2048)
                validationResult.AddError("Message", " entered exceeds the maximum lenght 2048");

            if (string.IsNullOrWhiteSpace(form.Description))
                validationResult.AddError("Message", " should not empty.");

            return validationResult.IsValid;
        }
    }
}