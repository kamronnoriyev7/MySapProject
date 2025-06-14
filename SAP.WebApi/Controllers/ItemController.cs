using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySapProject.Application.DTOs;
using MySapProject.Application.Services;

namespace MySapProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]   
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{itemCode}")]
        public async Task<IActionResult> GetByIdAsync(string itemCode)
        {
            if (string.IsNullOrWhiteSpace(itemCode))
                return BadRequest("Item code cannot be null or empty.");
            var item = await _itemService.GetByIdAsync(itemCode);
            if (item == null)
                return NotFound($"Item with code '{itemCode}' not found.");
            return Ok(item);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredAsync(
            [FromQuery] string? filter = null,
            [FromQuery] string? select = null,
            [FromQuery] string? orderBy = null,
            [FromQuery] int? top = null,
            [FromQuery] int? skip = null)
        {
            var items = await _itemService.GetFilteredAsync(filter, select, orderBy, top, skip);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ItemDto itemDto)
        {
            if (itemDto == null)
                return BadRequest("Item data cannot be null.");

            var createdItem = await _itemService.CreateAsync(itemDto);

             return Ok(createdItem); 
        }


        [HttpPut("{itemCode}")]
        public async Task<IActionResult> UpdateAsync(string itemCode, [FromBody] ItemDto itemDto)
        {
            if (string.IsNullOrWhiteSpace(itemCode))
                return BadRequest("Item code cannot be null or empty.");
            if (itemDto == null)
                return BadRequest("Item data cannot be null.");
            await _itemService.UpdateAsync(itemCode, itemDto);
            return NoContent();
        }

        [HttpDelete("{itemCode}")]
        public async Task<IActionResult> DeleteAsync(string itemCode)
        {
            if (string.IsNullOrWhiteSpace(itemCode))
                return BadRequest("Item code cannot be null or empty.");
            await _itemService.DeleteAsync(itemCode);
            return NoContent();
        }
    }
}
