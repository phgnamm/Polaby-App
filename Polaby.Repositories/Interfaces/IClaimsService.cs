namespace Polaby.Repositories.Interfaces
{
	public interface IClaimsService
	{
		public Guid? GetCurrentUserId { get; }
	}
}
