using System;

namespace UnitTests.Entity
{
    public class StudentRoster
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public int Limit { get; set; }
    }
}