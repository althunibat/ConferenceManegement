using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManagementSystem.Conference.Model;

namespace ConferenceManagementSystem.Conference.ApplicationServices
{
    public interface IConferenceService
    {
        Task CreateConference(ConferenceInfo conference);
        Task CreateSeat(SeatType seat);
        Task<ConferenceInfo> FindConference(string slug);
        Task<ConferenceInfo> FindConference(string email, string accessCode);
        Task<IEnumerable<SeatType>> FindSeatTypes(Guid conferenceId);
        Task<SeatType> FindSeatType(Guid seatTypeId);
        Task<IEnumerable<Order>> FindOrders(Guid conferenceId);
        Task UpdateConference(ConferenceInfo conference);
        Task UpdateSeat(SeatType seat);
        Task Publish(Guid conferenceId);
        Task Unpublish(Guid conferenceId);
        Task DeleteSeat(Guid id);
    }
}