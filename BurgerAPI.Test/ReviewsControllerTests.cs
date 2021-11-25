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
using Microsoft.WindowsAzure.Storage.Blob;

namespace BurgerAPI.Test
{
    public class ReviewsControllerTests
    {

        static readonly IReviewRepository rRep = A.Fake<IReviewRepository>();
        static readonly IMapper map = A.Fake<IMapper>();
        static readonly IMapper blobClient = A.Fake<IMapper>();
        readonly ReviewsController controller = new ReviewsController(rRep, map);

        [Fact]
        public void GetReviewsReturnsAllReviews()
        {
            //Arrange
            int count = 21;
            var fakeReviews = A.CollectionOfDummy<Review>(count);
            A.CallTo(() => rRep.GetReviews()).Returns(fakeReviews);

            //Act
            var actionResult = controller.GetReviews();

            //Assert
            var result = actionResult as OkObjectResult;
            var resultReviews = (IList<ReviewDto>)result.Value;
            Assert.Equal(count, resultReviews.Count);
        }

        [Fact]
        public void GetReviewReturnsAReview()
        {
            //Arrange
            var fakeReview = new Review();
            A.CallTo(() => rRep.GetReview(1)).Returns(fakeReview);

            //Act
            var actionResult = controller.GetReview(1);

            //Assert
            var result = actionResult as OkObjectResult;
            var resultReview = (ReviewDto)result.Value;
            Assert.Equal(resultReview.ToString(), map.Map<ReviewDto>(fakeReview).ToString());

        }

        [Fact]
        public void CreateReviewSuccess()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "InMemoryDb");
            var options = builder.Options;

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReviewsController(new ReviewRepository(context, new CloudBlobClient(new Uri("http://www.myserver.com"))), map);

                var testReview = new ReviewCreateDto()
                {
                    BurgerId = 1,
                    ImageUrl = "sdf",
                    Taste = 1,
                    Text="sg",
                    Texture=1,
                    Title = "ad",
                    UserId=1,
                    Visual=2

                };
                var actionResult = controller.CreateReview(testReview);

                var result = (ObjectResult)actionResult;

                //Assert
                Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            }
        }
    }
}
