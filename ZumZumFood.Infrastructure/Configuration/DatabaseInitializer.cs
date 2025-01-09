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
                             Request = "/login",
                             Response = "login successfully",
                             IpAdress = "192.168.10.242",
                             TimeLogin = DateTime.Now,
                             TimeLogout = DateTime.Now,
                         },
                        new Log
                        {
                            UserName = "User",
                            WorkTation = "DESKTOP-123",
                            Request = "/home",
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
            }
        }
    }
}
