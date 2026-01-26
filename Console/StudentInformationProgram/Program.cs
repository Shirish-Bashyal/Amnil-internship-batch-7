// stores student name in string format
string studentName = "Shirish Bashyal";

//stores the identity number of student
int studentId = 101;

// stores age iin years
int studentAge = 21;

// stores gpa of the student
double gpa = 3.5;

// stores couse enrollment status of student in bool format
bool isEnrolled = true;

// stores grade of the student
char gradeLevel = 'A';

//stores favourite subject of the student
string favoriteSubject = "Mathematics";

int ageInDays = studentAge * 365;
int ageInHours = ageInDays * 24;
int ageInMinutes = ageInHours * 60;

Console.WriteLine(" Student Information");
Console.WriteLine($"Student ID: {studentId}");
Console.WriteLine($"Name: {studentName}");
Console.WriteLine(
    $"Age: {studentAge} years ({ageInDays} days, {ageInHours} hours, {ageInMinutes} minutes)"
);
Console.WriteLine($"GPA: {gpa}");
Console.WriteLine($"Enrolled: {isEnrolled}");
Console.WriteLine($"Grade Level: {gradeLevel}");
Console.WriteLine($"Favorite Subject: {favoriteSubject}");

//second student
string student2Name = "Ram";
int student2Id = 102;
int student2Age = 19;
double student2Gpa = 3.8;
bool student2Enrolled = true;
char student2GradeLevel = 'B';
string student2FavoriteSubject = "Science";

int student2AgeInDays = student2Age * 365;
int student2AgeInHours = ageInDays * 24;
int student2AgeInMinutes = ageInHours * 60;

Console.WriteLine("\n Second Student Information ");
Console.WriteLine($"Student ID: {student2Id}");
Console.WriteLine($"Name: {student2Name}");
Console.WriteLine(
    $"Age: {student2Age} years ({student2AgeInDays} days, {student2AgeInHours} hours, {student2AgeInMinutes} minutes)"
);
Console.WriteLine($"GPA: {student2Gpa}");
Console.WriteLine($"Enrolled: {student2Enrolled}");
Console.WriteLine($"Grade Level: {student2GradeLevel}");
Console.WriteLine($"Favorite Subject: {student2FavoriteSubject}");






//try
//{
//    Console.WriteLine("Enter your name");
//    string studentName = Console.ReadLine();

//    Console.WriteLine("Enter your age");

//    int studentAge = Convert.ToInt32(Console.ReadLine());

//    Console.WriteLine("Enter your GPA");
//    double gpa = Convert.ToDouble(Console.ReadLine());

//    Console.WriteLine("Are you enrolled any course? Y for yes else no");
//    bool isEnrolled = Console.ReadLine() == "Y" ? true : false;

//    Console.WriteLine("Enter your current grade level i.e 'A', 'B', 'C' , 'D'");
//    char gradeLevel = Convert.ToChar(Console.ReadLine());

//    Console.WriteLine($"Student Info:");
//    Console.WriteLine(
//        $"Name: {studentName}, Age: {studentAge}, GPA: {gpa}, Enrolled: {isEnrolled}, Grade: {gradeLevel}"
//    );
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
