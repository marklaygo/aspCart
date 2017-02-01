using aspCart.Core.Domain.Catalog;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Infrastructure.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.Catalog
{
    public class ImageManagerService : IImageManagerService
    {
        #region Fields

        private readonly IRepository<Image> _imageRepository;

        #endregion

        #region Constructor

        public ImageManagerService(IRepository<Image> imageRepository)
        {
            _imageRepository = imageRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all images
        /// </summary>
        /// <returns>List of image entities</returns>
        public IList<Image> GetAllImages()
        {
            var entites = _imageRepository.GetAll()
                .OrderBy(x => x.FileName)
                .ToList();

            return entites;
        }

        /// <summary>
        /// Get Image using Id
        /// </summary>
        /// <param name="id">Image id</param>
        /// <returns>Image entity</returns>
        public Image GetImageById(Guid id)
        {
            return _imageRepository.FindByExpression(x => x.Id == id);
        }

        /// <summary>
        /// Search images
        /// </summary>
        /// <param name="keyword">keyword</param>
        /// <returns>List of image entities</returns>
        public IList<Image> SearchImages(string keyword)
        {
            return _imageRepository.FindManyByExpression(x => x.FileName.Contains(keyword))
                .OrderBy(x => x.FileName)
                .ToList();
        }

        /// <summary>
        /// Insert image
        /// </summary>
        /// <param name="images">List of image entities to insert</param>
        public void InsertImages(IList<Image> images)
        {
            if (images == null)
                throw new ArgumentNullException("images");

            foreach (var image in images)
                _imageRepository.Insert(image);

            _imageRepository.SaveChanges();
        }

        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="ids">Ids of image entities to delete</param>
        public void DeleteImages(IList<Guid> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            foreach (var id in ids)
                _imageRepository.Delete(GetImageById(id));

            _imageRepository.SaveChanges();
        }

        #endregion
    }
}
