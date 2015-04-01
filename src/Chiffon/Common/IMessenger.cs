namespace Chiffon.Common
{
    public interface IMessenger
    {
        void Publish(NewMemberMessage message);

        void Publish(NewContactMessage message);
    }
}
