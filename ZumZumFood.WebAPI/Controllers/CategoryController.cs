using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZumZumFood.Application.Abstracts;
using ZumZumFood.Application.Models.Request;
using ZumZumFood.Application.Models.Response;

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

        [HttpGet]
        public async Task<ResponseObject> FindAll(string? keyword, string? sort, int page = 1)
        {
            return await _categoryService.GetAllPaginationAsync(keyword, sort, page);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromForm] CategoryRequestModel model)
        {
            return await _categoryService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromForm] CategoryRequestModel model)
        {
            return await _categoryService.UpdateAsync(id, model);
        }

        [HttpPost("soft-delete/{id}")]
        public async Task<ResponseObject> SoftDelete(int id)
        {
            return await _categoryService.DeleteFlagAsync(id);
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

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _categoryService.DeleteAsync(id);
        }
    }
}
