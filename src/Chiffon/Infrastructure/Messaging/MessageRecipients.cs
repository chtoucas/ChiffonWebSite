namespace Chiffon.Infrastructure.Messaging
{
    using System;

    [Flags]
    public enum MessageRecipients
    {
        None = 0,
        Member = 1 << 0,
        Admin = 1 << 1,

        Default = Member | Admin,
    }
}
