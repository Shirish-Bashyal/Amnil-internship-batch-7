using LINQAssingment.Model;

namespace LINQAssingment;

internal class Program
{
    static void Main(string[] args)
    {
        //a list of students
        List<Student> students = new List<Student>
        {
            new Student
            {
                Name = "Alice",
                Age = 20,
                Gender = "Female",
                Marks = 92,
                City = "New York"
            },
            new Student
            {
                Name = "Bob",
                Age = 22,
                Gender = "Male",
                Marks = 78,
                City = "Chicago"
            },
            new Student
            {
                Name = "Charlie",
                Age = 19,
                Gender = "Male",
                Marks = 92,
                City = "New York"
            },
            new Student
            {
                Name = "Diana",
                Age = 21,
                Gender = "Female",
                Marks = 88,
                City = "Los Angeles"
            },
            new Student
            {
                Name = "Ethan",
                Age = 20,
                Gender = "Male",
                Marks = 65,
                City = "Chicago"
            },
        };

        // 1.Select all student names from the list.

        var studentNames = students.Select(x => x.Name);

        var studentNamesByQuery = from s in students select s.Name;

        Console.WriteLine("All students names only :");
        Console.Write(string.Join(", ", studentNames));

        Console.WriteLine(" \n \n All students names only By query syntax :");
        Console.Write(string.Join(", ", studentNamesByQuery));

        //2.Find students who scored more than 80 marks.
        var studentScoredEighty = students.Where(x => x.Marks > 80);

        var studentScoredEightyByQuery = from s in students where s.Marks > 80 select s;

        Console.WriteLine(" \n \nStudent who scored above 80");
        foreach (var student in studentScoredEighty)
        {
            Console.WriteLine("Name: " + student.Name + "  Marks: " + student.Marks);
        }

        Console.WriteLine(" \n \nStudent who scored above 80 By Query");
        foreach (var student in studentScoredEightyByQuery)
        {
            Console.WriteLine("Name: " + student.Name + "  Marks: " + student.Marks);
        }

        //3.Retrieve the first student whose name starts with "C".

        var firstStdWithLetterC = students.FirstOrDefault(x => x.Name[0] == 'C');

        var firstStdWithLetterCByQuery = (
            from s in students
            where s.Name[0] == 'C'
            select s
        ).FirstOrDefault();

        if (firstStdWithLetterC != null)
            Console.WriteLine(" \n first student with letter C is: " + firstStdWithLetterC.Name);

        if (firstStdWithLetterCByQuery != null)
            Console.WriteLine(
                " \n first student with letter C is by query syntas: "
                    + firstStdWithLetterCByQuery.Name
            );

        //4.Sort students by marks in descending order.

        var studentsDescending = students.OrderByDescending(x => x.Marks);

        var studentsDescendingByQuery = from s in students orderby s.Marks descending select s;

        Console.WriteLine("\n students arraged by decending marks:");
        foreach (var student in studentsDescending)
        {
            Console.WriteLine("Name: " + student.Name + "  Marks: " + student.Marks);
        }

        Console.WriteLine("\n students arraged by decending marks by query syntax:");

        foreach (var student in studentsDescending)
        {
            Console.WriteLine("Name: " + student.Name + "  Marks: " + student.Marks);
        }

        //5.Count the number of male and female students.
        var maleStudent = students.Where(x => x.Gender == "Male").ToList().Count;

        var femaleStudent = (from s in students where s.Gender == "Male" select s).ToList().Count;

        Console.WriteLine($" \n  Male students number= {maleStudent}");
        Console.WriteLine($" Female students number= {femaleStudent}");

        //6.Group students by city and display the total number of students in each city.

        var stdNumberByCity = students
            .GroupBy(x => x.City)
            .Select(x => new { City = x.Key, TotalStudent = x.Count() });

        Console.WriteLine("\n students number per city :");
        foreach (var student in stdNumberByCity)
        {
            Console.WriteLine($"City: {student.City}, StudentsNumber: {student.TotalStudent}");
        }

        var stdNumberByCityByQuery =
            from s in students
            group s by s.City into g
            select new { City = g.Key, TotalStudent = g.Count() };

        Console.WriteLine("\n students number per city by Query :");
        foreach (var student in stdNumberByCityByQuery)
        {
            Console.WriteLine($"City: {student.City}, StudentsNumber: {student.TotalStudent}");
        }

        //7.Calculate the average marks of all students.

        var avgMarks = students.Average(x => x.Marks);

        var avgMarksByQuery = (from s in students select s.Marks).Average();

        Console.WriteLine($"\n Average marks is: {avgMarks}");

        Console.WriteLine($"\n Average marks by query syntax: {avgMarksByQuery}");

        //8.Find the student(s) with the highest marks.
        var highestMarks = students.Max(x => x.Marks);
        var highestMarksByQuery = (from s in students select s.Marks).Max();

        var studentWithHighestMarks = students.Where(x => x.Marks == highestMarks);

        var studentWithHighestMarksFromQuery = (
            from s in students
            where s.Marks == highestMarksByQuery
            select s
        );

        Console.WriteLine("\n Student with highest marks:");
        if (studentWithHighestMarks != null)
        {
            foreach (var student in studentWithHighestMarks)
            {
                Console.WriteLine($"{student.Name}");
            }
        }

        Console.WriteLine("\n Student with highest marks by query:");
        if (studentWithHighestMarksFromQuery != null)
        {
            foreach (var student in studentWithHighestMarksFromQuery)
            {
                Console.WriteLine($"{student.Name}");
            }
        }

        //9.Check if any student has marks less than 50.

        var doesStdExistWithMarksLessFiftey = students.Any(x => x.Marks < 50);

        var doesStdExistWithMarksLessFifteyFromQuery = (
            from s in students
            where s.Marks < 50
            select s
        ).Any();
        Console.WriteLine(
            $"\n Does Student with marks less than 50 exists?: {doesStdExistWithMarksLessFiftey}"
        );

        Console.WriteLine(
            $"\n Does Student with marks less than 50 exists? from query: {doesStdExistWithMarksLessFifteyFromQuery}"
        );

        //10.Create a list of students containing only `Name` and `City`.

        var stdWithNameCity = students.Select(x => new { Name = x.Name, City = x.City, });

        var stdWithNameCityFromQuery =
            from s in students
            select new { Name = s.Name, City = s.City, };

        Console.WriteLine(" \n Students with their City: ");
        foreach (var student in stdWithNameCity)
        {
            Console.WriteLine($" Name: {student.Name}, City: {student.City}");
        }

        Console.WriteLine(" \n Students with their City from query syntax: ");
        foreach (var student in stdWithNameCityFromQuery)
        {
            Console.WriteLine($" Name: {student.Name}, City: {student.City}");
        }

        //11.Find the top 3 students with the highest marks.
        var topThreeStd = students.OrderByDescending(x => x.Marks).Take(3);

        var topThreeStdFromQuery = (from s in students orderby s.Marks descending select s).Take(3);
        Console.WriteLine(" \n Top 3 Students: ");
        foreach (var student in topThreeStd)
        {
            Console.WriteLine($" Name: {student.Name} , Marks: {student.Marks}");
        }

        Console.WriteLine(" \n Top 3 Students from query: ");
        foreach (var student in topThreeStdFromQuery)
        {
            Console.WriteLine($" Name: {student.Name} , Marks: {student.Marks}");
        }

        //12.Display students aged between 20 and 22, ordered by age.
        var stdFilteredByAge = students.Where(x => x.Age >= 20 && x.Age <= 22).OrderBy(x => x.Age);

        var stdFilteredByAgeFromQuery = (
            from s in students
            where s.Age >= 20 && s.Age <= 22
            orderby s.Age
            select s
        );
        Console.WriteLine(" \n Students with age 20 to 22 : ");
        foreach (var student in stdFilteredByAge)
        {
            Console.WriteLine($" Name: {student.Name} , Age: {student.Age}");
        }

        Console.WriteLine(" \n Students with age 20 to 22 by query syntax: ");
        foreach (var student in stdFilteredByAgeFromQuery)
        {
            Console.WriteLine($" Name: {student.Name} , Age: {student.Age}");
        }

        //13. Combine two lists of students and remove duplicates based on `Id`.

        //new list of students
        List<Student> students2 = new List<Student>
        {
            new Student
            {
                Name = "Rammy",
                Age = 20,
                Gender = "Female",
                Marks = 92,
                City = "New York"
            },
            new Student
            {
                Name = "Sham",
                Age = 22,
                Gender = "Male",
                Marks = 78,
                City = "Chicago"
            },
            new Student
            {
                Name = "Charles",
                Age = 19,
                Gender = "Male",
                Marks = 92,
                City = "New York"
            },
        };

        var combinedList = students.UnionBy(students2, x => x.Id);

        Console.WriteLine("\n Combined list of students is:");
        foreach (var student in combinedList)
        {
            Console.WriteLine($"Id: {student.Id} ,Name: {student.Name}");
        }

        // var combinedList2= students.Join(students2,x=>x.Id,y=>y.Id);
    }
}
