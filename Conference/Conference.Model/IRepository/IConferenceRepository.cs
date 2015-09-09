using System;
using System.Threading.Tasks;
using Mongo.CSharp.Repository;

namespace ConferenceManagementSystem.Conference.Model.IRepository {
    public interface IConferenceRepository
    {
        Task<bool> CheckExistenceBySlug(string slug);
        Task<ConferenceInfo> Get(Guid conferenceId);
        Task<ConferenceInfo> Get(string slug);
        Task<ConferenceInfo> Get(string email, string accessCode);
        Task<bool> CheckExistenceById(Guid conferenceId);
        Task UpdateConference(ConferenceInfo conference);
        Task Create(ConferenceInfo conference);

    }
}