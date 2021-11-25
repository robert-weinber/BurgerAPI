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
using Microsoft.EntityFrameworkCore;
using BurgerAPI.Data;
using BurgerAPI.Repository;
using Microsoft.AspNetCore.Http;

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

        [Fact]
        public void CreatePlaceSuccess()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "InMemoryDb");
            var options = builder.Options;

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new PlacesController(new PlaceRepository(context), map);

                var testPlace = new PlaceCreateDto()
                {
                    City = "asd",
                    Address = "asd",
                    Name = "asd",
                    Info = "asd",
                    Openings = "asd"

                };
                var actionResult = controller.CreatePlace(testPlace, new ApiVersion(1, 0));

                var result = (ObjectResult)actionResult;

                //Assert
                Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            }
        }
    }
}
