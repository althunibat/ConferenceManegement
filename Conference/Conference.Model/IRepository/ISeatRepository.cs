using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConferenceManagementSystem.Conference.Model.IRepository
{
    public interface ISeatRepository
    {
        Task AddConferenceSeat(SeatType seat);
        Task<IEnumerable<SeatType>> FindSeatTypes(Guid conferenceId);
        Task<SeatType> FindSeatType(Guid seatTypeId);
        Task<bool> CheckExistenceById(Guid seatTypeId);
        Task UpdateSeat(SeatType seat);
        Task DeleteSeat(Guid seatTypeId);
    }
}