namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using Chiffon.BuildingBlocks.Membership;

    public class DefaultMemberSiteMap : DefaultSiteMap, ISiteMap
    {
        readonly MemberId _memberId;

        internal DefaultMemberSiteMap(MemberId memberId, Uri baseUrl)
            : base(baseUrl)
        {

            //// FIXME: better not to throw an exception and simply handle the case via the factory???
            //if (memberId == MemberId.Anonymous) {
            //    throw new ArgumentException("memberId");
            //}

            _memberId = memberId;
        }
    }

}
