using Polaby.Repositories.Models.AccountModels;
using Polaby.Services.Common;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.CommonModels;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.TokenModels;

namespace Polaby.Services.Interfaces
{
	public interface IAccountService
	{
		Task<ResponseModel> Register(AccountRegisterModel accountRegisterModel);
		Task<ResponseDataModel<TokenModel>> Login(AccountLoginModel accountLoginModel);
		Task<ResponseDataModel<TokenModel>> RefreshToken(RefreshTokenModel refreshTokenModel);
		Task<ResponseModel> VerifyEmail(string email, string verificationCode);
		Task<ResponseModel> ResendVerificationEmail(EmailModel? emailModel);
		Task<ResponseModel> ChangePassword(AccountChangePasswordModel accountChangePasswordModel);
		Task<ResponseModel> ForgotPassword(EmailModel emailModel);
		Task<ResponseModel> ResetPassword(AccountResetPasswordModel accountResetPasswordModel);
		Task<ResponseDataModel<TokenModel>> LoginGoogle(string code);
		Task<ResponseModel> AddAccounts(List<AccountRegisterModel> accountRegisterModels);
		Task<ResponseDataModel<AccountModel>> GetAccount(Guid id);
		Task<Pagination<AccountModel>> GetAllAccounts(AccountFilterModel accountFilterModel);
		Task<ResponseModel> UpdateAccountUser(Guid id, AccountUserUpdateModel accountUserUpdateModel);
		Task<ResponseModel> UpdateAccountExpert(Guid id, AccountExpertUpdateModel accountExpertUpdateModel);
		Task<ResponseModel> UpdateAccount(Guid id, AccountUpdateModel accountUpdateModel);
		Task<ResponseModel> DeleteAccount(Guid id);
		Task<ResponseModel> RestoreAccount(Guid id);
	}
}
