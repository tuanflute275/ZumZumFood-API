namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ResponseObject> FindAll(string? keyword, string? sort, int page = 1)
        {
            return await _orderService.GetAllPaginationAsync(keyword, sort, page);
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _orderService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] OrderModel model)
        {
            return await _orderService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromBody] OrderUpdateModel model)
        {
            return await _orderService.UpdateAsync(id, model);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _orderService.DeleteAsync(id);
        }
    }
}
