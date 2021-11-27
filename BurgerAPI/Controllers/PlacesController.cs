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
    [Route("api/v{version:apiVersion}/places")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceRepository _pRepo;
        private readonly IMapper _mapper;
        public PlacesController(IPlaceRepository pRepo, IMapper mapper)
        {
            _pRepo = pRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of burger places.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlaceDto>))]
        public IActionResult GetPlaces()
        {
            var objList = _pRepo.GetPlaces();
            var objDto = new List<PlaceDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<PlaceDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get an individual burger place.
        /// </summary>
        /// <param name="PlaceId"> The Id of the burger place.</param>
        /// <returns></returns>

        [HttpGet("{PlaceId:int}", Name = "GetPlace")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaceDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [ProducesDefaultResponseType]
        public IActionResult GetPlace(int PlaceId)
        {
            var obj = _pRepo.GetPlace(PlaceId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<PlaceDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Create burger place
        /// </summary>
        /// <param name="PlaceDto"> Burger place to be added.</param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlaceDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreatePlace([FromBody]PlaceCreateDto PlaceDto, ApiVersion version)
        {
            if(PlaceDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_pRepo.PlaceExists(PlaceDto.Name))
            {
                ModelState.AddModelError("", "Place Already Exists!");
                return StatusCode(404, ModelState);
            }
            var Placeobj = _mapper.Map<Place>(PlaceDto);
            if(!_pRepo.CreatePlace(Placeobj))
            {
                ModelState.AddModelError("",$"Something went wrong when saving the record {Placeobj.Name}");
                return StatusCode(500, ModelState);
            }
            //var version = HttpContext.GetRequestedApiVersion();
            return CreatedAtRoute("GetPlace",new {
                V = version.ToString(), 
                PlaceId = Placeobj.Id },Placeobj);
        }

        /// <summary>
        /// Update a burger place
        /// </summary>
        /// <param name="PlaceId"> The Id of the burger place.</param>
        /// <param name="PlaceDto"> The details of the burger place.</param>
        /// <returns></returns>
        [HttpPatch("{PlaceId:int}", Name = "UpdatePlace")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePlace(int PlaceId, [FromBody] PlaceUpdateDto PlaceDto)
        {

            if (PlaceDto == null || PlaceId != PlaceDto.Id)
            {
                return BadRequest(ModelState);
            }
            var Placeobj = _mapper.Map<Place>(PlaceDto);
            if (!_pRepo.UpdatePlace(Placeobj))
            {
                ModelState.AddModelError("", $"Something went wrong when updatnig the record {Placeobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a burger place.
        /// </summary>
        /// <param name="PlaceId"> The Id of the burger place.</param>
        /// <returns></returns>
        [HttpDelete("{PlaceId:int}", Name = "DeletePlace")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePlace(int PlaceId)
        {

            if (!_pRepo.PlaceExists(PlaceId))
            {
                return NotFound();
            }
            var Placeobj = _pRepo.GetPlace(PlaceId);
            if (!_pRepo.DeletePlace(Placeobj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {Placeobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Get all available cities.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]",Name = "GetAllCities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        public IActionResult GetAllCities()
        {
            var cities = _pRepo.GetAllCities();

            if (cities.Count == 0)
            {
                return NotFound();
            }
            return Ok(cities);
        }
    }
}
