namespace Chiffon.Infrastructure.Addressing
{
    using Chiffon.BuildingBlocks.Membership;

    public interface ISiteMapFactory
    {
        ISiteMap CreateMap(MemberId memberId);
    }
}
