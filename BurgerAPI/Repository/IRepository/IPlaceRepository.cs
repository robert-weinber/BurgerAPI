using BurgerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Repository.IRepository
{
    public interface IPlaceRepository
    {
        ICollection<Place> GetPlaces();
        ICollection<Place> GetPlacesByCity(string City);
        Place GetPlace(int PlaceId);
        bool PlaceExists(string name);
        bool PlaceExists(int id);
        bool CreatePlace(Place Place);
        bool UpdatePlace(Place Place);
        bool DeletePlace(Place Place);
        bool Save();
    }
}
