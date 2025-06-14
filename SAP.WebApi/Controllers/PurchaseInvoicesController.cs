using Microsoft.AspNetCore.Mvc;
using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.Application.Services;

namespace MySapProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseInvoicesController : ControllerBase
{
    private readonly IPurchaseInvoiceService _service;

    public PurchaseInvoicesController(IPurchaseInvoiceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int top = 10)
    {
        var result = await _service.GetAllAsync(skip, top);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await _service.GetByIdAsync(id);
        if (invoice == null)
            return NotFound($"Purchase invoice with ID {id} not found.");
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseInvoiceDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PurchaseInvoiceDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _service.CancelAsync(id);
        return Ok($"Purchase invoice with ID {id} has been canceled.");
    }
}
