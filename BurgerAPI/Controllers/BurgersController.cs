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
    [Route("api/v{version:apiVersion}/burgers")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecBurgers")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class BurgersController : ControllerBase
    {
        private readonly IBurgerRepository _BurgerRepo;
        private readonly IMapper _mapper;
        public BurgersController(IBurgerRepository BurgerRepo, IMapper mapper)
        {
            _BurgerRepo = BurgerRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the list of all Burgers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BurgerDto>))]
        public IActionResult GetBurgers()
        {
            var objList = _BurgerRepo.GetBurgers();
            var objDto = new List<BurgerDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<BurgerDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get individual Burger.
        /// </summary>
        /// <param name="BurgerId"> The Id of the Burger.</param>
        /// <returns></returns>

        [HttpGet("{BurgerId:int}", Name = "GetBurger")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BurgerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBurger(int BurgerId)
        {
            var obj = _BurgerRepo.GetBurger(BurgerId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<BurgerDto>(obj);
            return Ok(objDto);
        }
        /// <summary>
        /// Get list of Burgers in a Place.
        /// </summary>
        /// <param name="nationalParkId"> The Id of the Place.</param>
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BurgerDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetBurgerInPlace(int nationalParkId)
        {
            var objList = _BurgerRepo.GetBurgersInPlace(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<BurgerDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<BurgerDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Create Burger
        /// </summary>
        /// <param name="BurgerDto"> Burger to be added.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BurgerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateBurger([FromBody]BurgerCreateDto BurgerDto)
        {
            if(BurgerDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_BurgerRepo.BurgerExistsInPlace(BurgerDto.PlaceId, BurgerDto.Name))
            {
                ModelState.AddModelError("", "Burger Already Exists!");
                return StatusCode(404, ModelState);
            }
            var Burgerobj = _mapper.Map<Burger>(BurgerDto);
            if(!_BurgerRepo.CreateBurger(Burgerobj))
            {
                ModelState.AddModelError("",$"Something went wrong when saving the record {Burgerobj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetBurger",new { BurgerId = Burgerobj.Id },Burgerobj);
        }

        /// <summary>
        /// Update a Burger
        /// </summary>
        /// <param name="BurgerId"> The Id of the Burger.</param>
        /// <param name="BurgerDto"> The details of the Burger.</param>
        /// <returns></returns>
        [HttpPatch("{BurgerId:int}", Name = "UpdateBurger")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateBurger(int BurgerId, [FromBody] BurgerUpdateDto BurgerDto)
        {

            if (BurgerDto == null || BurgerId != BurgerDto.Id)
            {
                return BadRequest(ModelState);
            }
            var Burgerobj = _mapper.Map<Burger>(BurgerDto);
            if (!_BurgerRepo.UpdateBurger(Burgerobj))
            {
                ModelState.AddModelError("", $"Something went wrong when updatnig the record {Burgerobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a Burger.
        /// </summary>
        /// <param name="BurgerId"> The Id of the Burger.</param>
        /// <returns></returns>
        [HttpDelete("{BurgerId:int}", Name = "DeleteBurger")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteBurger(int BurgerId)
        {

            if (!_BurgerRepo.BurgerExists(BurgerId))
            {
                return NotFound();
            }
            var Burgerobj = _BurgerRepo.GetBurger(BurgerId);
            if (!_BurgerRepo.DeleteBurger(Burgerobj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {Burgerobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
