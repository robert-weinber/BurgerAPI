using Microsoft.EntityFrameworkCore;
using BurgerAPI.Data;
using BurgerAPI.Models;
using BurgerAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Repository
{
    public class BurgerRepository : IBurgerRepository
    {
        private readonly ApplicationDbContext _db;
        public BurgerRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateBurger(Burger Burger)
        {
            _db.Burgers.Add(Burger);
            return Save();
        }

        public bool DeleteBurger(Burger Burger)
        {
            _db.Burgers.Remove(Burger);
            return Save();
        }

        public Burger GetBurger(int BurgerId)
        {
            return _db.Burgers.Include(a => a.Place).FirstOrDefault(n => n.Id == BurgerId);
        }

        public ICollection<Burger> GetBurgers()
        {
            return _db.Burgers.Include(a => a.Place).OrderBy(a=>a.Name).ToList();
        }

        public bool BurgerExistsInPlace(int placeId, string name)
        {
            return _db.Burgers.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim() && a.PlaceId == placeId);
        }

        public bool BurgerExists(int id)
        {
            return _db.Burgers.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public bool UpdateBurger(Burger Burger)
        {
            _db.Burgers.Update(Burger);
            return Save();
        }

        public ICollection<Burger> GetBurgersInPlace(int pId)
        {
            return _db.Burgers.Include(a => a.Place).Where(a => a.PlaceId == pId).ToList();
        }
    }
}
