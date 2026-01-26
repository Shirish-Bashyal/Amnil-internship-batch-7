namespace ZooManagementSystem.cs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Zoo Animals:- \n");

            Animal snake = new Snake(101, 3);
            Animal turtle = new Turtle(102, 90);
            Animal zebra = new Zebra(103, 10);
            Animal lion = new Lion(1, 10);

            var zooAnimals = new List<Animal>() { snake, turtle, zebra, lion };
            foreach (Animal animal in zooAnimals)
            {
                Console.WriteLine($"Animal Type: {animal.GetType().Name}");
                Console.WriteLine($"Animal ID: {animal.Id},  Age: {animal.Age}");
                Console.Write("Feeding Schedule: ");
                Console.WriteLine(animal.GetFeedingSchedule());
                Console.WriteLine($"Feeding cost: {animal.CalculateFoodCost()}\n");
            }
        }
    }
}
