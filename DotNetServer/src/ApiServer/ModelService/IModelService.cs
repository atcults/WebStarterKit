using Common.Base;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiRequests.LeadForms;
using Dto.ApiRequests.NewsLetter;

namespace WebApp.ModelService
{
    public interface IModelService
    {
        bool CheckAppUserRegistrationForm(RegistrationForm form, ValidationResult validationResult);
        bool CheckNewsLetterForm(AddNewsLetterForm form, ValidationResult validationResult);
        bool CheckAnonymousLeadForm(AddAnonymousLeadForm form, ValidationResult validationResult);
    }
}