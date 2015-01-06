using System;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;

namespace Core.Views
{
    [ViewName("TokenStoreView")]
	public class TokenStoreView : View
	{
		public string Name { get; set; }
        public string TokenHash { get; set; }
		public Guid? ClientId { get; set; }
		public string TicketHash { get; set; }
		public DateTime? IssuedUtc { get; set; }
		public DateTime? ExpiresUtc { get; set; }
	}
}
