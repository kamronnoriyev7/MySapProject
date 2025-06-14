using Microsoft.AspNetCore.Mvc;
using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.Application.Services;

namespace MySapProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnerController : ControllerBase
    {
        private readonly IBusinessPartnerService _service;

        public BusinessPartnerController(IBusinessPartnerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{cardCode}")]
        public async Task<IActionResult> GetById(string cardCode)
        {
            var result = await _service.GetByIdAsync(cardCode);
            if (result == null)
                return NotFound($"Business partner with CardCode '{cardCode}' not found.");

            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(
                [FromQuery] string? filter = null,
                [FromQuery] string? select = null,
                [FromQuery] string? orderBy = null,
                [FromQuery] int? top = null,
                [FromQuery] int? skip = null)
        {
            var result = await _service.GetFilteredAsync(
                filter: filter,
                select: select,
                orderBy: orderBy,
                top: top,
                skip: skip
            );

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessPartnerDto partnerDto)
        {
            if (partnerDto == null)
                return BadRequest("Invalid data.");

            var created = await _service.CreateAsync(partnerDto);
            return CreatedAtAction(nameof(GetById), new { cardCode = created.CardCode }, created);
        }

        [HttpPut("{cardCode}")]
        public async Task<IActionResult> Update(string cardCode, [FromBody] BusinessPartnerDto partnerDto)
        {
            if (partnerDto == null || string.IsNullOrWhiteSpace(cardCode))
                return BadRequest("Invalid data.");

            await _service.UpdateAsync(cardCode, partnerDto);
            return NoContent();
        }

        [HttpDelete("{cardCode}")]
        public async Task<IActionResult> Delete(string cardCode)
        {
            await _service.DeleteAsync(cardCode);
            return NoContent();
        }
    }
}
