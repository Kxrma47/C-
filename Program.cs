using NewVariant.Interfaces;

namespace Max
{
    class Program
    {
        static void Main()
        {
            var db = new N.DataBase();

            // Create tables
            db.CreateTable<Person>();
            db.CreateTable<Car>();

            // Insert data
            db.InsertInto<Person>(() => new Person { Id = 1, Name = "Alice", Age = 30 });
            db.InsertInto<Person>(() => new Person { Id = 2, Name = "Bob", Age = 40 });
            db.InsertInto<Car>(() => new Car { Id = 1, Make = "Honda", Model = "Civic", Year = 2021 });
            db.InsertInto<Car>(() => new Car { Id = 2, Make = "Toyota", Model = "Corolla", Year = 2022 });
            // Serialize to file
            db.Serialize<Person>("people.json");
            db.Serialize<Car>("cars.json");

            // Deserialize from file
            db.Deserialize<Person>("people.json");
            db.Deserialize<Car>("cars.json");

            // Print data
            Console.WriteLine("People:");
            foreach (var person in db.GetTable<Person>())
            {
                Console.WriteLine($"{person.Id} {person.Name} {person.Age}");
            }

            Console.WriteLine("Cars:");
            foreach (var car in db.GetTable<Car>())
            {
                Console.WriteLine($"{car.Id} {car.Make} {car.Model} {car.Year}");
            }
        }
    }

    class Person : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Car : IEntity
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}