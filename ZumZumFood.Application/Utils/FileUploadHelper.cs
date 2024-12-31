using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using Image = SixLabors.ImageSharp.Image;

namespace ZumZumFood.Application.Utils
{
    public static class FileUploadHelper
    {
        public static async Task<string> UploadImageAsync(IFormFile imageFile, string oldImage,
            string requestScheme, string requestHost, string folderName)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5MB

            if (imageFile != null && imageFile.Length > 0)
            {
                // Kiểm tra kích thước tệp
                if (imageFile.Length > maxFileSize)
                {
                    throw new ArgumentException("File size exceeds the 5MB limit.");
                }

                // Kiểm tra loại file (ví dụ: chỉ cho phép hình ảnh JPEG, PNG, GIF)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException("Invalid file type. Allowed types are: jpg, jpeg, png, gif.");
                }

                // Đường dẫn lưu file (dùng tên thư mục dynamic từ folderName)
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Xóa hình ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(oldImage))
                {
                    var oldFileName = oldImage.Split($"{requestScheme}://{requestHost}/uploads/{folderName}/").LastOrDefault();
                    if (!string.IsNullOrEmpty(oldFileName))
                    {
                        var oldFilePath = Path.Combine(uploadsFolder, oldFileName);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }
                }

                // Tạo tên file duy nhất để tránh trùng lặp
                var uniqueFileName = Guid.NewGuid().ToString() + ".webp";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    // nén hình ảnh với định dạng webp
                    using (var image = Image.Load(imageFile.OpenReadStream()))
                    {
                        // Giảm chất lượng nếu cần
                        var encoder = new WebpEncoder()
                        {
                            Quality = 75 // Bạn có thể điều chỉnh mức độ nén ở đây (0-100)
                        };

                        // Chuyển đổi hình ảnh thành WebP và lưu vào đĩa
                        await image.SaveAsync(filePath, encoder);
                    }

                    // Trả về đường dẫn URL của hình ảnh mới
                    var newImageUrl = $"{requestScheme}://{requestHost}/uploads/{folderName}/{uniqueFileName}";

                    return newImageUrl;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error uploading file: {ex.Message}");
                }
            }
            else
            {
                return oldImage;
            }
        }
    }
}
