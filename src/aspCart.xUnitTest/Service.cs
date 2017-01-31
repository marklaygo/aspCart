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

            // service
            CategoryService = new CategoryService(context, CategoryRepository);
        }

        // repository
        public Repository<Category> CategoryRepository { get; set; }

        // service
        public CategoryService CategoryService { get; set; }
    }
}
