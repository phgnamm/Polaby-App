using System.Security.Claims;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Utils;

namespace Polaby.API.Utils
{
    public class ClaimsService : IClaimsService
    {
		public Guid? GetCurrentUserId { get; }

		public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
			GetCurrentUserId = AuthenticationTools.GetCurrentUserId(identity!);
        }
    }
}
