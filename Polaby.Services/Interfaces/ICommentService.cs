using Polaby.Services.Common;
using Polaby.Services.Models.CommentModels;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface ICommentService
    {
        Task<ResponseDataModel<CommentModel>> Create(CommentCreateModel commentCreateModel);
        Task<ResponseDataModel<CommentModel>> Update(Guid id, CommentUpdateModel commentUpdateModel);
        Task<ResponseDataModel<CommentModel>> Delete(Guid id);
        Task<Pagination<CommentModel>> GetAllComments(CommentFilterModel commentFilterModel);
    }
}
