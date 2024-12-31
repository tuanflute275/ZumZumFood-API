using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZumZumFood.Application.Abstracts;
using ZumZumFood.Application.Models.RequestModel;
using ZumZumFood.Application.Models.Response;

namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ResponseObject> FindAll(string? keyword, string? sort, int page = 1)
        {
            return await _userService.GetAllPaginationAsync(keyword, sort, page);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _userService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromForm] UserRequestModel model)
        {
           return await _userService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromForm] UserRequestModel model)
        {
            return await _userService.UpdateAsync(id, model);
        }

        [HttpPost("soft-delete/{id}")]
        public async Task<ResponseObject> SoftDelete(int id)
        {
            return await _userService.DeleteFlagAsync(id);
        }

        [HttpGet("deleted-users")]
        public async Task<ResponseObject> GetDeletedUsers()
        {
            return await _userService.GetDeletedListAsync();
        }

        [HttpPost("restore/{id}")]
        public async Task<ResponseObject> RestoreUser(int id)
        {
            return await _userService.RestoreAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
           return await _userService.DeleteAsync(id);
        }
    }
}
