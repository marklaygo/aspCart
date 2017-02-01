using aspCart.Core.Domain.Catalog;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Infrastructure.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.Catalog
{
    public class ManufacturerService : IManufacturerService
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IRepository<Manufacturer> _manufacturerRepository;
        private readonly IRepository<ProductManufacturerMapping> _productManufacturerRepository;

        #endregion

        #region Constructor

        public ManufacturerService(
            ApplicationDbContext context,
            IRepository<Manufacturer> manufacturerRepository,
            IRepository<ProductManufacturerMapping> productManufacturerRepository)
        {
            _context = context;
            _manufacturerRepository = manufacturerRepository;
            _productManufacturerRepository = productManufacturerRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all manufacturers
        /// </summary>
        /// <returns>List of manufacturer entities</returns>
        public IList<Manufacturer> GetAllManufacturers()
        {
            var entities = _manufacturerRepository.GetAll()
                .OrderBy(x => x.Name)
                .ToList();

            return entities;
        }

        /// <summary>
        /// Get manufacturer using id
        /// </summary>
        /// <param name="id">Manufacturer id</param>
        /// <returns>Manufacturer entity</returns>
        public Manufacturer GetManufacturerById(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return _manufacturerRepository.FindByExpression(x => x.Id == id);
        }

        /// <summary>
        /// Get manufacturer using seo
        /// </summary>
        /// <param name="seo">Manufacturer SEO</param>
        /// <returns>Manufacturer entity</returns>
        public Manufacturer GetManufacturerBySeo(string seo)
        {
            if (seo == string.Empty)
                return null;

            return _manufacturerRepository.FindByExpression(x => x.SeoUrl == seo);
        }

        /// <summary>
        /// Insert manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer entity</param>
        public void InsertManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");

            _manufacturerRepository.Insert(manufacturer);
            _manufacturerRepository.SaveChanges();
        }

        /// <summary>
        /// Update manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer entity</param>
        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");

            _manufacturerRepository.Update(manufacturer);
            _manufacturerRepository.SaveChanges();
        }

        /// <summary>
        /// Delete manufacturers
        /// </summary>
        /// <param name="ids">Ids of manufacturer to delete</param>
        public void DeleteManufacturers(IList<Guid> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            foreach (var id in ids)
                _manufacturerRepository.Delete(GetManufacturerById(id));

            _manufacturerRepository.SaveChanges();
        }

        /// <summary>
        /// Insert product manufacturer mappings
        /// </summary>
        /// <param name="productManufacturerMappings">List of product manufacturer</param>
        public void InsertProductManufacturerMappings(IList<ProductManufacturerMapping> productManufacturerMappings)
        {
            if (productManufacturerMappings == null)
                throw new ArgumentNullException("productManufacturerMappings");

            foreach (var mapping in productManufacturerMappings)
                _productManufacturerRepository.Insert(mapping);

            _productManufacturerRepository.SaveChanges();
        }

        /// <summary>
        /// Delete all product manufacturer mappings using product id
        /// </summary>
        /// <param name="productId">Product id</param>
        public void DeleteAllProductManufacturersMappings(Guid productId)
        {
            if (productId == null)
                throw new ArgumentNullException("productId");

            var mappings = _productManufacturerRepository.FindManyByExpression(x => x.ProductId == productId);

            foreach (var mapping in mappings)
                _productManufacturerRepository.Delete(mapping);

            _productManufacturerRepository.SaveChanges();
        }

        #endregion
    }
}
