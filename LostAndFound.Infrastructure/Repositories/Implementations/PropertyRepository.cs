using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Property>> QueryPropertyAsync(PropertyQuery query, bool trackChanges = false)
        {
            IQueryable<Property> properties = _context.Properties
                .Include(p => p.Locations)
                .AsSplitQuery();

            if (!trackChanges)
            {
                properties = properties.AsNoTracking();
            }
            
            if (!string.IsNullOrWhiteSpace(query.PropertyName))
            {
                properties = properties.Where(p => p.PropertyName.ToLower().Contains(query.PropertyName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                properties = properties.Where(p => p.Address.ToLower().Contains(query.Address.ToLower()));
            }
            switch (query.IsActive)
            {
                case true:
                    properties = properties.Where(p => p.IsActive == true);
                    break;
                case false:
                    properties = properties.Where(p => p.IsActive == false);
                    break;
            }

            return await Task.FromResult(properties.ToList());
        }

        public async Task<IEnumerable<Property>> QueryPropertyIgnoreStatusAsync(PropertyQuery query, bool trackChanges = false)
        {
            IQueryable<Property> properties = _context.Properties
                .Include(p => p.Locations)
                .Where(p => p.IsActive == true)
                .AsSplitQuery();

            if (!trackChanges)
            {
                properties = properties.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.PropertyName))
            {
                properties = properties.Where(p => p.PropertyName.ToLower().Contains(query.PropertyName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                properties = properties.Where(p => p.Address.ToLower().Contains(query.Address.ToLower()));
            }

            return await Task.FromResult(properties.ToList());
        }

        public async Task<Property> FindPropertyByIdAsync(int propertyId)
        {
            return await _context.Properties
                .Include(p => p.Locations)
                .FirstOrDefaultAsync(p => p.Id == propertyId);
        }

        public async Task<Property> FindPropertyByNameAsync(string propertyName)
        {
            return await _context.Properties
                .Include(p => p.Locations)
                .FirstOrDefaultAsync(p => p.PropertyName.ToLower().Contains(propertyName.ToLower()));
        }
    }
}