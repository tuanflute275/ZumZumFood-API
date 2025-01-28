using ZumZumFood.Application.Models.Queries.Components;

namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("search")]
        public async Task<ResponseObject> FindAll(CategoryQuery categoryQuery)
        {
            return await _categoryService.GetAllPaginationAsync(categoryQuery);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromForm] CategoryModel model)
        {
            var user = HttpContext.User;
            model.CreateBy = TokenHelper.GetCurrentUsername(user);
            return await _categoryService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromForm] CategoryModel model)
        {
            var user = HttpContext.User;
            model.UpdateBy = TokenHelper.GetCurrentUsername(user);
            return await _categoryService.UpdateAsync(id, model);
        }

        [HttpPost("soft-delete/{id}")]
        public async Task<ResponseObject> SoftDelete(int id)
        {
            var user = HttpContext.User;
            string deleteBy = TokenHelper.GetCurrentUsername(user);
            return await _categoryService.DeleteFlagAsync(id, deleteBy);
        }

        [HttpGet("deleted-data")]
        public async Task<ResponseObject> GetDeletedUsers()
        {
            return await _categoryService.GetDeletedListAsync();
        }

        [HttpPost("restore/{id}")]
        public async Task<ResponseObject> RestoreUser(int id)
        {
            return await _categoryService.RestoreAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _categoryService.DeleteAsync(id);
        }
    }
}
