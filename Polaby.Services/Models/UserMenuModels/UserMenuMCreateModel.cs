
namespace Polaby.Services.Models.UserMenuModels
{
    public class UserMenuMCreateModel
    {
        public Guid? UserId { get; set; }
        public List<Guid>? MenuIds { get; set; }
    }
}
