using aspCart.Core.Domain.Catalog;
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

            SeedAdminAccount(context, configuration);
            TestDataSeed(context, configuration);
        }

        private static async void TestDataSeed(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            // delete all images
            var allImage = context.Images.ToList();
            context.Images.RemoveRange(allImage);
            await context.SaveChangesAsync();

            // delete all specifications
            var allSpecification = context.Specifications.ToList();
            context.Specifications.RemoveRange(allSpecification);
            await context.SaveChangesAsync();

            // delete all products
            var allProducts = context.Products.ToList();
            context.Products.RemoveRange(allProducts);
            await context.SaveChangesAsync();

            // delete all manufacturer
            var allManufacturers = context.Manufacturers.ToList();
            context.Manufacturers.RemoveRange(allManufacturers);
            await context.SaveChangesAsync();

            // delete all category
            var allCategory = context.Categories.ToList();
            context.Categories.RemoveRange(allCategory);
            await context.SaveChangesAsync();

            var categoryList = new List<Category>
            {
                new Category { Id = new Guid("8c4825ef-8c4c-4162-b2e3-08d46c337976"), Name = "Laptop", SeoUrl = "Laptop", DateAdded = DateTime.Now, DateModified = DateTime.Now, ParentCategoryId = Guid.Empty }
            };
            context.Categories.AddRange(categoryList);
            await context.SaveChangesAsync();

            // manufacturers 
            var manufacturerList = new List<Manufacturer>
            {
                new Manufacturer { Id = new Guid("609483bf-c285-4d67-92f3-08d46c31e55a"), Name = "Acer", SeoUrl = "Acer", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Manufacturer { Id = new Guid("8d942bc6-7407-417f-92f2-08d46c31e55a"), Name = "Asus", SeoUrl = "Asus", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now }
            };
            context.Manufacturers.AddRange(manufacturerList);
            await context.SaveChangesAsync();

            // images
            var imageList = new List<Image>
            {
                new Image { Id = new Guid("1c34435f-2dc2-45fc-a903-7bca40eb5674"), FileName = "/images/test_images/ROG G701VI (7th Gen Intel Core).jpg" },
                new Image { Id = new Guid("dd733338-513d-4e30-9e7f-d4b09f975dd3"), FileName = "/images/test_images/Predator_17X.png" }
            };

            context.Images.AddRange(imageList);
            await context.SaveChangesAsync();

            // specification
            var specificationList = new List<Specification>
            {
                new Specification { Id = new Guid("75477c08-8245-4211-ab74-9c7c14d4dae9"), Name = "Processor", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("ed46ee55-ac40-4d77-80be-69ab6b0d010c"), Name = "Operating", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("f48d837a-22e1-43e1-967a-a989d5889f37"), Name = "Chipset", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("928a7270-7d70-4a37-9440-c650c2a6d782"), Name = "Memory", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("6144e4ab-722e-4b13-bffe-6f0ea8b168b2"), Name = "Display", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("27201fbe-59d6-42a4-b698-a75dcb3e9f52"), Name = "Graphic", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("8ad5f582-c787-4ba2-a49e-d8f8dd2ee621"), Name = "Storage", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("c2a6ac96-7de8-4bdc-a322-fc56f27c8fc8"), Name = "Keyboard", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("e611379b-5c1f-4286-8a54-9c8c45a5697d"), Name = "WebCam", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("f7af0f50-137c-4ce6-b27d-920c83d4ebc7"), Name = "Networking", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("eca1dc44-190e-4806-ba8a-1af16fbd8d24"), Name = "Interface", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("d957dae6-c254-4266-b45c-27fa04f00761"), Name = "Battery", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("19dfc537-f02a-4c7d-9919-5b939d08186f"), Name = "Dimensions", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("93d8b1f6-8a3d-41e5-b3d5-7513bd7f3b33"), Name = "Weight", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("88bcd475-ceb8-4ae3-a385-c3ec07b787e7"), Name = "VR", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now },
                new Specification { Id = new Guid("42f0a5df-e976-4ab6-adab-e260f9cef244"), Name = "Gaming Series", Published = true, DateAdded = DateTime.Now, DateModified = DateTime.Now }
            };

            context.Specifications.AddRange(specificationList);
            await context.SaveChangesAsync();

            // product
            var productList = new List<Product>
            {
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
                        new ProductImageMapping { ProductId = new Guid("337acae3-7adf-4372-8619-1cc9345c61ea"), ImageId = new Guid("1c34435f-2dc2-45fc-a903-7bca40eb5674"), SortOrder = 0, Position = 0 }
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
                        new ProductImageMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), ImageId = new Guid("dd733338-513d-4e30-9e7f-d4b09f975dd3"), SortOrder = 0, Position = 0 }
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
                        new ProductSpecificationMapping { ProductId = new Guid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7"), SpecificationId = new Guid("93d8b1f6-8a3d-41e5-b3d5-7513bd7f3b33"), Value = "10.03 lbs.", SortOrder = 0, Position = 10 }
                    }
                }
            };

            context.Products.AddRange(productList);
            await context.SaveChangesAsync();
        }

        private static async void SeedAdminAccount(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            var user = new ApplicationUser()
            {
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

            await context.SaveChangesAsync();
        }
    }
}
