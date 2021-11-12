using BurgerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Repository.IRepository
{
    public interface IBurgerRepository
    {
        ICollection<Burger> GetBurgers();
        ICollection<Burger> GetBurgersInPlace(int pId);
        Burger GetBurger(int BurgerId);
        bool BurgerExistsInPlace(int placeId, string name);
        bool BurgerExists(int id);
        bool CreateBurger(Burger Burger);
        bool UpdateBurger(Burger Burger);
        bool DeleteBurger(Burger Burger);
        bool Save();
    }
}
