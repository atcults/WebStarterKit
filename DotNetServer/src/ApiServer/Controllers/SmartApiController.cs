using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Common.Base;
using Common.Extensions;
using Core.Commands;
using Core.Domain;
using Common.Enumerations;
using Core.ReadWrite;
using Core.Views;
using Dto.ApiResponses;
using WebApp.Initialization;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class SmartApiController : ApiController
    {
        protected IUserSession UserSession { get; private set; }
        protected IMappingEngine MappingEngine { get; private set; }

        public SmartApiController(IUserSession userSession, IMappingEngine mappingEngine)
        {
            UserSession = userSession;
            MappingEngine = mappingEngine;
        }

        public void EnsureCurrentUserHas(object form)
        {
            if (!CurrentUserHas(form))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        public bool CurrentUserHas(object permission)
        {
            var currentUser = GetCurrentUser();

            return !currentUser.Role.Equals(Role.Guest);
        }

        protected AppUserView GetCurrentUser()
        {
            return UserSession.GetCurrentUser();
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
                var performingUser = UserSession.GetCurrentUser() ?? UserSession.GetAnonymousUser();

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