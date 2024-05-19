using System;
using System.Linq;

namespace API.Domain.ValueObjects
{
    public class INN
    {
        public string Value { get; }

        public INN(string value)
        {
            if (!value.All(char.IsDigit))
            {
                throw new ArgumentException("INN must be a number");
            }

            if (value.Length != 12)
            {
                throw new ArgumentException("INN must be a 12 digit number");
            }
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is INN other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(INN inn) => inn.Value;
        public static explicit operator INN(string value) => new INN(value);
    }

}
