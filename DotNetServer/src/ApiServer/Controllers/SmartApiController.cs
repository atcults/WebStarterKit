using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Common.Base;
using Common.Extensions;
using Common.Helpers;
using Core.Commands;
using Core.Domain;
using Core.ReadWrite;
using Core.ViewOnly;
using Core.Views;
using Dto.ApiResponses;
using WebApp.Initialization;

namespace WebApp.Controllers
{
    public class SmartApiController : ApiController
    {
        protected IMappingEngine MappingEngine { get; private set; }

        private readonly IViewRepository<AppUserView> _appUserViewRepository;

        public SmartApiController()
        {
            MappingEngine = ClientEndPoint.Container.GetInstance<IMappingEngine>();
            _appUserViewRepository = ClientEndPoint.Container.GetInstance<IViewRepository<AppUserView>>();
        }

        protected AppUserView GetCurrentUser()
        {
            var owinContext = Request.GetOwinContext(); //This can be usefull for automapper resolving current user.
            var username = RequestContext.Principal.Identity.Name;
            return Formatter.EmailId(username) ? _appUserViewRepository.GetByKey(Property.Of<AppUserView>(x => x.Email), username) : _appUserViewRepository.GetByKey(Property.Of<AppUserView>(x => x.Mobile), username);
        }

        protected HttpResponseMessage ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var validationResult = command.Validate();

            IWebApiResponse response = new WebApiResponseBase(validationResult);

            if (!validationResult.IsValid)
            {
                return Content(response);
            }
            
            try
            {
                var performingUser = GetCurrentUser();

                var auditedCommand = command as AuditedCommand;
                if (auditedCommand != null)
                {
                    if (string.IsNullOrEmpty(auditedCommand.ImageData))
                    {
                        auditedCommand.ImageData = ImageUtility.NoImageData;
                    }
                }

                //TODO: Validate user access for this command via profile. Profile is to be implemented.

                //var processor = ClientEndPoint.Container.GetAllInstances<ICommandProcessor<TCommand>>().Last();

                var processor = ClientEndPoint.Container.GetInstance<ICommandProcessor<TCommand>>();

                var instance = ClientEndPoint.Container.GetInstance<IUnitOfWork>();

                instance.Begin();

                ClientEndPoint.Container.GetInstance<ICommandRecorder>().Record(command, performingUser.Id, performingUser.Name);

                try
                {
                    processor.Process(command, performingUser.Id, out response);
                    instance.Commit();
                }
                catch
                {
                    instance.RollBack();
                    throw;
                }
                finally
                {
                    instance.Dispose();
                }
            }
            catch (DomainProcessException domainException)
            {
                response.AddError("Form", domainException.GetBaseException().Message);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, typeof(SmartApiController), "Internal", e);
                response.AddError("Internal", e.GetBaseException().Message);
            }

            return Content(response);
        }

        protected HttpResponseMessage Content(IWebApiResponse response)
        {
            var httpStatusCode = HttpStatusCode.OK;

            if (! response.IsValid)
            {
                httpStatusCode = HttpStatusCode.NotAcceptable;
            }

            return Request.CreateResponse(httpStatusCode, response);
        }
    }
}