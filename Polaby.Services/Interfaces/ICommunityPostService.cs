using Polaby.Services.Common;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface ICommunityPostService
    {
        Task<ResponseDataModel<CommunityPostModel>> Create(CommunityPostCreateModel communityPostCreateModel);
        Task<ResponseDataModel<CommunityPostModel>> Update(Guid id, CommunityPostUpdateModel communityPostUpdateModel);
        Task<ResponseDataModel<CommunityPostModel>> Delete(Guid id);
        Task<Pagination<CommunityPostModel>> GetAllCommunityPosts(CommunityPostFilterModel communityPostFilterModel);
    }
}
