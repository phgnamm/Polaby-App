﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Common;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Repositories.Models.QueryModels;

namespace Polaby.Repositories.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<QueryResultModel<List<AccountModel>>> GetAllAsync(
            Expression<Func<AccountModel, bool>>? filter = null,
            Func<IQueryable<AccountModel>, IOrderedQueryable<AccountModel>>? orderBy = null,
            string include = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<AccountModel> query = _dbContext.Users
                .Join(
                    _dbContext.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { user, userRole }
                )
                .Join(
                    _dbContext.Roles,
                    userRolePair => userRolePair.userRole.RoleId,
                    role => role.Id,
                    (userRolePair, role) => new AccountModel()
                    {
                        Id = userRolePair.user.Id,
                        FirstName = userRolePair.user.FirstName,
                        LastName = userRolePair.user.LastName,
                        Gender = userRolePair.user.Gender,
                        DateOfBirth = userRolePair.user.DateOfBirth,
                        // Address = userRolePair.user.Address,
                        Image = userRolePair.user.Image,
                        Email = userRolePair.user.Email,
                        EmailConfirmed = userRolePair.user.EmailConfirmed,
                        // PhoneNumber = userRolePair.user.PhoneNumber,
                        Role = role.Name!,
                        Height = userRolePair.user.Height,
                        InitialWeight = userRolePair.user.InitialWeight,
                        Diet = userRolePair.user.Diet,
                        FrequencyOfActivity = userRolePair.user.FrequencyOfActivity,
                        FrequencyOfStress = userRolePair.user.FrequencyOfStress,
                        BabyName = userRolePair.user.BabyName,
                        BabyGender = userRolePair.user.BabyGender,
                        DueDate = userRolePair.user.DueDate,
                        BMI = userRolePair.user.BMI,
                        ClinicAddress = userRolePair.user.ClinicAddress,
                        Description = userRolePair.user.Description,
                        Education = userRolePair.user.Education,
                        YearsOfExperience = userRolePair.user.YearsOfExperience,
                        Workplace = userRolePair.user.Workplace,
                        Level = userRolePair.user.Level,
                        SubscriptionStartDate = userRolePair.user.SubscriptionStartDate,
                        SubscriptionEndDate = userRolePair.user.SubscriptionEndDate,
                        IsSubscriptionActive = userRolePair.user.IsSubscriptionActive,
                        CreationDate = userRolePair.user.CreationDate,
                        CreatedBy = userRolePair.user.CreatedBy,
                        ModificationDate = userRolePair.user.ModificationDate,
                        ModifiedBy = userRolePair.user.ModifiedBy,
                        DeletionDate = userRolePair.user.DeletionDate,
                        DeletedBy = userRolePair.user.DeletedBy,
                        IsDeleted = userRolePair.user.IsDeleted,
                    }
                );

            if (filter != null)
            {
                query = query.Where(filter);
            }

            int totalCount = await query.CountAsync();

            foreach (var includeProperty in include.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize =
                    pageSize.Value > 0
                        ? pageSize.Value
                        : PaginationConstant.DEFAULT_MIN_PAGE_SIZE;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return new QueryResultModel<List<AccountModel>>()
            {
                TotalCount = totalCount,
                Data = await query.ToListAsync(),
            };
        }

        public async Task<Account> GetAccountById(Guid id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}