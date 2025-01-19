namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/wishlist")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("{userId:int}")]
        public async Task<ResponseObject> FindById(int userId)
        {
            return await _wishlistService.GetByIdAsync(userId);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] WishlistModel model)
        {
            var user = HttpContext.User;
            model.CreateBy = TokenHelper.GetCurrentUsername(user);
            return await _wishlistService.SaveAsync(model);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _wishlistService.DeleteAsync(id);
        }
    }
}
