using aspCart.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Core.Interface.Services.Catalog
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of category entities</returns>
        IList<Category> GetAllCategories();

        /// <summary>
        /// Get all categories without parent
        /// </summary>
        /// <returns>List of category entities without parent</returns>
        IList<Category> GetAllCategoriesWithoutParent();

        /// <summary>
        /// Get category using id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Category entity</returns>
        Category GetCategoryById(Guid id);

        /// <summary>
        /// Get category using seo
        /// </summary>
        /// <param name="seo">Category SEO</param>
        /// <returns>Category entity</returns>
        Category GetCategoryBySeo(string seo);

        /// <summary>
        /// Insert category
        /// </summary>
        /// <param name="category">Category entity</param>
        void InsertCategory(Category category);

        /// <summary>
        /// Update category 
        /// </summary>
        /// <param name="category">Category entity</param>
        void UpdateCategory(Category category);

        /// <summary>
        /// Delete categories
        /// </summary>
        /// <param name="ids">Ids of categories to delete</param>
        void DeleteCategories(IList<Guid> ids);

        /// <summary>
        /// Insert product category mappings
        /// </summary>
        /// <param name="productCategoryMappings">List of product category mapping</param>
        void InsertProductCategoryMappings(IList<ProductCategoryMapping> productCategoryMappings);

        /// <summary>
        /// Delete all product category mappings using product id
        /// </summary>
        /// <param name="productId">Product id</param>
        void DeleteAllProductCategoryMappingsByProductId(Guid productId);
    }
}
