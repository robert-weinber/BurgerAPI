using Microsoft.EntityFrameworkCore;
using BurgerAPI.Data;
using BurgerAPI.Models;
using BurgerAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Http;

namespace BurgerAPI.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly CloudBlobClient _cloudBlobClient;
        public ReviewRepository(ApplicationDbContext db, CloudBlobClient cloudBlobClient)
        {
            _db = db;
            _cloudBlobClient = cloudBlobClient;
        }
        public bool CreateReview(Review Review)
        {
            _db.Reviews.Add(Review);
            return Save();
        }

        public bool DeleteReview(Review Review)
        {
            _db.Reviews.Remove(Review);
            return Save();
        }

        public Review GetReview(int ReviewId)
        {
            return _db.Reviews.Include(a => a.Burger).FirstOrDefault(n => n.Id == ReviewId);
        }

        public ICollection<Review> GetReviews()
        {
            return _db.Reviews.Include(a => a.Burger).OrderBy(a=>a.Text).ToList();
        }

        public bool ReviewExists(int burgerId, int userId)
        {
            return _db.Reviews.Any(a => a.BurgerId == burgerId && a.UserId == userId);
        }

        public bool ReviewExists(int id)
        {
            return _db.Reviews.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public bool UpdateReview(Review Review)
        {
            _db.Reviews.Update(Review);
            return Save();
        }

        public ICollection<Review> GetReviewsFromPlace(int pId)
        {
            return _db.Reviews.Include(a => a.Burger).Where(a => a.Burger.PlaceId == pId).ToList();
        }

        public ICollection<Review> GetReviewsFromBurger(int bId)
        {
            return _db.Reviews.Include(a => a.Burger).Where(a => a.BurgerId == bId).ToList();
        }

        public ICollection<Review> GetReviewsFromUser(int uId)
        {
            return _db.Reviews.Include(a => a.Burger).Where(a => a.UserId == uId).ToList();
        }

        public bool AddImageAsync(string name, IFormFile file)
        {
            string FileName = name;
            var container = _cloudBlobClient.GetContainerReference("reviews");
            var blob = container.GetBlockBlobReference(FileName);

            try
            {

                var _task = Task.Run(() => blob.UploadFromStreamAsync(file.OpenReadStream()));
                _task.Wait();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return true;
        }
    }
}
