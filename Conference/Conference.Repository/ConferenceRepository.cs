using System;
using System.Linq;
using System.Threading.Tasks;
using ConferenceManagementSystem.Conference.Model;
using ConferenceManagementSystem.Conference.Model.IRepository;
using Mongo.CSharp.Repository;
using MongoDB.Driver;

namespace ConferenceManagementSystem.Conference.Repository {
    public class ConferenceRepository : MongoRepository<ConferenceInfo>, IConferenceRepository {
        public async Task<bool> CheckExistenceBySlug(string slug) {
            var cursor = Collection.Find(c => c.Slug == slug);
            return (await cursor.CountAsync()) > 0;
        }

        public async Task<ConferenceInfo> Get(Guid conferenceId) {
            var cursor = Collection.Find(c => c.Id == conferenceId);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<ConferenceInfo> Get(string slug) {
            var cursor = Collection.Find(c => c.Slug == slug);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<ConferenceInfo> Get(string email, string accessCode) {
            var cursor = Collection.Find(c => c.OwnerEmail == email && c.AccessCode == accessCode);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<bool> CheckExistenceById(Guid conferenceId) {
            var cursor = Collection.Find(c => c.Id == conferenceId);
            return (await cursor.CountAsync()) > 0;
        }

        public async Task UpdateConference(ConferenceInfo conference) {
            var filter = Builders<ConferenceInfo>.Filter.Eq(c => c.Id, conference.Id);
            await Collection.ReplaceOneAsync(filter, conference);
        }
    }
}