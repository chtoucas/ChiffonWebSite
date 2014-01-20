namespace Chiffon.Handlers
{
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo.Fx;

    public class PatternImageQuery
    {
        public Maybe<DesignerKey> DesignerKey { get; set; }
        public Maybe<string> Reference { get; set; }
        public Maybe<PatternSize> Size { get; set; }
        public Maybe<string> Variant { get; set; }

        public bool IsIncomplete
        {
            get
            {
                return DesignerKey.IsNone || Reference.IsNone || Size.IsNone || Variant.IsNone;
            }
        }
    }
}