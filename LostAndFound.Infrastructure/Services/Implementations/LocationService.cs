using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.Repositories.Implementations;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocationRepository _locationRepository;

        public LocationService(IMapper mapper, IUnitOfWork unitOfWork, ILocationRepository locationRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _locationRepository = locationRepository;
        }

        public async Task<PaginatedResponse<LocationReadDTO>> QueryLocationAsync(LocationQuery query)
        {
            var locations = await _locationRepository.QueryLocationsAsync(query);
            return PaginatedResponse<LocationReadDTO>.FromEnumerableWithMapping(locations, query, _mapper);
        }
        
        public async Task<LocationReadDTO> FindLocationByIdAsync(int LocationId)
        {
            var Location = await _locationRepository.FindLocationByIdAsync(LocationId);

            if (Location == null)
            {
                throw new EntityWithIDNotFoundException<Location>(LocationId);
            }
            
            return _mapper.Map<LocationReadDTO>(Location);
        }
        
        public async Task<LocationReadDTO> CreateItemAsync(LocationWriteDTO locationWriteDTO)
        {
            var location = _mapper.Map<Location>(locationWriteDTO);
            await _locationRepository.AddAsync(location);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LocationReadDTO>(location);

        }
        
        public async Task DeleteLocationAsync(int LocationId)
        {
            var Location = await _locationRepository.FindLocationByIdAsync(LocationId);

            if (Location == null)
            {
                throw new EntityWithIDNotFoundException<Location>(LocationId);
            }
            
            _locationRepository.Delete(Location);
            await _unitOfWork.CommitAsync();
        }

        public async Task<LocationReadDTO> UpdateLocationDetailsAsync(int LocationId, LocationWriteDTO LocationWriteDTO)
        {
            var Location = await _locationRepository.FindLocationByIdAsync(LocationId);

            if (Location == null)
            {
                throw new EntityWithIDNotFoundException<Location>(LocationId);
            }

            _mapper.Map(LocationWriteDTO, Location);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LocationReadDTO>(Location);
        }
    }
}