﻿namespace ZumZumFood.WebAPI.Controllers
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

        [HttpPost("search")]
        public async Task<ResponseObject> FindAll(UserQuery userQuery)
        {
            return await _userService.GetAllPaginationAsync(userQuery);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _userService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromForm] UserModel model)
        {
            var user = HttpContext.User;
            model.CreateBy = TokenHelper.GetCurrentUsername(user);
            return await _userService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromForm] UserModel model)
        {
            var user = HttpContext.User;
            model.CreateBy = TokenHelper.GetCurrentUsername(user);
            return await _userService.UpdateAsync(id, model);
        }

        [HttpPost("soft-delete/{id}")]
        public async Task<ResponseObject> SoftDelete(int id)
        {
            var user = HttpContext.User;
            string deleteBy = TokenHelper.GetCurrentUsername(user);
            return await _userService.DeleteFlagAsync(id, deleteBy);
        }

        [HttpGet("deleted-data")]
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
