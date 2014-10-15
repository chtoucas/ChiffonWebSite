﻿namespace Chiffon.Entities
{
    using System;
    using Narvalo;
    using Narvalo.Fx;

    [Serializable]
    public struct PatternId : IEquatable<PatternId>
    {
        readonly DesignerKey _designerKey;
        readonly string _reference;

        public PatternId(DesignerKey designerKey, string reference)
        {
            _designerKey = designerKey;
            _reference = reference;
        }

        public DesignerKey DesignerKey { get { return _designerKey; } }
        public string Reference { get { return _reference; } }

        #region IEquatable<PatternId>

        public bool Equals(PatternId other)
        {
            return _designerKey == other._designerKey && _reference == other._reference;
        }

        #endregion

        public static bool operator ==(PatternId left, PatternId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PatternId left, PatternId right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PatternId)) {
                return false;
            }

            return Equals((PatternId)obj);
        }

        public override int GetHashCode()
        {
            return _designerKey.GetHashCode() ^ _reference.GetHashCode();
        }
    }
}
