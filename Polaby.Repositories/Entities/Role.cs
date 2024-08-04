using Microsoft.AspNetCore.Identity;

namespace Polaby.Repositories.Entities
{
	public class Role : IdentityRole<Guid>
	{
		public string? Description { get; set; }
	}
}
