using ZumZumFood.Application.Utils.Helpers.Token;

namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ResponseObject> FindAll(string? keyword, string? sort, int page = 1)
        {
            return await _productService.GetAllPaginationAsync(keyword, sort, page);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _productService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromForm] ProductModel model)
        {
            var user = HttpContext.User;
            model.CreateBy = TokenHelper.GetCurrentUsername(user);
            return await _productService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromForm] ProductModel model)
        {
            var user = HttpContext.User;
            model.UpdateBy = TokenHelper.GetCurrentUsername(user);
            return await _productService.UpdateAsync(id, model);
        }

        [HttpPost("soft-delete/{id}")]
        public async Task<ResponseObject> SoftDelete(int id)
        {
            var user = HttpContext.User;
            string deleteBy = TokenHelper.GetCurrentUsername(user);
            return await _productService.DeleteFlagAsync(id, deleteBy);
        }

        [HttpGet("deleted-data")]
        public async Task<ResponseObject> GetDeletedUsers()
        {
            return await _productService.GetDeletedListAsync();
        }

        [HttpPost("restore/{id}")]
        public async Task<ResponseObject> RestoreUser(int id)
        {
            return await _productService.RestoreAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _productService.DeleteAsync(id);
        }
    }
}
