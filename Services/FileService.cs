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

    // these should not be here
    private List<int> _movieIds;
    private List<string> _movieTitles;
    private List<string> _movieGenres;

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

        _movieIds = new List<int>();
        _movieTitles = new List<string>();
        _movieGenres = new List<string>();
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
                int idx = line.IndexOf('"');
                if (idx == -1)
                {
                    string[] movieDetails = line.Split(',');
                    _movieIds.Add(int.Parse(movieDetails[0]));
                    _movieTitles.Add(movieDetails[1]);
                    _movieGenres.Add(movieDetails[2].Replace("|", ", "));
                }
                else
                {
                    _movieIds.Add(int.Parse(line.Substring(0, idx - 1)));
                    line = line.Substring(idx + 1);
                    idx = line.IndexOf('"');
                    _movieTitles.Add(line.Substring(0, idx));
                    line = line.Substring(idx + 2);
                    _movieGenres.Add(line.Replace("|", ", "));
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
        List<string> lowerCaseMovieTitles = _movieTitles.ConvertAll(t => t.ToLower());
        if (lowerCaseMovieTitles.Contains(movieTitle.ToLower()))
        {
            Console.WriteLine("That movie has already been entered");
            _logger.LogInformation("Duplicate movie title {Title}", movieTitle);
        }
        else
        {
            movieId = _movieIds.Max() + 1;
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
            
        _movieIds.Add(movieId);
        _movieTitles.Add(movieTitle);
        _movieGenres.Add(genresString);
        _logger.LogInformation("Movie id {Id} added", movieId);

        }
    public void Display()
    {
        _logger.LogInformation("Displaying all movies");
        for (int i = 0; i < _movieIds.Count; i++)
        {
            Console.WriteLine($"Id: {_movieIds[i]}");
            Console.WriteLine($"Title: {_movieTitles[i]}");
            Console.WriteLine($"Genre(s): {_movieGenres[i]}");
            Console.WriteLine();
        }
        _logger.LogInformation("All movies displayed");
    }
}
        
    

