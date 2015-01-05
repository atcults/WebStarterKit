using System;
using Common.Base;
using Core.ReadWrite;
using NServiceBus;
using NServiceBus.Logging;

namespace NSBus.Server
{
    public class MessageHandler<T> : IHandleMessages<T>
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly ILog Log = LogManager.GetLogger(typeof(MessageHandler<>));
        protected readonly IBus Bus;
        public MessageHandler(IUnitOfWork unitOfWork, IBus bus)
        {
            _unitOfWork = unitOfWork;
            Bus = bus;
        }

        public void Handle(T message)
        {
            Log.InfoFormat("Got command : {0}", message.ToString());
            
            try
            {
                var sessionRequired = _unitOfWork.CurrentConnection == null;

                if (sessionRequired)
                {
                    _unitOfWork.Begin();
                }           

                HandleMessage(message);

                _unitOfWork.Commit();
            }
            catch(Exception exception)
            {
                Logger.Log(LogType.Error, this, "Error while data operation", exception);
                if(_unitOfWork.CurrentConnection != null) _unitOfWork.RollBack();
                throw;
            }
            finally
            {
                if (_unitOfWork.CurrentConnection != null) _unitOfWork.Dispose();
            }
        }

        public virtual void HandleMessage(T command)
        {
            throw new Exception("Method not implemented");
        }
    }
}
