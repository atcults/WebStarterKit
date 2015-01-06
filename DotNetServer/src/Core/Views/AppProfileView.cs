using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;

namespace Core.Views
{
	[ViewName("AppProfileView")]
	public class AppProfileView : View
	{
		public string Name { get; set; }
		public bool? IsActive { get; set; }
	}
}
