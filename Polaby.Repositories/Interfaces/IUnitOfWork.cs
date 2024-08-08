namespace Polaby.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		AppDbContext DbContext { get; }
		IAccountRepository AccountRepository { get; }
		ICommuntityPostRepository CommuntityPostRepository { get; }
		IMenuRepository MenuRepository { get; }
		IMenuMealRepository MenuMealRepository { get; }
		IMealRepository MealRepository { get; }
		IMealDishRepository MealDishRepository {get; }
		IDishRepository DishRepository { get; }
		IDishIngredientRepository DishIngredientRepository { get; }
		IIngredientRepository IngredientRepository { get; }
		INutrientRepository NutrientRepository { get; }
        public Task<int> SaveChangeAsync();
	}
}
