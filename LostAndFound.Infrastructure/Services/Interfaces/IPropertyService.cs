using System.Threading.Tasks;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Property;

namespace LostAndFound.Infrastructure.Services.Interfaces
{
    public interface IPropertyService
    {
        public Task<PaginatedResponse<PropertyReadDTO>> QueryPropertyAsync(PropertyQuery query);
        public Task<PropertyReadDTO> GetPropertyByIdAsync(int propertyId);
        public Task<PropertyReadDTO> CreatePropertyAsync(string userId, PropertyWriteDTO propertyWriteDTO);
        public Task<PropertyReadDTO> UpdatePropertyDetailsAsync(int propertyId, PropertyWriteDTO propertyWriteDTO);
        public Task ChangePropertyStatusAsync(int propertyId);
        public Task DeletePropertyAsync(int propertyId);
        
    }
}