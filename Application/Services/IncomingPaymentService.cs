using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using System.Text.Json;

namespace MySapProject.Application.Services
{
    public class IncomingPaymentService : IIncomingPaymentService
    {
        private readonly IIncomingPaymentRepository _repository;

        public IncomingPaymentService(IIncomingPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IncomingPaymentDto>> GetAllAsync()
        {
            var json = await _repository.GetAllAsync();
            using var doc = JsonDocument.Parse(json);
            var value = doc.RootElement.GetProperty("value");
            return JsonSerializer.Deserialize<List<IncomingPaymentDto>>(value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }

        public async Task<IncomingPaymentDto?> GetByIdAsync(int id)
        {
            var json = await _repository.GetByIdAsync(id);
            var entity = JsonSerializer.Deserialize<IncomingPayment>(json);

            if (entity == null)
            {
                return null; // Handle the case where deserialization returns null
            }

            return ToDto(entity);
        }

        public async Task<IEnumerable<IncomingPaymentDto>> GetFilteredAsync(string? filter, string? select, string? orderBy, int? top, int? skip)
        {
            var json = await _repository.GetFilteredAsync(filter, select, orderBy, top, skip);
            using var doc = JsonDocument.Parse(json);
            var value = doc.RootElement.GetProperty("value");
            return JsonSerializer.Deserialize<List<IncomingPaymentDto>>(value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }

        public async Task<IncomingPaymentDto> CreateAsync(IncomingPaymentDto dto)
        {
            var entity = ToEntity(dto);
            var result = await _repository.CreateAsync(entity);
            return JsonSerializer.Deserialize<IncomingPaymentDto>(result) ?? throw new Exception("Deserialization failed");
        }

        public async Task UpdateAsync(int id, object updatePayload)
        {
            await _repository.UpdateAsync(id, updatePayload);
        }

        public async Task CancelAsync(int id) => await _repository.CancelAsync(id);

        public async Task<string> GetApprovalTemplatesAsync(int id) => await _repository.GetApprovalTemplatesAsync(id);

        public async Task<string> CancelByCurrentSystemDateAsync(int id) => await _repository.CancelByCurrentSystemDateAsync(id);

        public async Task<string> RequestApproveCancellationAsync(int id) => await _repository.RequestApproveCancellationAsync(id);

        private IncomingPaymentDto ToDto(IncomingPayment entity) => new()
        {
            DocEntry = entity.DocEntry,
            Remarks = entity.Remarks,
            CardName = entity.CardName,
            DocDate = entity.DocDate,
            DocCurrency = entity.DocCurrency,
            CashSum = entity.CashSum,
            CardCode = entity.CardCode // Assuming CardCode is also part of the entity
        };

        private IncomingPayment ToEntity(IncomingPaymentDto dto) => new()
        {
            DocEntry = dto.DocEntry,
            Remarks = dto.Remarks,
            CardName = dto.CardName,
            DocDate = dto.DocDate,
            DocCurrency = dto.DocCurrency,
            CashSum = dto.CashSum,
            CardCode = dto.CardCode
        };
    }
}