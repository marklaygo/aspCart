using aspCart.Core.Domain.Catalog;
using aspCart.Core.Domain.User;
using aspCart.Infrastructure;
using aspCart.Infrastructure.EFRepository;
using aspCart.Infrastructure.Services.Catalog;
using aspCart.Infrastructure.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.xUnitTest
{
    public class Service
    {
        public Service(ApplicationDbContext context)
        {
            // repository
            BillingAddressRepository = new Repository<BillingAddress>(context);
            CategoryRepository = new Repository<Category>(context);
            ImageRepository = new Repository<Image>(context);
            ManufacturerRepository = new Repository<Manufacturer>(context);
            ProductRepository = new Repository<Product>(context);
            ProductCategoryMapping = new Repository<ProductCategoryMapping>(context);
            ProductImageMapping = new Repository<ProductImageMapping>(context);
            ProductManufacturerMapping = new Repository<ProductManufacturerMapping>(context);

            // service
            BillingAddressService = new BillingAddressService(context, BillingAddressRepository);
            CategoryService = new CategoryService(context, CategoryRepository, ProductCategoryMapping);
            ManufacturerService = new ManufacturerService(context, ManufacturerRepository, ProductManufacturerMapping);
            ImageManagerService = new ImageManagerService(ImageRepository, ProductImageMapping);
            ProductService = new ProductService(context, ProductRepository);
        }

        // repository
        public Repository<BillingAddress> BillingAddressRepository { get; set; }
        public Repository<Category> CategoryRepository { get; set; }
        public Repository<Image> ImageRepository { get; set; }
        public Repository<Manufacturer> ManufacturerRepository { get; set; }
        public Repository<Product> ProductRepository { get; set; }
        public Repository<ProductCategoryMapping> ProductCategoryMapping { get; set; }
        public Repository<ProductImageMapping> ProductImageMapping { get; set; }
        public Repository<ProductManufacturerMapping> ProductManufacturerMapping { get; set; }

        // service
        public BillingAddressService BillingAddressService { get; set; }
        public CategoryService CategoryService { get; set; }
        public ImageManagerService ImageManagerService { get; set; }
        public ManufacturerService ManufacturerService { get; set; }
        public ProductService ProductService { get; set; }
    }
}
