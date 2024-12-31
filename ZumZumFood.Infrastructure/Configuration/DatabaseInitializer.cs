using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Infrastructure.Configuration
{
    public static class DatabaseInitializer
    {
        public static async Task AutoMigration(this WebApplication webApplication)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Áp dụng migration
                await appContext.Database.MigrateAsync();

                // Khởi tạo dữ liệu nếu cần
                await SeedData(webApplication);  // Gọi SeedData và truyền WebApplication
            }
        }

        public static async Task SeedData(this WebApplication webApplication)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // Khởi tạo dữ liệu cho bảng Users
                if (!appContext.Users.Any())
                {
                    appContext.Users.AddRange(
                        new User
                        {
                            UserName = "admin",
                            FullName = "admin",
                            Email = "admin@gmail.com",
                            Password = "$2a$12$eefyE/f6G0AKFLfVl3B66.T6QfgGNHPIZLlDp.v527EuwVruYlTye", // Example hashed password
                            PhoneNumber = "0123456789",
                            Address = "HN",
                            Gender = 1,
                            Active = 1,
                            Avatar = ""
                        },
                       new User
                       {
                           UserName = "restaurantOwner",
                           FullName = "Restaurant Owner",
                           Email = "restaurantowner@example.com",
                           Password = "$2a$12$eefyE/f6G0AKFLfVl3B66.T6QfgGNHPIZLlDp.v527EuwVruYlTye", // Example hashed password
                           PhoneNumber = "0123456789",
                           Address = "Restaurant Address",
                           Gender = 1,
                           Active = 1, // Active user
                           Avatar = "restaurant-owner-avatar.png",
                       },
                        new User
                        {
                            UserName = "restaurantStaff",
                            FullName = "Restaurant Staff",
                            Email = "restaurantstaff@example.com",
                            Password = "$2a$12$eefyE/f6G0AKFLfVl3B66.T6QfgGNHPIZLlDp.v527EuwVruYlTye", // Example hashed password
                            PhoneNumber = "0123456789",
                            Address = "Restaurant Staff Address",
                            Gender = 2,
                            Active = 1, // Active user
                            Avatar = "restaurant-staff-avatar.png",
                        },
                        new User
                        {
                            UserName = "deliveryDriver",
                            FullName = "Delivery Driver",
                            Email = "deliverydriver@example.com",
                            Password = "$2a$12$eefyE/f6G0AKFLfVl3B66.T6QfgGNHPIZLlDp.v527EuwVruYlTye", // Example hashed password
                            PhoneNumber = "0123456789",
                            Address = "Delivery Address",
                            Gender = 1,
                            Active = 1, // Active user
                            Avatar = "delivery-driver-avatar.png",
                        },
                        new User
                        {
                            UserName = "johndoe",
                            FullName = "John Doe",
                            Email = "johndoe@example.com",
                            Password = "$2a$12$eefyE/f6G0AKFLfVl3B66.T6QfgGNHPIZLlDp.v527EuwVruYlTye", // Example hashed password
                            PhoneNumber = "0987654321",
                            Address = "Customer Address 1",
                            Gender = 1,
                            Active = 1, // Active user
                            Avatar = "john-doe-avatar.png",
                        },
                        new User
                        {
                            UserName = "janesmith",
                            FullName = "Jane Smith",
                            Email = "janesmith@example.com",
                            Password = "$2a$12$eefyE/f6G0AKFLfVl3B66.T6QfgGNHPIZLlDp.v527EuwVruYlTye", // Example hashed password
                            PhoneNumber = "0987654322",
                            Address = "Customer Address 2",
                            Gender = 2,
                            Active = 0, // InActive user
                            Avatar = "jane-smith-avatar.png",
                        }
                    );
                    await appContext.SaveChangesAsync();
                }

                // Khởi tạo dữ liệu cho bảng Roles
                if (!appContext.Roles.Any())
                {
                    appContext.Roles.AddRange(
                        new Role
                        {
                            RoleName = "Admin",
                            RoleDescription = "Quản trị viên hệ thống, có quyền truy cập và quản lý tất cả các chức năng."
                        },
                        new Role
                        {
                            RoleName = "RestaurantOwner",
                            RoleDescription = "Chủ nhà hàng, có quyền quản lý nhà hàng của mình và các đơn hàng liên quan."
                        },
                        new Role
                        {
                            RoleName = "RestaurantStaff",
                            RoleDescription = "Nhân viên nhà hàng, có thể quản lý đơn hàng, thực đơn và khách hàng trong nhà hàng của mình."
                        },
                        new Role
                        {
                            RoleName = "DeliveryDriver",
                            RoleDescription = "Nhân viên giao hàng, có thể xem và giao các đơn hàng đã được giao cho họ."
                        },
                        new Role
                        {
                            RoleName = "User",
                            RoleDescription = "Người dùng cuối, có thể đặt món ăn, xem thực đơn và theo dõi đơn hàng của mình."
                        }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng UserRoles
                if (!appContext.UserRoles.Any())
                {
                    appContext.UserRoles.AddRange(
                        new UserRole
                        {
                            UserId = 1,
                            RoleId = 1,
                        },
                       new UserRole
                       {
                           UserId = 2,
                           RoleId = 2,
                       },
                       new UserRole
                       {
                           UserId = 3,
                           RoleId = 3,
                       },
                       new UserRole
                       {
                           UserId = 4,
                           RoleId = 4,
                       },
                       new UserRole
                       {
                           UserId = 5,
                           RoleId = 5,
                       },
                       new UserRole
                       {
                           UserId = 6,
                           RoleId = 5,
                       }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }
            }
        }
    }
}
