using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Shared.Execution.Metadata
{
    public class MetaAttribute : IEquatable<MetaAttribute>
    {
        public MetaAttribute(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Attribute value cannot be null od empty.", nameof(value));
            }

            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public static MetaAttribute Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Cannot parse null or empty value.");
            }

            string metaKey = null;
            string metaValue;

            var parts = value.Split(':');

            if (parts.Length == 1 || string.IsNullOrEmpty(parts[1]))
            {
                metaValue = value;
            }
            else
            {
                if (parts[0] != string.Empty)
                {
                    metaKey = parts[0];
                }

                metaValue = value.Substring(parts[0].Length + 1);
            }

            return new MetaAttribute(metaKey, metaValue);
        }

        public bool Equals(MetaAttribute other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is null || GetType() != other.GetType())
            {
                return false;
            }

            return string.Equals(Key, other.Key) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj) => Equals(obj as MetaAttribute);

        public static implicit operator ItemAttribute(MetaAttribute a) => new ItemAttribute { Key = a.Key, Value = a.Value };
    }
}
