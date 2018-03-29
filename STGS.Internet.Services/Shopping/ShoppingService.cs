using STGS.Internet.Repositories.IRepository;

namespace STGS.Internet.Services.Shopping
{
    public class ShoppingService : IShoppingService
    {
        protected IShoppingRepository ShoppingRepository { get; set; }

        public ShoppingService(IShoppingRepository shoppingRepository)
        {
            ShoppingRepository = shoppingRepository;
        }
    }
}
