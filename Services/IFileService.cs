using System.Collections.Generic;

namespace MovieLibraryAssignment.Services;


public interface IFileService
{
    List<int> MovieIds { get; set; }
    List<string> MovieTitles{ get; set; }
    List<string> MovieGenres{ get; set; }
    void Read();
    void Display();
    void Write();
}
