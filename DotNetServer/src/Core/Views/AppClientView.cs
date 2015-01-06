using Common.Enumerations;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;
using Newtonsoft.Json;

namespace Core.Views
{
    [ViewName("AppClientView")]
	public class AppClientView : View
	{
        public string Name { get; set; }
        public string Secret { get; set; }
		
		[JsonIgnore]
		public ApplicationType ApplicationType { get; set; }
		public string ApplicationTypeValue { get { return ApplicationType == null ? "" : ApplicationType.Value; } set { ApplicationType = ApplicationType.FromValue(value); } }
		public string ApplicationTypeName { get { return ApplicationType == null ? "" : ApplicationType.DisplayName; } set { ApplicationType = ApplicationType.FromDisplay(value); } }

		public int? RefreshTokenLifeTime { get; set; }
		public string AllowedOrigin { get; set; }
		public bool? IsActive { get; set; }

	}
}
