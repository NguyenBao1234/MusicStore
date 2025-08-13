using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace MusicStore.Models;

public class Album
{

    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }
    
    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }       
    
    private static iTunesSearchManager s_SearchManager = new();
    public static async Task<IEnumerable<Album>> SearchAsync(string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Enumerable.Empty<Album>();
        }
            
        var query = await s_SearchManager.GetAlbumsAsync(searchTerm)
            .ConfigureAwait(false);
                
        return query.Albums.Select(x =>
            new Album(x.ArtistName, x.CollectionName, 
                x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }
    
    private static HttpClient s_httpClient = new();
    private string CachePath => $"./Cache/{SanitizeFileName(Artist)} - {SanitizeFileName(Title)}";

    public async Task<Stream> LoadCoverBitmapAsync()
    {
        if (File.Exists(CachePath + ".bmp"))
        {
            return File.OpenRead(CachePath + ".bmp");//Cache is not active at this time, will implement later
        }
        else
        {
            var data = await s_httpClient.GetByteArrayAsync(CoverUrl);//CoverURL get from constructor when search from iTune
            return new MemoryStream(data);
        }
    }

    private static string SanitizeFileName(string input)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            input = input.Replace(c, '_');
        }
        return input;
    }
}