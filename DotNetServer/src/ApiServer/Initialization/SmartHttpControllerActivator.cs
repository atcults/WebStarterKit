using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace WebApp.Initialization
{
    public class SmartHttpControllerActivator : IHttpControllerActivator
    {
    
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType == null) return null;
            var controller = ClientEndPoint.Container.GetInstance(controllerType);
            ClientEndPoint.Container.BuildUp(controller);

            return (IHttpController) controller;
        }
    }
}