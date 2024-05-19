using System;

namespace API.Domain.ValueObjects
{
    public class FullName
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string MiddleName { get; }

        public FullName(string firstName, string lastName, string middleName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(middleName))
            {
                throw new ArgumentException("FirstName and LastName cannot be null or empty");
            }

            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        // Переопределите методы Equals и GetHashCode
        public override bool Equals(object obj)
        {
            if (obj is FullName other)
            {
                return FirstName == other.FirstName && LastName == other.LastName && MiddleName == other.MiddleName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, MiddleName);
        }
    }

}
