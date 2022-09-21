using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MovieLibraryAssignment.Services;
public class FileService : IFileService
{
    private ILogger<IFileService> _logger;
    private string _fileName;
    
    
    public List<int> MovieIds { get; set; }
    public List<string> MovieTitles{ get; set; }
    public List<string> MovieGenres{ get; set; }

    #region constructors
    
    public FileService()
    {

    }
    public FileService(int myInt)
    {
        Console.WriteLine($"constructor value {myInt}");
    }

    public FileService(string myString)
    {
        Console.WriteLine($"constructor value {myString}");

    }

    #endregion

    public FileService(ILogger<IFileService> logger)
    {
        _logger = logger;
        logger.LogInformation("Movie Library");

        _fileName = $"{Environment.CurrentDirectory}/movies.csv";

        MovieIds = new List<int>();
        MovieTitles = new List<string>();
        MovieGenres = new List<string>();
    }

    public void Read()
    {
        try
        {
            StreamReader sr = new StreamReader(_fileName);
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line != null)
                {
                    int idx = line.IndexOf('"');
                    if (idx == -1)
                    {
                        string[] movieDetails = line.Split(',');
                        MovieIds.Add(int.Parse(movieDetails[0]));
                        MovieTitles.Add(movieDetails[1]);
                        MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                    }
                    else
                    {
                        MovieIds.Add(int.Parse(line.Substring(0, idx - 1)));
                        line = line.Substring(idx + 1);
                        idx = line.IndexOf('"');
                        MovieTitles.Add(line.Substring(0, idx));
                        line = line.Substring(idx + 2);
                        MovieGenres.Add(line.Replace("|", ", "));
                    }
                }
            }
            sr.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public void Write()
    {
        
        Console.WriteLine("Enter the movie title");
        var movieId = 0;
        string genresString = null;
        string genre;
        string movieTitle = Console.ReadLine();
        List<string> lowerCaseMovieTitles = MovieTitles.ConvertAll(t => t.ToLower());
        if (lowerCaseMovieTitles.Contains(movieTitle?.ToLower()))
        {
            Console.WriteLine("That movie has already been entered");
            _logger.LogInformation("Duplicate movie title {Title}", movieTitle);
        }
        else
        {
            movieId = MovieIds.Max() + 1;
            List<string> genres = new List<string>();
            do
            {
                Console.WriteLine("Enter genre (or done to quit)");
                genre = Console.ReadLine();
                if (genre != "done" && genre.Length > 0)
                {
                    genres.Add(genre);
                }
            } while (genre != "done");
            if (genres.Count == 0)
            {
                genres.Add("(no genres listed)");
            }
            genresString = string.Join("|", genres);
            movieTitle = movieTitle.IndexOf(',') != -1 ? $"\"{movieTitle}\"" : movieTitle;
        }
        StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine($"{movieId},{movieTitle},{genresString}");
        sw.Close();
            
        MovieIds.Add(movieId);
        MovieTitles.Add(movieTitle);
        MovieGenres.Add(genresString);
        _logger.LogInformation("Movie id {Id} added", movieId);

        }
    public void Display()
    {
        _logger.LogInformation("Displaying all movies");
        for (int i = 0; i < MovieIds.Count; i++)
        {
            Console.WriteLine($"Id: {MovieIds[i]}");
            Console.WriteLine($"Title: {MovieTitles[i]}");
            Console.WriteLine($"Genre(s): {MovieGenres[i]}");
            Console.WriteLine();
        }
        _logger.LogInformation("All movies displayed");
    }
}
        
    

