using BurgerAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Repository.IRepository
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        ICollection<Review> GetReviewsFromPlace(int pId);
        ICollection<Review> GetReviewsFromBurger(int bId);
        ICollection<Review> GetReviewsFromUser(int uId);
        Review GetReview(int ReviewId);
        bool ReviewExists(int burgerId, int userId);
        bool ReviewExists(int id);
        bool CreateReview(Review Review);
        bool UpdateReview(Review Review);
        bool DeleteReview(Review Review);
        bool Save();
        bool AddImageAsync(string name, IFormFile file);
        bool DeleteImageAsync(string name);
    }
}
