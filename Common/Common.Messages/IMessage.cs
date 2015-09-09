using System;
using MassTransit;

namespace ConferenceManagementSystem.Common.Messages
{
    public interface IMessage:CorrelatedBy<Guid>
    {
        Guid Id { get; }
        int Version { get; }
        DateTimeOffset TimeStamp { get; }
    }
}
