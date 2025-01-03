namespace ZumZumFood.Application.Services
{
    public class WishlistService : IWishlistService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<WishlistService> _logger;
        public WishlistService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<WishlistService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseObject> GetByIdAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/wishlist/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var dataQuery = await _unitOfWork.WishlistRepository.GetAllAsync(
                   expression: x => x.UserId == id ,
                   include: query => query.Include(x => x.Product).Include(x => x.User)
                );
                var wishlist = dataQuery.FirstOrDefault();
                if (wishlist == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/wishlist/{id}", null, wishlist);
                    return new ResponseObject(404, "Wishlist not found.", wishlist);
                }
                var result = new WishlistDTO
                {
                    WishlistId = wishlist.WishlistId,
                   Users = new UserMapperDTO
                   {
                       UserId = wishlist.UserId,
                       UserName = wishlist.User.UserName,
                       FullName = wishlist.User.FullName,
                       Email = wishlist.User.Email,
                       Avatar = wishlist.User.Avatar,
                       Address = wishlist.User.Address,
                       Gender = wishlist.User.Gender,
                       PlaceOfBirth = wishlist.User.PlaceOfBirth,
                       PhoneNumber = wishlist.User.PhoneNumber,
                       DateOfBirth = wishlist.User.DateOfBirth,
                       Nationality = wishlist.User.Nationality,
                   }
                };
                if (result == null)
                {
                    LogHelper.LogWarning(_logger, "GET", "/api/wishlist/{id}", null, result);
                    return new ResponseObject(404, "Wishlist not found.", result);
                }
                LogHelper.LogInformation(_logger, "GET", "/api/wishlist/{id}", null, result);
                return new ResponseObject(200, "Query data successfully", result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "GET", $"/api/wishlist/{id}");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        public Task<ResponseObject> SaveAsync(WishlistModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseObject> UpdateAsync(int id, WishlistModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseObject> DeleteAsync(int id)
        {
            try
            {
                // validate invalid special characters
                var validationResult = InputValidator.IsValidNumber(id);
                if (!validationResult)
                {
                    LogHelper.LogWarning(_logger, "GET", $"/api/wishlist/{id}", null, "Invalid ID. ID must be greater than 0.");
                    return new ResponseObject(400, "Input invalid", "Invalid ID. ID must be greater than 0 and less than or equal to the maximum value of int!.");
                }
                var wishlist = await _unitOfWork.WishlistRepository.GetByIdAsync(id);
                if (wishlist == null)
                {
                    LogHelper.LogWarning(_logger, "DELETE", $"/api/wishlist/{id}", id, "Wishlist not found.");
                    return new ResponseObject(404, "Wishlist not found.", null);
                }

                await _unitOfWork.WishlistRepository.DeleteAsync(wishlist);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "DELETE", $"/api/wishlist/{id}", id, "Deleted successfully");
                return new ResponseObject(200, "Delete data successfully", null);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "DELETE", $"/api/wishlist/{id}", id);
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }
    }
}
