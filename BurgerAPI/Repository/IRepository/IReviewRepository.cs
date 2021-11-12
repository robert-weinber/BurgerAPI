﻿using BurgerAPI.Models;
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
        Review GetReview(int ReviewId);
        bool ReviewExists(int burgerId, int userId);
        bool ReviewExists(int id);
        bool CreateReview(Review Review);
        bool UpdateReview(Review Review);
        bool DeleteReview(Review Review);
        bool Save();
    }
}