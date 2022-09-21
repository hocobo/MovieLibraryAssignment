using System;

namespace MovieLibraryAssignment.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{
    private readonly IFileService _fileService;
    public MainService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public void Invoke()
    {
        string choice;
        do
        {
            Console.WriteLine("1) Add Movie");
            Console.WriteLine("2) Display All Movies");
            Console.WriteLine("X) Quit");
            choice = Console.ReadLine()?.ToUpper();
            
            if (choice == "1")
            {
                _fileService.Read();
                _fileService.Write();
            }
            else if (choice == "2")
            {
                _fileService.Read();
                _fileService.Display();
            }
            else if(choice == "X")
            {
                Console.WriteLine("Goodbye");
            }
            else
            {
                Console.WriteLine("Not a valid input");
            }
        }
        while (choice != "X");
    }
}
