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
    public class BurgersControllerTests
    {

        static readonly IBurgerRepository bRep = A.Fake<IBurgerRepository>();
        static readonly IMapper map = A.Fake<IMapper>();
        readonly BurgersController controller = new BurgersController(bRep, map);

        [Fact]
        public void GetBurgersReturnsAllBurgers()
        {
            //Arrange
            int count = 21;
            var fakeBurgers = A.CollectionOfDummy<Burger>(count);
            A.CallTo(() => bRep.GetBurgers()).Returns(fakeBurgers);

            //Act
            var actionResult = controller.GetBurgers();

            //Assert
            var result = actionResult as OkObjectResult;
            var resultBurgers = (IList<BurgerDto>)result.Value;
            Assert.Equal(count, resultBurgers.Count);
        }

        [Fact]
        public void GetBurgerReturnsABurger()
        {
            //Arrange
            var fakeBurger = new Burger();
            A.CallTo(() => bRep.GetBurger(1)).Returns(fakeBurger);

            //Act
            var actionResult = controller.GetBurger(1);

            //Assert
            var result = actionResult as OkObjectResult;
            var resultBurger = (BurgerDto)result.Value;
            Assert.Equal(resultBurger.ToString(), map.Map<BurgerDto>(fakeBurger).ToString());

        }

        [Fact]
        public void CreateBurgerSuccess()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "InMemoryDb");
            var options = builder.Options;

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new BurgersController(new BurgerRepository(context), map);

                var testBurger = new BurgerCreateDto()
                {
                    Name = "123",
                    PlaceId = 1,
                    Size = Burger.BurgerSize.Big,
                    Type = Burger.BurgerType.Normal,

                };
                var actionResult = controller.CreateBurger(testBurger);

                var result = (ObjectResult)actionResult;

                //Assert
                Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            }
        }
    }
}
