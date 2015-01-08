using System;
using Common.Base;
using Common.Enumerations;

namespace Core.Commands.AppClientCommands
{
	public class AddAppClient : Command
	{
        public string Name { get; set; }
        public string Secret { get; set; }
		public ApplicationType ApplicationType { get; set; }
        public int AccessTokenLifeTime { get; set; }
        public int RefreshTokenLifeTime { get; set; }
		public string AllowedOrigin { get; set; }
		public bool IsActive { get; set; }

	    public override Guid? GetAggregateId()
	    {
	        return Id;
	    }

	    public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            
            return validationResult;
        }
		
	}

}
