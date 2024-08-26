using Polaby.Services.Models.CommentLikeModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface ICommentLikeService
    {
        Task<ResponseModel> Like(CommentLikeModel commentLikeModel);
        Task<ResponseModel> Unlike(CommentLikeModel commentLikeModel);
    }
}
