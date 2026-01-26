using FileAndDataAssingment.Service;

namespace FileAndDataAssingment;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(
            "Do you want to manipulate a file or convert a csv file to a json format \n Press 1 or 2 according to your choice"
        );

        var userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
            {
                FileService.ManipulateFile();
                break;
            }

            case "2":
            {
                FileService.ConversionToJSON();
                break;
            }

            default:
            {
                Console.WriteLine("Enter a valid option!!");
                break;
            }
        }
    }
}
