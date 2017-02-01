using aspCart.Core.Domain.Catalog;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Infrastructure.EFRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.Catalog
{
    public class ProductService : IProductService
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IRepository<Product> _productRepository;

        #endregion

        #region Constructor

        public ProductService(
            ApplicationDbContext context,
            IRepository<Product> productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of product entities</returns>
        public IList<Product> GetAllProducts()
        {
            var entities = _productRepository.GetAll().ToList();

            return entities;
        }

        /// <summary>
        /// Get product usind id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Product entity</returns>
        public Product GetProductById(Guid id)
        {
            if (id == null)
                return null;


            // TODO: update when lazy loading is available
            var entity = _context.Products
                .Include(x => x.Categories).ThenInclude(x => x.Category)
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == id);

            return entity;
        }

        /// <summary>
        /// Get product using seo
        /// </summary>
        /// <param name="seo">Product SEO</param>
        /// <returns>Product entity</returns>
        public Product GetProductBySeo(string seo)
        {
            if (seo == "")
                return null;

            // TODO: update when lazy loading is available
            var entity = _context.Products
                .Include(x => x.Categories).ThenInclude(x => x.Category)
                .AsNoTracking()
                .SingleOrDefault(x => x.SeoUrl == seo);

            return entity;
        }

        /// <summary>
        /// Insert product
        /// </summary>
        /// <param name="product">Product entity</param>
        public void InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            _productRepository.Insert(product);
            _productRepository.SaveChanges();
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product">Product entity</param>
        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            _productRepository.Update(product);
            _productRepository.SaveChanges();
        }

        /// <summary>
        /// Delete products
        /// </summary>
        /// <param name="ids">Ids of product to delete</param>
        public void DeleteProducts(IList<Guid> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            foreach (var id in ids)
                _productRepository.Delete(GetProductById(id));

            _productRepository.SaveChanges();
        }

        #endregion
    }
}
