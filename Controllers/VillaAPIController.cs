using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]


    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;

        public VillaAPIController(ILogger<VillaAPIController> logger) { _logger = logger; }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("getting all villas");
            return Ok(VillaStore.villalist);

        }

        [HttpGet("{id:int}",Name ="GetVilla" )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {


            if (id == 0)
            {
                _logger.LogError("Get Villa error" + id);
                return BadRequest();
            }
            var villa = (VillaStore.villalist.FirstOrDefault(u => u.Id == id));
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
        {
            if (VillaStore.villalist.FirstOrDefault(u => u.Name?.ToLower() == villaDto.Name?.ToLower())!=null)
            {
                ModelState.AddModelError("CustomError", "Villa already exist");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); 
            }

            //villaDto.Id= VillaStore.villalist.OrderByDescending(u=>u.Id).FirstOrDefault().Id+1;
            //VillaStore.villalist.Add(villaDto);

            var villa = VillaStore.villalist.OrderByDescending(u => u.Id).FirstOrDefault();
            if (villa != null)
            {
                villaDto.Id = villa.Id + 1;
            }


            return CreatedAtRoute("GetVilla",new { id= villaDto.Id},villaDto);       
        }


        [HttpDelete("{id:int}", Name = "DeleteVilla")] [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.villalist.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]

        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest(StatusCodes.Status500InternalServerError);
            }

            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound(); // Return NotFound if the villa with the specified id is not found
            }

            // Update the villa properties
            villa.Name = villaDto.Name;
            villa.Sqft = villaDto.Sqft;
            villa.Occupancy = villaDto.Occupancy;

            return NoContent();
        }


        [HttpPatch("{id: int}", Name = "UpdatePartialVilla")]

        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto) 
        {
            if (patchDto== null || id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id==id);

            if (villa== null) 
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villa, ModelState);
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
 