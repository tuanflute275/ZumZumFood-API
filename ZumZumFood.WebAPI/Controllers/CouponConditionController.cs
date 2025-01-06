namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/coupon-condition")]
    public class CouponConditionController : ControllerBase
    {
        private readonly ICouponConditionService _couponConditionService;
        public CouponConditionController(ICouponConditionService couponConditionService)
        {
            _couponConditionService = couponConditionService;
        }

        [HttpGet("{couponId:int}")]
        public async Task<ResponseObject> FindById(int couponId)
        {
            return await _couponConditionService.GetByIdAsync(couponId);
        }

        [HttpPost]
        public async Task<ResponseObject> Save([FromBody] CouponConditionModel model)
        {
            return await _couponConditionService.SaveAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<ResponseObject> Update(int id, [FromBody] CouponConditionModel model)
        {
            return await _couponConditionService.UpdateAsync(id, model);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ResponseObject> Delete(int id)
        {
            return await _couponConditionService.DeleteAsync(id);
        }
    }
}
