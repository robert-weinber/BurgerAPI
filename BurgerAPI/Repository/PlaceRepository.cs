using BurgerAPI.Data;
using BurgerAPI.Models;
using BurgerAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Repository
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly ApplicationDbContext _db;
        public PlaceRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreatePlace(Place Place)
        {
            _db.Places.Add(Place);
            return Save();
        }

        public bool DeletePlace(Place Place)
        {
            _db.Places.Remove(Place);
            return Save();
        }

        public ICollection<string> GetAllCities()
        {
            return _db.Places.Select(a=> a.City).Distinct().ToList();
        }

        public Place GetPlace(int PlaceId)
        {
            return _db.Places.FirstOrDefault(n => n.Id == PlaceId);
        }

        public ICollection<Place> GetPlaces()
        {
            return _db.Places.OrderBy(a=>a.Name).ToList();
        }

        public ICollection<Place> GetPlacesByCity(string City)
        {
            return _db.Places.Where(a => a.City == City).OrderBy(a => a.Name).ToList();
        }

        public bool PlaceExists(string name)
        {
            return _db.Places.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool PlaceExists(int id)
        {
            return _db.Places.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdatePlace(Place Place)
        {
            _db.Places.Update(Place);
            return Save();
        }
    }
}
