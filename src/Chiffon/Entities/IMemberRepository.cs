namespace Chiffon.Entities
{
    using System.Collections.Generic;

    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member GetMember(MemberId memberId);
    }
}
