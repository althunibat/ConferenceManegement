namespace ConferenceManagementSystem.Common.Messages
{
    public interface IFailureEvent:IEvent
    {
        string ErrorMessage { get; }
        string ErrorDetails { get; }

    }
}