using aspCart.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Core.Interface.Services.Catalog
{
    public interface IManufacturerService
    {
        /// <summary>
        /// Get all manufacturers
        /// </summary>
        /// <returns>List of manufacturer entities</returns>
        IList<Manufacturer> GetAllManufacturers();

        /// <summary>
        /// Get manufacturer using id
        /// </summary>
        /// <param name="id">Manufacturer id</param>
        /// <returns>Manufacturer entity</returns>
        Manufacturer GetManufacturerById(Guid id);

        /// <summary>
        /// Get manufacturer using seo
        /// </summary>
        /// <param name="seo">Manufacturer SEO</param>
        /// <returns>Manufacturer entity</returns>
        Manufacturer GetManufacturerBySeo(string seo);

        /// <summary>
        /// Insert manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer entity</param>
        void InsertManufacturer(Manufacturer manufacturer);

        /// <summary>
        /// Update manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer entity</param>
        void UpdateManufacturer(Manufacturer manufacturer);

        /// <summary>
        /// Delete manufacturers
        /// </summary>
        /// <param name="ids">Ids of manufacturer to delete</param>
        void DeleteManufacturers(IList<Guid> ids);

        /// <summary>
        /// Insert product manufacturer mappings
        /// </summary>
        /// <param name="productManufacturerMappings">List of product manufacturer</param>
        void InsertProductManufacturerMappings(IList<ProductManufacturerMapping> productManufacturerMappings);

        /// <summary>
        /// Delete all product manufacturer mappings using product id
        /// </summary>
        /// <param name="productId">Product id</param>
        void DeleteAllProductManufacturersMappings(Guid productId);
    }
}
