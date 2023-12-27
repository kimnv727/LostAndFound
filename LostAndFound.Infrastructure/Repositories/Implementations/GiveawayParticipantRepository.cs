using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class GiveawayParticipantRepository : GenericRepository<GiveawayParticipant>, IGiveawayParticipantRepository
    {
        public GiveawayParticipantRepository(LostAndFoundDbContext context) : base(context)
        {
        }
        
        public async Task<int> CountGiveawayParticipantsAsync(int giveawayId)
        {
            var result = _context.GiveawayParticipants.Where(gp => gp.GiveawayId == giveawayId && gp.IsActive == true);
            return await Task.FromResult(result.Count());
        }

        public async Task<IEnumerable<User>> FindUsersParticipateByGiveawayIdAsync(int giveawayId)
        {
            IQueryable<GiveawayParticipant> giveawayParticipants = _context.GiveawayParticipants.Where(gp => gp.GiveawayId == giveawayId);
            IQueryable<User> users = giveawayParticipants.Select(gp => gp.User);

            return await Task.FromResult(users.ToList());
        }
        
        public async Task<GiveawayParticipant> FindGiveawayParticipantAsync(int giveawayId, string userId)
        {
            return await _context.GiveawayParticipants.Include(gp => gp.User)
                .FirstOrDefaultAsync(gp => gp.GiveawayId == giveawayId && gp.UserId == userId);
        }

        public async Task<GiveawayParticipant> RandomizeGiveawayWinnerAsync(int giveawayId)
        {
            IEnumerable<GiveawayParticipant> giveawayParticipants = _context.GiveawayParticipants.Where(gp => gp.GiveawayId == giveawayId && gp.IsChosenAsWinner == false && gp.IsWinner == false);

            var myList = giveawayParticipants.ToList();
            var rng = new Random();
            var randomIndex = rng.Next(0, myList.Count);
            var randomParticipant = myList[randomIndex];

            return randomParticipant;
        }

        public async Task UpdateGiveawayParticipantRange(GiveawayParticipant[] giveawayParticipant)
        {
            _context.GiveawayParticipants.UpdateRange(giveawayParticipant);
            await _context.SaveChangesAsync();
        }
    }
}