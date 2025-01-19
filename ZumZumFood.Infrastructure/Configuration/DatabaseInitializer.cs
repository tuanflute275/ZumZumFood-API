using Log = ZumZumFood.Domain.Entities.Log;
using Role = ZumZumFood.Domain.Entities.Role;
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

                // Khởi tạo dữ liệu cho bảng Parameters
                if (!appContext.Parameters.Any())
                {
                    appContext.Parameters.AddRange(
                        new Parameter
                        {
                            ParaScope = "SYSTEM",
                            ParaName = "PORT_SERVER",
                            ParaShortValue = "http://192.168.10.242:8280",
                            ParaLobValue = null,
                            ParaDesc = "Url port server",
                            ParaType = "STRING",
                            UserAccessibleFlag = true,
                            AdminAccessibleFlag = true
                        },
                        new Parameter
                        {
                            ParaScope = "SYSTEM",
                            ParaName = "MAIN_CONNECTION_DATA_CENTER",
                            ParaShortValue = "true",
                            ParaLobValue = "Kết nối đến TT Lưu trữ chính",
                            ParaDesc = null,
                            ParaType = "BOOLEAN",
                            UserAccessibleFlag = true,
                            AdminAccessibleFlag = true
                        },
                         new Parameter
                         {
                             ParaScope = "API",
                             ParaName = "WS_MAIN",
                             ParaShortValue = "true",
                             ParaLobValue = "192.168.1.222:8880/eppws/services/rest/perso",
                             ParaDesc = "Restful API",
                             ParaType = "BOOLEAN",
                             UserAccessibleFlag = true,
                             AdminAccessibleFlag = true
                         },
                          new Parameter
                          {
                              ParaScope = "APPLICATION",
                              ParaName = "PASSWORD_EXPIRY",
                              ParaShortValue = "60",
                              ParaLobValue = null,
                              ParaDesc = "Number to expire user login password",
                              ParaType = "NUMBER",
                              UserAccessibleFlag = true,
                              AdminAccessibleFlag = true
                          },
                           new Parameter
                           {
                               ParaScope = "WORKFLOW",
                               ParaName = "MAX_JOBQUEUE_COUNT",
                               ParaShortValue = "100",
                               ParaLobValue = null,
                               ParaDesc = "Max Jobqueue Count",
                               ParaType = "STRING",
                               UserAccessibleFlag = true,
                               AdminAccessibleFlag = true
                           }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Logs
                if (!appContext.Logs.Any())
                {
                    appContext.Logs.AddRange(
                         new Log
                         {
                             UserName = "User",
                             WorkTation = "DESKTOP-123",
                             Url = "/login",
                             Request = "{username: 'admin', password: '12345678'}",
                             Response = "login successfully",
                             IpAdress = "192.168.10.242",
                             TimeLogin = DateTime.Now,
                             TimeLogout = DateTime.Now,
                         },
                        new Log
                        {
                            UserName = "User",
                            WorkTation = "DESKTOP-123",
                            Url = "/home",
                            Request = "{keyword: ''}",
                            Response = "data demo",
                            IpAdress = "192.168.10.242",
                            TimeActionRequest = DateTime.Now
                        }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Categories
                if (!appContext.Categories.Any())
                {
                    appContext.Categories.AddRange(
                         new Category { Name = "Pizza", Slug = "pizza", IsActive = true, Description = "Delicious pizza with various toppings." },
                         new Category { Name = "Burger", Slug = "burger", IsActive = true, Description = "Juicy burgers with fresh ingredients." },
                         new Category { Name = "Pasta", Slug = "pasta", IsActive = true, Description = "Traditional and creamy pasta dishes." },
                         new Category { Name = "Sushi", Slug = "sushi", IsActive = true, Description = "Fresh sushi rolls with authentic flavors." },
                         new Category { Name = "Salad", Slug = "salad", IsActive = true, Description = "Healthy and fresh salads with a variety of toppings." },
                         new Category { Name = "Desserts", Slug = "desserts", IsActive = true, Description = "Sweet treats including cakes, ice cream, and more." },
                         new Category { Name = "Drinks", Slug = "drinks", IsActive = true, Description = "Refreshing drinks, from juices to sodas." },
                         new Category { Name = "Vegetarian", Slug = "vegetarian", IsActive = true, Description = "Tasty vegetarian dishes for all preferences." },
                         new Category { Name = "Fast Food", Slug = "fast-food", IsActive = true, Description = "Quick and delicious fast food options." },
                         new Category { Name = "Grill", Slug = "grill", IsActive = true, Description = "Grilled dishes with smoky flavors and tender meat." }
                     );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Restaurants
                if (!appContext.Brands.Any())
                {
                    appContext.Brands.AddRange(
                         new Brand { Name = "Pizza Hut", Slug = "pizza-hut", Address = "123 Pizza Street, Hanoi", PhoneNumber = "0901234567", Email = "contact@pizzahut.com", Description = "Famous pizza chain offering a wide variety of pizzas.", IsActive = true, OpenTime = TimeSpan.FromHours(10), CloseTime = TimeSpan.FromHours(22) },
                         new Brand { Name = "Burger King", Slug = "burger-king", Address = "456 Burger Lane, Hanoi", PhoneNumber = "0907654321", Email = "support@burgerking.com", Description = "Fast food chain known for its flame-grilled burgers.", IsActive = true, OpenTime = TimeSpan.FromHours(8), CloseTime = TimeSpan.FromHours(23) },
                         new Brand { Name = "Sushi World", Slug = "sushi-world", Address = "789 Sushi Boulevard, Hanoi", PhoneNumber = "0908765432", Email = "info@sushiworld.com", Description = "Japanese sushi restaurant with fresh seafood.", IsActive = true, OpenTime = TimeSpan.FromHours(11), CloseTime = TimeSpan.FromHours(21) },
                         new Brand { Name = "Pasta House", Slug = "pasta-house", Address = "321 Pasta Avenue, Hanoi", PhoneNumber = "0901236547", Email = "service@pastahouse.com", Description = "Authentic Italian pasta dishes served in a cozy environment.", IsActive = true, OpenTime = TimeSpan.FromHours(10), CloseTime = TimeSpan.FromHours(22) },
                         new Brand { Name = "Grill Master", Slug = "grill-master", Address = "654 Grill Road, Hanoi", PhoneNumber = "0908765430", Email = "contact@grillmaster.com", Description = "Grilled meats and vegetables served with sides.", IsActive = true, OpenTime = TimeSpan.FromHours(9), CloseTime = TimeSpan.FromHours(22) },
                         new Brand { Name = "Vegan Delights", Slug = "vegan-delights", Address = "987 Vegan Street, Hanoi", PhoneNumber = "0905555555", Email = "hello@vegandelights.com", Description = "Vegan restaurant with plant-based dishes and desserts.", IsActive = true, OpenTime = TimeSpan.FromHours(8), CloseTime = TimeSpan.FromHours(21) },
                         new Brand { Name = "Noodle Express", Slug = "noodle-express", Address = "543 Noodle Place, Hanoi", PhoneNumber = "0903333333", Email = "info@noodleexpress.com", Description = "Asian noodle bar offering quick and tasty meals.", IsActive = true, OpenTime = TimeSpan.FromHours(9), CloseTime = TimeSpan.FromHours(20) },
                         new Brand { Name = "Seafood Paradise", Slug = "seafood-paradise", Address = "678 Ocean Road, Hanoi", PhoneNumber = "0902222222", Email = "contact@seafoodparadise.com", Description = "Fresh seafood dishes served with traditional flavors.", IsActive = true, OpenTime = TimeSpan.FromHours(10), CloseTime = TimeSpan.FromHours(23) },
                         new Brand { Name = "Burger House", Slug = "burger-house", Address = "321 Burger Street, Hanoi", PhoneNumber = "0904444444", Email = "support@burgerhouse.com", Description = "Specialty burgers with a range of toppings and sauces.", IsActive = true, OpenTime = TimeSpan.FromHours(10), CloseTime = TimeSpan.FromHours(22) },
                         new Brand { Name = "Café Delight", Slug = "cafe-Delight", Address = "111 Café Road, Hanoi", PhoneNumber = "0909999999", Email = "info@cafedelight.com", Description = "A cozy café serving coffee, pastries, and light snacks.", IsActive = true, OpenTime = TimeSpan.FromHours(7), CloseTime = TimeSpan.FromHours(20) }
                     );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Products
                if (!appContext.Products.Any())
                {
                    appContext.Products.AddRange(
                        new Product { Name = "Pizza Margherita",
                            Slug = "pizza-margherita",
                            Price = 150,
                            Discount = 20,
                            IsActive = true,
                            Description = "Classic pizza with fresh tomatoes, mozzarella, and basil.",
                            BrandId = 1,
                            CategoryId = 1
                        },
                        new Product
                        {
                            Name = "Cheeseburger",
                            Slug = "cheeseburger",
                            Price = 100,
                            Discount = 10,
                            IsActive = true,
                            Description = "Juicy cheeseburger with fresh lettuce and tomato.",
                            BrandId = 2,
                            CategoryId = 2
                        },
                        new Product
                        {
                            Name = "Sushi Rolls",
                            Slug = "sushi-rolls",
                            Price = 200,
                            Discount = 0,
                            IsActive = true,
                            Description = "Fresh sushi rolls with tuna, salmon, and avocado.",
                            BrandId = 3,
                            CategoryId = 3
                        },
                        new Product
                        {
                            Name = "Spaghetti Carbonara",
                            Slug = "spaghetti-carbonara",
                            Price = 120,
                            Discount = 15,
                            IsActive = true,
                            Description = "Italian spaghetti in a creamy carbonara sauce.",
                            BrandId = 4,
                            CategoryId = 4
                        },
                        new Product
                        {
                            Name = "Grilled Steak",
                            Slug = "grilled-steak",
                            Price = 250,
                            Discount = 30,
                            IsActive = true,
                            Description = "Tender grilled steak served with a side of vegetables.",
                            BrandId = 5,
                            CategoryId = 5
                        },
                        new Product
                        {
                            Name = "Vegan Burger",
                            Slug = "vegan-burger",
                            Price = 90,
                            Discount = 5,
                            IsActive = true,
                            Description = "A tasty vegan burger with plant-based ingredients.",
                            BrandId = 6,
                            CategoryId = 2
                        },
                        new Product
                        {
                            Name = "Vegetable Stir Fry",
                            Slug = "vegetable-stir-fry",
                            Price = 80,
                            Discount = 10,
                            IsActive = true,
                            Description = "Stir-fried mixed vegetables with soy sauce.",
                            BrandId = 7,
                            CategoryId = 6
                        },
                        new Product
                        {
                            Name = "Seafood Paella",
                            Slug = "seafood-paella",
                            Price = 300,
                            Discount = 20,
                            IsActive = true,
                            Description = "A Spanish dish with rice, seafood, and saffron.",
                            BrandId = 8,
                            CategoryId = 3
                        },
                        new Product
                        {
                            Name = "Double Cheeseburger",
                            Slug = "double-cheeseburger",
                            Price = 150,
                            Discount = 10,
                            IsActive = true,
                            Description = "A double cheeseburger with extra cheese and bacon.",
                            BrandId = 9,
                            CategoryId = 2
                        },
                        new Product
                        {
                            Name = "Café Mocha",
                            Slug = "cafe-mocha",
                            Price = 50,
                            Discount = 0,
                            IsActive = true,
                            Description = "A rich and creamy café mocha with whipped cream.",
                            BrandId = 10,
                            CategoryId = 7
                        }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng ProductComments
                if (!appContext.ProductComments.Any())
                {
                    appContext.ProductComments.AddRange(
                    new ProductComment
                    {
                        Name = "alice",
                        Email = "alice.smith@example.com",
                        Message = "The pizza was absolutely delicious, best I've had in a long time!",
                        UserId = 2,
                        ProductId = 3
                    },
                    new ProductComment
                    {
                        Name = "johnson",
                        Email = "bob.johnson@example.com",
                        Message = "Great sushi, but the rolls were a bit too small.",
                        UserId = 3,
                        ProductId = 3
                    },
                    new ProductComment
                    {
                        Name = "carol",
                        Email = "carol.white@example.com",
                        Message = "The burger was tasty, but I expected a bit more flavor from the sauce.",
                        UserId = 4,
                        ProductId = 3
                    },
                    new ProductComment
                    {
                        Name = "david",
                        Email = "david.brown@example.com",
                        Message = "The pasta was amazing, and the sauce was perfect.",
                        UserId = 5,
                        ProductId = 4
                    },
                    new ProductComment
                    {
                        Name = "emma",
                        Email = "emma.jones@example.com",
                        Message = "Not the best steak I've had, but still quite good for the price.",
                        UserId = 6,
                        ProductId = 5
                    },
                    new ProductComment
                    {
                        Name = "frank",
                        Email = "frank.martin@example.com",
                        Message = "Delicious vegan burger! Very satisfying.",
                        UserId = 2,
                        ProductId = 6
                    },
                    new ProductComment
                    {
                        Name = "grace",
                        Email = "grace.taylor@example.com",
                        Message = "The fries were undercooked and soggy, but the main dish was fine.",
                        UserId = 3,
                        ProductId = 7
                    },
                    new ProductComment
                    {
                        Name = "henry",
                        Email = "henry.clark@example.com",
                        Message = "Loved the seafood paella, it was so flavorful and fresh!",
                        UserId = 4,
                        ProductId = 8
                    },
                    new ProductComment
                    {
                        Name = "irene",
                        Email = "irene.miller@example.com",
                        Message = "I was impressed with the quality of the ingredients in the stir fry.",
                        UserId = 5,
                        ProductId = 9
                    },
                    new ProductComment
                    {
                        Name = "jack",
                        Email = "jack.davis@example.com",
                        Message = "The coffee was great, but the mocha could use more chocolate.",
                        UserId = 6,
                        ProductId = 10
                    }
                );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Combo
                if (!appContext.Combos.Any())
                {
                    appContext.Combos.AddRange(
                        new Combo { Name = "Super Combo 1", Price = new Random().Next(20000, 100000), Description = "A special combo offering a great variety of products.", IsActive = true },
                        new Combo { Name = "Deluxe Combo 2", Price = new Random().Next(20000, 100000), Description = "Premium combo with high-end items.", IsActive = true },
                        new Combo { Name = "Ultimate Combo 3", Price = new Random().Next(20000, 100000), Description = "The ultimate combo for the ultimate experience.", IsActive = true },
                        new Combo { Name = "Classic Combo 4", Price = new Random().Next(20000, 100000), Description = "Classic combo for those who prefer the basics.", IsActive = true },
                        new Combo { Name = "Luxury Combo 5", Price = new Random().Next(20000, 100000), Description = "A luxury combo designed for those who want the best.", IsActive = true },
                        new Combo { Name = "Family Combo 6", Price = new Random().Next(20000, 100000), Description = "Combo designed for a family meal with multiple servings.", IsActive = true },
                        new Combo { Name = "Veggie Combo 7", Price = new Random().Next(20000, 100000), Description = "A healthy and tasty vegetarian combo.", IsActive = true },
                        new Combo { Name = "Quick Meal Combo 8", Price = new Random().Next(20000, 100000), Description = "Fast and satisfying combo for quick meals.", IsActive = true },
                        new Combo { Name = "Student Combo 9", Price = new Random().Next(20000, 100000), Description = "Affordable combo for students on a budget.", IsActive = true },
                        new Combo { Name = "Party Combo 10", Price = new Random().Next(20000, 100000), Description = "Perfect combo for parties with a lot of guests.", IsActive = true }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Combo
                if (!appContext.ComboProducts.Any())
                {
                    appContext.ComboProducts.AddRange(
                     // Combo 1
                     new ComboProduct { ComboId = 1, ProductId = 1 },
                     new ComboProduct { ComboId = 1, ProductId = 2 },
                     new ComboProduct { ComboId = 1, ProductId = 3 },

                     // Combo 2
                     new ComboProduct { ComboId = 2, ProductId = 4 },
                     new ComboProduct { ComboId = 2, ProductId = 5 },
                     new ComboProduct { ComboId = 2, ProductId = 6 },

                     // Combo 3
                     new ComboProduct { ComboId = 3, ProductId = 7 },
                     new ComboProduct { ComboId = 3, ProductId = 8 },

                     // Combo 4
                     new ComboProduct { ComboId = 4, ProductId = 1 },
                     new ComboProduct { ComboId = 4, ProductId = 9 },

                     // Combo 5
                     new ComboProduct { ComboId = 5, ProductId = 2 },
                     new ComboProduct { ComboId = 5, ProductId = 10 },

                     // Combo 6
                     new ComboProduct { ComboId = 6, ProductId = 3 },
                     new ComboProduct { ComboId = 6, ProductId = 4 }
                 );
                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Order
                if (!appContext.Orders.Any())
                {
                    appContext.Orders.AddRange(
                        new Domain.Entities.Order
                        {
                            UserId = 2,
                            OrderFullName = "John Doe",
                            OrderAddress = "123 Main St, Hanoi",
                            OrderPhoneNumber = "0901234567",
                            OrderEmail = "john.doe@example.com",
                            OrderDate = DateTime.Now,
                            OrderPaymentMethods = "Credit Card",
                            OrderStatusPayment = "Paid",
                            OrderStatus = 1,
                            OrderQuantity = 3,
                            OrderAmount = 300,
                            OrderNote = "Please deliver between 9 AM and 12 PM"
                        }, 
                        new Domain.Entities.Order
                        {
                            UserId = 3,
                            OrderFullName = "Maria Johnson",
                            OrderAddress = "456 High St, Hanoi",
                            OrderPhoneNumber = "0909876543",
                            OrderEmail = "maria.johnson@example.com",
                            OrderDate = DateTime.Now,
                            OrderPaymentMethods = "PayPal",
                            OrderStatusPayment = "Pending",
                            OrderStatus = 0,
                            OrderQuantity = 1,
                            OrderAmount = 120,
                            OrderNote = "Leave the package at the front door"
                        },
                        new Domain.Entities.Order
                        {
                            UserId = 4,
                            OrderFullName = "Alex Smith",
                            OrderAddress = "789 Maple Ave, Hanoi",
                            OrderPhoneNumber = "0907654321",
                            OrderEmail = "alex.smith@example.com",
                            OrderDate = DateTime.Now,
                            OrderPaymentMethods = "Cash on Delivery",
                            OrderStatusPayment = "Unpaid",
                            OrderStatus = 0,
                            OrderQuantity = 2,
                            OrderAmount = 220,
                            OrderNote = "Call before delivery"
                        },
                        new Domain.Entities.Order
                        {
                            UserId = 5,
                            OrderFullName = "Sophia Brown",
                            OrderAddress = "321 Oak Ln, Hanoi",
                            OrderPhoneNumber = "0901122334",
                            OrderEmail = "sophia.brown@example.com",
                            OrderDate = DateTime.Now,
                            OrderPaymentMethods = "Credit Card",
                            OrderStatusPayment = "Paid",
                            OrderStatus = 1,
                            OrderQuantity = 5,
                            OrderAmount = 500,
                            OrderNote = "Delivery in the evening only"
                        },
                        new Domain.Entities.Order
                        {
                            UserId = 6,
                            OrderFullName = "Michael Taylor",
                            OrderAddress = "654 Pine Rd, Hanoi",
                            OrderPhoneNumber = "0905566778",
                            OrderEmail = "michael.taylor@example.com",
                            OrderDate = DateTime.Now,
                            OrderPaymentMethods = "Bank Transfer",
                            OrderStatusPayment = "Paid",
                            OrderStatus = 1,
                            OrderQuantity = 4,
                            OrderAmount = 450,
                            OrderNote = "Include a receipt in the package"
                        },
                         new Domain.Entities.Order
                         {
                             UserId = 6,
                             OrderFullName = "Min",
                             OrderAddress = "654 Pine Rd, Hanoi",
                             OrderPhoneNumber = "0905566778",
                             OrderEmail = "min.cute@example.com",
                             OrderDate = DateTime.Now,
                             OrderPaymentMethods = "Bank Transfer",
                             OrderStatusPayment = "Paid",
                             OrderStatus = 1,
                             OrderQuantity = 4,
                             OrderAmount = 450,
                             OrderNote = "Include a receipt in the package"
                         }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng OrderDetails
                if (!appContext.OrderDetails.Any())
                {
                    appContext.OrderDetails.AddRange(
                         // Đơn hàng chỉ có Product (OrderId = 1)
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 1,
                             Quantity = 2,
                             TotalMoney = 200,
                             ProductId = 1,
                             ComboProductId = null,
                             OrderDetailType = "Product"
                         },
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 1,
                             Quantity = 1,
                             TotalMoney = 100,
                             ProductId = 2,
                             ComboProductId = null,
                             OrderDetailType = "Product"
                         },

                         // Đơn hàng chỉ có Combo (OrderId = 2)
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 2,
                             Quantity = 1,
                             TotalMoney = 300,
                             ProductId = null,
                             ComboProductId = 1,
                             OrderDetailType = "Combo"
                         },
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 2,
                             Quantity = 2,
                             TotalMoney = 600,
                             ProductId = null,
                             ComboProductId = 2,
                             OrderDetailType = "Combo"
                         },

                         // Đơn hàng có cả Product và Combo (OrderId = 3)
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 3,
                             Quantity = 1,
                             TotalMoney = 150,
                             ProductId = 3,
                             ComboProductId = null,
                             OrderDetailType = "Product"
                         },
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 3,
                             Quantity = 1,
                             TotalMoney = 500,
                             ProductId = null,
                             ComboProductId = 3,
                             OrderDetailType = "Combo"
                         },
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 3,
                             Quantity = 3,
                             TotalMoney = 450,
                             ProductId = 4,
                             ComboProductId = null,
                             OrderDetailType = "Product"
                         },

                         // Đơn hàng chỉ có Product (OrderId = 4)
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 4,
                             Quantity = 4,
                             TotalMoney = 800,
                             ProductId = 5,
                             ComboProductId = null,
                             OrderDetailType = "Product"
                         },

                         // Đơn hàng chỉ có Combo (OrderId = 5)
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 5,
                             Quantity = 1,
                             TotalMoney = 400,
                             ProductId = null,
                             ComboProductId = 4,
                             OrderDetailType = "Combo"
                         },

                         // Đơn hàng có cả Product và Combo (OrderId = 6)
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 6,
                             Quantity = 1,
                             TotalMoney = 200,
                             ProductId = 6,
                             ComboProductId = null,
                             OrderDetailType = "Product"
                         },
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 6,
                             Quantity = 1,
                             TotalMoney = 500,
                             ProductId = null,
                             ComboProductId = 5,
                             OrderDetailType = "Combo"
                         },
                         new Domain.Entities.OrderDetail
                         {
                             OrderId = 6,
                             Quantity = 2,
                             TotalMoney = 400,
                             ProductId = 7,
                             ComboProductId = null,
                             OrderDetailType = "Product"
                         }
                    );


                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Coupon
                if (!appContext.Coupons.Any())
                {
                    appContext.Coupons.AddRange(
                         new Coupon
                         {
                             Code = "VIETNAMVODICH",
                             IsActive = true,
                             Scope = "System",
                             ScopeId = null,
                             Description = "Việt Nam vô địch"
                         },
                         new Coupon
                         {
                             Code = "WOMAN",
                             IsActive = true,
                             Scope = "System",
                             ScopeId = null,
                             Description = "8-3 Woman day"
                         }
                    );


                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng CouponConditions
                if (!appContext.CouponConditions.Any())
                {
                    appContext.CouponConditions.AddRange(
                         // 1. Giảm giá dựa trên số tiền tối thiểu (minimum_amount)
                         new CouponCondition
                         {
                             CouponId = 1,
                             Attribute = "minimum_amount",
                             Operator = ">",
                             Value = "100",
                             DiscountAmount = 10  // Giảm 10% nếu đơn hàng trên 100
                         },

                          // 2. Giảm giá theo ngày áp dụng (applicable_date)
                          new CouponCondition
                          {
                              CouponId = 2,
                              Attribute = "applicable_date",
                              Operator = "=",
                              Value = "2024-12-25", // Giáng sinh
                              DiscountAmount = 15 // Giảm 15%
                          },
                          new CouponCondition
                          {
                              CouponId = 1,
                              Attribute = "applicable_date",
                              Operator = "BETWEEN",
                              Value = "2024-12-20|2024-12-31", // Từ 20/12 đến 31/12
                              DiscountAmount = 20
                          },

                          // 3. Giảm giá theo danh mục sản phẩm (category)
                          new CouponCondition
                          {
                              CouponId = 2,
                              Attribute = "category",
                              Operator = "=",
                              Value = "Pizza", // Áp dụng cho danh mục 'food'
                              DiscountAmount = 5 // Giảm 5%
                          },

                          //4.Giảm giá theo số lượng sản phẩm(quantity)
                          new CouponCondition
                          {
                              CouponId = 1,
                              Attribute = "quantity",
                              Operator = ">=",
                              Value = "3", // Mua từ 3 sản phẩm trở lên
                              DiscountAmount = 10 // Giảm 10%
                          },


                          // 5. Giảm giá theo người dùng (user_type)
                          new CouponCondition
                          {
                              CouponId = 1,
                              Attribute = "user_type",
                              Operator = "=",
                              Value = "new_user", // Áp dụng cho khách hàng mới
                              DiscountAmount = 20 // Giảm 20%
                          },

                          // 6. Giảm giá theo phương thức thanh toán (payment_method)
                          new CouponCondition
                          {
                              CouponId = 2,
                              Attribute = "payment_method",
                              Operator = "=",
                              Value = "credit_card", // Áp dụng khi thanh toán qua thẻ tín dụng
                              DiscountAmount = 5 // Giảm 5%
                          },


                          // 7. Giảm giá theo thương hiệu sản phẩm (brand)
                          new CouponCondition
                          {
                              CouponId = 1,
                              Attribute = "brand",
                              Operator = "=",
                              Value = "Apple", // Áp dụng cho thương hiệu Apple
                              DiscountAmount = 15 // Giảm 15%
                          },

                          // 8. Giảm giá theo lần mua hàng (order_count)
                          new CouponCondition
                          {
                              CouponId = 2,
                              Attribute = "order_count",
                              Operator = "=",
                              Value = "1", // Áp dụng cho đơn hàng đầu tiên
                              DiscountAmount = 30 // Giảm 30%
                          },

                          // 9. Giảm giá theo tổng giá trị giỏ hàng trước khi giảm giá (total_amount)
                          new CouponCondition
                          {
                              CouponId = 2,
                              Attribute = "total_amount",
                              Operator = ">",
                              Value = "1000", // Áp dụng khi tổng đơn hàng trên 1000
                              DiscountAmount = 50 // Giảm 50%
                          }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Cart
                if (!appContext.Carts.Any())
                {
                    appContext.Carts.AddRange(
                       new Cart
                       {
                           UserId = 2,
                           ProductId = 1, 
                           ComboProductId = null,
                           Quantity = 2, 
                           TotalAmount = 40
                       },
                       new Cart
                       {
                           UserId = 2,
                           ProductId = null,
                           ComboProductId = 1,
                           Quantity = 1,
                           TotalAmount = 90041
                       },
                        new Cart
                        {
                            UserId = 3,
                            ProductId = null,
                            ComboProductId = 1,
                            Quantity = 1,
                            TotalAmount = 90041
                        },
                        new Cart
                        {
                            UserId = 4,
                            ProductId = 1,
                            ComboProductId = null,
                            Quantity = 1,
                            TotalAmount = 90041
                        }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                // Khởi tạo dữ liệu cho bảng Wishlists
                if (!appContext.Wishlists.Any())
                {
                    appContext.Wishlists.AddRange(
                       new Wishlist
                       {
                           UserId = 2,
                           ProductId = 1,
                           ComboProductId = null
                       },
                        new Wishlist
                        {
                            UserId = 2,
                            ProductId = null,
                            ComboProductId = 1
                        },
                        new Wishlist
                        {
                            UserId = 3,
                            ProductId = 2,
                            ComboProductId = null
                        },
                        new Wishlist
                        {
                            UserId = 4,
                            ProductId = 3,
                            ComboProductId = null
                        },
                        new Wishlist
                        {
                            UserId = 5,
                            ProductId = 8,
                            ComboProductId = null
                        },
                        new Wishlist
                        {
                            UserId = 5,
                            ProductId = 2,
                            ComboProductId = null
                        },
                        new Wishlist
                        {
                            UserId = 5,
                            ProductId = 6,
                            ComboProductId = null
                        }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }


                //Khởi tạo dữ liệu cho bảng Codes
                if (!appContext.Codes.Any())
                {
                    appContext.Codes.AddRange(
                       new Code
                       {
                           CodeId = "SOCIAL_NETWORK_VALUE",
                           CodeDes = "Mạng xã hội",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       },
                       new Code
                       {
                           CodeId = "Connectdatabase",
                           CodeDes = "Kết nối cơ sở dữ liệu",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       },
                       new Code
                       {
                           CodeId = "COUNTRY_EXCEL",
                           CodeDes = "Danh mục quốc tịch (Sheet quoc_tich_hien_tai)",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       },
                       new Code
                       {
                           CodeId = "CURRENCY_UNIT",
                           CodeDes = "Danh sách mã tiền",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       },
                       new Code
                       {
                           CodeId = "FASTLANE_AIRLINES",
                           CodeDes = "Hãng hàng không",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       },
                       new Code
                       {
                           CodeId = "FASTLANE_LIST_AIRPORT",
                           CodeDes = "Danh sách sân bay",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       },
                       new Code
                       {
                           CodeId = "LIST_CURRENCY",
                           CodeDes = "Loại tiền tệ",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       },
                       new Code
                       {
                           CodeId = "PARAMETERS_SHARED",
                           CodeDes = "Tham số cấu hình dùng chung",
                           CreateBy = "Admin",
                           CreateDate = DateTime.Now
                       }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                //Khởi tạo dữ liệu cho bảng CodeValues
                if (!appContext.CodeValues.Any())
                {
                    appContext.CodeValues.AddRange(
                        new CodeValues { CodeId = "SOCIAL_NETWORK_VALUE", CodeValue = "FACEBOOK", CodeValueDes = "Facebook",CreateBy = "Admin",CreateDate = DateTime.Now},
                        new CodeValues { CodeId = "SOCIAL_NETWORK_VALUE", CodeValue = "WECHAT", CodeValueDes = "Wechat", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "SOCIAL_NETWORK_VALUE", CodeValue = "WHATSAPP", CodeValueDes = "WHATSAPP", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "SOCIAL_NETWORK_VALUE", CodeValue = "ZALO", CodeValueDes = "Zalo", CreateBy = "Admin", CreateDate = DateTime.Now },

                        new CodeValues { CodeId = "Connectdatabase", CodeValue = "CONNECDATA", CodeValueDes = "Kết nối cơ sở dữ liệu admin/Sql/index", CodeValueDes1 = "Data Source=192.168.10.245;uid=user_login2; pwd=1JqC3UmEU%; database=db_dichvu_visa", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "CURRENCY_UNIT", CodeValue = "CNY", CodeValueDes = "¥", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "CURRENCY_UNIT", CodeValue = "USD", CodeValueDes = "$", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "CURRENCY_UNIT", CodeValue = "VND", CodeValueDes = "₫", CreateBy = "Admin", CreateDate = DateTime.Now },

                        new CodeValues { CodeId = "LIST_CURRENCY", CodeValue = "VND", CodeValueDes = "Việt Nam đồng", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "LIST_CURRENCY", CodeValue = "USD", CodeValueDes = "Đô la Mỹ", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "LIST_CURRENCY", CodeValue = "CNY", CodeValueDes = "Nhân dân tệ", CreateBy = "Admin", CreateDate = DateTime.Now },

                        new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "ADD_KM", CodeValueDes = "10", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "ATP", CodeValueDes = "CÔNG TY CỔ PHẦN QUỐC TẾ THƯƠNG MẠI TÀI CHÍNH AN THỊNH PHÁT", CodeValueDes1 = "DN", CreateBy = "Admin", CreateDate = DateTime.Now },
                        new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "ATP", CodeValueDes = "CÔNG TY CỔ PHẦN QUỐC TẾ THƯƠNG MẠI TÀI CHÍNH AN THỊNH PHÁT", CodeValueDes1 = "DN", CreateBy = "Admin", CreateDate = DateTime.Now },

                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "BLOCK_KM", CodeValueDes = "10" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "CODE_NOTE_FTA", CodeValueDes = "Tổng số là SO_LUONG_KHACH du khách" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "DEFAULT_KM", CodeValueDes = "20" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "EMBASSIES", CodeValueDes = "100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "EMBASSIES_DAYS", CodeValueDes = "3-5" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "GATES", CodeValueDes = "17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36," },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "GMAP_KEY", CodeValueDes = "AIzaSyDCcl04qUfgMUmuXNGrayk6bVggaiew6PM" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "HANDOVER_PRICE", CodeValueDes = "18,700" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "HOLIDAYS", CodeValueDes = "01/05/2023,02/05/2023,03/05/2023" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "MINIMUM_PAYMENT", CodeValueDes = "70" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "NOT_EMBASSIES_DAYS", CodeValueDes = "3-5" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "PAYMENT_CASHBACK_DEDUCT", CodeValueDes = "10177,10176" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "PAYMENT_CREDIT_AGENCY", CodeValueDes = "25,150,148" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "SIX_DAY_AGENCY", CodeValueDes = "1151,148,151" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "URL_API_GEN_EMAIL", CodeValueDes = "http://mail.yoctovn.com/api/v1/add/mailbox" },
                            new CodeValues { CodeId = "PARAMETERS_SHARED", CodeValue = "URL_KIEM_TRA_PASSPORT", CodeValueDes = "https://api.thithucdientu.gov.vn" },



                                new CodeValues { CodeId = "FASTLANE_LIST_AIRPORT", CodeValue = "KHN", CodeValueDes = "Cửa khẩu Hữu Nghị" },
                                new CodeValues { CodeId = "FASTLANE_LIST_AIRPORT", CodeValue = "KMC", CodeValueDes = "Cửa khẩu Móng Cái" },
                                new CodeValues { CodeId = "FASTLANE_LIST_AIRPORT", CodeValue = "SCR", CodeValueDes = "Sân bay quốc tế Cam Ranh" },
                                new CodeValues { CodeId = "FASTLANE_LIST_AIRPORT", CodeValue = "SDN", CodeValueDes = "Sân bay quốc tế Đà Nẵng" },
                                new CodeValues { CodeId = "FASTLANE_LIST_AIRPORT", CodeValue = "SNB", CodeValueDes = "Sân bay quốc tế Nội Bài" },
                                new CodeValues { CodeId = "FASTLANE_LIST_AIRPORT", CodeValue = "SPQ", CodeValueDes = "Sân bay quốc tế Phú Quốc" },
                                new CodeValues { CodeId = "FASTLANE_LIST_AIRPORT", CodeValue = "STS", CodeValueDes = "Sân bay quốc tế Tân Sơn Nhất" },



                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "Aeroflote", CodeValueDes = "Aeroflote" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "AirFrance", CodeValueDes = "Air France" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "AllNipponAirways", CodeValueDes = "All Nippon Airways" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "AsianaAirlines", CodeValueDes = "Asiana Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "BambooAirways", CodeValueDes = "Bamboo Airways" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "BritishAirways", CodeValueDes = "British Airways" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "CathayPacific", CodeValueDes = "Cathay Pacific" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "ChinaAirlines", CodeValueDes = "China Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "ChinaSouthernAirlines", CodeValueDes = "China Southern Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "EvaAir", CodeValueDes = "Eva Air" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "JAPANAirways", CodeValueDes = "Hãng hàng không Nhật Bản" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "KoreanAir", CodeValueDes = "Korean Air" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "LAoAirlines", CodeValueDes = "Lào Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "LionAir", CodeValueDes = "Lion Air" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "Lufthansa", CodeValueDes = "Lufthansa" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "MalaysiaAirlines", CodeValueDes = "Malaysia Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "PacificAirlines", CodeValueDes = "Pacific Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "PhilippineAirlines", CodeValueDes = "Philippine Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "Qantas", CodeValueDes = "Qantas" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "ScandinavianAirlinesSystem", CodeValueDes = "Scandinavian Airlines System" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "SiemReapAirways", CodeValueDes = "Siem Reap Airways" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "SingaporeAirlines", CodeValueDes = "Singapore Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "ThaiAirways", CodeValueDes = "Thai Airways" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "VietjetAir", CodeValueDes = "Vietjet Air" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "VietnamAirlines", CodeValueDes = "Vietnam Airlines" },
                                new CodeValues { CodeId = "FASTLANE_AIRLINES", CodeValue = "VietravelAirlines", CodeValueDes = "Vietravel Airlines" },


                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "AFG", CodeValueDes = "Afghanistan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "AGO", CodeValueDes = "Angola" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ALB", CodeValueDes = "Albania" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "AND", CodeValueDes = "Andorra" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ARE", CodeValueDes = "United Arab Emirates" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ARG", CodeValueDes = "Argentina" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ARM", CodeValueDes = "Armenia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ATG", CodeValueDes = "Antigua and Barbuda" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "AUS", CodeValueDes = "Australia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "AUT", CodeValueDes = "Austria" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "AZE", CodeValueDes = "Azerbaijan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BDI", CodeValueDes = "Burundi" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BEL", CodeValueDes = "Belgium" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BEN", CodeValueDes = "Benin" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BFA", CodeValueDes = "Burkina Faso" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BGD", CodeValueDes = "Bangladesh" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BGR", CodeValueDes = "Bulgaria" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BHR", CodeValueDes = "Bahrain" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BHS", CodeValueDes = "Bahamas" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BIH", CodeValueDes = "Bosnia and Herzegovina" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BLR", CodeValueDes = "Belarus" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BLZ", CodeValueDes = "Belize" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BMU", CodeValueDes = "Bermuda" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BOL", CodeValueDes = "Bolivia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BRA", CodeValueDes = "Brazil" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BRB", CodeValueDes = "Barbados" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BRN", CodeValueDes = "Brunei" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BTN", CodeValueDes = "Bhutan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "BWA", CodeValueDes = "Botswana" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CAF", CodeValueDes = "Central African Republic" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CAN", CodeValueDes = "Canada" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CHE", CodeValueDes = "Switzerland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CHL", CodeValueDes = "Chile" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CHN", CodeValueDes = "China" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CIV", CodeValueDes = "Cote d' Ivoire" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CMR", CodeValueDes = "Cameroon" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "COG", CodeValueDes = "Congo" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "COL", CodeValueDes = "Colombia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "COM", CodeValueDes = "Comoros" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CPV", CodeValueDes = "Cape Verde" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CRI", CodeValueDes = "Costa Rica" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CUB", CodeValueDes = "Cuba" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CYP", CodeValueDes = "Cyprus" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "CZE", CodeValueDes = "Czech Republic" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "D", CodeValueDes = "Germany" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "DJI", CodeValueDes = "Djibouti" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "DMA", CodeValueDes = "Dominica" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "DNK", CodeValueDes = "Denmark" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "DOM", CodeValueDes = "Dominican Republic" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "DZA", CodeValueDes = "Algeria" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ECU", CodeValueDes = "Ecuador" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "EGY", CodeValueDes = "Egypt" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ERI", CodeValueDes = "Eritrea" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ESP", CodeValueDes = "Spain" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "EST", CodeValueDes = "Estonia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ETH", CodeValueDes = "Ethiopia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "FIN", CodeValueDes = "Finland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "FJI", CodeValueDes = "Fiji" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "FRA", CodeValueDes = "France" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "FSM", CodeValueDes = "Micronesia Federated States of" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GAB", CodeValueDes = "Gabon" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GBD", CodeValueDes = "United Kingdom British Dependent Territories Citizen" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GBO", CodeValueDes = "Vương quốc Anh" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GBR", CodeValueDes = "United Kingdom British Citizen" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GBS", CodeValueDes = "Công dân các địa phận thuộc Vương quốc Liên hiệp Anh" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GEO", CodeValueDes = "Georgia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GHA", CodeValueDes = "Ghana" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GIB", CodeValueDes = "Gibraltar" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GIN", CodeValueDes = "Guinea" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GMB", CodeValueDes = "Gambia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GNB", CodeValueDes = "Guinea-Bissau" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GNQ", CodeValueDes = "Equatorial Guinea" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GRC", CodeValueDes = "Greece" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GRD", CodeValueDes = "Grenada" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GRL", CodeValueDes = "Greenland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GTM", CodeValueDes = "Guatemala" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "GUY", CodeValueDes = "Guyana" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "HND", CodeValueDes = "Honduras" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "HRV", CodeValueDes = "Croatia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "HTI", CodeValueDes = "Haiti" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "HUN", CodeValueDes = "Hungary" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "IDN", CodeValueDes = "Indonesia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "IND", CodeValueDes = "India" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "IOT", CodeValueDes = "British India Ocean Territoryia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "IRL", CodeValueDes = "Ireland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "IRN", CodeValueDes = "Iran Islamic Republic of" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "IRQ", CodeValueDes = "Iraq" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ISL", CodeValueDes = "Iceland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ISR", CodeValueDes = "Israel" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ITA", CodeValueDes = "Italy" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "JAM", CodeValueDes = "Jamaica" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "JOR", CodeValueDes = "Jordan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "JPN", CodeValueDes = "Japan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KAZ", CodeValueDes = "Kazakhstan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KEN", CodeValueDes = "Kenya" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KGZ", CodeValueDes = "Kyrgyzstan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KHM", CodeValueDes = "Cambodia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KIR", CodeValueDes = "Kiribati" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KNA", CodeValueDes = "Saint Kitts and Nevis" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KOR", CodeValueDes = "Republic of Korea" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "KWT", CodeValueDes = "Kuwait" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LAO", CodeValueDes = "Lao Peoples Democratic Republic" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LBN", CodeValueDes = "Lebanon" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LBR", CodeValueDes = "Liberia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LBY", CodeValueDes = "Libyan Arab Jamahiriya" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LCA", CodeValueDes = "Saint Lucia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LIE", CodeValueDes = "Liechtenstein" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LKA", CodeValueDes = "Sri Lanka" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LSO", CodeValueDes = "Lesotho" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LTU", CodeValueDes = "Lithuania" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LUX", CodeValueDes = "Luxembourg" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "LVA", CodeValueDes = "Latvia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MAR", CodeValueDes = "Morocco" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MCO", CodeValueDes = "Monaco" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MDA", CodeValueDes = "Moldova" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MDG", CodeValueDes = "Madagascar" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MDV", CodeValueDes = "Maldives" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MEX", CodeValueDes = "Mexico" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MHL", CodeValueDes = "Marshall Islands" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MKD", CodeValueDes = "Macedonia The former Yugoslav of" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MLI", CodeValueDes = "Mali" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MLT", CodeValueDes = "Malta" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MMR", CodeValueDes = "Myanmar" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MNE", CodeValueDes = "Montenegro" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MNG", CodeValueDes = "Mongolia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MOZ", CodeValueDes = "Mozambique" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MRT", CodeValueDes = "Mauritania" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MSR", CodeValueDes = "Montserrat" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MUS", CodeValueDes = "Mauritius" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MWI", CodeValueDes = "Malawi" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "MYS", CodeValueDes = "Malaysia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NAM", CodeValueDes = "Namibia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NER", CodeValueDes = "Niger" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NGA", CodeValueDes = "Nigeria" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NIC", CodeValueDes = "Nicaragua" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NLD", CodeValueDes = "Netherlands" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NOR", CodeValueDes = "Norway" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NPL", CodeValueDes = "Nepal" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NRU", CodeValueDes = "Nauru" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "NZL", CodeValueDes = "New Zealand" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "OMN", CodeValueDes = "Oman" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PAK", CodeValueDes = "Pakistan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PAN", CodeValueDes = "Panama" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PER", CodeValueDes = "Peru" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PHL", CodeValueDes = "Philippines" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PLW", CodeValueDes = "Palau" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PLX", CodeValueDes = "Palestine" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PNG", CodeValueDes = "Papua New Guinea" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "POL", CodeValueDes = "Poland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PRK", CodeValueDes = "Korea Democratic Peoples Republic of" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PRT", CodeValueDes = "Portugal" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "PRY", CodeValueDes = "Paraguay" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "QAT", CodeValueDes = "Qatar" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "RKS", CodeValueDes = "Kosovo" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ROM", CodeValueDes = "Romania" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "RUS", CodeValueDes = "Russian Federation" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "RWA", CodeValueDes = "Rwanda" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SAU", CodeValueDes = "Saudi Arabia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SC-", CodeValueDes = "Scotland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SDN", CodeValueDes = "Sudan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SE-", CodeValueDes = "Serbia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SEN", CodeValueDes = "Senegal" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SGP", CodeValueDes = "Singapore" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SLB", CodeValueDes = "Solomon Islands" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SLE", CodeValueDes = "Sierra Leone" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SLV", CodeValueDes = "El Salvador" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SMR", CodeValueDes = "San Marino" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SOM", CodeValueDes = "Somalia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "STP", CodeValueDes = "Sao Tome and Principe" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SUR", CodeValueDes = "Suriname" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SVK", CodeValueDes = "Slovakia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SVN", CodeValueDes = "Slovenia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SWE", CodeValueDes = "Sweden" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SWZ", CodeValueDes = "Swaziland" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SYC", CodeValueDes = "Seychelles" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "SYR", CodeValueDes = "Syrian Arab Republic" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TCD", CodeValueDes = "Chad" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TGO", CodeValueDes = "Togo" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "THA", CodeValueDes = "Thailand" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TJK", CodeValueDes = "Tajikistan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TKM", CodeValueDes = "Turkmenistan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TLS", CodeValueDes = "Timor Leste" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TON", CodeValueDes = "Tonga" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TTO", CodeValueDes = "Trinidad and Tobago" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TUN", CodeValueDes = "Tunisia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TUR", CodeValueDes = "Turkey" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TUV", CodeValueDes = "Tuvalu" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TWN", CodeValueDes = "China (Taiwan)" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "TZA", CodeValueDes = "Tanzania United Republic of" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "UGA", CodeValueDes = "Uganda" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "UKR", CodeValueDes = "Ukraine" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "UNO", CodeValueDes = "United Nations Organization" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "URY", CodeValueDes = "Uruguay" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "USA", CodeValueDes = "United States" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "UZB", CodeValueDes = "Uzbekistan" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "VAT", CodeValueDes = "Holy See (Vatican City State)" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "VCT", CodeValueDes = "Saint Vincent and the Grenadines" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "VEN", CodeValueDes = "Venezuela" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "VNM", CodeValueDes = "Viet Nam" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "VUT", CodeValueDes = "Vanuatu" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "WSM", CodeValueDes = "Samoa" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "YEM", CodeValueDes = "Yemen" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ZAF", CodeValueDes = "South Africa" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ZMB", CodeValueDes = "Zambia" },
                                new CodeValues { CodeId = "COUNTRY_EXCEL", CodeValue = "ZWE", CodeValueDes = "Zimbabwe" }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

                //Khởi tạo dữ liệu cho bảng Locations
                if (!appContext.Locations.Any())
                {
                    appContext.Locations.AddRange(
                        // 63 tỉnh thành
                        new Location { Status = 1, Code = "01", Name = "Thành phố Hà Nội", ParentId = -1 },
                        new Location { Status = 1, Code = "02", Name = "Tỉnh Hà Giang", ParentId = -1 },
                        new Location { Status = 1, Code = "04", Name = "Tỉnh Cao Bằng", ParentId = -1 },
                        new Location { Status = 1, Code = "06", Name = "Tỉnh Bắc Kạn", ParentId = -1 },
                        new Location { Status = 1, Code = "08", Name = "Tỉnh Tuyên Quang", ParentId = -1 },
                        new Location { Status = 1, Code = "10", Name = "Tỉnh Lào Cai", ParentId = -1 },
                        new Location { Status = 1, Code = "11", Name = "Tỉnh Điện Biên", ParentId = -1 },
                        new Location { Status = 1, Code = "12", Name = "Tỉnh Lai Châu", ParentId = -1 },
                        new Location { Status = 1, Code = "14", Name = "Tỉnh Sơn La", ParentId = -1 },
                        new Location { Status = 1, Code = "15", Name = "Tỉnh Yên Bái", ParentId = -1 },
                        new Location { Status = 1, Code = "17", Name = "Tỉnh Hoà Bình", ParentId = -1 },
                        new Location { Status = 1, Code = "19", Name = "Tỉnh Thái Nguyên", ParentId = -1 },
                        new Location { Status = 1, Code = "20", Name = "Tỉnh Lạng Sơn", ParentId = -1 },
                        new Location { Status = 1, Code = "22", Name = "Tỉnh Quảng Ninh", ParentId = -1 },
                        new Location { Status = 1, Code = "24", Name = "Tỉnh Bắc Giang", ParentId = -1 },
                        new Location { Status = 1, Code = "25", Name = "Tỉnh Phú Thọ", ParentId = -1 },
                        new Location { Status = 1, Code = "26", Name = "Tỉnh Vĩnh Phúc", ParentId = -1 },
                        new Location { Status = 1, Code = "27", Name = "Tỉnh Bắc Ninh", ParentId = -1 },
                        new Location { Status = 1, Code = "30", Name = "Tỉnh Hải Dương", ParentId = -1 },
                        new Location { Status = 1, Code = "31", Name = "Thành phố Hải Phòng", ParentId = -1 },
                        new Location { Status = 1, Code = "33", Name = "Tỉnh Hưng Yên", ParentId = -1 },
                        new Location { Status = 1, Code = "34", Name = "Tỉnh Thái Bình", ParentId = -1 },
                        new Location { Status = 1, Code = "35", Name = "Tỉnh Hà Nam", ParentId = -1 },
                        new Location { Status = 1, Code = "36", Name = "Tỉnh Nam Định", ParentId = -1 },
                        new Location { Status = 1, Code = "37", Name = "Tỉnh Ninh Bình", ParentId = -1 },
                        new Location { Status = 1, Code = "38", Name = "Tỉnh Thanh Hóa", ParentId = -1 },
                        new Location { Status = 1, Code = "40", Name = "Tỉnh Nghệ An", ParentId = -1 },
                        new Location { Status = 1, Code = "42", Name = "Tỉnh Hà Tĩnh", ParentId = -1 },
                        new Location { Status = 1, Code = "44", Name = "Tỉnh Quảng Bình", ParentId = -1 },
                        new Location { Status = 1, Code = "45", Name = "Tỉnh Quảng Trị", ParentId = -1 },
                        new Location { Status = 1, Code = "46", Name = "Tỉnh Thừa Thiên Huế", ParentId = -1 },
                        new Location { Status = 1, Code = "48", Name = "Thành phố Đà Nẵng", ParentId = -1 },
                        new Location { Status = 1, Code = "49", Name = "Tỉnh Quảng Nam", ParentId = -1 },
                        new Location { Status = 1, Code = "51", Name = "Tỉnh Quảng Ngãi", ParentId = -1 },
                        new Location { Status = 1, Code = "52", Name = "Tỉnh Bình Định", ParentId = -1 },
                        new Location { Status = 1, Code = "54", Name = "Tỉnh Phú Yên", ParentId = -1 },
                        new Location { Status = 1, Code = "56", Name = "Tỉnh Khánh Hòa", ParentId = -1 },
                        new Location { Status = 1, Code = "58", Name = "Tỉnh Ninh Thuận", ParentId = -1 },
                        new Location { Status = 1, Code = "60", Name = "Tỉnh Bình Thuận", ParentId = -1 },
                        new Location { Status = 1, Code = "62", Name = "Tỉnh Kon Tum", ParentId = -1 },
                        new Location { Status = 1, Code = "64", Name = "Tỉnh Gia Lai", ParentId = -1 },
                        new Location { Status = 1, Code = "66", Name = "Tỉnh Đắk Lắk", ParentId = -1 },
                        new Location { Status = 1, Code = "67", Name = "Tỉnh Đắk Nông", ParentId = -1 },
                        new Location { Status = 1, Code = "68", Name = "Tỉnh Lâm Đồng", ParentId = -1 },
                        new Location { Status = 1, Code = "70", Name = "Tỉnh Bình Phước", ParentId = -1 },
                        new Location { Status = 1, Code = "72", Name = "Tỉnh Tây Ninh", ParentId = -1 },
                        new Location { Status = 1, Code = "74", Name = "Tỉnh Bình Dương", ParentId = -1 },
                        new Location { Status = 1, Code = "75", Name = "Tỉnh Đồng Nai", ParentId = -1 },
                        new Location { Status = 1, Code = "77", Name = "Tỉnh Bà Rịa - Vũng Tàu", ParentId = -1 },
                        new Location { Status = 1, Code = "79", Name = "Thành phố Hồ Chí Minh", ParentId = -1 },
                        new Location { Status = 1, Code = "80", Name = "Tỉnh Long An", ParentId = -1 },
                        new Location { Status = 1, Code = "82", Name = "Tỉnh Tiền Giang", ParentId = -1 },
                        new Location { Status = 1, Code = "83", Name = "Tỉnh Bến Tre", ParentId = -1 },
                        new Location { Status = 1, Code = "84", Name = "Tỉnh Trà Vinh", ParentId = -1 },
                        new Location { Status = 1, Code = "86", Name = "Tỉnh Vĩnh Long", ParentId = -1 },
                        new Location { Status = 1, Code = "87", Name = "Tỉnh Đồng Tháp", ParentId = -1 },
                        new Location { Status = 1, Code = "89", Name = "Tỉnh An Giang", ParentId = -1 },
                        new Location { Status = 1, Code = "91", Name = "Tỉnh Kiên Giang", ParentId = -1 },
                        new Location { Status = 1, Code = "92", Name = "Thành phố Cần Thơ", ParentId = -1 },
                        new Location { Status = 1, Code = "93", Name = "Tỉnh Hậu Giang", ParentId = -1 },
                        new Location { Status = 1, Code = "94", Name = "Tỉnh Sóc Trăng", ParentId = -1 },
                        new Location { Status = 1, Code = "95", Name = "Tỉnh Bạc Liêu", ParentId = -1 },
                        new Location { Status = 1, Code = "96", Name = "Tỉnh Cà Mau", ParentId = -1 }
                    );

                    // TP Hà nội
                    appContext.Locations.AddRange(
                        new Location { Status = 1, Code = "001", Name = "Quận Ba Đình", ParentId = 1 },
                        new Location { Status = 1, Code = "002", Name = "Quận Hoàn Kiếm", ParentId = 1 },
                        new Location { Status = 1, Code = "003", Name = "Quận Tây Hồ", ParentId = 1 },
                        new Location { Status = 1, Code = "004", Name = "Quận Long Biên", ParentId = 1 },
                        new Location { Status = 1, Code = "005", Name = "Quận Cầu Giấy", ParentId = 1 },
                        new Location { Status = 1, Code = "006", Name = "Quận Đống Đa", ParentId = 1 },
                        new Location { Status = 1, Code = "007", Name = "Quận Hai Bà Trưng", ParentId = 1 },
                        new Location { Status = 1, Code = "008", Name = "Quận Hoàng Mai", ParentId = 1 },
                        new Location { Status = 1, Code = "009", Name = "Quận Thanh Xuân", ParentId = 1 },
                        new Location { Status = 1, Code = "016", Name = "Huyện Sóc Sơn", ParentId = 1 },
                        new Location { Status = 1, Code = "017", Name = "Huyện Đông Anh", ParentId = 1 },
                        new Location { Status = 1, Code = "018", Name = "Huyện Gia Lâm", ParentId = 1 },
                        new Location { Status = 1, Code = "019", Name = "Quận Nam Từ Liêm", ParentId = 1 },
                        new Location { Status = 1, Code = "020", Name = "Huyện Thanh Trì", ParentId = 1 },
                        new Location { Status = 1, Code = "021", Name = "Quận Bắc Từ Liêm", ParentId = 1 },
                        new Location { Status = 1, Code = "250", Name = "Huyện Mê Linh", ParentId = 1 },
                        new Location { Status = 1, Code = "268", Name = "Quận Hà Đông", ParentId = 1 },
                        new Location { Status = 1, Code = "269", Name = "Thị xã Sơn Tây", ParentId = 1 },
                        new Location { Status = 1, Code = "271", Name = "Huyện Ba Vì", ParentId = 1 },
                        new Location { Status = 1, Code = "272", Name = "Huyện Phúc Thọ", ParentId = 1 },
                        new Location { Status = 1, Code = "273", Name = "Huyện Đan Phượng", ParentId = 1 },
                        new Location { Status = 1, Code = "274", Name = "Huyện Hoài Đức", ParentId = 1 },
                        new Location { Status = 1, Code = "275", Name = "Huyện Quốc Oai", ParentId = 1 },
                        new Location { Status = 1, Code = "276", Name = "Huyện Thạch Thất", ParentId = 1 },
                        new Location { Status = 1, Code = "277", Name = "Huyện Chương Mỹ", ParentId = 1 },
                        new Location { Status = 1, Code = "278", Name = "Huyện Thanh Oai", ParentId = 1 },
                        new Location { Status = 1, Code = "279", Name = "Huyện Thường Tín", ParentId = 1 },
                        new Location { Status = 1, Code = "280", Name = "Huyện Phú Xuyên", ParentId = 1 },
                        new Location { Status = 1, Code = "281", Name = "Huyện Ứng Hòa", ParentId = 1 },
                        new Location { Status = 1, Code = "282", Name = "Huyện Mỹ Đức", ParentId = 1 }


                    );

                    // TP Hà Giang
                    appContext.AddRange(
                        new Location { Status = 1, Code = "024", Name = "Thành phố Hà Giang", ParentId = 2 },
                        new Location { Status = 1, Code = "026", Name = "Huyện Đồng Văn", ParentId = 2 },
                        new Location { Status = 1, Code = "027", Name = "Huyện Mèo Vạc", ParentId = 2 },
                        new Location { Status = 1, Code = "028", Name = "Huyện Yên Minh", ParentId = 2 },
                        new Location { Status = 1, Code = "029", Name = "Huyện Quản Bạ", ParentId = 2 },
                        new Location { Status = 1, Code = "030", Name = "Huyện Vị Xuyên", ParentId = 2 },
                        new Location { Status = 1, Code = "031", Name = "Huyện Bắc Mê", ParentId = 2 },
                        new Location { Status = 1, Code = "032", Name = "Huyện Hoàng Su Phì", ParentId = 2 },
                        new Location { Status = 1, Code = "033", Name = "Huyện Xín Mần", ParentId = 2 },
                        new Location { Status = 1, Code = "034", Name = "Huyện Bắc Quang", ParentId = 2 },
                        new Location { Status = 1, Code = "035", Name = "Huyện Quang Bình", ParentId = 2 }
                    );

                    // Các quận, huyện thuộc tỉnh Cao Bằng
                    appContext.AddRange(
                        new Location { Status = 1, Code = "040", Name = "Thành phố Cao Bằng", ParentId = 4 },
                        new Location { Status = 1, Code = "042", Name = "Huyện Bảo Lâm", ParentId = 4 },
                        new Location { Status = 1, Code = "043", Name = "Huyện Bảo Lạc", ParentId = 4 },
                        new Location { Status = 1, Code = "045", Name = "Huyện Hà Quảng", ParentId = 4 },
                        new Location { Status = 1, Code = "047", Name = "Huyện Trùng Khánh", ParentId = 4 },
                        new Location { Status = 1, Code = "048", Name = "Huyện Hạ Lang", ParentId = 4 },
                        new Location { Status = 1, Code = "049", Name = "Huyện Quảng Hòa", ParentId = 4 },
                        new Location { Status = 1, Code = "051", Name = "Huyện Hoà An", ParentId = 4 },
                        new Location { Status = 1, Code = "052", Name = "Huyện Nguyên Bình", ParentId = 4 },
                        new Location { Status = 1, Code = "053", Name = "Huyện Thạch An", ParentId = 4 }
                    );

                    // Các quận, huyện thuộc tỉnh Bắc Kạn
                    appContext.AddRange(
                        new Location { Status = 1, Code = "058", Name = "Thành phố Bắc Kạn", ParentId = 6 },
                        new Location { Status = 1, Code = "060", Name = "Huyện Pác Nặm", ParentId = 6 },
                        new Location { Status = 1, Code = "061", Name = "Huyện Ba Bể", ParentId = 6 },
                        new Location { Status = 1, Code = "062", Name = "Huyện Ngân Sơn", ParentId = 6 },
                        new Location { Status = 1, Code = "063", Name = "Huyện Bạch Thông", ParentId = 6 },
                        new Location { Status = 1, Code = "064", Name = "Huyện Chợ Đồn", ParentId = 6 },
                        new Location { Status = 1, Code = "065", Name = "Huyện Chợ Mới", ParentId = 6 },
                        new Location { Status = 1, Code = "066", Name = "Huyện Na Rì", ParentId = 6 }
                    );

                    // Các quận, huyện thuộc tỉnh Tuyên Quang
                    appContext.AddRange(
                        new Location { Status = 1, Code = "070", Name = "Thành phố Tuyên Quang", ParentId = 8 },
                        new Location { Status = 1, Code = "071", Name = "Huyện Lâm Bình", ParentId = 8 },
                        new Location { Status = 1, Code = "072", Name = "Huyện Na Hang", ParentId = 8 },
                        new Location { Status = 1, Code = "073", Name = "Huyện Chiêm Hóa", ParentId = 8 },
                        new Location { Status = 1, Code = "074", Name = "Huyện Hàm Yên", ParentId = 8 },
                        new Location { Status = 1, Code = "075", Name = "Huyện Yên Sơn", ParentId = 8 },
                        new Location { Status = 1, Code = "076", Name = "Huyện Sơn Dương", ParentId = 8 }
                    );

                    // Các quận, huyện thuộc tỉnh Lào Cai
                    appContext.AddRange(
                        new Location { Status = 1, Code = "080", Name = "Thành phố Lào Cai", ParentId = 10 },
                        new Location { Status = 1, Code = "082", Name = "Huyện Bát Xát", ParentId = 10 },
                        new Location { Status = 1, Code = "083", Name = "Huyện Mường Khương", ParentId = 10 },
                        new Location { Status = 1, Code = "084", Name = "Huyện Si Ma Cai", ParentId = 10 },
                        new Location { Status = 1, Code = "085", Name = "Huyện Bắc Hà", ParentId = 10 },
                        new Location { Status = 1, Code = "086", Name = "Huyện Bảo Thắng", ParentId = 10 },
                        new Location { Status = 1, Code = "087", Name = "Huyện Bảo Yên", ParentId = 10 },
                        new Location { Status = 1, Code = "088", Name = "Thị xã Sa Pa", ParentId = 10 },
                        new Location { Status = 1, Code = "089", Name = "Huyện Văn Bàn", ParentId = 10 }
                    );

                    // Các quận, huyện thuộc tỉnh Điện Biên
                    appContext.AddRange(
                        new Location { Status = 1, Code = "094", Name = "Thành phố Điện Biên Phủ", ParentId = 11 },
                        new Location { Status = 1, Code = "095", Name = "Thị Xã Mường Lay", ParentId = 11 },
                        new Location { Status = 1, Code = "096", Name = "Huyện Mường Nhé", ParentId = 11 },
                        new Location { Status = 1, Code = "097", Name = "Huyện Mường Chà", ParentId = 11 },
                        new Location { Status = 1, Code = "098", Name = "Huyện Tủa Chùa", ParentId = 11 },
                        new Location { Status = 1, Code = "099", Name = "Huyện Tuần Giáo", ParentId = 11 },
                        new Location { Status = 1, Code = "100", Name = "Huyện Điện Biên", ParentId = 11 },
                        new Location { Status = 1, Code = "101", Name = "Huyện Điện Biên Đông", ParentId = 11 },
                        new Location { Status = 1, Code = "102", Name = "Huyện Mường Ảng", ParentId = 11 },
                        new Location { Status = 1, Code = "103", Name = "Huyện Nậm Pồ", ParentId = 11 }
                    );

                    // Các quận, huyện thuộc tỉnh Lai Châu
                    appContext.AddRange(
                        new Location { Status = 1, Code = "105", Name = "Thành phố Lai Châu", ParentId = 12 },
                        new Location { Status = 1, Code = "106", Name = "Huyện Tam Đường", ParentId = 12 },
                        new Location { Status = 1, Code = "107", Name = "Huyện Mường Tè", ParentId = 12 },
                        new Location { Status = 1, Code = "108", Name = "Huyện Sìn Hồ", ParentId = 12 },
                        new Location { Status = 1, Code = "109", Name = "Huyện Phong Thổ", ParentId = 12 },
                        new Location { Status = 1, Code = "110", Name = "Huyện Than Uyên", ParentId = 12 },
                        new Location { Status = 1, Code = "111", Name = "Huyện Tân Uyên", ParentId = 12 },
                        new Location { Status = 1, Code = "112", Name = "Huyện Nậm Nhùn", ParentId = 12 }
                    );

                    // Các quận, huyện thuộc tỉnh Sơn La
                    appContext.AddRange(
                        new Location { Status = 1, Code = "116", Name = "Thành phố Sơn La", ParentId = 14 },
                        new Location { Status = 1, Code = "118", Name = "Huyện Quỳnh Nhai", ParentId = 14 },
                        new Location { Status = 1, Code = "119", Name = "Huyện Thuận Châu", ParentId = 14 },
                        new Location { Status = 1, Code = "120", Name = "Huyện Mường La", ParentId = 14 },
                        new Location { Status = 1, Code = "121", Name = "Huyện Bắc Yên", ParentId = 14 },
                        new Location { Status = 1, Code = "122", Name = "Huyện Phù Yên", ParentId = 14 },
                        new Location { Status = 1, Code = "123", Name = "Huyện Mộc Châu", ParentId = 14 },
                        new Location { Status = 1, Code = "124", Name = "Huyện Yên Châu", ParentId = 14 },
                        new Location { Status = 1, Code = "125", Name = "Huyện Mai Sơn", ParentId = 14 },
                        new Location { Status = 1, Code = "126", Name = "Huyện Sông Mã", ParentId = 14 },
                        new Location { Status = 1, Code = "127", Name = "Huyện Sốp Cộp", ParentId = 14 },
                        new Location { Status = 1, Code = "128", Name = "Huyện Vân Hồ", ParentId = 14 }
                    );

                    // Các quận, huyện thuộc tỉnh Yên Bái
                    appContext.AddRange(
                        new Location { Status = 1, Code = "132", Name = "Thành phố Yên Bái", ParentId = 15 },
                        new Location { Status = 1, Code = "133", Name = "Thị xã Nghĩa Lộ", ParentId = 15 },
                        new Location { Status = 1, Code = "135", Name = "Huyện Lục Yên", ParentId = 15 },
                        new Location { Status = 1, Code = "136", Name = "Huyện Văn Yên", ParentId = 15 },
                        new Location { Status = 1, Code = "137", Name = "Huyện Mù Căng Chải", ParentId = 15 },
                        new Location { Status = 1, Code = "138", Name = "Huyện Trấn Yên", ParentId = 15 },
                        new Location { Status = 1, Code = "139", Name = "Huyện Trạm Tấu", ParentId = 15 },
                        new Location { Status = 1, Code = "140", Name = "Huyện Văn Chấn", ParentId = 15 },
                        new Location { Status = 1, Code = "141", Name = "Huyện Yên Bình", ParentId = 15 }
                    );

                    // Các quận, huyện thuộc tỉnh Hòa Bình
                    appContext.AddRange(
                        new Location { Status = 1, Code = "148", Name = "Thành phố Hòa Bình", ParentId = 17 },
                        new Location { Status = 1, Code = "150", Name = "Huyện Đà Bắc", ParentId = 17 },
                        new Location { Status = 1, Code = "152", Name = "Huyện Lương Sơn", ParentId = 17 },
                        new Location { Status = 1, Code = "153", Name = "Huyện Kim Bôi", ParentId = 17 },
                        new Location { Status = 1, Code = "154", Name = "Huyện Cao Phong", ParentId = 17 },
                        new Location { Status = 1, Code = "155", Name = "Huyện Tân Lạc", ParentId = 17 },
                        new Location { Status = 1, Code = "156", Name = "Huyện Mai Châu", ParentId = 17 },
                        new Location { Status = 1, Code = "157", Name = "Huyện Lạc Sơn", ParentId = 17 },
                        new Location { Status = 1, Code = "158", Name = "Huyện Yên Thủy", ParentId = 17 },
                        new Location { Status = 1, Code = "159", Name = "Huyện Lạc Thủy", ParentId = 17 }
                    );

                    // Các quận, huyện thuộc tỉnh Thái Nguyên
                    appContext.AddRange(
                        new Location { Status = 1, Code = "164", Name = "Thành phố Thái Nguyên", ParentId = 19 },
                        new Location { Status = 1, Code = "165", Name = "Thành phố Sông Công", ParentId = 19 },
                        new Location { Status = 1, Code = "167", Name = "Huyện Định Hóa", ParentId = 19 },
                        new Location { Status = 1, Code = "168", Name = "Huyện Phú Lương", ParentId = 19 },
                        new Location { Status = 1, Code = "169", Name = "Huyện Đồng Hỷ", ParentId = 19 },
                        new Location { Status = 1, Code = "170", Name = "Huyện Võ Nhai", ParentId = 19 },
                        new Location { Status = 1, Code = "171", Name = "Huyện Đại Từ", ParentId = 19 },
                        new Location { Status = 1, Code = "172", Name = "Thị xã Phổ Yên", ParentId = 19 },
                        new Location { Status = 1, Code = "173", Name = "Huyện Phú Bình", ParentId = 19 }
                    );

                    // Các quận, huyện thuộc tỉnh Lạng Sơn
                    appContext.AddRange(
                        new Location { Status = 1, Code = "178", Name = "Thành phố Lạng Sơn", ParentId = 20 },
                        new Location { Status = 1, Code = "180", Name = "Huyện Tràng Định", ParentId = 20 },
                        new Location { Status = 1, Code = "181", Name = "Huyện Bình Gia", ParentId = 20 },
                        new Location { Status = 1, Code = "182", Name = "Huyện Văn Lãng", ParentId = 20 },
                        new Location { Status = 1, Code = "183", Name = "Huyện Cao Lộc", ParentId = 20 },
                        new Location { Status = 1, Code = "184", Name = "Huyện Văn Quan", ParentId = 20 },
                        new Location { Status = 1, Code = "185", Name = "Huyện Bắc Sơn", ParentId = 20 },
                        new Location { Status = 1, Code = "186", Name = "Huyện Hữu Lũng", ParentId = 20 },
                        new Location { Status = 1, Code = "187", Name = "Huyện Chi Lăng", ParentId = 20 },
                        new Location { Status = 1, Code = "188", Name = "Huyện Lộc Bình", ParentId = 20 },
                        new Location { Status = 1, Code = "189", Name = "Huyện Đình Lập", ParentId = 20 }
                    );

                    // Các quận, huyện thuộc tỉnh Quảng Ninh
                    appContext.AddRange(
                        new Location { Status = 1, Code = "193", Name = "Thành phố Hạ Long", ParentId = 22 },
                        new Location { Status = 1, Code = "194", Name = "Thành phố Móng Cái", ParentId = 22 },
                        new Location { Status = 1, Code = "195", Name = "Thành phố Cẩm Phả", ParentId = 22 },
                        new Location { Status = 1, Code = "196", Name = "Thành phố Uông Bí", ParentId = 22 },
                        new Location { Status = 1, Code = "198", Name = "Huyện Bình Liêu", ParentId = 22 },
                        new Location { Status = 1, Code = "199", Name = "Huyện Tiên Yên", ParentId = 22 },
                        new Location { Status = 1, Code = "200", Name = "Huyện Đầm Hà", ParentId = 22 },
                        new Location { Status = 1, Code = "201", Name = "Huyện Hải Hà", ParentId = 22 },
                        new Location { Status = 1, Code = "202", Name = "Huyện Ba Chẽ", ParentId = 22 },
                        new Location { Status = 1, Code = "203", Name = "Huyện Vân Đồn", ParentId = 22 },
                        new Location { Status = 1, Code = "205", Name = "Thị xã Đông Triều", ParentId = 22 },
                        new Location { Status = 1, Code = "206", Name = "Thị xã Quảng Yên", ParentId = 22 },
                        new Location { Status = 1, Code = "207", Name = "Huyện Cô Tô", ParentId = 22 }
                    );

                    // Các quận, huyện thuộc tỉnh Bắc Giang
                    appContext.AddRange(
                        new Location { Status = 1, Code = "213", Name = "Thành phố Bắc Giang", ParentId = 24 },
                        new Location { Status = 1, Code = "215", Name = "Huyện Yên Thế", ParentId = 24 },
                        new Location { Status = 1, Code = "216", Name = "Huyện Tân Yên", ParentId = 24 },
                        new Location { Status = 1, Code = "217", Name = "Huyện Lạng Giang", ParentId = 24 },
                        new Location { Status = 1, Code = "218", Name = "Huyện Lục Nam", ParentId = 24 },
                        new Location { Status = 1, Code = "219", Name = "Huyện Lục Ngạn", ParentId = 24 },
                        new Location { Status = 1, Code = "220", Name = "Huyện Sơn Động", ParentId = 24 },
                        new Location { Status = 1, Code = "221", Name = "Huyện Yên Dũng", ParentId = 24 },
                        new Location { Status = 1, Code = "222", Name = "Huyện Việt Yên", ParentId = 24 },
                        new Location { Status = 1, Code = "223", Name = "Huyện Hiệp Hòa", ParentId = 24 }
                    );

                    // Các quận, huyện thuộc tỉnh Phú Thọ
                    appContext.AddRange(
                        new Location { Status = 1, Code = "227", Name = "Thành phố Việt Trì", ParentId = 25 },
                        new Location { Status = 1, Code = "228", Name = "Thị xã Phú Thọ", ParentId = 25 },
                        new Location { Status = 1, Code = "230", Name = "Huyện Đoan Hùng", ParentId = 25 },
                        new Location { Status = 1, Code = "231", Name = "Huyện Hạ Hoà", ParentId = 25 },
                        new Location { Status = 1, Code = "232", Name = "Huyện Thanh Ba", ParentId = 25 },
                        new Location { Status = 1, Code = "233", Name = "Huyện Phù Ninh", ParentId = 25 },
                        new Location { Status = 1, Code = "234", Name = "Huyện Yên Lập", ParentId = 25 },
                        new Location { Status = 1, Code = "235", Name = "Huyện Cẩm Khê", ParentId = 25 },
                        new Location { Status = 1, Code = "236", Name = "Huyện Tam Nông", ParentId = 25 },
                        new Location { Status = 1, Code = "237", Name = "Huyện Lâm Thao", ParentId = 25 },
                        new Location { Status = 1, Code = "238", Name = "Huyện Thanh Sơn", ParentId = 25 },
                        new Location { Status = 1, Code = "239", Name = "Huyện Thanh Thuỷ", ParentId = 25 },
                        new Location { Status = 1, Code = "240", Name = "Huyện Tân Sơn", ParentId = 25 }
                    );

                    // Các quận, huyện thuộc tỉnh Vĩnh Phúc
                    appContext.AddRange(
                        new Location { Status = 1, Code = "243", Name = "Thành phố Vĩnh Yên", ParentId = 26 },
                        new Location { Status = 1, Code = "244", Name = "Thành phố Phúc Yên", ParentId = 26 },
                        new Location { Status = 1, Code = "246", Name = "Huyện Lập Thạch", ParentId = 26 },
                        new Location { Status = 1, Code = "247", Name = "Huyện Tam Dương", ParentId = 26 },
                        new Location { Status = 1, Code = "248", Name = "Huyện Tam Đảo", ParentId = 26 },
                        new Location { Status = 1, Code = "249", Name = "Huyện Bình Xuyên", ParentId = 26 },
                        new Location { Status = 1, Code = "251", Name = "Huyện Yên Lạc", ParentId = 26 },
                        new Location { Status = 1, Code = "252", Name = "Huyện Vĩnh Tường", ParentId = 26 },
                        new Location { Status = 1, Code = "253", Name = "Huyện Sông Lô", ParentId = 26 }
                    );

                    // Các quận, huyện thuộc tỉnh Bắc Ninh
                    appContext.AddRange(
                        new Location { Status = 1, Code = "256", Name = "Thành phố Bắc Ninh", ParentId = 27 },
                        new Location { Status = 1, Code = "258", Name = "Huyện Yên Phong", ParentId = 27 },
                        new Location { Status = 1, Code = "259", Name = "Huyện Quế Võ", ParentId = 27 },
                        new Location { Status = 1, Code = "260", Name = "Huyện Tiên Du", ParentId = 27 },
                        new Location { Status = 1, Code = "261", Name = "Thị xã Từ Sơn", ParentId = 27 },
                        new Location { Status = 1, Code = "262", Name = "Huyện Thuận Thành", ParentId = 27 },
                        new Location { Status = 1, Code = "263", Name = "Huyện Gia Bình", ParentId = 27 },
                        new Location { Status = 1, Code = "264", Name = "Huyện Lương Tài", ParentId = 27 }
                    );

                    // Các quận, huyện thuộc tỉnh Hải Dương
                    appContext.AddRange(
                        new Location { Status = 1, Code = "288", Name = "Thành phố Hải Dương", ParentId = 30 },
                        new Location { Status = 1, Code = "290", Name = "Thành phố Chí Linh", ParentId = 30 },
                        new Location { Status = 1, Code = "291", Name = "Huyện Nam Sách", ParentId = 30 },
                        new Location { Status = 1, Code = "292", Name = "Thị xã Kinh Môn", ParentId = 30 },
                        new Location { Status = 1, Code = "293", Name = "Huyện Kim Thành", ParentId = 30 },
                        new Location { Status = 1, Code = "294", Name = "Huyện Thanh Hà", ParentId = 30 },
                        new Location { Status = 1, Code = "295", Name = "Huyện Cẩm Giàng", ParentId = 30 },
                        new Location { Status = 1, Code = "296", Name = "Huyện Bình Giang", ParentId = 30 },
                        new Location { Status = 1, Code = "297", Name = "Huyện Gia Lộc", ParentId = 30 },
                        new Location { Status = 1, Code = "298", Name = "Huyện Tứ Kỳ", ParentId = 30 },
                        new Location { Status = 1, Code = "299", Name = "Huyện Ninh Giang", ParentId = 30 },
                        new Location { Status = 1, Code = "300", Name = "Huyện Thanh Miện", ParentId = 30 }
                    );

                    // Các quận, huyện thuộc thành phố Hải Phòng
                    appContext.AddRange(
                        new Location { Status = 1, Code = "303", Name = "Quận Hồng Bàng", ParentId = 31 },
                        new Location { Status = 1, Code = "304", Name = "Quận Ngô Quyền", ParentId = 31 },
                        new Location { Status = 1, Code = "305", Name = "Quận Lê Chân", ParentId = 31 },
                        new Location { Status = 1, Code = "306", Name = "Quận Hải An", ParentId = 31 },
                        new Location { Status = 1, Code = "307", Name = "Quận Kiến An", ParentId = 31 },
                        new Location { Status = 1, Code = "308", Name = "Quận Đồ Sơn", ParentId = 31 },
                        new Location { Status = 1, Code = "309", Name = "Quận Dương Kinh", ParentId = 31 },
                        new Location { Status = 1, Code = "311", Name = "Huyện Thuỷ Nguyên", ParentId = 31 },
                        new Location { Status = 1, Code = "312", Name = "Huyện An Dương", ParentId = 31 },
                        new Location { Status = 1, Code = "313", Name = "Huyện An Lão", ParentId = 31 },
                        new Location { Status = 1, Code = "314", Name = "Huyện Kiến Thuỵ", ParentId = 31 },
                        new Location { Status = 1, Code = "315", Name = "Huyện Tiên Lãng", ParentId = 31 },
                        new Location { Status = 1, Code = "316", Name = "Huyện Vĩnh Bảo", ParentId = 31 },
                        new Location { Status = 1, Code = "317", Name = "Huyện Cát Hải", ParentId = 31 },
                        new Location { Status = 1, Code = "318", Name = "Huyện Bạch Long Vĩ", ParentId = 31 }
                    );

                    // Các quận, huyện thuộc tỉnh Hưng Yên
                    appContext.AddRange(
                        new Location { Status = 1, Code = "323", Name = "Thành phố Hưng Yên", ParentId = 33 },
                        new Location { Status = 1, Code = "325", Name = "Huyện Văn Lâm", ParentId = 33 },
                        new Location { Status = 1, Code = "326", Name = "Huyện Văn Giang", ParentId = 33 },
                        new Location { Status = 1, Code = "327", Name = "Huyện Yên Mỹ", ParentId = 33 },
                        new Location { Status = 1, Code = "328", Name = "Thị xã Mỹ Hào", ParentId = 33 },
                        new Location { Status = 1, Code = "329", Name = "Huyện Ân Thi", ParentId = 33 },
                        new Location { Status = 1, Code = "330", Name = "Huyện Khoái Châu", ParentId = 33 },
                        new Location { Status = 1, Code = "331", Name = "Huyện Kim Động", ParentId = 33 },
                        new Location { Status = 1, Code = "332", Name = "Huyện Tiên Lữ", ParentId = 33 },
                        new Location { Status = 1, Code = "333", Name = "Huyện Phù Cừ", ParentId = 33 }
                    );

                    // Các quận, huyện thuộc tỉnh Thái Bình
                    appContext.AddRange(
                        new Location { Status = 1, Code = "336", Name = "Thành phố Thái Bình", ParentId = 34 },
                        new Location { Status = 1, Code = "338", Name = "Huyện Quỳnh Phụ", ParentId = 34 },
                        new Location { Status = 1, Code = "339", Name = "Huyện Hưng Hà", ParentId = 34 },
                        new Location { Status = 1, Code = "340", Name = "Huyện Đông Hưng", ParentId = 34 },
                        new Location { Status = 1, Code = "341", Name = "Huyện Thái Thụy", ParentId = 34 },
                        new Location { Status = 1, Code = "342", Name = "Huyện Tiền Hải", ParentId = 34 },
                        new Location { Status = 1, Code = "343", Name = "Huyện Kiến Xương", ParentId = 34 },
                        new Location { Status = 1, Code = "344", Name = "Huyện Vũ Thư", ParentId = 34 }
                    );

                    // Các quận, huyện thuộc tỉnh Hà Nam
                    appContext.AddRange(
                        new Location { Status = 1, Code = "347", Name = "Thành phố Phủ Lý", ParentId = 35 },
                        new Location { Status = 1, Code = "349", Name = "Thị xã Duy Tiên", ParentId = 35 },
                        new Location { Status = 1, Code = "350", Name = "Huyện Kim Bảng", ParentId = 35 },
                        new Location { Status = 1, Code = "351", Name = "Huyện Thanh Liêm", ParentId = 35 },
                        new Location { Status = 1, Code = "352", Name = "Huyện Bình Lục", ParentId = 35 },
                        new Location { Status = 1, Code = "353", Name = "Huyện Lý Nhân", ParentId = 35 }
                    );

                    // Các quận, huyện thuộc tỉnh Nam Định
                    appContext.AddRange(
                        new Location { Status = 1, Code = "356", Name = "Thành phố Nam Định", ParentId = 36 },
                        new Location { Status = 1, Code = "358", Name = "Huyện Mỹ Lộc", ParentId = 36 },
                        new Location { Status = 1, Code = "359", Name = "Huyện Vụ Bản", ParentId = 36 },
                        new Location { Status = 1, Code = "360", Name = "Huyện Ý Yên", ParentId = 36 },
                        new Location { Status = 1, Code = "361", Name = "Huyện Nghĩa Hưng", ParentId = 36 },
                        new Location { Status = 1, Code = "362", Name = "Huyện Nam Trực", ParentId = 36 },
                        new Location { Status = 1, Code = "363", Name = "Huyện Trực Ninh", ParentId = 36 },
                        new Location { Status = 1, Code = "364", Name = "Huyện Xuân Trường", ParentId = 36 },
                        new Location { Status = 1, Code = "365", Name = "Huyện Giao Thủy", ParentId = 36 },
                        new Location { Status = 1, Code = "366", Name = "Huyện Hải Hậu", ParentId = 36 }
                    );

                    // Các quận, huyện thuộc tỉnh Ninh Bình
                    appContext.AddRange(
                        new Location { Status = 1, Code = "369", Name = "Thành phố Ninh Bình", ParentId = 37 },
                        new Location { Status = 1, Code = "370", Name = "Thành phố Tam Điệp", ParentId = 37 },
                        new Location { Status = 1, Code = "372", Name = "Huyện Nho Quan", ParentId = 37 },
                        new Location { Status = 1, Code = "373", Name = "Huyện Gia Viễn", ParentId = 37 },
                        new Location { Status = 1, Code = "374", Name = "Huyện Hoa Lư", ParentId = 37 },
                        new Location { Status = 1, Code = "375", Name = "Huyện Yên Khánh", ParentId = 37 },
                        new Location { Status = 1, Code = "376", Name = "Huyện Kim Sơn", ParentId = 37 },
                        new Location { Status = 1, Code = "377", Name = "Huyện Yên Mô", ParentId = 37 }
                    );

                    // Các quận, huyện thuộc tỉnh Thanh Hóa
                    appContext.AddRange(
                        new Location { Status = 1, Code = "380", Name = "Thành phố Thanh Hóa", ParentId = 38 },
                        new Location { Status = 1, Code = "381", Name = "Thị xã Bỉm Sơn", ParentId = 38 },
                        new Location { Status = 1, Code = "382", Name = "Thành phố Sầm Sơn", ParentId = 38 },
                        new Location { Status = 1, Code = "384", Name = "Huyện Mường Lát", ParentId = 38 },
                        new Location { Status = 1, Code = "385", Name = "Huyện Quan Hóa", ParentId = 38 },
                        new Location { Status = 1, Code = "386", Name = "Huyện Bá Thước", ParentId = 38 },
                        new Location { Status = 1, Code = "387", Name = "Huyện Quan Sơn", ParentId = 38 },
                        new Location { Status = 1, Code = "388", Name = "Huyện Lang Chánh", ParentId = 38 },
                        new Location { Status = 1, Code = "389", Name = "Huyện Ngọc Lặc", ParentId = 38 },
                        new Location { Status = 1, Code = "390", Name = "Huyện Cẩm Thủy", ParentId = 38 },
                        new Location { Status = 1, Code = "391", Name = "Huyện Thạch Thành", ParentId = 38 },
                        new Location { Status = 1, Code = "392", Name = "Huyện Hà Trung", ParentId = 38 },
                        new Location { Status = 1, Code = "393", Name = "Huyện Vĩnh Lộc", ParentId = 38 },
                        new Location { Status = 1, Code = "394", Name = "Huyện Yên Định", ParentId = 38 },
                        new Location { Status = 1, Code = "395", Name = "Huyện Thọ Xuân", ParentId = 38 },
                        new Location { Status = 1, Code = "396", Name = "Huyện Thường Xuân", ParentId = 38 },
                        new Location { Status = 1, Code = "397", Name = "Huyện Triệu Sơn", ParentId = 38 },
                        new Location { Status = 1, Code = "398", Name = "Huyện Thiệu Hóa", ParentId = 38 },
                        new Location { Status = 1, Code = "399", Name = "Huyện Hoằng Hóa", ParentId = 38 },
                        new Location { Status = 1, Code = "400", Name = "Huyện Hậu Lộc", ParentId = 38 },
                        new Location { Status = 1, Code = "401", Name = "Huyện Nga Sơn", ParentId = 38 },
                        new Location { Status = 1, Code = "402", Name = "Huyện Như Xuân", ParentId = 38 },
                        new Location { Status = 1, Code = "403", Name = "Huyện Như Thanh", ParentId = 38 },
                        new Location { Status = 1, Code = "404", Name = "Huyện Nông Cống", ParentId = 38 },
                        new Location { Status = 1, Code = "405", Name = "Huyện Đông Sơn", ParentId = 38 },
                        new Location { Status = 1, Code = "406", Name = "Huyện Quảng Xương", ParentId = 38 },
                        new Location { Status = 1, Code = "407", Name = "Huyện Tĩnh Gia", ParentId = 38 }
                    );

                    // Các quận, huyện thuộc tỉnh Nghệ An
                    appContext.AddRange(
                        new Location { Status = 1, Code = "412", Name = "Thành phố Vinh", ParentId = 40 },
                        new Location { Status = 1, Code = "413", Name = "Thị xã Cửa Lò", ParentId = 40 },
                        new Location { Status = 1, Code = "414", Name = "Thị xã Thái Hoà", ParentId = 40 },
                        new Location { Status = 1, Code = "415", Name = "Huyện Quế Phong", ParentId = 40 },
                        new Location { Status = 1, Code = "416", Name = "Huyện Quỳ Châu", ParentId = 40 },
                        new Location { Status = 1, Code = "417", Name = "Huyện Kỳ Sơn", ParentId = 40 },
                        new Location { Status = 1, Code = "418", Name = "Huyện Tương Dương", ParentId = 40 },
                        new Location { Status = 1, Code = "419", Name = "Huyện Nghĩa Đàn", ParentId = 40 },
                        new Location { Status = 1, Code = "420", Name = "Huyện Quỳ Hợp", ParentId = 40 },
                        new Location { Status = 1, Code = "421", Name = "Huyện Quỳnh Lưu", ParentId = 40 },
                        new Location { Status = 1, Code = "422", Name = "Huyện Con Cuông", ParentId = 40 },
                        new Location { Status = 1, Code = "423", Name = "Huyện Tân Kỳ", ParentId = 40 },
                        new Location { Status = 1, Code = "424", Name = "Huyện Anh Sơn", ParentId = 40 },
                        new Location { Status = 1, Code = "425", Name = "Huyện Diễn Châu", ParentId = 40 },
                        new Location { Status = 1, Code = "426", Name = "Huyện Yên Thành", ParentId = 40 },
                        new Location { Status = 1, Code = "427", Name = "Huyện Đô Lương", ParentId = 40 },
                        new Location { Status = 1, Code = "428", Name = "Huyện Thanh Chương", ParentId = 40 },
                        new Location { Status = 1, Code = "429", Name = "Huyện Nghi Lộc", ParentId = 40 },
                        new Location { Status = 1, Code = "430", Name = "Huyện Nam Đàn", ParentId = 40 },
                        new Location { Status = 1, Code = "431", Name = "Huyện Hưng Nguyên", ParentId = 40 },
                        new Location { Status = 1, Code = "432", Name = "Thị xã Hoàng Mai", ParentId = 40 }
                    );

                    // Các quận, huyện thuộc tỉnh Hà Tĩnh
                    appContext.AddRange(
                        new Location { Status = 1, Code = "436", Name = "Thành phố Hà Tĩnh", ParentId = 42 },
                        new Location { Status = 1, Code = "437", Name = "Thị xã Hồng Lĩnh", ParentId = 42 },
                        new Location { Status = 1, Code = "439", Name = "Huyện Hương Sơn", ParentId = 42 },
                        new Location { Status = 1, Code = "440", Name = "Huyện Đức Thọ", ParentId = 42 },
                        new Location { Status = 1, Code = "441", Name = "Huyện Vũ Quang", ParentId = 42 },
                        new Location { Status = 1, Code = "442", Name = "Huyện Nghi Xuân", ParentId = 42 },
                        new Location { Status = 1, Code = "443", Name = "Huyện Can Lộc", ParentId = 42 },
                        new Location { Status = 1, Code = "444", Name = "Huyện Hương Khê", ParentId = 42 },
                        new Location { Status = 1, Code = "445", Name = "Huyện Thạch Hà", ParentId = 42 },
                        new Location { Status = 1, Code = "446", Name = "Huyện Cẩm Xuyên", ParentId = 42 },
                        new Location { Status = 1, Code = "447", Name = "Huyện Kỳ Anh", ParentId = 42 },
                        new Location { Status = 1, Code = "448", Name = "Huyện Lộc Hà", ParentId = 42 },
                        new Location { Status = 1, Code = "449", Name = "Thị xã Kỳ Anh", ParentId = 42 }
                    );

                    // Các quận, huyện thuộc tỉnh Quảng Bình
                    appContext.AddRange(
                        new Location { Status = 1, Code = "450", Name = "Thành Phố Đồng Hới", ParentId = 44 },
                        new Location { Status = 1, Code = "452", Name = "Huyện Minh Hóa", ParentId = 44 },
                        new Location { Status = 1, Code = "453", Name = "Huyện Tuyên Hóa", ParentId = 44 },
                        new Location { Status = 1, Code = "454", Name = "Huyện Quảng Trạch", ParentId = 44 },
                        new Location { Status = 1, Code = "455", Name = "Huyện Bố Trạch", ParentId = 44 },
                        new Location { Status = 1, Code = "456", Name = "Huyện Quảng Ninh", ParentId = 44 },
                        new Location { Status = 1, Code = "457", Name = "Huyện Lệ Thủy", ParentId = 44 },
                        new Location { Status = 1, Code = "458", Name = "Thị xã Ba Đồn", ParentId = 44 }
                    );

                    // Các quận, huyện thuộc tỉnh Quảng Trị
                    appContext.AddRange(
                        new Location { Status = 1, Code = "461", Name = "Thành phố Đông Hà", ParentId = 45 },
                        new Location { Status = 1, Code = "462", Name = "Thị xã Quảng Trị", ParentId = 45 },
                        new Location { Status = 1, Code = "464", Name = "Huyện Vĩnh Linh", ParentId = 45 },
                        new Location { Status = 1, Code = "465", Name = "Huyện Hướng Hóa", ParentId = 45 },
                        new Location { Status = 1, Code = "466", Name = "Huyện Gio Linh", ParentId = 45 },
                        new Location { Status = 1, Code = "467", Name = "Huyện Đa Krông", ParentId = 45 },
                        new Location { Status = 1, Code = "468", Name = "Huyện Cam Lộ", ParentId = 45 },
                        new Location { Status = 1, Code = "469", Name = "Huyện Triệu Phong", ParentId = 45 },
                        new Location { Status = 1, Code = "470", Name = "Huyện Hải Lăng", ParentId = 45 },
                        new Location { Status = 1, Code = "471", Name = "Huyện Cồn Cỏ", ParentId = 45 }
                    );

                    // Các quận, huyện thuộc tỉnh Thừa Thiên Huế
                    appContext.AddRange(
                        new Location { Status = 1, Code = "474", Name = "Thành phố Huế", ParentId = 46 },
                        new Location { Status = 1, Code = "476", Name = "Huyện Phong Điền", ParentId = 46 },
                        new Location { Status = 1, Code = "477", Name = "Huyện Quảng Điền", ParentId = 46 },
                        new Location { Status = 1, Code = "478", Name = "Huyện Phú Vang", ParentId = 46 },
                        new Location { Status = 1, Code = "479", Name = "Thị xã Hương Thủy", ParentId = 46 },
                        new Location { Status = 1, Code = "480", Name = "Thị xã Hương Trà", ParentId = 46 },
                        new Location { Status = 1, Code = "481", Name = "Huyện A Lưới", ParentId = 46 },
                        new Location { Status = 1, Code = "482", Name = "Huyện Phú Lộc", ParentId = 46 },
                        new Location { Status = 1, Code = "483", Name = "Huyện Nam Đông", ParentId = 46 }
                    );

                    // Các quận, huyện thuộc thành phố Đà Nẵng
                    appContext.AddRange(
                        new Location { Status = 1, Code = "490", Name = "Quận Liên Chiểu", ParentId = 48 },
                        new Location { Status = 1, Code = "491", Name = "Quận Thanh Khê", ParentId = 48 },
                        new Location { Status = 1, Code = "492", Name = "Quận Hải Châu", ParentId = 48 },
                        new Location { Status = 1, Code = "493", Name = "Quận Sơn Trà", ParentId = 48 },
                        new Location { Status = 1, Code = "494", Name = "Quận Ngũ Hành Sơn", ParentId = 48 },
                        new Location { Status = 1, Code = "495", Name = "Quận Cẩm Lệ", ParentId = 48 },
                        new Location { Status = 1, Code = "497", Name = "Huyện Hòa Vang", ParentId = 48 },
                        new Location { Status = 1, Code = "498", Name = "Huyện Hoàng Sa", ParentId = 48 }
                    );

                    // Các quận, huyện thuộc tỉnh Quảng Nam
                    appContext.AddRange(
                        new Location { Status = 1, Code = "502", Name = "Thành phố Tam Kỳ", ParentId = 49 },
                        new Location { Status = 1, Code = "503", Name = "Thành phố Hội An", ParentId = 49 },
                        new Location { Status = 1, Code = "504", Name = "Huyện Tây Giang", ParentId = 49 },
                        new Location { Status = 1, Code = "505", Name = "Huyện Đông Giang", ParentId = 49 },
                        new Location { Status = 1, Code = "506", Name = "Huyện Đại Lộc", ParentId = 49 },
                        new Location { Status = 1, Code = "507", Name = "Thị xã Điện Bàn", ParentId = 49 },
                        new Location { Status = 1, Code = "508", Name = "Huyện Duy Xuyên", ParentId = 49 },
                        new Location { Status = 1, Code = "509", Name = "Huyện Quế Sơn", ParentId = 49 },
                        new Location { Status = 1, Code = "510", Name = "Huyện Nam Giang", ParentId = 49 },
                        new Location { Status = 1, Code = "511", Name = "Huyện Phước Sơn", ParentId = 49 },
                        new Location { Status = 1, Code = "512", Name = "Huyện Hiệp Đức", ParentId = 49 },
                        new Location { Status = 1, Code = "513", Name = "Huyện Thăng Bình", ParentId = 49 },
                        new Location { Status = 1, Code = "514", Name = "Huyện Tiên Phước", ParentId = 49 },
                        new Location { Status = 1, Code = "515", Name = "Huyện Bắc Trà My", ParentId = 49 },
                        new Location { Status = 1, Code = "516", Name = "Huyện Nam Trà My", ParentId = 49 },
                        new Location { Status = 1, Code = "517", Name = "Huyện Núi Thành", ParentId = 49 },
                        new Location { Status = 1, Code = "518", Name = "Huyện Phú Ninh", ParentId = 49 },
                        new Location { Status = 1, Code = "519", Name = "Huyện Nông Sơn", ParentId = 49 }
                    );

                    // Các quận, huyện thuộc tỉnh Quảng Ngãi
                    appContext.AddRange(
                        new Location { Status = 1, Code = "522", Name = "Thành phố Quảng Ngãi", ParentId = 51 },
                        new Location { Status = 1, Code = "524", Name = "Huyện Bình Sơn", ParentId = 51 },
                        new Location { Status = 1, Code = "525", Name = "Huyện Trà Bồng", ParentId = 51 },
                        new Location { Status = 1, Code = "527", Name = "Huyện Sơn Tịnh", ParentId = 51 },
                        new Location { Status = 1, Code = "528", Name = "Huyện Tư Nghĩa", ParentId = 51 },
                        new Location { Status = 1, Code = "529", Name = "Huyện Sơn Hà", ParentId = 51 },
                        new Location { Status = 1, Code = "530", Name = "Huyện Sơn Tây", ParentId = 51 },
                        new Location { Status = 1, Code = "531", Name = "Huyện Minh Long", ParentId = 51 },
                        new Location { Status = 1, Code = "532", Name = "Huyện Nghĩa Hành", ParentId = 51 },
                        new Location { Status = 1, Code = "533", Name = "Huyện Mộ Đức", ParentId = 51 },
                        new Location { Status = 1, Code = "534", Name = "Thị xã Đức Phổ", ParentId = 51 },
                        new Location { Status = 1, Code = "535", Name = "Huyện Ba Tơ", ParentId = 51 },
                        new Location { Status = 1, Code = "536", Name = "Huyện Lý Sơn", ParentId = 51 }
                    );

                    // Các quận, huyện thuộc tỉnh Bình Định
                    appContext.AddRange(
                        new Location { Status = 1, Code = "540", Name = "Thành phố Qui Nhơn", ParentId = 52 },
                        new Location { Status = 1, Code = "542", Name = "Huyện An Lão", ParentId = 52 },
                        new Location { Status = 1, Code = "543", Name = "Huyện Hoài Nhơn", ParentId = 52 },
                        new Location { Status = 1, Code = "544", Name = "Huyện Hoài Ân", ParentId = 52 },
                        new Location { Status = 1, Code = "545", Name = "Huyện Phù Mỹ", ParentId = 52 },
                        new Location { Status = 1, Code = "546", Name = "Huyện Vĩnh Thạnh", ParentId = 52 },
                        new Location { Status = 1, Code = "547", Name = "Huyện Tây Sơn", ParentId = 52 },
                        new Location { Status = 1, Code = "548", Name = "Huyện Phù Cát", ParentId = 52 },
                        new Location { Status = 1, Code = "549", Name = "Thị xã An Nhơn", ParentId = 52 },
                        new Location { Status = 1, Code = "550", Name = "Huyện Tuy Phước", ParentId = 52 },
                        new Location { Status = 1, Code = "551", Name = "Huyện Vân Canh", ParentId = 52 }
                    );

                    // Các quận, huyện thuộc tỉnh Phú Yên
                    appContext.AddRange(
                        new Location { Status = 1, Code = "555", Name = "Thành phố Tuy Hoà", ParentId = 54 },
                        new Location { Status = 1, Code = "557", Name = "Thị xã Sông Cầu", ParentId = 54 },
                        new Location { Status = 1, Code = "558", Name = "Huyện Đồng Xuân", ParentId = 54 },
                        new Location { Status = 1, Code = "559", Name = "Huyện Tuy An", ParentId = 54 },
                        new Location { Status = 1, Code = "560", Name = "Huyện Sơn Hòa", ParentId = 54 },
                        new Location { Status = 1, Code = "561", Name = "Huyện Sông Hinh", ParentId = 54 },
                        new Location { Status = 1, Code = "562", Name = "Huyện Tây Hoà", ParentId = 54 },
                        new Location { Status = 1, Code = "563", Name = "Huyện Phú Hoà", ParentId = 54 },
                        new Location { Status = 1, Code = "564", Name = "Huyện Đông Hòa", ParentId = 54 }
                    );

                    // Các quận, huyện thuộc tỉnh Khánh Hòa
                    appContext.AddRange(
                        new Location { Status = 1, Code = "568", Name = "Thành phố Nha Trang", ParentId = 56 },
                        new Location { Status = 1, Code = "569", Name = "Thành phố Cam Ranh", ParentId = 56 },
                        new Location { Status = 1, Code = "570", Name = "Huyện Cam Lâm", ParentId = 56 },
                        new Location { Status = 1, Code = "571", Name = "Huyện Vạn Ninh", ParentId = 56 },
                        new Location { Status = 1, Code = "572", Name = "Thị xã Ninh Hòa", ParentId = 56 },
                        new Location { Status = 1, Code = "573", Name = "Huyện Khánh Vĩnh", ParentId = 56 },
                        new Location { Status = 1, Code = "574", Name = "Huyện Diên Khánh", ParentId = 56 },
                        new Location { Status = 1, Code = "575", Name = "Huyện Khánh Sơn", ParentId = 56 },
                        new Location { Status = 1, Code = "576", Name = "Huyện Trường Sa", ParentId = 56 }
                    );

                    // Các quận, huyện thuộc tỉnh Ninh Thuận
                    appContext.AddRange(
                        new Location { Status = 1, Code = "582", Name = "Thành phố Phan Rang-Tháp Chàm", ParentId = 58 },
                        new Location { Status = 1, Code = "584", Name = "Huyện Bác Ái", ParentId = 58 },
                        new Location { Status = 1, Code = "585", Name = "Huyện Ninh Sơn", ParentId = 58 },
                        new Location { Status = 1, Code = "586", Name = "Huyện Ninh Hải", ParentId = 58 },
                        new Location { Status = 1, Code = "587", Name = "Huyện Ninh Phước", ParentId = 58 },
                        new Location { Status = 1, Code = "588", Name = "Huyện Thuận Bắc", ParentId = 58 },
                        new Location { Status = 1, Code = "589", Name = "Huyện Thuận Nam", ParentId = 58 }
                    );

                    // Các quận, huyện thuộc tỉnh Bình Thuận
                    appContext.AddRange(
                        new Location { Status = 1, Code = "593", Name = "Thành phố Phan Thiết", ParentId = 60 },
                        new Location { Status = 1, Code = "594", Name = "Thị xã La Gi", ParentId = 60 },
                        new Location { Status = 1, Code = "595", Name = "Huyện Tuy Phong", ParentId = 60 },
                        new Location { Status = 1, Code = "596", Name = "Huyện Bắc Bình", ParentId = 60 },
                        new Location { Status = 1, Code = "597", Name = "Huyện Hàm Thuận Bắc", ParentId = 60 },
                        new Location { Status = 1, Code = "598", Name = "Huyện Hàm Thuận Nam", ParentId = 60 },
                        new Location { Status = 1, Code = "599", Name = "Huyện Tánh Linh", ParentId = 60 },
                        new Location { Status = 1, Code = "600", Name = "Huyện Đức Linh", ParentId = 60 },
                        new Location { Status = 1, Code = "601", Name = "Huyện Hàm Tân", ParentId = 60 },
                        new Location { Status = 1, Code = "602", Name = "Huyện Phú Quí", ParentId = 60 }
                    );

                    // Các quận, huyện thuộc tỉnh Kon Tum
                    appContext.AddRange(
                        new Location { Status = 1, Code = "608", Name = "Thành phố Kon Tum", ParentId = 62 },
                        new Location { Status = 1, Code = "610", Name = "Huyện Đắk Glei", ParentId = 62 },
                        new Location { Status = 1, Code = "611", Name = "Huyện Ngọc Hồi", ParentId = 62 },
                        new Location { Status = 1, Code = "612", Name = "Huyện Đắk Tô", ParentId = 62 },
                        new Location { Status = 1, Code = "613", Name = "Huyện Kon Plông", ParentId = 62 },
                        new Location { Status = 1, Code = "614", Name = "Huyện Kon Rẫy", ParentId = 62 },
                        new Location { Status = 1, Code = "615", Name = "Huyện Đắk Hà", ParentId = 62 },
                        new Location { Status = 1, Code = "616", Name = "Huyện Sa Thầy", ParentId = 62 },
                        new Location { Status = 1, Code = "617", Name = "Huyện Tu Mơ Rông", ParentId = 62 },
                        new Location { Status = 1, Code = "618", Name = "Huyện Ia H' Drai", ParentId = 62 }
                    );

                    // Các quận, huyện thuộc tỉnh Gia Lai
                    appContext.AddRange(
                        new Location { Status = 1, Code = "622", Name = "Thành phố Pleiku", ParentId = 64 },
                        new Location { Status = 1, Code = "623", Name = "Thị xã An Khê", ParentId = 64 },
                        new Location { Status = 1, Code = "624", Name = "Thị xã Ayun Pa", ParentId = 64 },
                        new Location { Status = 1, Code = "625", Name = "Huyện KBang", ParentId = 64 },
                        new Location { Status = 1, Code = "626", Name = "Huyện Đăk Đoa", ParentId = 64 },
                        new Location { Status = 1, Code = "627", Name = "Huyện Chư Păh", ParentId = 64 },
                        new Location { Status = 1, Code = "628", Name = "Huyện Ia Grai", ParentId = 64 },
                        new Location { Status = 1, Code = "629", Name = "Huyện Mang Yang", ParentId = 64 },
                        new Location { Status = 1, Code = "630", Name = "Huyện Kông Chro", ParentId = 64 },
                        new Location { Status = 1, Code = "631", Name = "Huyện Đức Cơ", ParentId = 64 },
                        new Location { Status = 1, Code = "632", Name = "Huyện Chư Prông", ParentId = 64 },
                        new Location { Status = 1, Code = "633", Name = "Huyện Chư Sê", ParentId = 64 },
                        new Location { Status = 1, Code = "634", Name = "Huyện Đăk Pơ", ParentId = 64 },
                        new Location { Status = 1, Code = "635", Name = "Huyện Ia Pa", ParentId = 64 },
                        new Location { Status = 1, Code = "637", Name = "Huyện Krông Pa", ParentId = 64 },
                        new Location { Status = 1, Code = "638", Name = "Huyện Phú Thiện", ParentId = 64 },
                        new Location { Status = 1, Code = "639", Name = "Huyện Chư Pưh", ParentId = 64 }
                    );

                    // Các quận, huyện thuộc tỉnh Đắk Lắk
                    appContext.AddRange(
                        new Location { Status = 1, Code = "643", Name = "Thành phố Buôn Ma Thuột", ParentId = 66 },
                        new Location { Status = 1, Code = "644", Name = "Thị Xã Buôn Hồ", ParentId = 66 },
                        new Location { Status = 1, Code = "645", Name = "Huyện Ea H'leo", ParentId = 66 },
                        new Location { Status = 1, Code = "646", Name = "Huyện Ea Súp", ParentId = 66 },
                        new Location { Status = 1, Code = "647", Name = "Huyện Buôn Đôn", ParentId = 66 },
                        new Location { Status = 1, Code = "648", Name = "Huyện Cư M'gar", ParentId = 66 },
                        new Location { Status = 1, Code = "649", Name = "Huyện Krông Búk", ParentId = 66 },
                        new Location { Status = 1, Code = "650", Name = "Huyện Krông Năng", ParentId = 66 },
                        new Location { Status = 1, Code = "651", Name = "Huyện Ea Kar", ParentId = 66 },
                        new Location { Status = 1, Code = "652", Name = "Huyện M'Đrắk", ParentId = 66 },
                        new Location { Status = 1, Code = "653", Name = "Huyện Krông Bông", ParentId = 66 },
                        new Location { Status = 1, Code = "654", Name = "Huyện Krông Pắc", ParentId = 66 },
                        new Location { Status = 1, Code = "655", Name = "Huyện Krông A Na", ParentId = 66 },
                        new Location { Status = 1, Code = "656", Name = "Huyện Lắk", ParentId = 66 },
                        new Location { Status = 1, Code = "657", Name = "Huyện Cư Kuin", ParentId = 66 }
                    );

                    // Các quận, huyện thuộc tỉnh Đắk Nông
                    appContext.AddRange(
                        new Location { Status = 1, Code = "660", Name = "Thành phố Gia Nghĩa", ParentId = 67 },
                        new Location { Status = 1, Code = "661", Name = "Huyện Đăk Glong", ParentId = 67 },
                        new Location { Status = 1, Code = "662", Name = "Huyện Cư Jút", ParentId = 67 },
                        new Location { Status = 1, Code = "663", Name = "Huyện Đắk Mil", ParentId = 67 },
                        new Location { Status = 1, Code = "664", Name = "Huyện Krông Nô", ParentId = 67 },
                        new Location { Status = 1, Code = "665", Name = "Huyện Đắk Song", ParentId = 67 },
                        new Location { Status = 1, Code = "666", Name = "Huyện Đắk R'Lấp", ParentId = 67 },
                        new Location { Status = 1, Code = "667", Name = "Huyện Tuy Đức", ParentId = 67 }
                    );

                    // Các quận, huyện thuộc tỉnh Lâm Đồng
                    appContext.AddRange(
                        new Location { Status = 1, Code = "672", Name = "Thành phố Đà Lạt", ParentId = 68 },
                        new Location { Status = 1, Code = "673", Name = "Thành phố Bảo Lộc", ParentId = 68 },
                        new Location { Status = 1, Code = "674", Name = "Huyện Đam Rông", ParentId = 68 },
                        new Location { Status = 1, Code = "675", Name = "Huyện Lạc Dương", ParentId = 68 },
                        new Location { Status = 1, Code = "676", Name = "Huyện Lâm Hà", ParentId = 68 },
                        new Location { Status = 1, Code = "677", Name = "Huyện Đơn Dương", ParentId = 68 },
                        new Location { Status = 1, Code = "678", Name = "Huyện Đức Trọng", ParentId = 68 },
                        new Location { Status = 1, Code = "679", Name = "Huyện Di Linh", ParentId = 68 },
                        new Location { Status = 1, Code = "680", Name = "Huyện Bảo Lâm", ParentId = 68 },
                        new Location { Status = 1, Code = "681", Name = "Huyện Đạ Huoai", ParentId = 68 },
                        new Location { Status = 1, Code = "682", Name = "Huyện Đạ Tẻh", ParentId = 68 },
                        new Location { Status = 1, Code = "683", Name = "Huyện Cát Tiên", ParentId = 68 }
                    );

                    // Các quận, huyện thuộc tỉnh Bình Phước
                    appContext.AddRange(
                        new Location { Status = 1, Code = "688", Name = "Thị xã Phước Long", ParentId = 70 },
                        new Location { Status = 1, Code = "689", Name = "Thành phố Đồng Xoài", ParentId = 70 },
                        new Location { Status = 1, Code = "690", Name = "Thị xã Bình Long", ParentId = 70 },
                        new Location { Status = 1, Code = "691", Name = "Huyện Bù Gia Mập", ParentId = 70 },
                        new Location { Status = 1, Code = "692", Name = "Huyện Lộc Ninh", ParentId = 70 },
                        new Location { Status = 1, Code = "693", Name = "Huyện Bù Đốp", ParentId = 70 },
                        new Location { Status = 1, Code = "694", Name = "Huyện Hớn Quản", ParentId = 70 },
                        new Location { Status = 1, Code = "695", Name = "Huyện Đồng Phú", ParentId = 70 },
                        new Location { Status = 1, Code = "696", Name = "Huyện Bù Đăng", ParentId = 70 },
                        new Location { Status = 1, Code = "697", Name = "Huyện Chơn Thành", ParentId = 70 },
                        new Location { Status = 1, Code = "698", Name = "Huyện Phú Riềng", ParentId = 70 }
                    );

                    // Các quận, huyện thuộc tỉnh Tây Ninh
                    appContext.AddRange(
                        new Location { Status = 1, Code = "703", Name = "Thành phố Tây Ninh", ParentId = 72 },
                        new Location { Status = 1, Code = "705", Name = "Huyện Tân Biên", ParentId = 72 },
                        new Location { Status = 1, Code = "706", Name = "Huyện Tân Châu", ParentId = 72 },
                        new Location { Status = 1, Code = "707", Name = "Huyện Dương Minh Châu", ParentId = 72 },
                        new Location { Status = 1, Code = "708", Name = "Huyện Châu Thành", ParentId = 72 },
                        new Location { Status = 1, Code = "709", Name = "Thị xã Hòa Thành", ParentId = 72 },
                        new Location { Status = 1, Code = "710", Name = "Huyện Gò Dầu", ParentId = 72 },
                        new Location { Status = 1, Code = "711", Name = "Huyện Bến Cầu", ParentId = 72 },
                        new Location { Status = 1, Code = "712", Name = "Thị xã Trảng Bàng", ParentId = 72 }
                    );

                    // Các quận, huyện thuộc tỉnh Bình Dương
                    appContext.AddRange(
                        new Location { Status = 1, Code = "718", Name = "Thành phố Thủ Dầu Một", ParentId = 74 },
                        new Location { Status = 1, Code = "719", Name = "Huyện Bàu Bàng", ParentId = 74 },
                        new Location { Status = 1, Code = "720", Name = "Huyện Dầu Tiếng", ParentId = 74 },
                        new Location { Status = 1, Code = "721", Name = "Thị xã Bến Cát", ParentId = 74 },
                        new Location { Status = 1, Code = "722", Name = "Huyện Phú Giáo", ParentId = 74 },
                        new Location { Status = 1, Code = "723", Name = "Thị xã Tân Uyên", ParentId = 74 },
                        new Location { Status = 1, Code = "724", Name = "Thành phố Dĩ An", ParentId = 74 },
                        new Location { Status = 1, Code = "725", Name = "Thành phố Thuận An", ParentId = 74 },
                        new Location { Status = 1, Code = "726", Name = "Huyện Bắc Tân Uyên", ParentId = 74 }
                    );

                    // Các quận, huyện thuộc tỉnh Đồng Nai
                    appContext.AddRange(
                        new Location { Status = 1, Code = "731", Name = "Thành phố Biên Hòa", ParentId = 75 },
                        new Location { Status = 1, Code = "732", Name = "Thành phố Long Khánh", ParentId = 75 },
                        new Location { Status = 1, Code = "734", Name = "Huyện Tân Phú", ParentId = 75 },
                        new Location { Status = 1, Code = "735", Name = "Huyện Vĩnh Cửu", ParentId = 75 },
                        new Location { Status = 1, Code = "736", Name = "Huyện Định Quán", ParentId = 75 },
                        new Location { Status = 1, Code = "737", Name = "Huyện Trảng Bom", ParentId = 75 },
                        new Location { Status = 1, Code = "738", Name = "Huyện Thống Nhất", ParentId = 75 },
                        new Location { Status = 1, Code = "739", Name = "Huyện Cẩm Mỹ", ParentId = 75 },
                        new Location { Status = 1, Code = "740", Name = "Huyện Long Thành", ParentId = 75 },
                        new Location { Status = 1, Code = "741", Name = "Huyện Xuân Lộc", ParentId = 75 },
                        new Location { Status = 1, Code = "742", Name = "Huyện Nhơn Trạch", ParentId = 75 }
                    );

                    // Các quận, huyện thuộc tỉnh Bà Rịa - Vũng Tàu
                    appContext.AddRange(
                        new Location { Status = 1, Code = "747", Name = "Thành phố Vũng Tàu", ParentId = 77 },
                        new Location { Status = 1, Code = "748", Name = "Thành phố Bà Rịa", ParentId = 77 },
                        new Location { Status = 1, Code = "750", Name = "Huyện Châu Đức", ParentId = 77 },
                        new Location { Status = 1, Code = "751", Name = "Huyện Xuyên Mộc", ParentId = 77 },
                        new Location { Status = 1, Code = "752", Name = "Huyện Long Điền", ParentId = 77 },
                        new Location { Status = 1, Code = "753", Name = "Huyện Đất Đỏ", ParentId = 77 },
                        new Location { Status = 1, Code = "754", Name = "Thị xã Phú Mỹ", ParentId = 77 },
                        new Location { Status = 1, Code = "755", Name = "Huyện Côn Đảo", ParentId = 77 }
                    );

                    // Các quận, huyện thuộc TP.Hồ Chí Minh
                    appContext.AddRange(
                        new Location { Status = 1, Code = "760", Name = "Quận 1", ParentId = 79 },
                        new Location { Status = 1, Code = "761", Name = "Quận 12", ParentId = 79 },
                        new Location { Status = 1, Code = "762", Name = "Quận Thủ Đức", ParentId = 79 },
                        new Location { Status = 1, Code = "763", Name = "Quận 9", ParentId = 79 },
                        new Location { Status = 1, Code = "764", Name = "Quận Gò Vấp", ParentId = 79 },
                        new Location { Status = 1, Code = "765", Name = "Quận Bình Thạnh", ParentId = 79 },
                        new Location { Status = 1, Code = "766", Name = "Quận Tân Bình", ParentId = 79 },
                        new Location { Status = 1, Code = "767", Name = "Quận Tân Phú", ParentId = 79 },
                        new Location { Status = 1, Code = "768", Name = "Quận Phú Nhuận", ParentId = 79 },
                        new Location { Status = 1, Code = "769", Name = "Quận 2", ParentId = 79 },
                        new Location { Status = 1, Code = "770", Name = "Quận 3", ParentId = 79 },
                        new Location { Status = 1, Code = "771", Name = "Quận 10", ParentId = 79 },
                        new Location { Status = 1, Code = "772", Name = "Quận 11", ParentId = 79 },
                        new Location { Status = 1, Code = "773", Name = "Quận 4", ParentId = 79 },
                        new Location { Status = 1, Code = "774", Name = "Quận 5", ParentId = 79 },
                        new Location { Status = 1, Code = "775", Name = "Quận 6", ParentId = 79 },
                        new Location { Status = 1, Code = "776", Name = "Quận 8", ParentId = 79 },
                        new Location { Status = 1, Code = "777", Name = "Quận Bình Tân", ParentId = 79 },
                        new Location { Status = 1, Code = "778", Name = "Quận 7", ParentId = 79 },
                        new Location { Status = 1, Code = "783", Name = "Huyện Củ Chi", ParentId = 79 },
                        new Location { Status = 1, Code = "784", Name = "Huyện Hóc Môn", ParentId = 79 },
                        new Location { Status = 1, Code = "785", Name = "Huyện Bình Chánh", ParentId = 79 },
                        new Location { Status = 1, Code = "786", Name = "Huyện Nhà Bè", ParentId = 79 },
                        new Location { Status = 1, Code = "787", Name = "Huyện Cần Giờ", ParentId = 79 }
                    );

                    // Các quận, huyện thuộc tỉnh Long An
                    appContext.AddRange(
                        new Location { Status = 1, Code = "794", Name = "Thành phố Tân An", ParentId = 80 },
                        new Location { Status = 1, Code = "795", Name = "Thị xã Kiến Tường", ParentId = 80 },
                        new Location { Status = 1, Code = "796", Name = "Huyện Tân Hưng", ParentId = 80 },
                        new Location { Status = 1, Code = "797", Name = "Huyện Vĩnh Hưng", ParentId = 80 },
                        new Location { Status = 1, Code = "798", Name = "Huyện Mộc Hóa", ParentId = 80 },
                        new Location { Status = 1, Code = "799", Name = "Huyện Tân Thạnh", ParentId = 80 },
                        new Location { Status = 1, Code = "800", Name = "Huyện Thạnh Hóa", ParentId = 80 },
                        new Location { Status = 1, Code = "801", Name = "Huyện Đức Huệ", ParentId = 80 },
                        new Location { Status = 1, Code = "802", Name = "Huyện Đức Hòa", ParentId = 80 },
                        new Location { Status = 1, Code = "803", Name = "Huyện Bến Lức", ParentId = 80 },
                        new Location { Status = 1, Code = "804", Name = "Huyện Thủ Thừa", ParentId = 80 },
                        new Location { Status = 1, Code = "805", Name = "Huyện Tân Trụ", ParentId = 80 },
                        new Location { Status = 1, Code = "806", Name = "Huyện Cần Đước", ParentId = 80 },
                        new Location { Status = 1, Code = "807", Name = "Huyện Cần Giuộc", ParentId = 80 },
                        new Location { Status = 1, Code = "808", Name = "Huyện Châu Thành", ParentId = 80 }
                    );

                    // Các quận, huyện thuộc tỉnh Tiền Giang
                    appContext.AddRange(
                        new Location { Status = 1, Code = "815", Name = "Thành phố Mỹ Tho", ParentId = 82 },
                        new Location { Status = 1, Code = "816", Name = "Thị xã Gò Công", ParentId = 82 },
                        new Location { Status = 1, Code = "817", Name = "Thị xã Cai Lậy", ParentId = 82 },
                        new Location { Status = 1, Code = "818", Name = "Huyện Tân Phước", ParentId = 82 },
                        new Location { Status = 1, Code = "819", Name = "Huyện Cái Bè", ParentId = 82 },
                        new Location { Status = 1, Code = "820", Name = "Huyện Cai Lậy", ParentId = 82 },
                        new Location { Status = 1, Code = "821", Name = "Huyện Châu Thành", ParentId = 82 },
                        new Location { Status = 1, Code = "822", Name = "Huyện Chợ Gạo", ParentId = 82 },
                        new Location { Status = 1, Code = "823", Name = "Huyện Gò Công Tây", ParentId = 82 },
                        new Location { Status = 1, Code = "824", Name = "Huyện Gò Công Đông", ParentId = 82 },
                        new Location { Status = 1, Code = "825", Name = "Huyện Tân Phú Đông", ParentId = 82 }
                    );

                    // Các quận, huyện thuộc tỉnh Bến Tre
                    appContext.AddRange(
                        new Location { Status = 1, Code = "829", Name = "Thành phố Bến Tre", ParentId = 83 },
                        new Location { Status = 1, Code = "831", Name = "Huyện Châu Thành", ParentId = 83 },
                        new Location { Status = 1, Code = "832", Name = "Huyện Chợ Lách", ParentId = 83 },
                        new Location { Status = 1, Code = "833", Name = "Huyện Mỏ Cày Nam", ParentId = 83 },
                        new Location { Status = 1, Code = "834", Name = "Huyện Giồng Trôm", ParentId = 83 },
                        new Location { Status = 1, Code = "835", Name = "Huyện Bình Đại", ParentId = 83 },
                        new Location { Status = 1, Code = "836", Name = "Huyện Ba Tri", ParentId = 83 },
                        new Location { Status = 1, Code = "837", Name = "Huyện Thạnh Phú", ParentId = 83 },
                        new Location { Status = 1, Code = "838", Name = "Huyện Mỏ Cày Bắc", ParentId = 83 }
                    );

                    // Các quận, huyện thuộc tỉnh Trà Vinh
                    appContext.AddRange(
                        new Location { Status = 1, Code = "842", Name = "Thành phố Trà Vinh", ParentId = 84 },
                        new Location { Status = 1, Code = "844", Name = "Huyện Càng Long", ParentId = 84 },
                        new Location { Status = 1, Code = "845", Name = "Huyện Cầu Kè", ParentId = 84 },
                        new Location { Status = 1, Code = "846", Name = "Huyện Tiểu Cần", ParentId = 84 },
                        new Location { Status = 1, Code = "847", Name = "Huyện Châu Thành", ParentId = 84 },
                        new Location { Status = 1, Code = "848", Name = "Huyện Cầu Ngang", ParentId = 84 },
                        new Location { Status = 1, Code = "849", Name = "Huyện Trà Cú", ParentId = 84 },
                        new Location { Status = 1, Code = "850", Name = "Huyện Duyên Hải", ParentId = 84 },
                        new Location { Status = 1, Code = "851", Name = "Thị xã Duyên Hải", ParentId = 84 }
                    );

                    // Các quận, huyện thuộc tỉnh Vĩnh Long
                    appContext.AddRange(
                        new Location { Status = 1, Code = "855", Name = "Thành phố Vĩnh Long", ParentId = 86 },
                        new Location { Status = 1, Code = "857", Name = "Huyện Long Hồ", ParentId = 86 },
                        new Location { Status = 1, Code = "858", Name = "Huyện Mang Thít", ParentId = 86 },
                        new Location { Status = 1, Code = "859", Name = "Huyện Vũng Liêm", ParentId = 86 },
                        new Location { Status = 1, Code = "860", Name = "Huyện Tam Bình", ParentId = 86 },
                        new Location { Status = 1, Code = "861", Name = "Thị xã Bình Minh", ParentId = 86 },
                        new Location { Status = 1, Code = "862", Name = "Huyện Trà Ôn", ParentId = 86 },
                        new Location { Status = 1, Code = "863", Name = "Huyện Bình Tân", ParentId = 86 }
                    );

                    // Các quận, huyện thuộc tỉnh Đồng Tháp
                    appContext.AddRange(
                        new Location { Status = 1, Code = "866", Name = "Thành phố Cao Lãnh", ParentId = 87 },
                        new Location { Status = 1, Code = "867", Name = "Thành phố Sa Đéc", ParentId = 87 },
                        new Location { Status = 1, Code = "868", Name = "Thị xã Hồng Ngự", ParentId = 87 },
                        new Location { Status = 1, Code = "869", Name = "Huyện Tân Hồng", ParentId = 87 },
                        new Location { Status = 1, Code = "870", Name = "Huyện Hồng Ngự", ParentId = 87 },
                        new Location { Status = 1, Code = "871", Name = "Huyện Tam Nông", ParentId = 87 },
                        new Location { Status = 1, Code = "872", Name = "Huyện Tháp Mười", ParentId = 87 },
                        new Location { Status = 1, Code = "873", Name = "Huyện Cao Lãnh", ParentId = 87 },
                        new Location { Status = 1, Code = "874", Name = "Huyện Thanh Bình", ParentId = 87 },
                        new Location { Status = 1, Code = "875", Name = "Huyện Lấp Vò", ParentId = 87 },
                        new Location { Status = 1, Code = "876", Name = "Huyện Lai Vung", ParentId = 87 },
                        new Location { Status = 1, Code = "877", Name = "Huyện Châu Thành", ParentId = 87 }
                    );

                    // Các quận, huyện thuộc tỉnh An Giang
                    appContext.AddRange(
                        new Location { Status = 1, Code = "883", Name = "Thành phố Long Xuyên", ParentId = 89 },
                        new Location { Status = 1, Code = "884", Name = "Thành phố Châu Đốc", ParentId = 89 },
                        new Location { Status = 1, Code = "886", Name = "Huyện An Phú", ParentId = 89 },
                        new Location { Status = 1, Code = "887", Name = "Thị xã Tân Châu", ParentId = 89 },
                        new Location { Status = 1, Code = "888", Name = "Huyện Phú Tân", ParentId = 89 },
                        new Location { Status = 1, Code = "889", Name = "Huyện Châu Phú", ParentId = 89 },
                        new Location { Status = 1, Code = "890", Name = "Huyện Tịnh Biên", ParentId = 89 },
                        new Location { Status = 1, Code = "891", Name = "Huyện Tri Tôn", ParentId = 89 },
                        new Location { Status = 1, Code = "892", Name = "Huyện Châu Thành", ParentId = 89 },
                        new Location { Status = 1, Code = "893", Name = "Huyện Chợ Mới", ParentId = 89 },
                        new Location { Status = 1, Code = "894", Name = "Huyện Thoại Sơn", ParentId = 89 }
                    );

                    // Các quận, huyện thuộc tỉnh Kiên Giang
                    appContext.AddRange(
                        new Location { Status = 1, Code = "899", Name = "Thành phố Rạch Giá", ParentId = 91 },
                        new Location { Status = 1, Code = "900", Name = "Thành phố Hà Tiên", ParentId = 91 },
                        new Location { Status = 1, Code = "902", Name = "Huyện Kiên Lương", ParentId = 91 },
                        new Location { Status = 1, Code = "903", Name = "Huyện Hòn Đất", ParentId = 91 },
                        new Location { Status = 1, Code = "904", Name = "Huyện Tân Hiệp", ParentId = 91 },
                        new Location { Status = 1, Code = "905", Name = "Huyện Châu Thành", ParentId = 91 },
                        new Location { Status = 1, Code = "906", Name = "Huyện Giồng Riềng", ParentId = 91 },
                        new Location { Status = 1, Code = "907", Name = "Huyện Gò Quao", ParentId = 91 },
                        new Location { Status = 1, Code = "908", Name = "Huyện An Biên", ParentId = 91 },
                        new Location { Status = 1, Code = "909", Name = "Huyện An Minh", ParentId = 91 },
                        new Location { Status = 1, Code = "910", Name = "Huyện Vĩnh Thuận", ParentId = 91 },
                        new Location { Status = 1, Code = "911", Name = "Huyện Phú Quốc", ParentId = 91 },
                        new Location { Status = 1, Code = "912", Name = "Huyện Kiên Hải", ParentId = 91 },
                        new Location { Status = 1, Code = "913", Name = "Huyện U Minh Thượng", ParentId = 91 },
                        new Location { Status = 1, Code = "914", Name = "Huyện Giang Thành", ParentId = 91 }
                    );

                    // Các quận, huyện thuộc thành phố Cần Thơ
                    appContext.AddRange(
                        new Location { Status = 1, Code = "916", Name = "Quận Ninh Kiều", ParentId = 92 },
                        new Location { Status = 1, Code = "917", Name = "Quận Ô Môn", ParentId = 92 },
                        new Location { Status = 1, Code = "918", Name = "Quận Bình Thuỷ", ParentId = 92 },
                        new Location { Status = 1, Code = "919", Name = "Quận Cái Răng", ParentId = 92 },
                        new Location { Status = 1, Code = "923", Name = "Quận Thốt Nốt", ParentId = 92 },
                        new Location { Status = 1, Code = "924", Name = "Huyện Vĩnh Thạnh", ParentId = 92 },
                        new Location { Status = 1, Code = "925", Name = "Huyện Cờ Đỏ", ParentId = 92 },
                        new Location { Status = 1, Code = "926", Name = "Huyện Phong Điền", ParentId = 92 },
                        new Location { Status = 1, Code = "927", Name = "Huyện Thới Lai", ParentId = 92 }
                    );

                    // Các quận, huyện thuộc tỉnh Hậu Giang
                    appContext.AddRange(
                        new Location { Status = 1, Code = "930", Name = "Thành phố Vị Thanh", ParentId = 93 },
                        new Location { Status = 1, Code = "931", Name = "Thành phố Ngã Bảy", ParentId = 93 },
                        new Location { Status = 1, Code = "932", Name = "Huyện Châu Thành A", ParentId = 93 },
                        new Location { Status = 1, Code = "933", Name = "Huyện Châu Thành", ParentId = 93 },
                        new Location { Status = 1, Code = "934", Name = "Huyện Phụng Hiệp", ParentId = 93 },
                        new Location { Status = 1, Code = "935", Name = "Huyện Vị Thuỷ", ParentId = 93 },
                        new Location { Status = 1, Code = "936", Name = "Huyện Long Mỹ", ParentId = 93 },
                        new Location { Status = 1, Code = "937", Name = "Thị xã Long Mỹ", ParentId = 93 }
                    );

                    // Các quận, huyện thuộc tỉnh Sóc Trăng
                    appContext.AddRange(
                        new Location { Status = 1, Code = "941", Name = "Thành phố Sóc Trăng", ParentId = 94 },
                        new Location { Status = 1, Code = "942", Name = "Huyện Châu Thành", ParentId = 94 },
                        new Location { Status = 1, Code = "943", Name = "Huyện Kế Sách", ParentId = 94 },
                        new Location { Status = 1, Code = "944", Name = "Huyện Mỹ Tú", ParentId = 94 },
                        new Location { Status = 1, Code = "945", Name = "Huyện Cù Lao Dung", ParentId = 94 },
                        new Location { Status = 1, Code = "946", Name = "Huyện Long Phú", ParentId = 94 },
                        new Location { Status = 1, Code = "947", Name = "Huyện Mỹ Xuyên", ParentId = 94 },
                        new Location { Status = 1, Code = "948", Name = "Thị xã Ngã Năm", ParentId = 94 },
                        new Location { Status = 1, Code = "949", Name = "Huyện Thạnh Trị", ParentId = 94 },
                        new Location { Status = 1, Code = "950", Name = "Thị xã Vĩnh Châu", ParentId = 94 },
                        new Location { Status = 1, Code = "951", Name = "Huyện Trần Đề", ParentId = 94 }
                    );

                    // Các quận, huyện thuộc tỉnh Bạc Liêu
                    appContext.AddRange(
                        new Location { Status = 1, Code = "954", Name = "Thành phố Bạc Liêu", ParentId = 95 },
                        new Location { Status = 1, Code = "956", Name = "Huyện Hồng Dân", ParentId = 95 },
                        new Location { Status = 1, Code = "957", Name = "Huyện Phước Long", ParentId = 95 },
                        new Location { Status = 1, Code = "958", Name = "Huyện Vĩnh Lợi", ParentId = 95 },
                        new Location { Status = 1, Code = "959", Name = "Thị xã Giá Rai", ParentId = 95 },
                        new Location { Status = 1, Code = "960", Name = "Huyện Đông Hải", ParentId = 95 },
                        new Location { Status = 1, Code = "961", Name = "Huyện Hoà Bình", ParentId = 95 }
                    );


                    // Các quận, huyện thuộc tỉnh Cà Mau
                    appContext.AddRange(
                        new Location { Status = 1, Code = "964", Name = "Thành phố Cà Mau", ParentId = 96 },
                        new Location { Status = 1, Code = "966", Name = "Huyện U Minh", ParentId = 96 },
                        new Location { Status = 1, Code = "967", Name = "Huyện Thới Bình", ParentId = 96 },
                        new Location { Status = 1, Code = "968", Name = "Huyện Trần Văn Thời", ParentId = 96 },
                        new Location { Status = 1, Code = "969", Name = "Huyện Cái Nước", ParentId = 96 },
                        new Location { Status = 1, Code = "970", Name = "Huyện Đầm Dơi", ParentId = 96 },
                        new Location { Status = 1, Code = "971", Name = "Huyện Năm Căn", ParentId = 96 },
                        new Location { Status = 1, Code = "972", Name = "Huyện Phú Tân", ParentId = 96 },
                        new Location { Status = 1, Code = "973", Name = "Huyện Ngọc Hiển", ParentId = 96 }
                    );

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    appContext.SaveChanges();
                }

            }
        }
    }
}
