using Polaby.Services.Models.CommunityPostLikeModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface ICommunityPostLikeService
    {
        Task<ResponseModel> Like(CommunityPostLikeModel communityPostLikeModel);
        Task<ResponseModel> Unlike(CommunityPostLikeModel communityPostLikeModel);
    }
}
