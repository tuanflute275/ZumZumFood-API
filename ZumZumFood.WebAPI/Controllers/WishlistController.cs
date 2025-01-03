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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] WishlistModel model)
        {
            return await _wishlistService.SaveAsync(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromBody] WishlistModel model)
        {
            return await _wishlistService.UpdateAsync(id, model);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _wishlistService.DeleteAsync(id);
        }
    }
}
