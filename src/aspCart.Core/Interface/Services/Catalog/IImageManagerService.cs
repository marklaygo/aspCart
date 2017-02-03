using aspCart.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Core.Interface.Services.Catalog
{
    public interface IImageManagerService
    {
        /// <summary>
        /// Get all images
        /// </summary>
        /// <returns>List of image entities</returns>
        IList<Image> GetAllImages();

        /// <summary>
        /// Get Image using Id
        /// </summary>
        /// <param name="id">Image id</param>
        /// <returns>Image entity</returns>
        Image GetImageById(Guid id);

        /// <summary>
        /// Search images
        /// </summary>
        /// <param name="keyword">keyword</param>
        /// <returns>List of image entities</returns>
        IList<Image> SearchImages(string keyword);

        /// <summary>
        /// Insert image
        /// </summary>
        /// <param name="images">List of image entities to insert</param>
        void InsertImages(IList<Image> images);

        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="ids">Ids of image entities to delete</param>
        void DeleteImages(IList<Guid> ids);

        /// <summary>
        /// Insert product image mapping
        /// </summary>
        /// <param name="productImageMappings">List of product image mapping</param>
        void InsertProductImageMappings(IList<ProductImageMapping> productImageMappings);

        /// <summary>
        /// Delete product image mapping
        /// </summary>
        /// <param name="productId">Product id</param>
        void DeleteAllProductImageMappings(Guid productId);
    }
}
