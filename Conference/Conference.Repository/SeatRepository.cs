using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManagementSystem.Conference.Model;
using ConferenceManagementSystem.Conference.Model.IRepository;
using Mongo.CSharp.Repository;
using MongoDB.Driver;

namespace ConferenceManagementSystem.Conference.Repository {
    public class SeatRepository : MongoRepository<SeatType>, ISeatRepository {

        public async Task AddConferenceSeat(Guid conferenceId, SeatType seat) {
            seat.ConferenceId = conferenceId;
            await Create(seat);
        }

        public async Task AddConferenceSeat(SeatType seat)
        {
            await Collection.InsertOneAsync(seat);
        }

        public async Task<IEnumerable<SeatType>> FindSeatTypes(Guid conferenceId) {
            var cursor = Collection.Find(s => s.ConferenceId == conferenceId);
            return (await cursor.ToListAsync());
        }

        public async Task<SeatType> FindSeatType(Guid seatTypeId) {
            var cursor = Collection.Find(s => s.Id == seatTypeId);
            return (await cursor.FirstOrDefaultAsync());
        }

        public async Task<bool> CheckExistenceById(Guid seatTypeId)
        {
            var cursor = Collection.Find(c => c.Id == seatTypeId);
            return (await cursor.CountAsync()) > 0;
        }

        public async Task UpdateSeat(SeatType seat)
        {
            var filter = Builders<SeatType>.Filter.Eq(c => c.Id, seat.Id);
            await Collection.ReplaceOneAsync(filter, seat);
        }

        public async Task DeleteSeat(Guid seatTypeId)
        {
            var filter = Builders<SeatType>.Filter.Eq(c => c.Id, seatTypeId);
            await Collection.DeleteOneAsync(filter);
        }
    }
}