using System.Text;

namespace StudentManagementMenuSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // determines whether to continue running program or exit
                var isRunning = true;

                //number of students that will be added
                Console.Write("Enter number of students: ");
                int studentCount = Convert.ToInt32(Console.ReadLine());

                //number of grades for all students
                Console.Write("Enter number of grades per student: ");
                int gradeCount = Convert.ToInt32(Console.ReadLine());

                //stores a array of student names
                string[] studentNames = new string[studentCount];

                //2D array storing respective grades for respective student names
                int[,] studentGrades = new int[studentCount, gradeCount];

                Console.WriteLine("\n \n=====WELCOME TO STUDENT MANAGEMENT MENU SYSTEM===== \n ");

                while (isRunning)
                {
                    Console.WriteLine("------------------------------------------------");

                    Console.WriteLine("1. Add Student");
                    Console.WriteLine("2. Enter Grades");
                    Console.WriteLine("3. View Report");
                    Console.WriteLine("4. Exit ");

                    Console.WriteLine("-------------------------------------------------");
                    Console.Write("Enter your choice (1-4): ");
                    var userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "1":
                            AddStudents(studentNames);
                            break;

                        case "2":
                            EnterGrades(studentNames, studentGrades, gradeCount);
                            break;

                        case "3":
                            ViewReport(studentNames, studentGrades, gradeCount);
                            break;

                        case "4":
                            isRunning = false;
                            Console.WriteLine("Program ended.");
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please enter 1-4.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        ///Adds students to the array
        /// </summary>
        /// <param name="names"></param>
        static void AddStudents(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (string.IsNullOrEmpty(names[i]))
                {
                    string inputName;
                    do
                    {
                        Console.Write($"Enter valid name for student {i + 1}: ");
                        inputName = Console.ReadLine();
                    } while (string.IsNullOrWhiteSpace(inputName));

                    names[i] = inputName;
                }
            }
        }

        /// <summary>
        ///Enter grades for all students
        /// </summary>
        /// <param name="names"> represents student names in array form</param>
        /// <param name="grades"> represents grades in array form</param>
        /// <param name="gradeCount"> represents number of grades</param>
        static void EnterGrades(string[] names, int[,] grades, int gradeCount)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(names[i]))
                {
                    Console.WriteLine(
                        $"\nStudent at position {i + 1} does not exist. Please add the student first."
                    );
                    break;
                }

                Console.WriteLine($"\nEnter grades for {names[i]}:");

                for (int j = 0; j < gradeCount; j++)
                {
                    int grade;
                    do
                    {
                        Console.Write($"Grade {j + 1}, enter values with in (0-100): ");
                        grade = Convert.ToInt32(Console.ReadLine());
                    } while (grade < 0 || grade > 100);

                    grades[i, j] = grade;
                }
            }
        }

        /// <summary>
        ///gets all students information along with class performance
        /// </summary>
        /// <param name="names"> repesents students in array form</param>
        /// <param name="grades"> represents grades obtained by student in array form</param>
        /// <param name="gradeCount"> represents no of grades per student</param>
        static void ViewReport(string[] names, int[,] grades, int gradeCount)
        {
            int studentCount = names.Length;
            double classTotal = 0;
            double topAverage = 0;
            double lowAverage = 100;
            string topStudent = "",
                lowStudent = "";

            Console.WriteLine("\n \n===== STUDENT REPORT =====");

            for (int i = 0; i < studentCount; i++)
            {
                int total = 0;
                StringBuilder resultGrade = new StringBuilder();

                for (int j = 0; j < gradeCount; j++)
                {
                    total += grades[i, j];

                    //converting each grade of a student into a string
                    resultGrade.Append(grades[i, j]);

                    resultGrade.Append(", ");
                }

                double average = (double)total / gradeCount;
                classTotal += average;

                if (average > topAverage)
                {
                    topAverage = average;
                    topStudent = names[i];
                }

                if (average < lowAverage)
                {
                    lowAverage = average;
                    lowStudent = names[i];
                }

                Console.WriteLine(
                    $"{names[i]} - Grades: {resultGrade} Avg: {average}, Letter Grade: {GetLetterGrade(average)}"
                );
            }

            double classAverage = classTotal / studentCount;
            Console.WriteLine($"\nClass Average: {classAverage}");
            Console.WriteLine($"Top Performer: {topStudent} ({topAverage})");
            Console.WriteLine($"Lowest Performer: {lowStudent} ({lowAverage}) \n \n");
        }

        /// <summary>
        ///converts average marks to letter grade
        /// </summary>
        /// <param name="avg"> represents average marks obtained by the student </param>
        /// <returns> grade in string format</returns>
        static string GetLetterGrade(double avg)
        {
            if (avg >= 90)
                return "A";
            else if (avg >= 80)
                return "B";
            else if (avg >= 60)
                return "C";
            else if (avg >= 40)
                return "D";
            else
                return "F";
        }
    }
}
