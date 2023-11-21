using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Exceptions.Common;
using LostAndFound.Core.Exceptions.Location;
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
        private readonly IStorageRepository _storageRepository;

        public LocationService(IMapper mapper, IUnitOfWork unitOfWork, ILocationRepository locationRepository, IStorageRepository storageRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _locationRepository = locationRepository;
            _storageRepository = storageRepository;
        }

        public async Task<PaginatedResponse<LocationReadDTO>> QueryLocationWithPaginationAsync(LocationQuery query)
        {
            var locations = await _locationRepository.QueryLocationsAsync(query);
            return PaginatedResponse<LocationReadDTO>.FromEnumerableWithMapping(locations, query, _mapper);
        }

        public async Task<IEnumerable<LocationReadDTO>> ListAllWithCampusAsync()
        {
            var locations = await _locationRepository.GetAllWithCampusAsync();
            return _mapper.Map <List<LocationReadDTO>>(locations);
        }

        public async Task<IEnumerable<LocationReadDTO>> ListAllWithCampusSortedByFloorAsync()
        {
            var locations = await _locationRepository.GetAllWithCampusSortedByFloorAsync();
            return _mapper.Map<List<LocationReadDTO>>(locations);
        }

        public async Task<IEnumerable<LocationReadDTO>> QueryLocationAsync(LocationQuery query)
        {
            var locations = await _locationRepository.QueryLocationsAsync(query);
            return _mapper.Map<List<LocationReadDTO>>(locations.ToList());
        }

        public async Task<IEnumerable<LocationReadDTO>> ListAllAsync()
        {
            var locations = await _locationRepository.GetAllAsync();
            return _mapper.Map<List<LocationReadDTO>>(locations);
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
        
        public async Task<LocationReadDTO> FindLocationByNameAsync(string locationName)
        {
            var Location = await _locationRepository.FindLocationByNameAsync(locationName);

            if (Location == null)
            {
                throw new EntityWithAttributeNotFoundException<Location>("Location name ", locationName);
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
            var location = await _locationRepository.FindLocationByIdAsync(LocationId);

            if (location == null)
            {
                throw new EntityWithIDNotFoundException<Location>(LocationId);
            }

            if (location.IsActive == false)
            {
                throw new LocationAlreadyDisabledException();
            }

            if (location.IsActive == true)
            {
                foreach (var item in location.Items)
                {
                    if (item.ItemStatus == Core.Enums.ItemStatus.ACTIVE ||
                        item.ItemStatus == Core.Enums.ItemStatus.PENDING ||
                        item.ItemStatus == Core.Enums.ItemStatus.REJECTED)
                    {
                        throw new LocationStillHaveItemOrPostException();
                    }
                }

                foreach (var post in location.Posts)
                {
                    if (post.PostStatus == Core.Enums.PostStatus.ACTIVE ||
                        post.PostStatus == Core.Enums.PostStatus.PENDING ||
                        post.PostStatus == Core.Enums.PostStatus.REJECTED)
                    {
                        throw new LocationStillHaveItemOrPostException();
                    }
                }
            }

            _locationRepository.Delete(location);
            await _unitOfWork.CommitAsync();
        }

        public async Task<LocationReadDTO> ChangeLocationStatusAsync(int id)
        {
            var location = await _locationRepository.FindLocationByIdAsync(id);
            if (location == null)
            {
                throw new EntityWithIDNotFoundException<Category>(id);
            }
            if (location.IsActive == true)
            {
                foreach (var item in location.Items)
                {
                    if (item.ItemStatus == Core.Enums.ItemStatus.ACTIVE ||
                        item.ItemStatus == Core.Enums.ItemStatus.PENDING ||
                        item.ItemStatus == Core.Enums.ItemStatus.REJECTED)
                    {
                        throw new LocationStillHaveItemOrPostException();
                    }
                }

                foreach (var post in location.Posts)
                {
                    if (post.PostStatus == Core.Enums.PostStatus.ACTIVE ||
                        post.PostStatus == Core.Enums.PostStatus.PENDING ||
                        post.PostStatus == Core.Enums.PostStatus.REJECTED)
                    {
                        throw new LocationStillHaveItemOrPostException();
                    }
                }
            }

            location.IsActive = !location.IsActive;
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LocationReadDTO>(location);
        }

        public async Task<LocationReadDTO> UpdateLocationDetailsAsync(int LocationId, LocationWriteDTO LocationWriteDTO)
        {
            var Location = await _locationRepository.FindLocationByIdAsync(LocationId);

            if (Location == null)
            {
                throw new EntityWithIDNotFoundException<Location>(LocationId);
            }

            //Check Storage with this Location
            var storages = await _storageRepository.FindAllStoragesByCampusIdIgnoreStatusAsync(Location.PropertyId);
            if(storages.Count() > 0)
            {
                foreach(var s in storages)
                {
                    if(s.Location == Location.LocationName)
                    {
                        s.Location = LocationWriteDTO.LocationName;
                        await _unitOfWork.CommitAsync();
                    }
                }
            }

            _mapper.Map(LocationWriteDTO, Location);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LocationReadDTO>(Location);
        }
    }
}