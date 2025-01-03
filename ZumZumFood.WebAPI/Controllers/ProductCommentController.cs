namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/product-comment")]
    public class ProductCommentController : ControllerBase
    {
        private readonly IProductCommentService _productCommentService;
        public ProductCommentController(IProductCommentService productCommentService)
        {
            _productCommentService = productCommentService;
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseObject> FindById(int id)
        {
            return await _productCommentService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] ProductCommentRequestModel model)
        {
            return await _productCommentService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromBody] ProductCommentUpdateRequestModel model)
        {
            return await _productCommentService.UpdateAsync(id, model);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _productCommentService.DeleteAsync(id);
        }
    }
}
