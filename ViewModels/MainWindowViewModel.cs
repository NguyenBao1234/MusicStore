using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Message;
using MusicStore.Models;

namespace MusicStore.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<AlbumViewModel> AlbumVMs { get; } = new();
    public MainWindowViewModel()
    {
        // ViewModel initialization logic.
        LoadAlbums();
    }

    [RelayCommand]
    private async Task AddAlbumAsync()
    {
        System.Console.WriteLine("Add album Executed...");
        AlbumViewModel? album = await WeakReferenceMessenger.Default.Send(new PurchaseAlbumMessage());
        if (album != null)
        {
            AlbumVMs.Add(album);
            await album.SaveToDiskAsync();
        }
    }
    
    private async void LoadAlbums()
    {
        var albums = await Album.LoadCachedAsync();
        foreach (var album in albums)
        {
            AlbumVMs.Add(new AlbumViewModel(album));
        }

        // foreach (var album in AlbumVMs)
        // {
        //     await album.LoadCover();//Load one by one
        // }
        
        var coverTasks = AlbumVMs.Select(album => album.LoadCover());//Load all parallel
        await Task.WhenAll(coverTasks);
    }

}