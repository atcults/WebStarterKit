using System;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;

namespace Core.Views
{
    [ViewName("TokenStoreView")]
	public class TokenStoreView : View
	{
		public string ClientName { get; set; }
        public string UserName { get; set; }
        public string AccessTokenHash { get; set; }
        public string AccessTicket { get; set; }
        public DateTime? AccessTokenIssuedUtc { get; set; }
        public DateTime? AccessTokenExpiresUtc { get; set; }
        public string RefreshTokenHash { get; set; }
        public string RefreshTicket { get; set; }
		public DateTime? RefreshTokenIssuedUtc { get; set; }
        public DateTime? RefreshTokenExpiresUtc { get; set; }
        public int? TimesTokenGiven { get; set; }
	}
}
