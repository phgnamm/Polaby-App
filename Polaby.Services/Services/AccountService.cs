﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.AccountModels.Validation;
using Polaby.Services.Models.CommonModels;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.TokenModels;

namespace Polaby.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Account> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IClaimsService _claimsService;

        public AccountService(UserManager<Account> userManager, IUnitOfWork unitOfWork, IMapper mapper,
            IConfiguration configuration, IEmailService emailService, IClaimsService claimsService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _claimsService = claimsService;
        }

        public async Task<ResponseModel> Register(AccountRegisterModel accountRegisterModel)
        {
            // Check if email already exists
            var existedEmail = await _userManager.FindByEmailAsync(accountRegisterModel.Email);

            if (existedEmail != null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Email already exists"
                };
            }

            // Create new account
            var user = _mapper.Map<Account>(accountRegisterModel);
            user.UserName = user.Email;
            user.Gender = Gender.Female; // User is female by default
            user.CreationDate = DateTime.Now;
            user.VerificationCode = AuthenticationTools.GenerateVerificationCode(6);
            user.VerificationCodeExpiryTime = DateTime.Now.AddMinutes(15);

            var result = await _userManager.CreateAsync(user, accountRegisterModel.Password);

            if (result.Succeeded)
            {
                // Add role
                await _userManager.AddToRoleAsync(user, Repositories.Enums.Role.User.ToString());

                // Email verification (disable this function if users are not required to verify their email)
                await SendVerificationEmail(user);

                return new ResponseModel
                {
                    Status = true,
                    Message = "Account has been created successfully, please verify your email",
                    EmailVerificationRequired = true
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot create account"
            };
        }

        private async Task SendVerificationEmail(Account account)
        {
            await _emailService.SendEmailAsync(account.Email!, "Polaby - Xác nhận tạo tài khoản",
                $"Mã xác nhận của bạn là {account.VerificationCode}. Mã sẽ hết hạn sau 15 phút.", true);
        }

        public async Task<ResponseDataModel<TokenModel>> Login(AccountLoginModel accountLoginModel)
        {
            var user = await _userManager.FindByNameAsync(accountLoginModel.Email);

            if (user != null)
            {
                if (user.IsDeleted)
                {
                    return new ResponseDataModel<TokenModel>
                    {
                        Status = false,
                        Message = "Account has been deleted"
                    };
                }

                if (await _userManager.CheckPasswordAsync(user, accountLoginModel.Password))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim("userId", user.Id.ToString()),
                        new Claim("userEmail", user.Email!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var userRoles = await _userManager.GetRolesAsync(user);

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    // Check if refresh token is expired, if so then update
                    if (user.RefreshToken == null || user.RefreshTokenExpiryTime < DateTime.Now)
                    {
                        var refreshToken = TokenTools.GenerateRefreshToken();
                        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"],
                            out int refreshTokenValidityInDays);

                        // Update user's refresh token
                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                        var result = await _userManager.UpdateAsync(user);

                        if (!result.Succeeded)
                        {
                            return new ResponseDataModel<TokenModel>
                            {
                                Status = false,
                                Message = "Cannot login"
                            };
                        }
                    }

                    var jwtToken = TokenTools.CreateJWTToken(authClaims, _configuration);

                    return new ResponseDataModel<TokenModel>
                    {
                        Status = true,
                        Message = "Login successfully",
                        EmailVerificationRequired = !user.EmailConfirmed,
                        UserId = user.Id,
                        Data = new TokenModel
                        {
                            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            AccessTokenExpiryTime = jwtToken.ValidTo.ToLocalTime(),
                            RefreshToken = user.RefreshToken,
                            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
                        }
                    };
                }
            }

            return new ResponseDataModel<TokenModel>
            {
                Status = false,
                Message = "Invalid email or password"
            };
        }

        public async Task<ResponseDataModel<TokenModel>> RefreshToken(RefreshTokenModel refreshTokenModel)
        {
            // Validate access token and refresh token
            var principal = TokenTools.GetPrincipalFromExpiredToken(refreshTokenModel.AccessToken, _configuration);

            var user = await _userManager.FindByIdAsync(principal!.FindFirst("userId")!.Value);

            if (user == null || user.RefreshToken != refreshTokenModel.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Invalid access token or refresh token"
                };
            }

            // Start to refresh access token and refresh token
            var refreshToken = TokenTools.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            // Update user's refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Cannot refresh the token"
                };
            }

            var jwtToken = TokenTools.CreateJWTToken(principal.Claims.ToList(), _configuration);

            return new ResponseDataModel<TokenModel>
            {
                Status = true,
                Message = "Refresh token successfully",
                Data = new TokenModel
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    AccessTokenExpiryTime = jwtToken.ValidTo.ToLocalTime(),
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
                }
            };
        }

        public async Task<ResponseModel> VerifyEmail(string email, string verificationCode)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            if (user.VerificationCodeExpiryTime < DateTime.Now)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "The code is expired",
                    EmailVerificationRequired = false
                };
            }

            if (user.VerificationCode == verificationCode)
            {
                user.EmailConfirmed = true;
                user.VerificationCode = null;
                user.VerificationCodeExpiryTime = null;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Verify email successfully",
                    };
                }
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot verify email",
            };
        }

        public async Task<ResponseModel> ResendVerificationEmail(EmailModel? emailModel)
        {
            var currentUserId = _claimsService.GetCurrentUserId;

            if (emailModel == null && currentUserId == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found",
                };
            }

            Account? user = null;

            if (emailModel != null && currentUserId == null)
            {
                user = await _userManager.FindByEmailAsync(emailModel.Email);

                if (user == null)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "User not found",
                    };
                }
            }
            else if (emailModel == null && currentUserId != null)
            {
                user = await _userManager.FindByIdAsync(currentUserId.ToString()!);
            }
            else if (emailModel != null && currentUserId != null)
            {
                user = await _userManager.FindByEmailAsync(emailModel.Email);

                if (user == null || user.Id != currentUserId)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "Cannot resend verification email",
                        EmailVerificationRequired = true
                    };
                }
            }

            if (user!.EmailConfirmed)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Email has been verified",
                };
            }

            // Update new verification code
            user.VerificationCode = AuthenticationTools.GenerateVerificationCode(6);
            user.VerificationCodeExpiryTime = DateTime.Now.AddMinutes(15);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await SendVerificationEmail(user);

                return new ResponseModel
                {
                    Status = true,
                    Message = "Resend Verification email successfully",
                    EmailVerificationRequired = true
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot resend verification email",
                EmailVerificationRequired = true
            };
        }

        public async Task<ResponseModel> ChangePassword(AccountChangePasswordModel accountChangePasswordModel)
        {
            var currentUserId = _claimsService.GetCurrentUserId;
            var user = await _userManager.FindByIdAsync(currentUserId.ToString()!);

            var result = await _userManager.ChangePasswordAsync(user!, accountChangePasswordModel.OldPassword,
                accountChangePasswordModel.NewPassword);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Change password successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot change password",
            };
        }

        public async Task<ResponseModel> ForgotPassword(EmailModel emailModel)
        {
            var user = await _userManager.FindByEmailAsync(emailModel.Email);

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found",
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // todo modify this Email body to send a URL redirect to the frontend page and contain the token as a parameter in the URL
            await _emailService.SendEmailAsync(user.Email!, "Polaby - Khôi phục mật khẩu",
                $"Truy cập vào <a href={_configuration["OAuth2:Server:RedirectURI"]}/khoi-phuc-mat-khau?email={user.Email}&token={token}>liên kết này</a> để khôi phục mật khẩu cho tài khoản. Liên kết sẽ hết hạn sau 15 phút.",
                true);

            return new ResponseModel
            {
                Status = true,
                Message = "An email has been sent, please check your inbox",
            };
        }

        public async Task<ResponseModel> ResetPassword(AccountResetPasswordModel accountResetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(accountResetPasswordModel.Email);

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found",
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, accountResetPasswordModel.Token,
                accountResetPasswordModel.Password);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Reset password successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot reset password",
            };
        }

        public async Task<ResponseDataModel<TokenModel>> LoginGoogle(string code)
        {
            // Exchange authorization code for refresh and access tokens
            HttpClient tokenClient = new HttpClient { BaseAddress = new Uri("https://oauth2.googleapis.com/token") };
            tokenClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var googleTokenRequestData = new
            {
                client_id = _configuration["OAuth2:Google:ClientId"],
                client_secret = _configuration["OAuth2:Google:ClientSecret"],
                code,
                grant_type = "authorization_code",
                redirect_uri = _configuration["OAuth2:Server:RedirectURI"] + "/api/v1/authentication/login/google"
            };

            HttpResponseMessage googleTokenResponse = await tokenClient.PostAsJsonAsync("", googleTokenRequestData);

            if (!googleTokenResponse.IsSuccessStatusCode)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Error when trying to connect to Google API"
                };
            }

            // Get user information with Google access token
            var googleTokenModel =
                JsonConvert.DeserializeObject<GoogleTokenModel>(await googleTokenResponse.Content.ReadAsStringAsync());
            var userInfoClient = new HttpClient { BaseAddress = new Uri("https://www.googleapis.com/oauth2/v1/") };
            HttpResponseMessage googleUserInformationResponse =
                await userInfoClient.GetAsync($"userinfo?access_token={googleTokenModel!.AccessToken}");

            if (!googleUserInformationResponse.IsSuccessStatusCode)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Error when trying to connect to Google API"
                };
            }

            var googleUserInformationModel =
                JsonConvert.DeserializeObject<GoogleUserInformationModel>(await googleUserInformationResponse.Content
                    .ReadAsStringAsync());

            // Handle user information
            var user = await _userManager.FindByEmailAsync(googleUserInformationModel!.Email!);

            if (user == null)
            {
                //return new ResponseDataModel<TokenModel>
                //{
                //    Status = false,
                //    Message = "User not found"
                //};

                user = _mapper.Map<Account>(googleUserInformationModel);
                user.UserName = user.Email;
                var saveUserResult = await _userManager.CreateAsync(user);

                if (saveUserResult.Succeeded)
                {
                    // Add role
                    await _userManager.AddToRoleAsync(user, Repositories.Enums.Role.User.ToString());
                }
                else
                {
                    return new ResponseDataModel<TokenModel>
                    {
                        Status = false,
                        Message = "Cannot create account"
                    };
                }
            }

            if (user.IsDeleted)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Account has been deleted"
                };
            }

            // JWT token
            var authClaims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("userEmail", user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // Check if refresh token is expired, if so then update
            if (user.RefreshToken == null || user.RefreshTokenExpiryTime < DateTime.Now)
            {
                var refreshToken = TokenTools.GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                // Update user's refresh token
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return new ResponseDataModel<TokenModel>
                    {
                        Status = false,
                        Message = "Cannot login"
                    };
                }
            }

            var jwtToken = TokenTools.CreateJWTToken(authClaims, _configuration);

            return new ResponseDataModel<TokenModel>
            {
                Status = true,
                Message = "Login successfully",
                EmailVerificationRequired = !user.EmailConfirmed,
                Data = new TokenModel
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    AccessTokenExpiryTime = jwtToken.ValidTo.ToLocalTime(),
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
                }
            };
        }

        public async Task<ResponseModel> AddAccounts(List<AccountRegisterModel> accountRegisterModels)
        {
            int count = 0;
            foreach (var accountRegisterModel in accountRegisterModels)
            {
                // Check if email already exists
                var existedEmail = await _userManager.FindByEmailAsync(accountRegisterModel.Email);

                if (existedEmail == null)
                {
                    // Create new account
                    var user = _mapper.Map<Account>(accountRegisterModel);
                    user.UserName = user.Email;
                    user.CreationDate = DateTime.Now;
                    // user.VerificationCode = AuthenticationTools.GenerateVerificationCode(6);
                    // user.VerificationCodeExpiryTime = DateTime.Now.AddMinutes(15);

                    var result = await _userManager.CreateAsync(user, accountRegisterModel.Password);

                    if (result.Succeeded)
                    {
                        // Add role
                        await _userManager.AddToRoleAsync(user,
                            accountRegisterModel.Role != null
                                ? accountRegisterModel.Role.ToString()
                                : Repositories.Enums.Role.User.ToString());

                        // Email verification (disable this function if users are not required to verify their email)
                        // await SendVerificationEmail(user);

                        count++;
                    }
                }
            }

            return new ResponseModel
            {
                Status = true,
                Message = $"Add {count} accounts successfully"
            };
        }

        public async Task<ResponseDataModel<AccountModel>> GetAccount(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseDataModel<AccountModel>()
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            var userModel = _mapper.Map<AccountModel>(user);
            var role = await _userManager.GetRolesAsync(user);
            userModel.Role = Enum.Parse(typeof(Repositories.Enums.Role), role[0]).ToString()!;

            return new ResponseDataModel<AccountModel>()
            {
                Status = true,
                Message = "Get account successfully",
                Data = userModel
            };
        }

        public async Task<Pagination<AccountModel>> GetAllAccounts(AccountFilterModel accountFilterModel)
        {
            var accountList = await _unitOfWork.AccountRepository.GetAllAsync(pageIndex: accountFilterModel.PageIndex,
                pageSize: accountFilterModel.PageSize,
                filter: (x =>
                    x.IsDeleted == accountFilterModel.IsDeleted &&
                    (accountFilterModel.IsSubscriptionActive == null ||
                     x.IsSubscriptionActive == accountFilterModel.IsSubscriptionActive) &&
                    (accountFilterModel.EmailConfirmed == null ||
                     x.EmailConfirmed == accountFilterModel.EmailConfirmed) &&
                    (accountFilterModel.Gender == null || x.Gender == accountFilterModel.Gender) &&
                    (accountFilterModel.BMI == null || x.BMI == accountFilterModel.BMI) &&
                    (accountFilterModel.FrequencyOfActivity == null ||
                     x.FrequencyOfActivity == accountFilterModel.FrequencyOfActivity) &&
                    (accountFilterModel.FrequencyOfStress == null ||
                     x.FrequencyOfStress == accountFilterModel.FrequencyOfStress) &&
                    (accountFilterModel.Diet == null || x.Diet == accountFilterModel.Diet) &&
                    (accountFilterModel.Level == null || x.Level == accountFilterModel.Level) &&
                    (accountFilterModel.Role == null || x.Role == accountFilterModel.Role.ToString()) &&
                    (string.IsNullOrEmpty(accountFilterModel.Search) ||
                     x.FirstName!.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.LastName!.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.Description.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.Education.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.ClinicAddress.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.Workplace.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.Email!.ToLower().Contains(accountFilterModel.Search.ToLower()))),
                orderBy: (x =>
                {
                    switch (accountFilterModel.Order.ToLower())
                    {
                        case "firstname":
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.FirstName)
                                : x.OrderBy(x => x.FirstName);
                        case "lastname":
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.LastName)
                                : x.OrderBy(x => x.LastName);
                        case "dateofbirth":
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.DateOfBirth)
                                : x.OrderBy(x => x.DateOfBirth);
                        default:
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                })
            );

            var accountModelList = _mapper.Map<List<AccountModel>>(accountList.Data);
            return new Pagination<AccountModel>(accountModelList, accountFilterModel.PageIndex,
                accountFilterModel.PageSize, accountList.TotalCount);
        }

        public async Task<ResponseModel> UpdateAccountUser(Guid id, AccountUserUpdateModel accountUserUpdateModel)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            user.FirstName = accountUserUpdateModel.FirstName;
            user.LastName = accountUserUpdateModel.LastName;
            // user.Gender = accountUpdateModel.Gender;
            user.DateOfBirth = accountUserUpdateModel.DateOfBirth;
            // user.Address = accountUpdateModel.Address;
            user.Image = accountUserUpdateModel.Image;
            // user.PhoneNumber = accountUpdateModel.PhoneNumber;
            user.ModificationDate = DateTime.Now;
            user.ModifiedBy = _claimsService.GetCurrentUserId;
            user.Height = accountUserUpdateModel.Height;
            user.InitialWeight = accountUserUpdateModel.InitialWeight;
            user.Diet = accountUserUpdateModel.Diet;
            user.FrequencyOfActivity = accountUserUpdateModel.FrequencyOfActivity;
            user.FrequencyOfStress = accountUserUpdateModel.FrequencyOfStress;
            user.BabyName = accountUserUpdateModel.BabyName;
            user.BabyGender = accountUserUpdateModel.BabyGender;
            user.DueDate = accountUserUpdateModel.DueDate;
            user.BMI = CalculateBMI(user.Height, user.InitialWeight);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Update account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot update account",
            };
        }

        private BMI? CalculateBMI(double? height, double? weight)
        {
            if (height != null && weight != null)
            {
                double? heightInMeters = height / 100.0; // Convert height from cm to meters
                double? bmi = weight / (heightInMeters * heightInMeters);

                if (bmi < 18.5)
                {
                    return BMI.Underweight;
                }

                if (bmi is >= 18.5 and < 24.9)
                {
                    return BMI.NormalWeight;
                }

                if (bmi >= 25)
                {
                    return BMI.Overweight;
                }
            }

            return null;
        }

        public async Task<ResponseModel> UpdateAccountExpert(Guid id, AccountExpertUpdateModel accountExpertUpdateModel)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            user.FirstName = accountExpertUpdateModel.FirstName;
            user.LastName = accountExpertUpdateModel.LastName;
            // user.Gender = accountUpdateModel.Gender;
            user.DateOfBirth = accountExpertUpdateModel.DateOfBirth;
            // user.Address = accountUpdateModel.Address;
            user.Image = accountExpertUpdateModel.Image;
            // user.PhoneNumber = accountUpdateModel.PhoneNumber;
            user.ModificationDate = DateTime.Now;
            user.ModifiedBy = _claimsService.GetCurrentUserId;
            user.ClinicAddress = accountExpertUpdateModel.ClinicAddress;
            user.Description = accountExpertUpdateModel.Description;
            user.Education = accountExpertUpdateModel.Education;
            user.YearsOfExperience = accountExpertUpdateModel.YearsOfExperience;
            user.Workplace = accountExpertUpdateModel.Workplace;
            user.Level = accountExpertUpdateModel.Level;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Update account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot update account",
            };
        }

        public async Task<ResponseModel> UpdateAccount(Guid id, AccountUpdateModel accountUpdateModel)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            user.FirstName = accountUpdateModel.FirstName;
            user.LastName = accountUpdateModel.LastName;
            // user.Gender = accountUpdateModel.Gender;
            user.DateOfBirth = accountUpdateModel.DateOfBirth;
            // user.Address = accountUpdateModel.Address;
            user.Image = accountUpdateModel.Image;
            // user.PhoneNumber = accountUpdateModel.PhoneNumber;
            user.ModificationDate = DateTime.Now;
            user.ModifiedBy = _claimsService.GetCurrentUserId;

            user.ClinicAddress = accountUpdateModel.ClinicAddress;
            user.Description = accountUpdateModel.Description;
            user.Education = accountUpdateModel.Education;
            user.YearsOfExperience = accountUpdateModel.YearsOfExperience;
            user.Workplace = accountUpdateModel.Workplace;
            user.Level = accountUpdateModel.Level;

            user.Height = accountUpdateModel.Height;
            user.InitialWeight = accountUpdateModel.InitialWeight;
            user.Diet = accountUpdateModel.Diet;
            user.FrequencyOfActivity = accountUpdateModel.FrequencyOfActivity;
            user.FrequencyOfStress = accountUpdateModel.FrequencyOfStress;
            user.BabyName = accountUpdateModel.BabyName;
            user.BabyGender = accountUpdateModel.BabyGender;
            user.DueDate = accountUpdateModel.DueDate;
            user.BMI = CalculateBMI(user.Height, user.InitialWeight);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Update account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot update account",
            };
        }

        public async Task<ResponseModel> DeleteAccount(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            user.IsDeleted = true;
            user.DeletionDate = DateTime.Now;
            user.DeletedBy = _claimsService.GetCurrentUserId;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Delete account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot delete account",
            };
        }

        public async Task<ResponseModel> RestoreAccount(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            user.IsDeleted = false;
            user.DeletionDate = null;
            user.DeletedBy = null;
            user.ModificationDate = DateTime.UtcNow;
            user.ModifiedBy = _claimsService.GetCurrentUserId;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Restore account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot restore account",
            };
        }

        public async Task<ResponseModel> CheckPassword(Guid id, AccountCheckPasswordModel accountCheckPasswordModel)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            if (await _userManager.CheckPasswordAsync(user, accountCheckPasswordModel.Password))
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Password is correct"
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Password is wrong"
            };
        }

        public async Task<ResponseModel> ExpertCreatePassword(AccountExpertCreatePassword accountExpertCreatePassword)
        {
            var user = await _userManager.FindByEmailAsync(accountExpertCreatePassword.Email);

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            if (user.VerificationCode != accountExpertCreatePassword.VerificationCode)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Invalid verification code"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, accountExpertCreatePassword.Password);
            
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Create password successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot create password",
            };
        }
    }
}