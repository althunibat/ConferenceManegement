using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mongo.CSharp.Repository;

namespace ConferenceManagementSystem.Conference.Model.IRepository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> FindOrders(Guid conferenceId);
    }
}