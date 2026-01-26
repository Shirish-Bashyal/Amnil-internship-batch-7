using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FileAndDataAssingment.Model;

namespace FileAndDataAssingment.Service;

public static class FileService
{
    //log file name
    private static readonly string LogFilePath =
        @"C:\Users\Dell\source\repos\internship-batch-7\Console\FileAndDataAssingment.cs\OutputFiles\app.log";

    static FileService()
    {
        if (!File.Exists(LogFilePath))
        {
            using (File.Create(LogFilePath)) { } // dispose immediately
            Console.WriteLine($"Log file created: {LogFilePath}");
        }
    }

    //helper logging method
    private static void Log(string level, string message)
    {
        File.AppendAllText(LogFilePath, $"[{DateTime.Now}] [{level}] {message}\n");
    }

    /// <summary>
    /// Manipulate a  text file i.e search for a word, gets no of lines, words and chracters
    /// </summary>
    public static void ManipulateFile()
    {
        try
        {
            //path for sample text
            string path =
                @"C:\Users\Dell\source\repos\internship-batch-7\Console\FileAndDataAssingment.cs\SampleFiles\Sample.txt";

            //check if sample text exist or not
            if (!File.Exists(path))
            {
                Console.WriteLine(" Sample File not found!");
                Log("ERROR", "Sample text file not found");

                return;
            }

            // converts all lines into a series of array elements
            var linesArray = File.ReadAllLines(path);
            int numOfLines = linesArray.Length;

            //gets all text
            var allText = File.ReadAllText(path);

            var wordsArray = allText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($" Lines: {numOfLines}");
            Console.WriteLine($" Words: {wordsArray.Length}");
            Console.WriteLine($" Characters: {allText.Length}");

            Console.WriteLine("Please enter a word to find in the text");
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Enter a valid word!!");
                Log("WARN", "Empty word entered");

                return;
            }

            var linesContainingWord = new List<int>();

            for (int i = 0; i < linesArray.Length; i++)
            {
                var singleLineArray = linesArray[i].Split(" ");
                var doesContainWord = Array.Exists(singleLineArray, x => x.Equals(userInput));

                if (doesContainWord)
                {
                    linesContainingWord.Add(i + 1);
                }
            }

            if (linesContainingWord.Count > 0)
            {
                Console.WriteLine(
                    $"Word '{userInput}' is found at lines: {string.Join(",", linesContainingWord)}"
                );

                Log("INFO", $"Word '{userInput}' found successfully");
                return;
            }

            Console.WriteLine("Word is not found in the text");
            Log("WARN", $"Word '{userInput}' not found in file");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Log("ERROR", $"Exception: {ex.Message}");
        }
    }

    /// <summary>
    /// Converts csv file to json
    /// </summary>
    public static void ConversionToJSON()
    {
        try
        {
            //path for csv file
            string csvPath =
                @"C:\Users\Dell\source\repos\internship-batch-7\Console\FileAndDataAssingment.cs\SampleFiles\Student.csv";

            //path for json file
            string jsonPath =
                @"C:\Users\Dell\source\repos\internship-batch-7\Console\FileAndDataAssingment.cs\OutputFiles\Student.json";

            if (!File.Exists(csvPath))
            {
                Console.WriteLine("CSV file not found!");
                Log("WARN", "CSV file not found");

                return;
            }
            var linesArray = File.ReadAllLines(csvPath);

            var students = new List<Student>();

            //begining from 1 to skip header
            for (int i = 1; i < linesArray.Length; i++)
            {
                var values = linesArray[i].Split(',');

                var student = new Student
                {
                    Id = int.TryParse(values[0], out int id) ? id : 0,
                    FirstName = values[1],
                    LastName = values[2],
                    Email = values[3]
                };

                students.Add(student);
            }

            var jsonStudent = JsonSerializer.Serialize(students);

            File.WriteAllText(jsonPath, jsonStudent);

            Console.WriteLine($"CSV converted to JSON successfully");
            Log("INFO", "CSV converted to JSON successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while converting CSV: {ex.Message}");
            Log("ERROR", $"CSV conversion failed: {ex.Message}");
        }
    }
}
