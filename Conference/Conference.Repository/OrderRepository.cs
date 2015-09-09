using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManagementSystem.Conference.Model;
using ConferenceManagementSystem.Conference.Model.IRepository;
using Mongo.CSharp.Repository;
using MongoDB.Driver;

namespace ConferenceManagementSystem.Conference.Repository
{
    public class OrderRepository:MongoRepository<Order>,IOrderRepository
    {
        public async Task<IEnumerable<Order>> FindOrders(Guid conferenceId)
        {
            var cursor = Collection.Find(s => s.ConferenceId == conferenceId);
            return (await cursor.ToListAsync());

        }
    }
}