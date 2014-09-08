namespace Chiffon.Infrastructure.Messaging
{
    public interface IMessenger
    {
        void Publish(NewMemberMessage message);
        void Publish(NewContactMessage message);
    }
}
