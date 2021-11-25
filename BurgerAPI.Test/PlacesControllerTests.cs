using System;
using Xunit;
using FakeItEasy;
using BurgerAPI.Controllers;
using BurgerAPI.Repository.IRepository;
using AutoMapper;
using BurgerAPI.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BurgerAPI.Models.Dtos;

namespace BurgerAPI.Test
{
    public class PlacesControllerTests
    {

        static readonly IPlaceRepository pRep = A.Fake<IPlaceRepository>();
        static readonly IMapper map = A.Fake<IMapper>();
        readonly PlacesController controller = new PlacesController(pRep, map);

        [Fact]
        public void GetPlacesReturnsAllPlaces()
        {
            //Arrange
            int count = 21;
            var fakePlaces = A.CollectionOfDummy<Place>(count);
            A.CallTo(() => pRep.GetPlaces()).Returns(fakePlaces);

            //Act
            var actionResult = controller.GetPlaces();

            //Assert
            var result = actionResult as OkObjectResult;
            var resultPlaces = (IList<PlaceDto>)result.Value;
            Assert.Equal(count, resultPlaces.Count);
        }

        [Fact]
        public void GetPlaceReturnsAPlace()
        {
            //Arrange
            var fakePlace = new Place();
            A.CallTo(() => pRep.GetPlace(1)).Returns(fakePlace);

            //Act
            var actionResult = controller.GetPlace(1);

            //Assert
            var result = actionResult as OkObjectResult;
            var resultPlace = (PlaceDto)result.Value;
            Assert.Equal(resultPlace.ToString(), map.Map<PlaceDto>(fakePlace).ToString());

        }
    }
}
