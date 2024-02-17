using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Shared.Execution.Metadata
{
    /// <summary>
    /// Represents a metadata attribute.
    /// </summary>
    public class MetaAttribute : IEquatable<MetaAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetaAttribute"/> class.
        /// </summary>
        /// <param name="key">The attribute key.</param>
        /// <param name="value">The attribute value.</param>
        /// <exception cref="ArgumentException">Thrown when the attribute value is null or empty.</exception>
        public MetaAttribute(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Attribute value cannot be null or empty.", nameof(value));
            }

            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the attribute key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Parses a string value into a <see cref="MetaAttribute"/> object.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>A new instance of the <see cref="MetaAttribute"/> class.</returns>
        /// <exception cref="ArgumentException">Thrown when the value is null or empty.</exception>
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

        /// <summary>
        /// Determines whether the current <see cref="MetaAttribute"/> object is equal to another <see cref="MetaAttribute"/> object.
        /// </summary>
        /// <param name="other">The <see cref="MetaAttribute"/> object to compare with the current object.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
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

        /// <summary>
        /// Determines whether the current <see cref="MetaAttribute"/> object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as MetaAttribute);

        /// <summary>
        /// Implicitly converts a <see cref="MetaAttribute"/> object to an <see cref="ItemAttribute"/> object.
        /// </summary>
        /// <param name="a">The <see cref="MetaAttribute"/> object to convert.</param>
        /// <returns>An <see cref="ItemAttribute"/> object.</returns>
        public static implicit operator ItemAttribute(MetaAttribute a) => new ItemAttribute { Key = a.Key, Value = a.Value };
    }
}
