using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZumZumFood.Application.Abstracts;
using ZumZumFood.Application.Models.Request;
using ZumZumFood.Application.Models.Response;

namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
           _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ResponseObject> FindAll(string? keyword, string? sort, int page = 1)
        {
            return await _restaurantService.GetAllPaginationAsync(keyword, sort, page);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _restaurantService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] RestaurantRequestModel model)
        {
            return await _restaurantService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromBody] RestaurantRequestModel model)
        {
            return await _restaurantService.UpdateAsync(id, model);
        }

        [HttpPost("soft-delete/{id}")]
        public async Task<ResponseObject> SoftDelete(int id)
        {
            return await _restaurantService.DeleteFlagAsync(id);
        }

        [HttpGet("deleted-data")]
        public async Task<ResponseObject> GetDeletedUsers()
        {
            return await _restaurantService.GetDeletedListAsync();
        }

        [HttpPost("restore/{id}")]
        public async Task<ResponseObject> RestoreUser(int id)
        {
            return await _restaurantService.RestoreAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _restaurantService.DeleteAsync(id);
        }
    }
}
