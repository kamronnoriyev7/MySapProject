using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MySapProject.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IItemRepository repository, ILogger<ItemService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            try
            {
                var json = await _repository.GetAllAsync();
                _logger.LogInformation("GetAllAsync - SAP Response: {Json}", json);

                if (string.IsNullOrEmpty(json)) throw new Exception("Bo'sh javob keldi");

                using var document = JsonDocument.Parse(json);
                var valueElement = document.RootElement.GetProperty("value");

                return JsonSerializer.Deserialize<IEnumerable<ItemDto>>(valueElement.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ItemDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync - xatolik yuz berdi");
                throw;
            }
        }

        public async Task<ItemDto?> GetByIdAsync(string itemCode)
        {
            try
            {
                var response = await _repository.GetByIdAsync(itemCode);
                _logger.LogInformation("GetByIdAsync - SAP Response: {Response}", response);

                if (string.IsNullOrEmpty(response)) throw new Exception($"Item '{itemCode}' topilmadi.");

                var entity = JsonSerializer.Deserialize<Item>(response)
                    ?? throw new Exception("Deserializatsiya xatoligi.");
                return ToDto(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByIdAsync - xatolik");
                throw;
            }
        }

        public async Task<IEnumerable<ItemDto>> GetFilteredAsync(string filter, string select, string orderBy, int? top, int? skip)
        {
            try
            {
                var json = await _repository.GetFilteredAsync(filter, select, orderBy, top, skip);
                _logger.LogInformation("GetFilteredAsync - SAP Response: {Json}", json);

                using var doc = JsonDocument.Parse(json);
                var value = doc.RootElement.GetProperty("value");

                return JsonSerializer.Deserialize<List<ItemDto>>(value.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ItemDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFilteredAsync - xatolik");
                throw;
            }
        }

        public async Task<ItemDto> CreateAsync(ItemDto dto)
        {
            try
            {
                var entity = ToEntity(dto);
                var requestJson = JsonSerializer.Serialize(entity);
                _logger.LogInformation("CreateAsync - Yuborilayotgan JSON: {RequestJson}", requestJson);

                var created = await _repository.CreateAsync(entity);
                _logger.LogInformation("CreateAsync - SAP Response: {Created}", created);

                return JsonSerializer.Deserialize<ItemDto>(created)
                    ?? throw new Exception("Deserializatsiya muvaffaqiyatsiz.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAsync - xatolik");
                throw;
            }
        }

        public async Task UpdateAsync(string itemCode, ItemDto dto)
        {
            try
            {
                var entity = ToEntity(dto);
                var updateJson = JsonSerializer.Serialize(entity);
                _logger.LogInformation("UpdateAsync - Yuborilayotgan JSON: {UpdateJson}", updateJson);

                await _repository.UpdateAsync(itemCode, entity);
                _logger.LogInformation("UpdateAsync - itemCode: {ItemCode} muvaffaqiyatli yangilandi", itemCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync - xatolik, itemCode: {ItemCode}", itemCode);
                throw;
            }
        }

        public async Task DeleteAsync(string itemCode)
        {
            try
            {
                await _repository.DeleteAsync(itemCode);
                _logger.LogInformation("DeleteAsync - ItemCode: {ItemCode} o'chirildi", itemCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAsync - xatolik, itemCode: {ItemCode}", itemCode);
                throw;
            }
        }

        private static Item ToEntity(ItemDto dto)
        {
            return new Item
            {
                ItemCode = dto.ItemCode,
                ItemName = dto.ItemName,
                ItemType = "itItems", // SAP talabi
                InventoryUOM = dto.InventoryUOM,
                ItemsGroupCode = dto.ItemsGroupCode,
                U_TypeGroup = dto.U_TypeGroup  // Default qiymat qo'shish
            };
        }

        private static ItemDto ToDto(Item item)
        {
            return new ItemDto
            {
                ItemCode = item.ItemCode,
                ItemName = item.ItemName,
                InventoryUOM = item.InventoryUOM,
                ItemsGroupCode = item.ItemsGroupCode,
                U_TypeGroup = item.U_TypeGroup  // Default qiymat qo'shish
            };
        }
    }
}
