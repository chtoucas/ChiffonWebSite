namespace Chiffon.Services
{
    using System;
    using Chiffon.Domain;

    public class MemberCreatedEventArgs : EventArgs
    {
        readonly Member _member;

        public MemberCreatedEventArgs(Member member)
        {
            _member = member;
        }

        public Member Member
        {
            get { return _member; }
        }
    }

}
