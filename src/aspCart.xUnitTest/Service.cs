using aspCart.Core.Domain.Catalog;
using aspCart.Infrastructure;
using aspCart.Infrastructure.EFRepository;
using aspCart.Infrastructure.Services.Catalog;
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
            CategoryRepository = new Repository<Category>(context);
            ImageRepository = new Repository<Image>(context);
            ProductRepository = new Repository<Product>(context);
            ProductCategoryMapping = new Repository<ProductCategoryMapping>(context);

            // service
            CategoryService = new CategoryService(context, CategoryRepository, ProductCategoryMapping);
            ImageManagerService = new ImageManagerService(ImageRepository);
            ProductService = new ProductService(context, ProductRepository);
        }

        // repository
        public Repository<Category> CategoryRepository { get; set; }
        public Repository<Image> ImageRepository { get; set; }
        public Repository<Product> ProductRepository { get; set; }
        public Repository<ProductCategoryMapping> ProductCategoryMapping { get; set; }

        // service
        public CategoryService CategoryService { get; set; }
        public ImageManagerService ImageManagerService { get; set; }
        public ProductService ProductService { get; set; }
    }
}
