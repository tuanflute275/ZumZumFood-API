namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId:int}")]
        public async Task<ResponseObject> FindById(int userId)
        {
            return await _cartService.GetByIdAsync(userId);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] CartModel model)
        {
            return await _cartService.SaveAsync(model);
        }

        [HttpPut("{id}/{type}")]
        public async Task<ResponseObject> Update(int id, string type)
        {
            return await _cartService.UpdateAsync(id, type);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _cartService.DeleteAsync(id);
        }
    }
}
