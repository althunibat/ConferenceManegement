using System;
using ConferenceManagementSystem.Common.Messages;

namespace ConferenceManagementSystem.Conference.Contracts.Events {
    public interface ISeatUpdated : IEvent {
        string Name { get; }
        string Description { get; }
        decimal Price { get; }
        int Quantity { get; }
    }
}