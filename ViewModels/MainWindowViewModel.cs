using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Message;

namespace MusicStore.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<AlbumViewModel> Albums { get; } = new();
    public MainWindowViewModel()
    {
        // ViewModel initialization logic.
    }

    [RelayCommand]
    private async Task AddAlbumAsync()
    {
        System.Console.WriteLine("Add album Executed...");
        AlbumViewModel? album = await WeakReferenceMessenger.Default.Send(new PurchaseAlbumMessage());
        if (album != null)
        {
            Albums.Add(album);
            await album.SaveToDiskAsync();
        }
    }
}