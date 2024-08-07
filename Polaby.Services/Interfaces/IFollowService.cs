using Polaby.Services.Models.FollowModels;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Interfaces
{
    public interface IFollowService
    {
        Task<ResponseModel> Follow(FollowModel followModel);
        Task<ResponseModel> Unfollow(FollowModel followModel);
    }
}
