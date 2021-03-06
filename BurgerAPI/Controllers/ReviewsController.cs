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
using Microsoft.WindowsAzure.Storage.Blob;

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
        /// Get individual Review.
        /// </summary>
        /// <param name="ReviewId"> The Id of the Review.</param>
        /// <returns></returns>

        [HttpGet("{ReviewId:int}", Name = "GetReview")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        public IActionResult GetReview(int ReviewId)
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
        /// Get list of Reviews from a specific Place.
        /// </summary>
        /// <param name="placeId"> The Id of the Place.</param>
        /// <returns></returns>
        [HttpGet("[action]/{placeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReviewDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetReviewFromPlace(int placeId)
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
        /// Get list of Reviews about a specific Burger.
        /// </summary>
        /// <param name="burgerId"> The Id of the Burger.</param>
        /// <returns></returns>
        [HttpGet("[action]/{burgerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReviewDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetReviewFromBurger(int burgerId)
        {
            var objList = _ReviewRepo.GetReviewsFromBurger(burgerId);
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
        /// Get list of Reviews from a specific User.
        /// </summary>
        /// <param name="userId"> The Id of the User.</param>
        /// <returns></returns>
        [HttpGet("[action]/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReviewDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetReviewFromUser(int userId)
        {
            var objList = _ReviewRepo.GetReviewsFromUser(userId);
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
            var delImage = (ObjectResult)DeleteImageFromReview(ReviewId);
            if (delImage.StatusCode != StatusCodes.Status204NoContent)
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the image of he record {Reviewobj.Title}");
            }
            return NoContent();
        }

        /// <summary>
        /// Adds an image to a review.
        /// </summary>
        /// <param name="ReviewId"> The Id of the Review.</param>
        /// <param name="file"> The image to be uploaded</param>
        /// <returns></returns>
        [HttpPatch("[action]/{ReviewId:int}", Name = "AddImageToReview")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult AddImageToReview(int ReviewId, [FromForm] IFormFile file)
        {
            var obj = _ReviewRepo.GetReview(ReviewId);
            if (obj == null)
            {
                return NotFound();
            }

            var FileName = $"{ReviewId}.jpg";

            if (!_ReviewRepo.AddImageAsync(FileName, file))
            {
                ModelState.AddModelError("", $"Something went wrong when uploading image to review {ReviewId}");
                return StatusCode(500, ModelState);
            }

            obj.ImageUrl = $"https://wrobi.blob.core.windows.net/reviews/{FileName}";
            if (!_ReviewRepo.UpdateReview(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when updatnig the image URL of review {ReviewId}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes image from a review.
        /// </summary>
        /// <param name="ReviewId"> The Id of the Review.</param>
        /// <returns></returns>
        [HttpPatch("[action]/{ReviewId:int}", Name = "DeleteImageFromReview")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult DeleteImageFromReview(int ReviewId)
        {
            var obj = _ReviewRepo.GetReview(ReviewId);
            if (obj == null)
            {
                return NotFound();
            }

            var FileName = $"{ReviewId}.jpg";

            if (!_ReviewRepo.DeleteImageAsync(FileName))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting image from review {ReviewId}");
                return StatusCode(500, ModelState);
            }

            obj.ImageUrl = "";
            if (!_ReviewRepo.UpdateReview(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when updatnig the image URL of review {ReviewId}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
