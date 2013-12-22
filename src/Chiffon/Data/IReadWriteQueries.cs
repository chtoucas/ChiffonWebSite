namespace Chiffon.Data
{
    using Chiffon.Entities;

    public interface IReadWriteQueries
    {
        Member NewMember(NewMemberModel model);
    }
}
