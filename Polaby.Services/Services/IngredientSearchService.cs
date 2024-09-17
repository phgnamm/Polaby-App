
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.IngredientSearchModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.IngredientSearchModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class IngredientSearchService : IIngredientSearchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public IngredientSearchService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<ResponseModel> AddIngredientSearchWithNutrients(IngredientSearchtImportModel ingredientImport)
        {
            if (ingredientImport == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "No ingredient provided!"
                };
            }

            var ingredientSearch = _mapper.Map<IngredientSearch>(ingredientImport);
            await _unitOfWork.IngredientSearchRepository.AddAsync(ingredientSearch);
            await _unitOfWork.SaveChangeAsync();

            var nutrientsToAdd = ingredientImport.Nutrients?
                .Select(nutrientModel => _mapper.Map<Nutrient>(nutrientModel))
                .ToList();

            if (nutrientsToAdd != null && nutrientsToAdd.Any())
            {
                await _unitOfWork.NutrientRepository.AddRangeAsync(nutrientsToAdd);
                await _unitOfWork.SaveChangeAsync();
            }

            int count = nutrientsToAdd.Count;
            var addedNutrients = await _unitOfWork.NutrientRepository
                 .GetAll()
                 .OrderByDescending(n => n.CreationDate)
                 .Take(count)
                 .ToListAsync();

            var ingredientSearchNutrients = addedNutrients
                .Select(nutrient => new IngredientSearchNutrient
                {
                    IngredientSearchId = ingredientSearch.Id,
                    NutrientId = nutrient.Id
                }).ToList();

            if (ingredientSearchNutrients.Any())
            {
                await _unitOfWork.IngredientSearchNutrientRepository.AddRangeAsync(ingredientSearchNutrients);
            }

            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Status = true,
                Message = "Ingredient created successfully with associated Nutrients and links"
            };
        }

        public async Task<Pagination<IngredientSearchModel>> GetAllIngredient(IngredientSearchFilterModel ingredientFilterModel)
        {
            var ingredientList = await _unitOfWork.IngredientSearchRepository.GetAllAsync(
                pageIndex: ingredientFilterModel.PageIndex,
                pageSize: ingredientFilterModel.PageSize,
                filter: x =>
                        x.IsDeleted == ingredientFilterModel.IsDeleted &&
                        (ingredientFilterModel.FoodGroup == null ||
                        x.FoodGroup.Equals(ingredientFilterModel.FoodGroup.Value.ToFriendlyString())) &&
                        (string.IsNullOrEmpty(ingredientFilterModel.Search) ||
                        x.Name.ToLower().Contains(ingredientFilterModel.Search.ToLower()) ||
                        x.Kcal.ToString().Contains(ingredientFilterModel.Search) ||
                        x.Protein.ToString().Contains(ingredientFilterModel.Search) ||
                        x.Water.ToString().Contains(ingredientFilterModel.Search) ||
                        x.Fat.ToString().Contains(ingredientFilterModel.Search) ||
                        x.Carbohydrates.ToString().Contains(ingredientFilterModel.Search)),
                orderBy: x =>
                {
                    switch (ingredientFilterModel.Order.ToLower())
                    {
                        case "name":
                            return ingredientFilterModel.OrderByDescending
                                ? x.OrderByDescending(i => i.Name)
                                : x.OrderBy(i => i.Name);
                        case "kcal":
                            return ingredientFilterModel.OrderByDescending
                                ? x.OrderByDescending(i => i.Kcal)
                                : x.OrderBy(i => i.Kcal);
                        default:
                            return ingredientFilterModel.OrderByDescending
                                ? x.OrderByDescending(i => i.CreationDate)
                                : x.OrderBy(i => i.CreationDate);
                    }
                },
                include: "IngredientSearchNutrients,IngredientSearchNutrients.Nutrient"
            );

            if (ingredientList != null)
            {
                var ingredientModelList = _mapper.Map<List<IngredientSearchModel>>(ingredientList.Data);
                return new Pagination<IngredientSearchModel>(
                    ingredientModelList,
                    ingredientFilterModel.PageIndex,
                    ingredientFilterModel.PageSize, ingredientList.TotalCount
                );
            }

            return null;
        }


        public async Task<ResponseModel> UpdateIngredient(Guid id, IngredientSearchUpdateModel updateModel)
        {
            var existingIngredient = await _unitOfWork.IngredientSearchRepository.GetAsync(id);
            if (existingIngredient == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Ingredient not found!"
                };
            }

            _mapper.Map(updateModel, existingIngredient);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel()
            {
                Status = true,
                Message = "Ingredient updated successfully"
            };
        }

        public async Task<ResponseModel> DeleteIngredient(Guid id)
        {
            var existingIngredient = await _unitOfWork.IngredientSearchRepository.GetAsync(id, "IngredientSearchNutrients");
            if (existingIngredient == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Ingredient not found!"
                };
            }

            var relatedNutrientIds = existingIngredient.IngredientSearchNutrients.Select(x => x.NutrientId).Distinct().ToList();

            var nutrientQueryResult = await _unitOfWork.NutrientRepository.GetAllAsync(
                filter: n => relatedNutrientIds.Contains(n.Id)
            );
            var allNutrients = nutrientQueryResult.Data;

            _unitOfWork.IngredientSearchNutrientRepository.HardDeleteRange(existingIngredient.IngredientSearchNutrients.ToList());
            if (allNutrients.Any())
            {
                _unitOfWork.NutrientRepository.HardDeleteRange(allNutrients);
            }
            _unitOfWork.IngredientSearchRepository.HardDelete(existingIngredient);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Ingredient and its related nutrients deleted successfully"
            };
        }



        public async Task<ResponseDataModel<IngredientSearchModel>> GetById(Guid id)
        {
            var ingredient = await _unitOfWork.IngredientSearchRepository.GetById(id);

            if (ingredient == null)
            {
                return new ResponseDataModel<IngredientSearchModel>()
                {
                    Status = false,
                    Message = "Ingredient not found"
                };
            }

            var ingredientModel = _mapper.Map<IngredientSearchModel>(ingredient);

            return new ResponseDataModel<IngredientSearchModel>()
            {
                Status = true,
                Message = "Get ingredient successfully",
                Data = ingredientModel
            };
        }
    }
}

