using aspCart.Core.Domain.Catalog;
using aspCart.Core.Domain.Sale;
using aspCart.Core.Domain.User;
using aspCart.Infrastructure;
using aspCart.Infrastructure.EFModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Helpers
{
    public class DefaultDataProvider
    {
        public static void ApplyMigration(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            // apply migration
            context.Database.Migrate();
        }

        public static void Seed(IServiceProvider serviceProvider, IConfigurationRoot configuration)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            
            SeedAdminAccount(context, configuration).GetAwaiter().GetResult();
            SeedTestAccount(context, configuration).GetAwaiter().GetResult();
            TestDataSeed(context, configuration).GetAwaiter().GetResult();
        }

        private static async Task TestDataSeed(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            #region RemoveRange

            context.Images.RemoveRange(context.Images);
            context.Specifications.RemoveRange(context.Specifications);
            context.Products.RemoveRange(context.Products);
            context.Manufacturers.RemoveRange(context.Manufacturers);
            context.Categories.RemoveRange(context.Categories);
            context.OrderItems.RemoveRange(context.OrderItems);
            context.Orders.RemoveRange(context.Orders);
            context.BillingAddresses.RemoveRange(context.BillingAddresses);
            context.Reviews.RemoveRange(context.Reviews);
            await context.SaveChangesAsync();

            #endregion

            #region Categories

            // categories
            var categoryList = new List<Category>
            {
                new Category { Id = new Guid("8c4825ef-8c4c-4162-b2e3-08d46c337976"), Name = "Laptop", SeoUrl = "Laptop", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now, ParentCategoryId = Guid.Empty },
                new Category { Id = new Guid("21e88188-057e-41e0-8746-57ec2fd76a51"), Name = "Processors", SeoUrl = "Processors", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now, ParentCategoryId = Guid.Empty },
                new Category { Id = new Guid("fdc32bdd-013d-4ced-9106-c3e722e4650a"), Name = "Video Card", SeoUrl = "Video-Card", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now, ParentCategoryId = Guid.Empty }
            };
            context.Categories.AddRange(categoryList);
            await context.SaveChangesAsync();

            #endregion

            #region Manufacturers

            // manufacturers 
            var manufacturerList = new List<Manufacturer>
            {
                new Manufacturer { Id = new Guid("609483bf-c285-4d67-92f3-08d46c31e55a"), Name = "Acer", SeoUrl = "Acer", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Manufacturer { Id = new Guid("8d942bc6-7407-417f-92f2-08d46c31e55a"), Name = "Asus", SeoUrl = "Asus", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Manufacturer { Id = new Guid("d96116b4-d107-4db2-bdbc-be493989d557"), Name = "Intel", SeoUrl = "Intel", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now }
            };
            context.Manufacturers.AddRange(manufacturerList);
            await context.SaveChangesAsync();

            #endregion

            #region Images

            // images
            var imageList = new List<Image>
            {
                new Image { Id = new Guid("1c34435f-2dc2-45fc-a903-7bca40eb5674"), FileName = "/images/test_images/ROG G701VI (7th Gen Intel Core).jpg" },
                new Image { Id = new Guid("cb7d5d64-283c-4d45-9c45-d48c9763956c"), FileName = "/images/test_images/ROG G701VI (7th Gen Intel Core) back.jpg" },
                new Image { Id = new Guid("dd733338-513d-4e30-9e7f-d4b09f975dd3"), FileName = "/images/test_images/Predator_17X.png" },
                new Image { Id = new Guid("f27092f1-2931-4dd5-ab7f-3ef2126c9cf8"), FileName = "/images/test_images/Predator_17X back.png" },
                new Image { Id = new Guid("af740663-4919-47dd-b2a5-a393af28bbd5"), FileName = "/images/test_images/asus gtx 1080 ti founder.jpg" },
                new Image { Id = new Guid("4ae5aa7a-b7b7-4b47-94d9-180876233776"), FileName = "/images/test_images/asus gtx 1080 ti founder 2.png" },
                new Image { Id = new Guid("04096de0-531e-4f9d-848e-a2c36794181e"), FileName = "/images/test_images/asus gtx 1070 strix.png" },
                new Image { Id = new Guid("2f077ad1-ab0c-4ff0-864c-4b45e4c31d8c"), FileName = "/images/test_images/rog strix rx480 O8G gaming.jpg" },
                new Image { Id = new Guid("e09f3dd2-a176-47b3-8518-2015eaef32cc"), FileName = "/images/test_images/Intel Core i7-7700K.jpg" }
            };
            context.Images.AddRange(imageList);
            await context.SaveChangesAsync();

            #endregion

            #region Specifications

            // specification
            var specificationList = new List<Specification>
            {
                new Specification { Id = new Guid("18f698d1-7060-485e-b411-7949491f54db"), Name = "# Cores", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("af498fc2-aa53-4a36-b535-891428a92a84"), Name = "# of Threads", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("473dd9cf-ec4d-452e-9e8a-88d9670cc75b"), Name = "64-Bit Support", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("58f74c41-14b7-426f-aa46-ddbd73989292"), Name = "Accessories", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("d957dae6-c254-4266-b45c-27fa04f00761"), Name = "Battery", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("8d7163a1-ef0b-442b-88ce-55363cd9ddbc"), Name = "Brand", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("739b9689-b4e5-4c30-8108-8cec7419aba9"), Name = "Core Name", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("f48d837a-22e1-43e1-967a-a989d5889f37"), Name = "Chipset", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("b9fe1b06-213a-4128-9329-250da2906852"), Name = "CPU Socket Type", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("e229d9c0-459c-44f9-8f7d-d670cfb4d1d6"), Name = "CUDA Core", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("19dfc537-f02a-4c7d-9919-5b939d08186f"), Name = "Dimensions", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("6144e4ab-722e-4b13-bffe-6f0ea8b168b2"), Name = "Display", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("6dcd49e6-7aa5-4971-80d3-30be12898633"), Name = "Engine Clock", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("42f0a5df-e976-4ab6-adab-e260f9cef244"), Name = "Gaming Series", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("27201fbe-59d6-42a4-b698-a75dcb3e9f52"), Name = "Graphic", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("a0e252f2-39df-4f19-a139-260dd2935097"), Name = "Graphics Engine", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("79136369-aadd-47e2-ac71-2511933881e5"), Name = "Hyper-Threading Support", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("eca1dc44-190e-4806-ba8a-1af16fbd8d24"), Name = "Interface", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("c2a6ac96-7de8-4bdc-a322-fc56f27c8fc8"), Name = "Keyboard", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("282b9279-919d-4300-8109-eb568c02e839"), Name = "L2 Cache", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("f1c8f4aa-9693-4cda-b6e5-c9e967439577"), Name = "L3 Cache", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("1af15e3b-f60d-4d7d-a562-f7dff9a99f20"), Name = "Manufacturing Tech", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("26eb8a9b-3b09-4f47-889c-28859a35b777"), Name = "Max Turbo Frequency", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("928a7270-7d70-4a37-9440-c650c2a6d782"), Name = "Memory", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("20dabff6-aea8-4197-88a6-a3de73d9c36c"), Name = "Memory Clock", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("d969d702-d7da-4eac-b203-2450c576bde7"), Name = "Memory Interface", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("f7af0f50-137c-4ce6-b27d-920c83d4ebc7"), Name = "Networking", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("ed46ee55-ac40-4d77-80be-69ab6b0d010c"), Name = "Operating", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("a5f9adab-4415-415c-8bcc-dac939acfa2f"), Name = "Operating Frequency", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("1ecc8164-69d8-4134-96a3-8ac89618be75"), Name = "Power Connectors", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("75477c08-8245-4211-ab74-9c7c14d4dae9"), Name = "Processor", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("d073700d-c17e-41c8-a283-9abfeb8a8f6a"), Name = "Processors Type", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("9ed7299e-1637-4809-a375-5d0bdff8b613"), Name = "Resolution", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("2ac500ca-e15a-4785-9654-3eef7f26fb1c"), Name = "Series", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("1c9cef2e-b9df-4c90-b88e-f45f7d688646"), Name = "Software", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("8ad5f582-c787-4ba2-a49e-d8f8dd2ee621"), Name = "Storage", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("a74cfddc-9450-4a3d-922b-e7670c9b7924"), Name = "Thermal Design Power", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("2db5ac64-a42d-4bad-8eba-d26ff4e7f727"), Name = "Video Memory", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("a73e43a3-d3d8-4b76-b1e6-274fb682d0a5"), Name = "Virtualization Technology Support", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("88bcd475-ceb8-4ae3-a385-c3ec07b787e7"), Name = "VR", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("e611379b-5c1f-4286-8a54-9c8c45a5697d"), Name = "WebCam", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("93d8b1f6-8a3d-41e5-b3d5-7513bd7f3b33"), Name = "Weight", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now }
            };
            context.Specifications.AddRange(specificationList);
            await context.SaveChangesAsync();

            #endregion

            #region Products

            // product
            var productList = new List<Product>
            {
                #region rog g7
                new Product // rog g7
                {
                    Id = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"),
                    Name = "ROG G701VI (7th Gen Intel Core)",
                    Description = "<strong>FEATURES AT-A-GLANCE</strong><hr /><ul><li>Latest-generation NVIDIA GTX 1080 8GB Graphics Card, Unlocked Intel Core i7-7820HK 2.9 GHz Processor (8M Cache, up to 3.9 GHz)</li><li>Overclocked 64GB DDR4 2800MHz RAM, 1TB NVMe PCIe SSD (512GB x2 RAID0), Windows 10 Pro, CM238 Express Chipset</li><li>17.3\u201D FHD 1920x1080 G-SYNC Display, 120Hz refresh rate, 178\u00B0 Viewing angles</li><li>1x HDMI 2.0 Port, 1x mini Displayport, 802.11ac WiFi 2x2, Bluetooth 4.1, 1x USB 3.1 Type C, 1x Thunderbolt Port (up to 20Gbit/s.), 1x RJ45 LAN Jack, 3x USB 3.0</li><li>Powerful battery rated 93WHrs, 6 cell Li-ion Battery Pack, ESS Sabre headphone DAC and amplifier, Anti-Ghosting (30-Key Rollover), ROG Macro Keys Illuminated Chiclet</li></ul><br /><strong>FEATURES</strong><hr /><h4><strong>OVERCLOCKING BEAST</strong></h4><br /><strong><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/5073c8af-dbbb-4f63-8e51-0207ba6d97bd.jpg.w960.jpg\" style=\"height:320px; width:320px\" /></strong><br /><br />ROG G701VI OC Edition OC Edition is designed to be overclocked to maximum potential. It\u2019s equipped with an unlocked Intel Core i7-7820K processor.&nbsp;<br /><br />Overclock ROG G701 using ROG Gaming Center, providing access to three modes: Standard, Extreme, and Manual for quick and easy access to performance levels.<hr /><br /><strong>BLAZING FAST RAM, BLAZING FAST SSD<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/4dad08b7-ebca-4415-9029-7ed2f4e44b7d.jpg.w960.jpg\" style=\"height:320px; width:320px\" /></strong><br /><br />Go beyond stock speeds. ROG G701 is equipped with 64GB of 2800MHz DDR4 Memory for superior application performance. It\u2019s paired with a next generation NVMe PCIe SSD, that supports theoretical speeds 4-times greater than SATA-based drives. This results in near-instant application launches and fast boot times.&nbsp;<hr /><br /><strong>120HZ WIDEVIEW PANEL WITH G-SYNC SUPPORT<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/e1a37c7e-cc94-40a2-96a3-2587bfe3c645.jpg.w960.jpg\" style=\"height:320px; width:320px\" /></strong><br /><br />ROG G701 features a super-fast 120Hz panel that supports NVIDIA\u00AE G-SYNC\u2122 technology for fast-paced games. G-SYNC synchronizes the display's refresh rate to the GeForce graphics card to ensure super-smooth visuals. G-SYNC minimizes frame-rate stutter, and eliminates input lag and visual tearing. It delivers the smoothest and fastest gaming graphics \u2014 all without affecting system performance. See everything with accurate detail with the 178\u00B0 Viewing Angle IPS-Type panel.<hr /><br /><strong>VR READY<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/b3387c6e-9b96-4557-bd5a-961e3c62978e.jpg.w960.jpg\" style=\"height:320px; width:320px\" /></strong><br /><br />GeForce GTX 10-series graphics cards are powered by Pascal to deliver up to 3x the performance of previous-generation graphics cards, plus innovative new gaming technologies and breakthrough VR experiences.&nbsp;<br /><br />Enjoy immersive virtual reality that is smooth, low-latency, and stutter-free with the G701VI OC Edition.<hr /><br /><strong>ANTI-GHOSTING KEYBOARD WITH 30-KEY ROLLOVER<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/292a14db-1d74-4cf9-96b5-019fd6101758.jpg.w960.jpg\" style=\"height:320px; width:320px\" /></strong><br /><br />ROG G701 has an anti-ghosting keyboard with 30-key rollover technology so up to 30 keystrokes can be instantaneously and correctly logged, even when you hit several of them at once.&nbsp;<br /><br />Each key is ergonomically-designed to ensure solid and responsive keystrokes when typing or entering commands \u2013 making it easy for you to dominate the battlefield. And with a new Record key and more macro keys at your disposal, everything you need is at your fingertips.<hr /><br /><strong>ESS SABRE HEADPHONE DAC AND AMPLIFIER<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/66af7726-defd-4384-90b4-ac9ad4a3ce9a.jpg.w960.jpg\" style=\"height:320px; width:320px\" /></strong><br /><br />ROG G701 features an ESS Sabre headphone DAC and amplifier to give you a sample rate eight times greater than CD-quality audio. The ESS Sabre headphone DAC improves sound quality to provide you with a high dynamic range (DNR) and less noise for rich 384Hz/32bit sound output. In-game audio sounds richer, with greater detail and less distortion, even when you're using a headset.<hr /><br /><strong>STREAMING FRIENDLY FEATURES<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/fd5da23f-88c8-48ca-81e2-4101b6b77733.jpg.w960.jpg\" style=\"height:320px; width:320px\" /></strong><br /><br />ROG G701 is designed to for optimal streaming performance while playing games due to its high end Core i7 Processor and GTX 1080 graphics. ROG G701 comes with a lifetime XSplit license. It has a dedicated recording key that lets you launch XSplit Gamecaster with just one click so you can record or broadcast your gaming session. XSplit Gamecaster allows you to easily stream or record gameplay via a convenient in-game overlay. Make in-game annotations to highlight what's happening onscreen. You can even interact with friends and fans by broadcasting on Twitch.<hr /><br /><strong>COOLING WITHOUT COMPROMISE<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/ece89df9-2ce3-4e8d-adff-cde0f0aba3bc.png.w960.png\" style=\"height:321px; width:320px\" /></strong><br /><br />ROG G701VI has a unique thermal design that directs dust into a dust-release tunnel to keep it away from internal components. This prolongs the component lifespan and enhances overall stability of the laptop by preventing dust from clogging the radiator and reducing cooling effectiveness. The G701VI has dedicated cooling modules for the CPU and GPU to effectively cool each component. In the cooling process, hot exhaust is efficiently managed and expelled through rear vents, directing heat away in order to provide a more comfortable gaming session.<hr /><br /><strong>ROG GAMING CENTER<br /><br /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-6f36d4a4-a37a-4a56-81e4-acfef128bc74/4b725c85-05ac-4b0d-b606-7935b98b734f.png.w960.png\" style=\"height:321px; width:320px\" /></strong><br /><br />The new ROG Gaming Center improves your gaming experience. This integrated control center provides access to Turbo Gear's three overclocking modes (Standard, Extreme and Manual) for quick and easy access to extreme performance levels. Extreme mode goes all in - letting you experience everything ROG G701VI has to offer with just one click. You also have the option to overclock manually, so you can unlock your own personal overclocking achievements!<hr /><br /><strong>ASUS ACCIDENTAL DAMAGE PROTECTION (ADP)</strong><hr /><strong>YOUR ALWAYS ON-CALL PC MEDICS</strong><br />One-year Accidental Damage Protection<br /><br />It's a fact -- accidents happen to all of us. ASUS ADP program1 is created to bring you peace of mind and help protect your devices against damages such as: liquid spills, electrical surges, and drops.<br /><br />*ASUS ADP program applies only to select ASUS branded notebook and tablet products sold within the United States and Canada from select Authorized ASUS Resellers. Products must be purchased in brand new factory sealed condition and not of refurbished or open-box condition. Units sold and purchased outside of the United States and Canada are not eligible.For more details and a list of excluded Resellers, please visit http://adp.asus.com.<br /><br /><strong>WHAT'S IN THE BOX</strong><hr /><ul><li>ASUS ROG G701VI Laptop</li><li>AC Adapter</li><li>User Manual</li><li>Warranty Card</li></ul><br /><strong>ROG 10TH</strong><hr /><img alt=\"\" src=\"https://smedia.webcollage.net/rwvfp/wc/cp/23137514/module/asus/_cp/products/1485198472421/tab-f7be1a10-2dbb-474c-ac59-9482c7ebbcf7/a5df2b68-aade-4c77-b750-d8ea0fb3537e.png.w960.png\" style=\"height:221px; width:234px\" /><br />Now in its 10th year, ROG celebrates the ever-evolving collaboration between world-class R&amp;D, insatiable enthusiasts and devoted gamers needed to embrace (and tame) the bleeding edge and is ready to keep outrunning technology for ten more.",
                    Price = 3499,
                    StockQuantity = 1000,
                    NotifyForQuantityBelow = 1,
                    MinimumCartQuantity = 1,
                    MaximumCartQuantity = 1000,
                    SeoUrl = "ROG-G701VI-7th-Gen-Intel-Core",
                    Published = true,
                    DateAdded = DateTime.Now,
                    DateModified = DateTime.Now,
                    Categories = new List<ProductCategoryMapping>
                    {
                        new ProductCategoryMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), CategoryId = new Guid("8c4825ef-8c4c-4162-b2e3-08d46c337976") }
                    },
                    Manufacturers = new List<ProductManufacturerMapping>
                    {
                        new ProductManufacturerMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), ManufacturerId = new Guid("8d942bc6-7407-417f-92f2-08d46c31e55a") }
                    },
                    Images = new List<ProductImageMapping>
                    {
                        new ProductImageMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), ImageId = new Guid("1c34435f-2dc2-45fc-a903-7bca40eb5674"), SortOrder = 0, Position = 0 },
                        new ProductImageMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), ImageId = new Guid("cb7d5d64-283c-4d45-9c45-d48c9763956c"), SortOrder = 0, Position = 1 }
                    },
                    Specifications = new List<ProductSpecificationMapping>
                    {
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("75477c08-8245-4211-ab74-9c7c14d4dae9"), Value = "Intel® Core™ i7 7820HK Processor", SortOrder = 0, Position = 0 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("ed46ee55-ac40-4d77-80be-69ab6b0d010c"), Value = "Windows 10 Home", SortOrder = 0, Position = 1 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("f48d837a-22e1-43e1-967a-a989d5889f37"), Value = "Intel® CM238 Express Chipset", SortOrder = 0, Position = 2 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("928a7270-7d70-4a37-9440-c650c2a6d782"), Value = "DDR4 2400MHz SDRAM, up to 64 GB SDRAM ( Overclocking to 2800MHz Supported )", SortOrder = 0, Position = 3 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("6144e4ab-722e-4b13-bffe-6f0ea8b168b2"), Value = "17.3&quot; (16:9) LED backlit FHD (1920x1080) 120Hz Anti-Glare Panel with 72% NTSC&amp;nbsp;&lt;br /&gt;17.3&quot; (16:9) LED backlit UHD (3840x2160) 60Hz Anti-Glare Panel with 100% NTSC&amp;nbsp;&lt;br /&gt;With WideView Technology", SortOrder = 0, Position = 4 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("27201fbe-59d6-42a4-b698-a75dcb3e9f52"), Value = "NVIDIA GeForce GTX 1080", SortOrder = 0, Position = 5 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("8ad5f582-c787-4ba2-a49e-d8f8dd2ee621"), Value = "<strong>Solid State Drives:</strong><br />256GB/512GB PCIE Gen3X4 SSD RAID 0 Support", SortOrder = 0, Position = 6 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("c2a6ac96-7de8-4bdc-a322-fc56f27c8fc8"), Value = "Chicklet keyboard with isolated Num key", SortOrder = 0, Position = 7 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("e611379b-5c1f-4286-8a54-9c8c45a5697d"), Value = "HD Web Camera", SortOrder = 0, Position = 8 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("f7af0f50-137c-4ce6-b27d-920c83d4ebc7"), Value = "<strong>Wi-Fi</strong><br />Integrated 802.11 AC", SortOrder = 0, Position = 9 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("eca1dc44-190e-4806-ba8a-1af16fbd8d24"), Value = "1 x Microphone-in jack<br />1 x Headphone-out jack&nbsp;<br />2 x Type A USB3.0 (USB3.1 GEN1)&nbsp;<br />3 x Type A USB3.0 (USB3.1 GEN1)&nbsp;<br />1 x HDMI&nbsp;<br />1 x mini Display Port&nbsp;", SortOrder = 0, Position = 10 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("d957dae6-c254-4266-b45c-27fa04f00761"), Value = "6 Cells 93 Whrs Battery", SortOrder = 0, Position = 11 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("19dfc537-f02a-4c7d-9919-5b939d08186f"), Value = "429 x 309 x 44 mm (WxDxH)", SortOrder = 0, Position = 12 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("93d8b1f6-8a3d-41e5-b3d5-7513bd7f3b33"), Value = "with Battery", SortOrder = 0, Position = 13 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("88bcd475-ceb8-4ae3-a385-c3ec07b787e7"), Value = "VR Ready", SortOrder = 0, Position = 14 },
                        new ProductSpecificationMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), SpecificationId = new Guid("42f0a5df-e976-4ab6-adab-e260f9cef244"), Value = "G Series", SortOrder = 0, Position = 15 }
                    }
                },
                #endregion

                #region acer predator gx
                new Product // acer predator gx
                {
                    Id = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"),
                    Name = "Acer Predator GX-792-77BL 17.3\" 4K/UHD Intel Core i7 7th Gen 7820HK (2.90 GHz) NVIDIA GeForce GTX 1080 32 GB Memory 512 GB SSD 1 TB HDD Windows 10 Home 64-Bit Gaming Laptop",
                    Description = "<strong>Immense Potential</strong><hr />The potential of this device is immense. It sports the latest 7th Gen Intel\u00AE&nbsp;Core\u2122i7 processor and NVIDIA\u00AEGeForce\u2122&nbsp;GTX 1080 graphics1.<br /><br /><br /><strong>Custom-Engineered Cooling</strong><hr />Unlock the potential of Predator hardware with a custom-engineered triple-fan cooling system with a front air intake design.<br /><br /><br /><strong>Front</strong><hr /><p><img alt=\"\" src=\"https://static.acer.com/up/Resource/Acer/Predator_Minisite/Product_Series/Predator_17X/Benefit/20160407/Benefit_F-B_Predator_17X_front.png\" style=\"height:222px; width:320px\" /><br />Glass-fiber construction with edgy triangle element and powerful color accents.<br /><br /><br /><strong>Back</strong></p><hr /><p><strong><img alt=\"\" src=\"https://static.acer.com/up/Resource/Acer/Predator_Minisite/Product_Series/Predator_17X/Benefit/20160407/Benefit_F-B_Predator_17X_back.png\" style=\"height:222px; width:320px\" /></strong><br />A full complement of port connections.<br /><br /><br /><strong>PredatorSense</strong></p><hr /><p>Control and customize your gaming experience from one place with PredatorSense, from the RGB backlit keyboard to overclocking.<br /><br /><br /><strong>Faster Everything</strong></p><hr /><p>Feed your need for speed with 3 SATA in RAID 0 or PCIe NVMe SSD. Enjoy faster data and charging with USB-C Thunderbolt\u2122&nbsp;3 and optimize your precious bandwidth with Killer DoubleShot Pro\u2122.</p>",
                    Price = 2699,
                    StockQuantity = 1000,
                    NotifyForQuantityBelow = 1,
                    MinimumCartQuantity = 1,
                    MaximumCartQuantity = 1000,
                    SeoUrl = "Acer-Predator-GX-792-77BL-Intel-Core-i7-7th-Gen",
                    Published = true,
                    DateAdded = DateTime.Now,
                    DateModified = DateTime.Now,
                    Categories = new List<ProductCategoryMapping>
                    {
                        new ProductCategoryMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), CategoryId = new Guid("8c4825ef-8c4c-4162-b2e3-08d46c337976") }
                    },
                    Manufacturers = new List<ProductManufacturerMapping>
                    {
                        new ProductManufacturerMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), ManufacturerId = new Guid("609483bf-c285-4d67-92f3-08d46c31e55a") }
                    },
                    Images = new List<ProductImageMapping>
                    {
                        new ProductImageMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), ImageId = new Guid("dd733338-513d-4e30-9e7f-d4b09f975dd3"), SortOrder = 0, Position = 0 },
                        new ProductImageMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), ImageId = new Guid("f27092f1-2931-4dd5-ab7f-3ef2126c9cf8"), SortOrder = 0, Position = 1 }
                    },
                    Specifications = new List<ProductSpecificationMapping>
                    {
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("75477c08-8245-4211-ab74-9c7c14d4dae9"), Value = "Intel® Core™ i7 7820HK Processor", SortOrder = 0, Position = 0 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("ed46ee55-ac40-4d77-80be-69ab6b0d010c"), Value = "Windows 10 Home", SortOrder = 0, Position = 1 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("928a7270-7d70-4a37-9440-c650c2a6d782"), Value = "32 GB DDR4 2400MHz (16 GB x 2)", SortOrder = 0, Position = 2 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("6144e4ab-722e-4b13-bffe-6f0ea8b168b2"), Value = "17.3\" 4K/UHD<br />3840 x 2160<br />LED-backlit IPS display with NVIDIA G-SYNC technology<br />Wide viewing angle", SortOrder = 0, Position = 3 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("27201fbe-59d6-42a4-b698-a75dcb3e9f52"), Value = "NVIDIA GeForce GTX 1080", SortOrder = 0, Position = 4 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("8ad5f582-c787-4ba2-a49e-d8f8dd2ee621"), Value = "1 TB HDD + 512 GB SSD", SortOrder = 0, Position = 5 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("e611379b-5c1f-4286-8a54-9c8c45a5697d"), Value = "HD Webcam (1280 x 720)", SortOrder = 0, Position = 6 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("f7af0f50-137c-4ce6-b27d-920c83d4ebc7"), Value = "Killer Double Shot Pro Wireless-AC 1535 802.11ac WiFi featuring 2x2 MU-MIMO technology (Dual-Band 2.4GHz and 5GHz)", SortOrder = 0, Position = 7 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("eca1dc44-190e-4806-ba8a-1af16fbd8d24"), Value = "4 x USB 3.0 (One with Power-off Charging)<br />1 x Thunderbolt 3 (Full USB 3.1 Type C)<br />1 x DisplayPort<br />1 x HDMI 2.0<br />1 x Headphone / Speaker / Line-out Jack", SortOrder = 0, Position = 8 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("d957dae6-c254-4266-b45c-27fa04f00761"), Value = "8-cell Li-ion Battery (6000 mAh)", SortOrder = 0, Position = 9 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("19dfc537-f02a-4c7d-9919-5b939d08186f"), Value = "16.65\" x 12.66\" x 1.77\" (WxDxH)", SortOrder = 0, Position = 10 },
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("93d8b1f6-8a3d-41e5-b3d5-7513bd7f3b33"), Value = "10.03 lbs.", SortOrder = 0, Position = 11 }
                    }
                },
                #endregion

                #region asus gtx 1080 ti founder
                new Product // asus gtx 1080 ti founder
                {
                    Id = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"),
                    Name = "ASUS GeForce GTX 1080 TI 11GB GDDR5X Founders Edition",
                    Description = "<ul><li>Fastest gaming GPU with 3584 NVIDIA\u00AE CUDA\u00AE cores and massive 11GB frame buffer, delivering 35% faster performance than the GeForce GTX 1080</li><li>Get up to 3X the performance and power efficiency of previous generation GPUs.</li><li>Ultra-fast FinFET and high-speed GDDR5X technologies, plus support for DirectX\u2122 12 features.</li><li>NVIDIA GameWorks\u2122 for a more interactive and cinematic experience, as well as incredibly smooth gameplay.</li><li>ASUS GPU Tweak II with XSplit Gamecaster for intuitive performance tweaking and instant gameplay streaming.</li></ul><br /><br /><strong>Ultimate ASUS GeForce GTX 1080 Ti</strong><hr /><br /><img alt=\"\" src=\"https://www.asus.com/websites/global/products/jkXGrxp17lHUXBxV/img/GTX1080-8G_3D_500.png\" style=\"height:210px; width:300px\" /><br /><br />ASUS GeForce\u00AE GTX 1080 Ti Founders Edition is the most powerful and efficient hardware that is up to 35% faster than the GeForce GTX 1080, and is even faster in games than the NVIDIA TITAN X. The card is packed with extreme gaming horsepower, next-gen 11 Gbps GDDR5X memory, and a massive 11GB frame buffer. It is bundled with a free 1-year premium license of customized ASUS GPU Tweak II and XSplit Gamecaster for intuitive performance tweaking and instant gameplay streaming.<br /><br /><br /><strong>Ultimate GeForce</strong><hr /><img alt=\"\" src=\"http://cdn.wccftech.com/wp-content/uploads/2012/12/GeForce-logo.jpg\" style=\"height:88px; width:200px\" /><br /><br />The GeForce\u00AE GTX 1080 Ti is NVIDIA\u2019s new flagship gaming GPU, based on the NVIDIA Pascal\u2122 architecture. The latest addition to the ultimate gaming platform, this card is packed with extreme gaming horsepower, next-gen 11 Gbps GDDR5X memory, and a massive 11 GB frame buffer. #GameReady.<br /><br /><br /><strong>Performance</strong><hr />The GeForce\u00AE GTX 1080 Ti is the world\u2019s fastest gaming GPU. 3584 NVIDIA\u00AE CUDA\u00AE cores and a massive 11 GB frame buffer deliver 35% faster performance than the GeForce GTX 1080.<br /><br /><br /><strong>NVIDIA Pascal</strong><hr />GeForce\u00AE GTX 10-Series graphics cards are powered by Pascal to deliver up to 3X the performance of previous-generation graphics cards, plus breakthrough gaming technologies and VR experiences.<br /><br /><br /><strong>VR Ready</strong><hr /><img alt=\"\" src=\"http://images.nvidia.com/content/virtual-reality/vr-ready-program/geforce-gtx-virtual-reality.jpg\" style=\"height:96px; width:400px\" /><br />Discover next-generation VR performance, the lowest latency, and plug-and-play compatibility with leading headsets\u2014driven by NVIDIA VRWorks\u2122 technologies. VR audio, physics, and haptics let you hear and feel every moment.<br /><br /><br /><strong>Innovative Design</strong><hr /><img alt=\"\" src=\"https://www.msi.com/asset/resize/image/global/product/product_10_20170302172320_58b7e488df2c8.png62405b38c58fe0f07fcef2367d8a9ba1/600.png\" style=\"height:240px; width:300px\" /><br />Unprecedented gaming horsepower. Exceptional craftsmanship. A 7-phase dualFET power supply. All cooled by a radial fan with an advanced vapor chamber designed to for consistent performance in even the most thermally challenging environments. This is the forward-thinking innovation that makes the GeForce\u00AE GTX 1080 Ti the Ultimate GeForce.",
                    Price = 829.99m,
                    StockQuantity = 1000,
                    NotifyForQuantityBelow = 1,
                    MinimumCartQuantity = 1,
                    MaximumCartQuantity = 1000,
                    SeoUrl = "ASUS-GeForce-GTX-1080-TI-11GB-GDDR5X-Founders-Edition",
                    Published = true,
                    DateAdded = DateTime.Now,
                    DateModified = DateTime.Now,
                    Categories = new List<ProductCategoryMapping>
                    {
                        new ProductCategoryMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), CategoryId = new Guid("fdc32bdd-013d-4ced-9106-c3e722e4650a") }
                    },
                    Manufacturers = new List<ProductManufacturerMapping>
                    {
                        new ProductManufacturerMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), ManufacturerId = new Guid("8d942bc6-7407-417f-92f2-08d46c31e55a") }
                    },
                    Images = new List<ProductImageMapping>
                    {
                        new ProductImageMapping { ProductId = new Guid("851559fb-1684-47d8-8fb9-348a39c90c7d"), ImageId = new Guid("af740663-4919-47dd-b2a5-a393af28bbd5"), SortOrder = 0, Position = 0 },
                        new ProductImageMapping { ProductId = new Guid("851559fb-1684-47d8-8fb9-348a39c90c7d"), ImageId = new Guid("4ae5aa7a-b7b7-4b47-94d9-180876233776"), SortOrder = 0, Position = 1 }
                    },
                    Specifications = new List<ProductSpecificationMapping>
                    {
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("a0e252f2-39df-4f19-a139-260dd2935097"), Value = "NVIDIA GeForce GTX 1080 TI", SortOrder = 0, Position = 0 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("2db5ac64-a42d-4bad-8eba-d26ff4e7f727"), Value = "GDDR5X 11GB", SortOrder = 0, Position = 1 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("6dcd49e6-7aa5-4971-80d3-30be12898633"), Value = "GPU Boost Clock : 1582 MHz<br />GPU Base Clock : 1480 MHz", SortOrder = 0, Position = 2 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("e229d9c0-459c-44f9-8f7d-d670cfb4d1d6"), Value = "3584", SortOrder = 0, Position = 3 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("20dabff6-aea8-4197-88a6-a3de73d9c36c"), Value = "11010 MHz", SortOrder = 0, Position = 4 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("d969d702-d7da-4eac-b203-2450c576bde7"), Value = "352-bit", SortOrder = 0, Position = 5 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("9ed7299e-1637-4809-a375-5d0bdff8b613"), Value = "Digital Max Resolution: 7680 x 4320", SortOrder = 0, Position = 6 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("eca1dc44-190e-4806-ba8a-1af16fbd8d24"), Value = "HDMI Output : Yes x 1 (Native) (HDMI 2.0)<br />Display Port : Yes x 3 (Native) (Regular DP)<br />HDCP Support : Yes", SortOrder = 0, Position = 7 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("1ecc8164-69d8-4134-96a3-8ac89618be75"), Value = "1 x 6-pin, 1 x 8-pin", SortOrder = 0, Position = 8 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("58f74c41-14b7-426f-aa46-ddbd73989292"), Value = "1 x DP-DVI Dongle", SortOrder = 0, Position = 9 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("1c9cef2e-b9df-4c90-b88e-f45f7d688646"), Value = "ASUS GPU Tweak II & Driver", SortOrder = 0, Position = 10 },
                        new ProductSpecificationMapping { ProductId = new Guid("9de9aad6-7dca-4861-842f-20021a2c5fa0"), SpecificationId = new Guid("19dfc537-f02a-4c7d-9919-5b939d08186f"), Value = "10.5 \" x 4.376 \" x 1.5 \" Inch<br />26.67 x 11.12 x3.81 Centimeter", SortOrder = 0, Position = 11 }
                    }
                },
                #endregion

                #region asus gtx 1070 strix
                new Product // asus gtx 1070 strix
                {
                    Id = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"),
                    Name = "ASUS ROG GeForce GTX 1070 STRIX 8GB 256-Bit GDDR5",
                    Description = "<strong>ROG Strix GeForce\u00AE GTX 1070 OC edition 8GB GDDR5 with Aura Sync RGB for best VR &amp; 4K gaming</strong><hr /><ul><li>1860 MHz boost clock in OC mode for outstanding performance and gaming experience.</li><li>DirectCU III with Patented Wing-Blade Fans for 30% cooler and 3X quieter performance.</li><li>ASUS FanConnect features 4-pin GPU-controlled headers connected to system fans for optimal thermal performance.</li><li>Industry Only Auto-Extreme Technology with Super Alloy Power II delivers premium quality and best reliability.</li><li>Aura RGB Lighting to express personalized gaming style.</li><li>VR-friendly HDMI ports for immersive virtual reality experiences.</li><li>GPU Tweak II with Xsplit Gamecaster provides intuitive performance tweaking and lets you stream your gameplay instantly.</li><li>NVIDIA ANSEL for a revolutionary new way to capture in-game screenshots.</li><li>NVIDIA GameWorks\u2122 provides an interactive and cinematic experience, as well as incredibly smooth gameplay.</li></ul><br /><strong>OUTSHINE THE COMPETITION</strong><hr />ROG Strix GeForce\u00AE GTX 1070 gaming graphics cards are packed with exclusive ASUS technologies, including DirectCU III Technology with Patented Wing-Blade Fans for 30% cooler and 3X quieter performance, and Industry-only Auto-Extreme Technology for premium quality and the best reliability. Aura RGB Lighting enables a gaming system personalization and VR-friendly HDMI ports let gamers easily enjoy immersive virtual reality experiences. ROG Strix GeForce\u00AE GTX 1070 also has GPU Tweak II with XSplit Gamecaster that provides intuitive performance tweaking and instant gameplay streaming.<br /><br /><br /><img alt=\"\" src=\"https://www.asus.com/Graphics-Cards/ROG-STRIX-GTX1070-O8G-GAMING/overview/websites/global/products/IkF5VLiS13T4hUIG/img/overview.jpg\" style=\"height:300px; width:700px\" /><br /><br /><br /><strong>LEVEL UP PERFORMANCE</strong><hr />DOOM\u2122 Resolution: 3840 x 2160 Setting: Ultra<br /><img alt=\"\" src=\"https://www.asus.com/Graphics-Cards/ROG-STRIX-GTX1070-O8G-GAMING/overview/websites/global/products/IkF5VLiS13T4hUIG/img/performance-chart1.png\" style=\"height:125px; width:300px\" /><br /><br />Ashes of the Singularity\u2122 Resolution: 2560 x 1440 Setting: Crazy<br /><img alt=\"\" src=\"https://www.asus.com/Graphics-Cards/ROG-STRIX-GTX1070-O8G-GAMING/overview/websites/global/products/IkF5VLiS13T4hUIG/img/performance-chart2.png\" style=\"height:125px; width:300px\" /><br /><br /><br /><strong>GAME COOL AND PLAY SILENT</strong><hr /><ul><li><strong>DirectCU III Technology&nbsp;with Direct-GPU Contact Heatpipes</strong><ul><li>30% Cooler and 3X Quieter Performance</li><li>Exclusive DirectCU III cooling technology features direct-GPU contact heatpipes that transports more heat away from the GPU and outperform reference designs, achieving up to 30% cooler gaming performance.</li></ul></li></ul>\u00A0<ul><li><strong>Patented Triple Wing-Blade 0dB Fans</strong><ul><li>Max Air Flow with 105% More Air Pressure</li><li>DirectCU III features triple 0dB fans engineered with a patented wing-blade design that delivers maximum air flow and improved 105% static pressure over the heat sink, while operating at 3X quieter volumes than reference cards. The 0dB fans also let you enjoy games in complete silence and make DirectCU III the coolest and quietest graphics card in the market.</li></ul></li></ul>\u00A0<ul><li><strong>ASUS FanConnect</strong><ul><li>Targeted Supplemental Cooling</li><li>When gaming, GPU temperatures are often higher than CPU temps. However, chassis fans usually reference CPU temperatures only, which results in inefficient cooling of the system. For optimal thermal performance, ROG Strix graphics cards feature two 4-pin GPU-controlled headers that can be connected to system fans for targeted cooling.</li></ul></li></ul>",
                    Price = 444.99m,
                    StockQuantity = 1000,
                    NotifyForQuantityBelow = 1,
                    MinimumCartQuantity = 1,
                    MaximumCartQuantity = 1000,
                    SeoUrl = "ASUS-ROG-GeForce-GTX-1070-STRIX-8GB-256-Bit-GDDR5",
                    Published = true,
                    DateAdded = DateTime.Now,
                    DateModified = DateTime.Now,
                    Categories = new List<ProductCategoryMapping>
                    {
                        new ProductCategoryMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), CategoryId = new Guid("fdc32bdd-013d-4ced-9106-c3e722e4650a") }
                    },
                    Manufacturers = new List<ProductManufacturerMapping>
                    {
                        new ProductManufacturerMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), ManufacturerId = new Guid("8d942bc6-7407-417f-92f2-08d46c31e55a") }
                    },
                    Images = new List<ProductImageMapping>
                    {
                        new ProductImageMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), ImageId = new Guid("04096de0-531e-4f9d-848e-a2c36794181e"), SortOrder = 0, Position = 0 }
                    },
                    Specifications = new List<ProductSpecificationMapping>
                    {
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("a0e252f2-39df-4f19-a139-260dd2935097"), Value = "NVIDIA GeForce GTX 1070", SortOrder = 0, Position = 0 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("2db5ac64-a42d-4bad-8eba-d26ff4e7f727"), Value = "GDDR5 8GB", SortOrder = 0, Position = 1 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("6dcd49e6-7aa5-4971-80d3-30be12898633"), Value = "OC Mode - GPU Boost Clock : 1860 MHz , GPU Base Clock : 1657 MHz<br />Gaming Mode (Default) - GPU Boost Clock : 1835 MHz , GPU Base Clock : 1632 MHz<br />*Retail goods are with default Gaming Mode, OC Mode can be adjusted with one click on GPU Tweak II", SortOrder = 0, Position = 2 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("e229d9c0-459c-44f9-8f7d-d670cfb4d1d6"), Value = "1920", SortOrder = 0, Position = 3 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("20dabff6-aea8-4197-88a6-a3de73d9c36c"), Value = "8008 MHz", SortOrder = 0, Position = 4 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("d969d702-d7da-4eac-b203-2450c576bde7"), Value = "256-bit", SortOrder = 0, Position = 5 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("9ed7299e-1637-4809-a375-5d0bdff8b613"), Value = "Digital Max Resolution:7680 x 4320", SortOrder = 0, Position = 6 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("eca1dc44-190e-4806-ba8a-1af16fbd8d24"), Value = "DVI Output : Yes x 1 (Native) (DVI-D)<br />HDMI Output : Yes x 2 (Native) (HDMI 2.0)<br />Display Port : Yes x 2 (Native) (Regular DP)<br />HDCP Support : Yes", SortOrder = 0, Position = 7 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("1ecc8164-69d8-4134-96a3-8ac89618be75"), Value = "1 x 8-pin", SortOrder = 0, Position = 8 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("58f74c41-14b7-426f-aa46-ddbd73989292"), Value = "2 x ROG Cable Ties", SortOrder = 0, Position = 9 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("1c9cef2e-b9df-4c90-b88e-f45f7d688646"), Value = "ASUS GPU Tweak II & Driver", SortOrder = 0, Position = 10 },
                        new ProductSpecificationMapping { ProductId = new Guid("d9122044-3401-4bee-aaac-9c7802a7027e"), SpecificationId = new Guid("19dfc537-f02a-4c7d-9919-5b939d08186f"), Value = "11.73\" x 5.28\" x 1.57\" Inch<br />29.8 x 13.4 x4 Centimeter", SortOrder = 0, Position = 11 }
                    }
                },
                #endregion

                #region rog strix rx480 O8G gaming
                new Product // rog strix rx480 O8G gaming
                {
                    Id = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"),
                    Name = "ROG STRIX RX480 O8G GAMING",
                    Description = "<strong>ASUS ROG Strix RX 480 outshines the competition with Aura RGB Lighting</strong><hr /><ul><li>1330MHz boost clock in OC mode for outstanding performance and gaming experience</li><li>DirectCU III with Patented Wing-Blade Fans delivers 30% cooler and 3X quieter performance.</li><li>ASUS FanConnect features 4-pin GPU-controlled headers connected to system fans for optimal thermal performance.</li><li>Industry Only Auto-Extreme Technology with Super Alloy Power II delivers premium quality and best reliability.</li><li>Aura RGB Lighting to express personalized gaming style.</li><li>VR-friendly HDMI Ports for immersive virtual reality experiences.</li><li>GPU Tweak II with XSplit Gamecaster provides intuitive performance tweaking and lets you stream your gameplay instantly.</li></ul><br /><strong>OUTSHINE THE COMPETITION</strong><hr />ROG Strix Radeon RX 480 gaming graphics cards are packed with exclusive ASUS technologies, including DirectCU III Technology with Patented Wing-Blade Fans for 30% cooler and 3X quieter performance, and Industry-only Auto-Extreme Technology for premium quality and the best reliability. Aura RGB Lighting enables a gaming system personalization and VR-friendly HDMI ports let gamers easily enjoy immersive virtual reality experiences. ROG Strix Radeon RX 480 also has GPU Tweak II with XSplit Gamecaster that provides intuitive performance tweaking and instant gameplay streaming.<br /><br /><br /><img alt=\"\" src=\"https://www.asus.com/Graphics-Cards/ROG-STRIX-GTX1070-O8G-GAMING/overview/websites/global/products/IkF5VLiS13T4hUIG/img/overview.jpg\" style=\"height:300px; width:700px\" /><br /><br /><br /><strong>LEVEL UP PERFORMANCE</strong><hr />DOOM\u2122 Resolution: 3840 x 2160 Setting: Ultra<br /><img alt=\"\" src=\"https://www.asus.com/ROG-Republic-Of-Gamers/ROG-STRIX-RX480-O8G-GAMING/overview/websites/global/products/FEFh1zLUB6OEcm1o/img/performance-chart1.png\" style=\"height:202px; width:485px\" /><br /><br />Hitman\u2122 Resolution: 2560 x 1440 Setting: Ultra<br /><img alt=\"\" src=\"https://www.asus.com/ROG-Republic-Of-Gamers/ROG-STRIX-RX480-O8G-GAMING/overview/websites/global/products/FEFh1zLUB6OEcm1o/img/performance-chart2.png\" style=\"height:202px; width:485px\" /><br /><br /><br /><strong>GAME COOL AND PLAY SILENT</strong><hr /><ul><li><strong>DirectCU III Technology&nbsp;with Direct-GPU Contact Heatpipes</strong><ul><li>30% Cooler and 3X Quieter Performance</li><li>Exclusive DirectCU III cooling technology features direct-GPU contact heatpipes that transports more heat away from the GPU and outperform reference designs, achieving up to 30% cooler gaming performance.</li></ul></li></ul>\u00A0<ul><li><strong>Patented Triple Wing-Blade 0dB Fans</strong><ul><li>Max Air Flow with 105% More Air Pressure</li><li>DirectCU III features triple 0dB fans engineered with a patented wing-blade design that delivers maximum air flow and improved 105% static pressure over the heat sink, while operating at 3X quieter volumes than reference cards. The 0dB fans also let you enjoy games in complete silence and make DirectCU III the coolest and quietest graphics card in the market.</li></ul></li></ul>\u00A0<ul><li><strong>ASUS FanConnect</strong><ul><li>Targeted Supplemental Cooling</li><li>When gaming, GPU temperatures are often higher than CPU temps. However, chassis fans usually reference CPU temperatures only, which results in inefficient cooling of the system. For optimal thermal performance, ROG Strix graphics cards feature two 4-pin GPU-controlled headers that can be connected to system fans for targeted cooling.</li></ul></li></ul>",
                    Price = 264.99m,
                    StockQuantity = 1000,
                    NotifyForQuantityBelow = 1,
                    MinimumCartQuantity = 1,
                    MaximumCartQuantity = 1000,
                    SeoUrl = "ROG-STRIX-RX480-O8G-GAMING",
                    Published = true,
                    DateAdded = DateTime.Now,
                    DateModified = DateTime.Now,
                    Categories = new List<ProductCategoryMapping>
                    {
                        new ProductCategoryMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), CategoryId = new Guid("fdc32bdd-013d-4ced-9106-c3e722e4650a") }
                    },
                    Manufacturers = new List<ProductManufacturerMapping>
                    {
                        new ProductManufacturerMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), ManufacturerId = new Guid("8d942bc6-7407-417f-92f2-08d46c31e55a") }
                    },
                    Images = new List<ProductImageMapping>
                    {
                        new ProductImageMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), ImageId = new Guid("2f077ad1-ab0c-4ff0-864c-4b45e4c31d8c"), SortOrder = 0, Position = 0 }
                    },
                    Specifications = new List<ProductSpecificationMapping>
                    {
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("a0e252f2-39df-4f19-a139-260dd2935097"), Value = "AMD Radeon RX 480", SortOrder = 0, Position = 0 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("2db5ac64-a42d-4bad-8eba-d26ff4e7f727"), Value = "GDDR5 8GB", SortOrder = 0, Position = 1 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("6dcd49e6-7aa5-4971-80d3-30be12898633"), Value = "1330 MHz (OC Mode)<br />1310 MHz (Gaming Mode)<br />*Retail goods are with default Gaming Mode, OC Mode can be adjusted with one click on GPU Tweak II", SortOrder = 0, Position = 2 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("20dabff6-aea8-4197-88a6-a3de73d9c36c"), Value = "8000 MHz", SortOrder = 0, Position = 3 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("d969d702-d7da-4eac-b203-2450c576bde7"), Value = "256-bit", SortOrder = 0, Position = 4 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("9ed7299e-1637-4809-a375-5d0bdff8b613"), Value = "Digital Max Resolution:7680 x 4320", SortOrder = 0, Position = 5 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("eca1dc44-190e-4806-ba8a-1af16fbd8d24"), Value = "DVI Output : Yes x 1 (Native) (DVI-D)<br />HDMI Output : Yes x 2 (Native) (HDMI 2.0)<br />Display Port : Yes x 2 (Native) (Regular DP)<br />HDCP Support : Yes", SortOrder = 0, Position = 6 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("1ecc8164-69d8-4134-96a3-8ac89618be75"), Value = "1 x 8-pin", SortOrder = 0, Position = 7 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("58f74c41-14b7-426f-aa46-ddbd73989292"), Value = "2 x ROG Cable Ties", SortOrder = 0, Position = 8 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("1c9cef2e-b9df-4c90-b88e-f45f7d688646"), Value = "ASUS GPU Tweak II & Driver<br />Aura (Graphics Card) Software", SortOrder = 0, Position = 9 },
                        new ProductSpecificationMapping { ProductId = new Guid("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), SpecificationId = new Guid("19dfc537-f02a-4c7d-9919-5b939d08186f"), Value = "11.73\" x 5.28\" x 1.57\" Inch<br />29.8 x 13.4 x4 Centimeter", SortOrder = 0, Position = 10 }
                    }
                },
                #endregion

                #region Intel Core i7-7700K
                new Product // Intel Core i7-7700K
                {
                    Id = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"),
                    Name = "Intel Core i7-7700K Kaby Lake Quad-Core 4.2 GHz LGA 1151 91W Desktop Processor",
                    Description = "",
                    Price = 349.99m,
                    StockQuantity = 1000,
                    NotifyForQuantityBelow = 1,
                    MinimumCartQuantity = 1,
                    MaximumCartQuantity = 1000,
                    SeoUrl = "Intel-Core-i7-7700K-Kaby-Lake-Quad-Core-42-GHz-LGA-1151-91W-Desktop-Processor",
                    Published = true,
                    DateAdded = DateTime.Now,
                    DateModified = DateTime.Now,
                    Categories = new List<ProductCategoryMapping>
                    {
                        new ProductCategoryMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), CategoryId = new Guid("21e88188-057e-41e0-8746-57ec2fd76a51") }
                    },
                    Manufacturers = new List<ProductManufacturerMapping>
                    {
                        new ProductManufacturerMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), ManufacturerId = new Guid("d96116b4-d107-4db2-bdbc-be493989d557") }
                    },
                    Images = new List<ProductImageMapping>
                    {
                        new ProductImageMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), ImageId = new Guid("e09f3dd2-a176-47b3-8518-2015eaef32cc"), SortOrder = 0, Position = 0 }
                    },
                    Specifications = new List<ProductSpecificationMapping>
                    {
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("8d7163a1-ef0b-442b-88ce-55363cd9ddbc"), Value = "Intel", SortOrder = 0, Position = 0 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("d073700d-c17e-41c8-a283-9abfeb8a8f6a"), Value = "Desktop", SortOrder = 0, Position = 1 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("2ac500ca-e15a-4785-9654-3eef7f26fb1c"), Value = "Core i7 7th Gen", SortOrder = 0, Position = 2 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("b9fe1b06-213a-4128-9329-250da2906852"), Value = "LGA 1151", SortOrder = 0, Position = 3 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("739b9689-b4e5-4c30-8108-8cec7419aba9"), Value = "Kaby Lake", SortOrder = 0, Position = 4 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("18f698d1-7060-485e-b411-7949491f54db"), Value = "Quad-Core", SortOrder = 0, Position = 5 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("af498fc2-aa53-4a36-b535-891428a92a84"), Value = "8", SortOrder = 0, Position = 6 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("a5f9adab-4415-415c-8bcc-dac939acfa2f"), Value = "4.2 GHz", SortOrder = 0, Position = 7 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("26eb8a9b-3b09-4f47-889c-28859a35b777"), Value = "4.50 GHz", SortOrder = 0, Position = 8 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("282b9279-919d-4300-8109-eb568c02e839"), Value = "4 x 256KB", SortOrder = 0, Position = 9 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("f1c8f4aa-9693-4cda-b6e5-c9e967439577"), Value = "8MB", SortOrder = 0, Position = 10 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("1af15e3b-f60d-4d7d-a562-f7dff9a99f20"), Value = "14nm", SortOrder = 0, Position = 11 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("473dd9cf-ec4d-452e-9e8a-88d9670cc75b"), Value = "Yes", SortOrder = 0, Position = 12 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("79136369-aadd-47e2-ac71-2511933881e5"), Value = "Yes", SortOrder = 0, Position = 13 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("a73e43a3-d3d8-4b76-b1e6-274fb682d0a5"), Value = "Yes", SortOrder = 0, Position = 14 },
                        new ProductSpecificationMapping { ProductId = new Guid("6176109c-2219-4013-a3f3-7cf90b60d8be"), SpecificationId = new Guid("a74cfddc-9450-4a3d-922b-e7670c9b7924"), Value = "91W", SortOrder = 0, Position = 15 }
                    }
                }
                #endregion
            };
            context.Products.AddRange(productList);
            await context.SaveChangesAsync();

            #endregion

            #region Billing Address

            // billing address
            var billingAddressList = new List<BillingAddress>
            {
                new BillingAddress { Id = Guid.Parse("6670f705-8a06-451c-b267-5dceb9c130b1"), FirstName = "user", LastName = "1", Email = "user1@aspcart.com", Address = "localhost", City = "localhost", StateProvince = "localhost", ZipPostalCode = "11234", Country = "localhost", Telephone = "0123456789" },
                new BillingAddress { Id = Guid.Parse("8ed5b69b-b4b2-452d-25e0-08d483e4de1d"), FirstName = "user", LastName = "1", Email = "user1@aspcart.com", Address = "localhost", City = "localhost", StateProvince = "localhost", ZipPostalCode = "11234", Country = "localhost", Telephone = "0123456789" }
            };
            context.BillingAddresses.AddRange(billingAddressList);
            await context.SaveChangesAsync();

            #endregion

            #region Order

            // order
            var orderList = new List<Order>
            {
                new Order { Id = Guid.Parse("50ec3a54-0eab-4dfc-bad7-d1538f62f25e"), OrderNumber = "662-347-330787", UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), BillingAddressId = Guid.Parse("6670f705-8a06-451c-b267-5dceb9c130b1"), TotalOrderPrice = 2699.00m, Status = (OrderStatus)2, OrderPlacementDateTime = DateTime.Now, OrderCompletedDateTime = DateTime.Now },
                new Order { Id = Guid.Parse("693721cb-61e5-41cd-9d99-376c08ab627b"), OrderNumber = "475-676-632537", UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), BillingAddressId = Guid.Parse("6670f705-8a06-451c-b267-5dceb9c130b1"), TotalOrderPrice = 1179.98m, Status = 0, OrderPlacementDateTime = DateTime.Now, OrderCompletedDateTime = DateTime.Now }
            };
            context.Orders.AddRange(orderList);
            await context.SaveChangesAsync();

            #endregion

            #region Order item

            // order item
            var orderItemList = new List<OrderItem>
            {
                new OrderItem { Id = Guid.Parse("6f7d2300-67ef-470c-9c67-52e9f228a918"), OrderId = Guid.Parse("50ec3a54-0eab-4dfc-bad7-d1538f62f25e"), ProductId = "c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7", Name = "Acer Predator GX-792-77BL 17.3\" 4K/UHD Intel Core i7 7th Gen 7820HK (2.90 GHz) NVIDIA GeForce GTX 1080 32 GB Memory 512 GB SSD 1 TB HDD Windows 10 Home 64-Bit Gaming Laptop", Quantity = 1, Price = 2699.00m, TotalPrice = 2699.00m },
                new OrderItem { Id = Guid.Parse("f976cda7-419c-451a-9bfd-78eb4170e441"), OrderId = Guid.Parse("693721cb-61e5-41cd-9d99-376c08ab627b"), ProductId = "6176109c-2219-4013-a3f3-7cf90b60d8be", Name = "Intel Core i7-7700K Kaby Lake Quad-Core 4.2 GHz LGA 1151 91W Desktop Processor", Quantity = 1, Price = 349.99m, TotalPrice = 349.99m },
                new OrderItem { Id = Guid.Parse("d5a65357-c77e-4a65-aefd-a288ee6c27d0"), OrderId = Guid.Parse("693721cb-61e5-41cd-9d99-376c08ab627b"), ProductId = "9de9aad6-7dca-4861-842f-20021a2c5fa0", Name = "ASUS GeForce GTX 1080 TI 11GB GDDR5X Founders Edition", Quantity = 1, Price = 829.99m, TotalPrice = 829.99m }
            };
            context.OrderItems.AddRange(orderItemList);
            await context.SaveChangesAsync();

            #endregion

            #region Reviews

            // product review

            var reviewList = new List<Review>
            {
                new Review { Id = Guid.Parse("4940f169-932d-4cee-88eb-630cf02c707d"), UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), ProductId = Guid.Parse("337acae3-7adf-4372-8619-1cc9345c61ea"), Title = "Lorem ipsum", Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras quam massa, lobortis a ex at, euismod varius orci. Maecenas posuere justo non interdum fringilla. Praesent nec consectetur nulla. Vestibulum tortor lectus, lacinia id ante quis, elementum malesuada libero. Mauris nec semper orci, sit amet vulputate elit. Fusce id dignissim sapien. Aenean tempor erat elit, quis gravida nisi molestie vel. Duis in hendrerit mi. Proin gravida, purus ut porttitor porttitor, lectus nisl eleifend nunc, sit amet mollis urna nibh ac magna. Nullam feugiat sem odio. Aliquam erat volutpat. Fusce tincidunt metus nec quam pretium pretium quis vitae augue. Ut libero orci, laoreet sed justo ut, pretium convallis nisi. Etiam quis massa eu elit facilisis egestas. Duis ultrices ex mauris, ac iaculis nibh mattis vitae. Aenean sodales ante sed lorem consequat, non blandit dui efficitur.", Rating = 3, CreatedOn = DateTime.Now },
                new Review { Id = Guid.Parse("4ee720b4-d5cd-4f01-8181-175fb5f59fab"), UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), ProductId = Guid.Parse("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), Title = "Lorem ipsum", Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras quam massa, lobortis a ex at, euismod varius orci. Maecenas posuere justo non interdum fringilla. Praesent nec consectetur nulla. Vestibulum tortor lectus, lacinia id ante quis, elementum malesuada libero. Mauris nec semper orci, sit amet vulputate elit. Fusce id dignissim sapien. Aenean tempor erat elit, quis gravida nisi molestie vel. Duis in hendrerit mi. Proin gravida, purus ut porttitor porttitor, lectus nisl eleifend nunc, sit amet mollis urna nibh ac magna. Nullam feugiat sem odio. Aliquam erat volutpat. Fusce tincidunt metus nec quam pretium pretium quis vitae augue. Ut libero orci, laoreet sed justo ut, pretium convallis nisi. Etiam quis massa eu elit facilisis egestas. Duis ultrices ex mauris, ac iaculis nibh mattis vitae. Aenean sodales ante sed lorem consequat, non blandit dui efficitur.", Rating = 3, CreatedOn = DateTime.Now },
                new Review { Id = Guid.Parse("9a0dd123-2ddc-4850-ab89-b7de75c6c7e4"), UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), ProductId = Guid.Parse("9de9aad6-7dca-4861-842f-20021a2c5fa0"), Title = "Lorem ipsum", Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras quam massa, lobortis a ex at, euismod varius orci. Maecenas posuere justo non interdum fringilla. Praesent nec consectetur nulla. Vestibulum tortor lectus, lacinia id ante quis, elementum malesuada libero. Mauris nec semper orci, sit amet vulputate elit. Fusce id dignissim sapien. Aenean tempor erat elit, quis gravida nisi molestie vel. Duis in hendrerit mi. Proin gravida, purus ut porttitor porttitor, lectus nisl eleifend nunc, sit amet mollis urna nibh ac magna. Nullam feugiat sem odio. Aliquam erat volutpat. Fusce tincidunt metus nec quam pretium pretium quis vitae augue. Ut libero orci, laoreet sed justo ut, pretium convallis nisi. Etiam quis massa eu elit facilisis egestas. Duis ultrices ex mauris, ac iaculis nibh mattis vitae. Aenean sodales ante sed lorem consequat, non blandit dui efficitur.", Rating = 3, CreatedOn = DateTime.Now },
                new Review { Id = Guid.Parse("01acf2bf-cb15-496e-899b-94e4de3fc08c"), UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), ProductId = Guid.Parse("d9122044-3401-4bee-aaac-9c7802a7027e"), Title = "Lorem ipsum", Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras quam massa, lobortis a ex at, euismod varius orci. Maecenas posuere justo non interdum fringilla. Praesent nec consectetur nulla. Vestibulum tortor lectus, lacinia id ante quis, elementum malesuada libero. Mauris nec semper orci, sit amet vulputate elit. Fusce id dignissim sapien. Aenean tempor erat elit, quis gravida nisi molestie vel. Duis in hendrerit mi. Proin gravida, purus ut porttitor porttitor, lectus nisl eleifend nunc, sit amet mollis urna nibh ac magna. Nullam feugiat sem odio. Aliquam erat volutpat. Fusce tincidunt metus nec quam pretium pretium quis vitae augue. Ut libero orci, laoreet sed justo ut, pretium convallis nisi. Etiam quis massa eu elit facilisis egestas. Duis ultrices ex mauris, ac iaculis nibh mattis vitae. Aenean sodales ante sed lorem consequat, non blandit dui efficitur.", Rating = 3, CreatedOn = DateTime.Now },
                new Review { Id = Guid.Parse("fd7412ad-98c3-4ed7-bc70-df2f22530335"), UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), ProductId = Guid.Parse("5f1c200c-b551-4ceb-9273-3ccf9c4718da"), Title = "Lorem ipsum", Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras quam massa, lobortis a ex at, euismod varius orci. Maecenas posuere justo non interdum fringilla. Praesent nec consectetur nulla. Vestibulum tortor lectus, lacinia id ante quis, elementum malesuada libero. Mauris nec semper orci, sit amet vulputate elit. Fusce id dignissim sapien. Aenean tempor erat elit, quis gravida nisi molestie vel. Duis in hendrerit mi. Proin gravida, purus ut porttitor porttitor, lectus nisl eleifend nunc, sit amet mollis urna nibh ac magna. Nullam feugiat sem odio. Aliquam erat volutpat. Fusce tincidunt metus nec quam pretium pretium quis vitae augue. Ut libero orci, laoreet sed justo ut, pretium convallis nisi. Etiam quis massa eu elit facilisis egestas. Duis ultrices ex mauris, ac iaculis nibh mattis vitae. Aenean sodales ante sed lorem consequat, non blandit dui efficitur.", Rating = 3, CreatedOn = DateTime.Now },
                new Review { Id = Guid.Parse("ab13fa29-e462-49a3-84c3-8e901d0bfe61"), UserId = Guid.Parse("1966c895-c0d4-40d6-b201-47c0dd0228e1"), ProductId = Guid.Parse("6176109c-2219-4013-a3f3-7cf90b60d8be"), Title = "Lorem ipsum", Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras quam massa, lobortis a ex at, euismod varius orci. Maecenas posuere justo non interdum fringilla. Praesent nec consectetur nulla. Vestibulum tortor lectus, lacinia id ante quis, elementum malesuada libero. Mauris nec semper orci, sit amet vulputate elit. Fusce id dignissim sapien. Aenean tempor erat elit, quis gravida nisi molestie vel. Duis in hendrerit mi. Proin gravida, purus ut porttitor porttitor, lectus nisl eleifend nunc, sit amet mollis urna nibh ac magna. Nullam feugiat sem odio. Aliquam erat volutpat. Fusce tincidunt metus nec quam pretium pretium quis vitae augue. Ut libero orci, laoreet sed justo ut, pretium convallis nisi. Etiam quis massa eu elit facilisis egestas. Duis ultrices ex mauris, ac iaculis nibh mattis vitae. Aenean sodales ante sed lorem consequat, non blandit dui efficitur.", Rating = 3, CreatedOn = DateTime.Now }
            };
            context.Reviews.AddRange(reviewList);
            await context.SaveChangesAsync();

            #endregion
        }

        private static async Task SeedAdminAccount(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            context.UserRoles.RemoveRange(context.UserRoles);
            context.Roles.RemoveRange(context.Roles);
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();

            var user = new ApplicationUser()
            {
                Id = "240de0d4-ade7-417e-a034-9b63cc2de853",
                UserName = configuration.GetValue<string>("AdminAccount:Email"),
                NormalizedUserName = configuration.GetValue<string>("AdminAccount:Email").ToUpper(),
                Email = configuration.GetValue<string>("AdminAccount:Email"),
                NormalizedEmail = configuration.GetValue<string>("AdminAccount:Email").ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(context);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var hashed = passwordHasher.HashPassword(user, configuration.GetValue<string>("AdminAccount:Password"));
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
            }
        }

        private static async Task SeedTestAccount(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            // user1
            var user1 = new ApplicationUser()
            {
                Id = "1966c895-c0d4-40d6-b201-47c0dd0228e1",
                UserName = "user1@aspcart.com",
                NormalizedUserName = "user1@aspcart.com".ToUpper(),
                Email = "user1@aspcart.com",
                NormalizedEmail = "user1@aspcart.com".ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!context.Users.Any(u => u.UserName == user1.UserName))
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var hashed = passwordHasher.HashPassword(user1, "11234");
                user1.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user1);
            }
        }
    }
}
