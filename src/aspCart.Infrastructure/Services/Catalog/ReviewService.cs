using aspCart.Core.Domain.Catalog;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Infrastructure.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.Services.Catalog
{
    public class ReviewService : IReviewService
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IRepository<Review> _reviewRepository;

        #endregion

        #region Constructor

        public ReviewService(
            ApplicationDbContext context,
            IRepository<Review> reviewRepository)
        {
            _context = context;
            _reviewRepository = reviewRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get review using product id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<Review> GetReviewsByProductId(Guid productId)
        {
            if (productId == null)
                return null;

            return _reviewRepository.FindManyByExpression(x => x.ProductId == productId).ToList();
        }

        /// <summary>
        /// Get review using product id and user id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Review GetReviewByProductIdUserId(Guid productId, Guid userId)
        {
            if (productId == null || userId == null)
                return null;

            return _reviewRepository.FindByExpression(x => x.ProductId == productId && x.UserId == userId);
        }

        /// <summary>
        /// Insert review
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public void InsertReview(Review review)
        {
            if (review == null)
                throw new ArgumentNullException("review");

            _reviewRepository.Insert(review);
            _reviewRepository.SaveChanges();
        }

        /// <summary>
        /// Update review
        /// </summary>
        /// <param name="review"></param>
        public void UpdateReview(Review review)
        {
            if (review == null)
                throw new ArgumentNullException("review");

            _reviewRepository.Update(review);
            _reviewRepository.SaveChanges();
        }

        #endregion
    }
}
