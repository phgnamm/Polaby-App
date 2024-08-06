using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Interfaces
{
    public interface ICommunityPostService
    {
        Task<ResponseDataModel<CommunityPostModel>> Create(CommunityPostCreateModel communityPostCreateModel);
    }
}
