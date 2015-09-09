using System;
using ConferenceManagementSystem.Common.Messages;

namespace ConferenceManagementSystem.Conference.Contracts.Events
{
    public interface IConferenceUpdated:IEvent
    {
        string Name { get; }
        string Description { get; }
        string Location { get; }
        string Slug { get; }
        string Tagline { get; }
        string TwitterSearch { get; }

        DateTimeOffset StartDate { get; }
        DateTimeOffset EndDate { get; }

        Owner Owner { get; }
    }
}