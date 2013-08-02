namespace Chiffon.Persistence.InMemory
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;

    public class MemberRepositoryImpl : IMemberRepository
    {
        static IEnumerable<Member> Members_
        {
            get
            {
                yield return new Member {
                    MemberId = new MemberId(1),
                    DisplayName = "Chicamancha",
                    PatternDirectory = "chicamancha",
                    UrlKey = "chicamancha",
                };

                yield return new Member {
                    MemberId = new MemberId(2),
                    DisplayName = "Viviane Devaux",
                    PatternDirectory = "viviane-devaux",
                    UrlKey = "viviane-devaux",
                };

                yield return new Member {
                    MemberId = new MemberId(3),
                    DisplayName = "Christine Légeret",
                    PatternDirectory = "christine-legeret",
                    UrlKey = "christine-legeret",
                };

                yield return new Member {
                    MemberId = new MemberId(4),
                    DisplayName = "Laure Roussel",
                    PatternDirectory = "laure-roussel",
                    UrlKey = "laure-roussel",
                };
            }
        }

        #region IMemberRepository

        public IEnumerable<Member> GetAll()
        {
            return Members_;
        }

        public Member GetMember(MemberId memberId)
        {
            return (from _ in Members_ where _.MemberId == memberId select _).Single();
        }

        #endregion
    }
}
