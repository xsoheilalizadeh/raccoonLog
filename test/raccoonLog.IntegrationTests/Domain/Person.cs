using System;

namespace raccoonLog.IntegrationTests.Domain
{
    public class Person
    {
        public static Person Default =>
            new Person("Soheil", 20, DateTime.Now, Gender.Male, true, new[] {"Learning"},
                new Address("WL", 33, "DOO", 22));

        public Person(string name, int age, DateTime birthDate, Gender gender, bool isActive, string[] hobbies,
            Address address)
        {
            Name = name;
            Age = age;
            BirthDate = birthDate;
            Gender = gender;
            IsActive = isActive;
            Hobbies = hobbies;
            Address = address;
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public bool IsActive { get; set; }

        public string[] Hobbies { get; set; }

        public Address Address { get; set; }
    }
}