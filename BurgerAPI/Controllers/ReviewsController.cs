using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BurgerAPI.Models;
using BurgerAPI.Models.Dtos;
using BurgerAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _ReviewRepo;
        private readonly IMapper _mapper;
        public ReviewsController(IReviewRepository ReviewRepo, IMapper mapper)
        {
            _ReviewRepo = ReviewRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the list of all Reviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var objList = _ReviewRepo.GetReviews();
            var objDto = new List<ReviewDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ReviewDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get individual Burger.
        /// </summary>
        /// <param name="ReviewId"> The Id of the Burger.</param>
        /// <returns></returns>

        [HttpGet("{ReviewId:int}", Name = "GetReview")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBurger(int ReviewId)
        {
            var obj = _ReviewRepo.GetReview(ReviewId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<ReviewDto>(obj);
            return Ok(objDto);
        }
        /// <summary>
        /// Get list of Reviews in a Place.
        /// </summary>
        /// <param name="placeId"> The Id of the Place.</param>
        /// <returns></returns>
        [HttpGet("[action]/{placeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReviewDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetBurgerInPlace(int placeId)
        {
            var objList = _ReviewRepo.GetReviewsFromPlace(placeId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<ReviewDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ReviewDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Create Review
        /// </summary>
        /// <param name="ReviewDto"> Review to be added.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReviewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateReview([FromBody]ReviewCreateDto ReviewDto)
        {
            if(ReviewDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_ReviewRepo.ReviewExists(ReviewDto.BurgerId, ReviewDto.UserId))
            {
                ModelState.AddModelError("", "Review Already Exists!");
                return StatusCode(404, ModelState);
            }
            var Reviewobj = _mapper.Map<Review>(ReviewDto);
            if(!_ReviewRepo.CreateReview(Reviewobj))
            {
                ModelState.AddModelError("",$"Something went wrong when saving the record {Reviewobj.Title}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetReview", new { ReviewId = Reviewobj.Id }, Reviewobj);
        }

        /// <summary>
        /// Update a Review
        /// </summary>
        /// <param name="ReviewId"> The Id of the Review.</param>
        /// <param name="ReviewDto"> The details of the Review.</param>
        /// <returns></returns>
        [HttpPatch("{ReviewId:int}", Name = "UpdateReview")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateReview(int ReviewId, [FromBody] ReviewUpdateDto ReviewDto)
        {

            if (ReviewDto == null || ReviewId != ReviewDto.Id)
            {
                return BadRequest(ModelState);
            }
            var Reviewobj = _mapper.Map<Review>(ReviewDto);
            if (!_ReviewRepo.UpdateReview(Reviewobj))
            {
                ModelState.AddModelError("", $"Something went wrong when updatnig the record {Reviewobj.Title}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a Review.
        /// </summary>
        /// <param name="ReviewId"> The Id of the Review.</param>
        /// <returns></returns>
        [HttpDelete("{ReviewId:int}", Name = "DeleteReview")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteReview(int ReviewId)
        {

            if (!_ReviewRepo.ReviewExists(ReviewId))
            {
                return NotFound();
            }
            var Reviewobj = _ReviewRepo.GetReview(ReviewId);
            if (!_ReviewRepo.DeleteReview(Reviewobj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {Reviewobj.Title}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
