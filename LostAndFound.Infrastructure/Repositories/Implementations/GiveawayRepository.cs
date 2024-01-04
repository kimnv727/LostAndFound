using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Giveaway;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class GiveawayRepository : GenericRepository<Giveaway>, IGiveawayRepository
    {
        public GiveawayRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<Giveaway> FindGiveawayByIdAsync(int id)
        {
            return await _context.Giveaways
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Giveaway> FindGiveawayIncludeParticipantsAsync(int id)
        {
            return await _context.Giveaways
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Giveaway>> QueryGiveawayAsync(GiveawayQuery query, bool trackChanges = false)
        {
            IQueryable<Giveaway> giveaways = _context.Giveaways
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .Where(g => g.GiveawayStatus == GiveawayStatus.ONGOING).AsSplitQuery();

            if (!trackChanges)
            {
                giveaways = giveaways.AsNoTracking();
            }

            /*if (query.ItemCategoryGroupId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Category.CategoryGroupId == query.ItemCategoryGroupId);
            }
            
            if (query.ItemCategoryId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.CategoryId == query.ItemCategoryId);
            }*/

            if (query.ItemCategoryGroupId != null)
            {
                giveaways = giveaways.Where(g => query.ItemCategoryGroupId.Contains(g.Item.Category.CategoryGroupId));
            }

            if (query.ItemCategoryId != null)
            {
                giveaways = giveaways.Where(g => query.ItemCategoryId.Contains(g.Item.CategoryId));
            }

            if (query.StartAt != null)
            {
                giveaways = giveaways.Where(g => g.StartAt >= query.StartAt);
            }
            
            if (query.EndAt != null)
            {
                giveaways = giveaways.Where(g => g.EndAt <= query.EndAt);
            }

            if (query.CampusId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Location.CampusId == query.CampusId);
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                giveaways = giveaways.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(giveaways.ToList());
        }
        
        public async Task<IEnumerable<Giveaway>> QueryGiveawayWithStatusAsync(GiveawayQueryWithStatus query, bool trackChanges = false)
        {
            IQueryable<Giveaway> giveaways = _context.Giveaways
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .AsSplitQuery();

            if (!trackChanges)
            {
                giveaways = giveaways.AsNoTracking();
            }

            /*if (query.ItemCategoryGroupId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Category.CategoryGroupId == query.ItemCategoryGroupId);
            }
            
            if (query.ItemCategoryId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.CategoryId == query.ItemCategoryId);
            }*/

            if (query.ItemCategoryGroupId != null)
            {
                giveaways = giveaways.Where(g => query.ItemCategoryGroupId.Contains(g.Item.Category.CategoryGroupId));
            }

            if (query.ItemCategoryId != null)
            {
                giveaways = giveaways.Where(g => query.ItemCategoryId.Contains(g.Item.CategoryId));
            }

            if (query.StartAt != null)
            {
                giveaways = giveaways.Where(g => g.StartAt >= query.StartAt);
            }
            
            if (query.EndAt != null)
            {
                giveaways = giveaways.Where(g => g.EndAt <= query.EndAt);
            }

            if (Enum.IsDefined(query.GiveawayStatus))
            {
                if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.NOT_STARTED)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.NOT_STARTED);
                }
                else if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.ONGOING)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.ONGOING);
                }
                else if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.CLOSED)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.CLOSED);
                }
                else if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.REWARD_DISTRIBUTION_IN_PROGRESS)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.REWARD_DISTRIBUTION_IN_PROGRESS);
                }
                else if (query.GiveawayStatus == GiveawayQueryWithStatus.GiveawayStatusQuery.DISABLED)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.DISABLED);
                }
            }

            if (query.CampusId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Location.CampusId == query.CampusId);
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                giveaways = giveaways.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(giveaways.ToList());
        }

        public async Task<IEnumerable<Giveaway>> QueryGiveawayExcludeNotstartedAsync(GiveawayQueryExcludeNotStarted query, bool trackChanges = false)
        {
            IQueryable<Giveaway> giveaways = _context.Giveaways
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .Where(g => g.GiveawayStatus != GiveawayStatus.NOT_STARTED && g.GiveawayStatus != GiveawayStatus.DISABLED).AsSplitQuery();

            if (!trackChanges)
            {
                giveaways = giveaways.AsNoTracking();
            }

            /*if (query.ItemCategoryGroupId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Category.CategoryGroupId == query.ItemCategoryGroupId);
            }
            
            if (query.ItemCategoryId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.CategoryId == query.ItemCategoryId);
            }*/

            if (query.ItemCategoryGroupId != null)
            {
                giveaways = giveaways.Where(g => query.ItemCategoryGroupId.Contains(g.Item.Category.CategoryGroupId));
            }

            if (query.ItemCategoryId != null)
            {
                giveaways = giveaways.Where(g => query.ItemCategoryId.Contains(g.Item.CategoryId));
            }

            if (Enum.IsDefined(query.GiveawayStatus))
            {
                if (query.GiveawayStatus == GiveawayQueryExcludeNotStarted.GiveawayStatusQuery.ONGOING)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.ONGOING);
                }
                else if (query.GiveawayStatus == GiveawayQueryExcludeNotStarted.GiveawayStatusQuery.CLOSED)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.CLOSED);
                }
                else if (query.GiveawayStatus == GiveawayQueryExcludeNotStarted.GiveawayStatusQuery.REWARD_DISTRIBUTION_IN_PROGRESS)
                {
                    giveaways = giveaways.Where(g => g.GiveawayStatus == GiveawayStatus.REWARD_DISTRIBUTION_IN_PROGRESS);
                }
            }

            if (query.StartAt != null)
            {
                giveaways = giveaways.Where(g => g.StartAt >= query.StartAt);
            }

            if (query.EndAt != null)
            {
                giveaways = giveaways.Where(g => g.EndAt <= query.EndAt);
            }

            if (query.CampusId > 0)
            {
                giveaways = giveaways.Where(g => g.Item.Location.CampusId == query.CampusId);
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                giveaways = giveaways.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(giveaways.ToList());
        }

        public async Task UpdateGiveawayRange(Giveaway[] giveaway)
        {
            _context.Giveaways.UpdateRange(giveaway);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Giveaway>> GetAllOngoingGiveaways()
        {
            var giveaways = _context.Giveaways
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .Where(g => g.GiveawayStatus == GiveawayStatus.ONGOING);

            return await Task.FromResult(giveaways.ToList());
        }

        public async Task<IEnumerable<Giveaway>> GetAllNotStartedGiveaways()
        {
            var giveaways = _context.Giveaways
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .Where(g => g.GiveawayStatus == GiveawayStatus.NOT_STARTED);

            return await Task.FromResult(giveaways.ToList());
        }

        public async Task<IEnumerable<Giveaway>> GetAllWaitingGiveaways()
        {
            var giveaways = _context.Giveaways
                .Include(g => g.GiveawayParticipants.Where(gp => gp.IsActive == true))
                .ThenInclude(gp => gp.User)
                .Include(g => g.Item)
                .ThenInclude(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Include(g => g.Item)
                .ThenInclude(i => i.Cabinet)
                .Include(g => g.Item)
                .ThenInclude(i => i.Category)
                .Where(g => g.GiveawayStatus == GiveawayStatus.REWARD_DISTRIBUTION_IN_PROGRESS);

            return await Task.FromResult(giveaways.ToList());
        }

        public async Task PushNotificationForGiveawayResult(PushNotification notification)
        {
            string baseUrl = "https://lostandfound.io.vn/api/notifications/push";
            using (var httpClient = new HttpClient())
            {
                /*httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));*/

                var request = new HttpRequestMessage
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri(baseUrl),
                };

                request.Content = JsonContent.Create(new { userId = notification.UserId, title = notification.Title,
                content = notification.Content, notificationType = notification.NotificationType});

                HttpResponseMessage response = await httpClient.SendAsync(request);

                /*if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject jObj = JObject.Parse(result);

                    return jObj["id_token"].ToString();
                }

                return null;*/
            }
        }

        public async Task<IEnumerable<Item>> GetAllItemsSuitableForGiveaway()
        {
            var itemsAlreadyInGiveawayId = _context.Giveaways.Where(g => g.GiveawayStatus != GiveawayStatus.DISABLED).Select(g => g.ItemId).ToList();

            var items = _context.Items
                .Include(i => i.User)
                .ThenInclude(u => u.Campus)
                .Include(i => i.Category)
                .Include(i => i.Location)
                .ThenInclude(l => l.Campus)
                .Include(i => i.Cabinet)
                .ThenInclude(c => c.Storage)
                .Include(i => i.ItemMedias.Where(im => im.Media.IsActive == true && im.Media.DeletedDate == null))
                .ThenInclude(im => im.Media)
                .Where(i => i.ItemStatus == ItemStatus.EXPIRED && i.IsInStorage == true && i.Category.Value == ItemValue.Low
                && i.Category.IsSensitive == false && !itemsAlreadyInGiveawayId.Contains(i.Id))
                .AsSplitQuery();

            items = items
                .OrderBy(i => i.CreatedDate);

            items = items.AsNoTracking();

            return await Task.FromResult(items.ToList());
        }

        public async Task<IEnumerable<Giveaway>> CountFinishedGiveaways(int month, int year)
        {
            var result = _context.Giveaways.Where(g => ((DateTime)g.EndAt).Month == month && ((DateTime)g.EndAt).Year == year
            && g.GiveawayStatus == GiveawayStatus.CLOSED);
            return await Task.FromResult(result.ToList());
        }
    }
}