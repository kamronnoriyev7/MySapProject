using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySapProject.Application.DTOs;
using MySapProject.Application.Services;

namespace MySapProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomingPaymentController : ControllerBase
    {
        private readonly IIncomingPaymentService _incomingPaymentService;

        public IncomingPaymentController(IIncomingPaymentService incomingPaymentService)
        {
            _incomingPaymentService = incomingPaymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var payments = await _incomingPaymentService.GetAllAsync();
            return Ok(payments);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest("ID cannot be null.");
            var payment = await _incomingPaymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound($"Incoming payment with ID '{id}' not found.");
            return Ok(payment);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredAsync(
            [FromQuery] string? filter = null,
            [FromQuery] string? select = null,
            [FromQuery] string? orderBy = null,
            [FromQuery] int? top = null,
            [FromQuery] int? skip = null)
        {
            var payments = await _incomingPaymentService.GetFilteredAsync(filter, select, orderBy, top, skip);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] IncomingPaymentDto incomingPaymentDto)
        {
            if (incomingPaymentDto == null)
                return BadRequest("Incoming payment data cannot be null.");
            var createdPayment = await _incomingPaymentService.CreateAsync(incomingPaymentDto);
            return Ok(createdPayment);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] object updatePayload)
        {
            if (id <= 0)
                return BadRequest("ID cannot be null.");
            await _incomingPaymentService.UpdateAsync(id, updatePayload);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> CancelAsync(int id)
        {
            if (id <= 0)
                return BadRequest("ID cannot be null.");
            await _incomingPaymentService.CancelAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}/approval-templates")]
        public async Task<IActionResult> GetApprovalTemplatesAsync(int id)
        {
            if (id <= 0)
                return BadRequest("ID cannot be null.");
            var templates = await _incomingPaymentService.GetApprovalTemplatesAsync(id);
            return Ok(templates);
        }

        [HttpPost("{id:int}/cancel-by-current-system-date")]
        public async Task<IActionResult> CancelByCurrentSystemDateAsync(int id)
        {
            if (id <= 0)
                return BadRequest("ID cannot be null.");
            var result = await _incomingPaymentService.CancelByCurrentSystemDateAsync(id);
            return Ok(result);
        }

        [HttpPost("{id:int}/request-approve-cancellation")]
        public async Task<IActionResult> RequestApproveCancellationAsync(int id)
        {
            if (id <= 0)
                return BadRequest("ID cannot be null.");
            var result = await _incomingPaymentService.RequestApproveCancellationAsync(id);
            return Ok(result);
        }
    }
}
