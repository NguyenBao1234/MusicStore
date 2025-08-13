using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicStore.Models;

namespace MusicStore.ViewModels;

public partial class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;

    public AlbumViewModel(Album album)
    {
        _album = album;
    }

    public string Artist => _album.Artist;

    public string Title => _album.Title;
    
    [ObservableProperty] public partial Bitmap? CoverImgBit { get; private set; }
    
    public async Task LoadCover()
    {
        await using (var imageStream = await _album.LoadCoverBitmapAsync())
        {
            CoverImgBit = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
    }
    public async Task SaveToDiskAsync()
    {
        await _album.SaveAsync();

        if (CoverImgBit != null)
        {
            await Task.Run(() =>
            {
                using (var fs = _album.SaveCoverBitmapStream())
                {
                    CoverImgBit.Save(fs);
                }
            });
        }
    }
}