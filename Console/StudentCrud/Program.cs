using StudentCrudConsole;

try
{
    StudentCrud StudentManager = new StudentCrud();
    while (true)
    {
        Console.WriteLine("enter 1,2,3,4 to create, read, update, delete students data");
        var userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                Console.WriteLine("Enter id, name and address for student with a space ");
                var inputData = Console.ReadLine().Split(" ");
                var stdData = new StudentModel()
                {
                    Id = Convert.ToInt32(inputData[0]),
                    Name = inputData[1],
                    Address = inputData[2]
                };

                var result = StudentManager.AddStudent(stdData);
                Console.WriteLine(result);
                break;

            case "2":
                Console.WriteLine("The students information is:");
                var students = StudentManager.GetStudents();
                foreach (var std in students)
                {
                    Console.WriteLine($"Name= {std.Name}, Address= {std.Address}");
                }
                break;

            case "3":
                Console.WriteLine("Provide the student id and both name and address to be updated");
                var inputUpdateData = Console.ReadLine().Split(" ");
                var stdUpdateData = new StudentModel()
                {
                    Id = Convert.ToInt32(inputUpdateData[0]),
                    Name = inputUpdateData[1],
                    Address = inputUpdateData[2]
                };
                var updateResult = StudentManager.UpdateStudent(stdUpdateData);
                Console.WriteLine(updateResult);
                break;

            case "4":
                Console.WriteLine("Provide the id of the student to delete");
                var studentId = Convert.ToInt32(Console.ReadLine());
                var deleteResult = StudentManager.DeleteStudent(studentId);

                Console.WriteLine(deleteResult);
                break;

            default:
                Console.WriteLine("Invalid option. Please enter 1, 2, 3, or 4.");
                break;
        }
        Console.WriteLine("Do you want to continue: Y for yes ");
        var userentry = Console.ReadLine();
        if (userentry == "Y")
            continue;
        else
        {
            Console.WriteLine("Program ended ");

            break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
