using aspCart.Core.Domain.Catalog;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Infrastructure.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.Catalog
{
    public class SpecificationService : ISpecificationService
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IRepository<Specification> _specificationRepository;
        private readonly IRepository<ProductSpecificationMapping> _productSpecificationMappingRepository;

        #endregion

        #region Constructor

        public SpecificationService(
            ApplicationDbContext context,
            IRepository<Specification> specificationRepository,
            IRepository<ProductSpecificationMapping> productSpecificationMappingRepository)
        {
            _context = context;
            _specificationRepository = specificationRepository;
            _productSpecificationMappingRepository = productSpecificationMappingRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all specifications
        /// </summary>
        /// <returns>List of specification entities</returns>
        public IList<Specification> GetAllSpecifications()
        {
            var entities = _specificationRepository.GetAll()
                .OrderBy(x => x.Name)
                .ToList();

            return entities;
        }

        /// <summary>
        /// Get specification by id
        /// </summary>
        /// <param name="id">Specification id</param>
        /// <returns>Specification entity</returns>
        public Specification GetSpecificationById(Guid id)
        {
            return _specificationRepository.FindByExpression(x => x.Id == id);
        }

        /// <summary>
        /// Insert specification
        /// </summary>
        /// <param name="specification">Specification entity</param>
        public void InsertSpecification(Specification specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            _specificationRepository.Insert(specification);
            _specificationRepository.SaveChanges();
        }

        /// <summary>
        /// Update specification
        /// </summary>
        /// <param name="specification">Specification entity</param>
        public void UpdateSpecification(Specification specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            _specificationRepository.Update(specification);
            _specificationRepository.SaveChanges();
        }

        /// <summary>
        /// Delete specifications
        /// </summary>
        /// <param name="ids">List of specification ids</param>
        public void DeleteSpecifications(IList<Guid> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("specifications");

            foreach (var id in ids)
                _specificationRepository.Delete(GetSpecificationById(id));

            _specificationRepository.SaveChanges();
        }

        /// <summary>
        /// Insert product specification mappings
        /// </summary>
        /// <param name="productSpecificationMappings">Product specification mappings</param>
        public void InsertProductSpecificationMappings(IList<ProductSpecificationMapping> productSpecificationMappings)
        {
            if (productSpecificationMappings == null)
                throw new ArgumentNullException("productSpecificationMappings");

            foreach (var mapping in productSpecificationMappings)
                _productSpecificationMappingRepository.Insert(mapping);

            _productSpecificationMappingRepository.SaveChanges();
        }

        /// <summary>
        /// Delete all product specification by product id
        /// </summary>
        /// <param name="productId">Product id</param>
        public void DeleteAllProductSpecificationMappings(Guid productId)
        {
            if (productId == null)
                throw new ArgumentNullException("productId");

            var mappings = _productSpecificationMappingRepository.FindManyByExpression(x => x.ProductId == productId);

            foreach (var mapping in mappings)
                _productSpecificationMappingRepository.Delete(mapping);

            _productSpecificationMappingRepository.SaveChanges();
        }

        #endregion
    }
}
