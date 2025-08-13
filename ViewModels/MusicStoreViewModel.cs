using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicStore.Message;
using MusicStore.Models;

namespace MusicStore.ViewModels;

public partial class MusicStoreViewModel:ViewModelBase
{
    [ObservableProperty] public partial string? SearchText { get; set; }
        
    [ObservableProperty] public partial bool IsBusy { get; private set; }
    [ObservableProperty] public partial AlbumViewModel? SelectedAlbum { get; set; }

    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();// <=> [] in c#12; this var like List<> or Array<>
    
    private CancellationTokenSource? _cancellationTokenSource;
    
    private async Task DoSearch(string? term)
    {
        _cancellationTokenSource?.Cancel();//Cancel Loading Old Token ( Previous Albums )
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;
        IsBusy = true;
        SearchResults.Clear();

        var albums = await Album.SearchAsync(term);// Get from Service, time-consuming

        foreach (var album in albums)
        {
            AlbumViewModel albumVm = new AlbumViewModel(album);
            SearchResults.Add(albumVm);
        }

        IsBusy = false;
        if (!cancellationToken.IsCancellationRequested)//Only Load New Result search, ensure only load new result's token after time-consuming
        {
            LoadCovers(cancellationToken);
        }
    }
    partial void OnSearchTextChanged(string? value)//override hook function
    {
        _ = DoSearch(SearchText);// Discard Return, call to get Side Effect only
    }
    
    private async void LoadCovers(CancellationToken cancellationToken)
    {
        foreach (var album in SearchResults.ToList())
        {
            await album.LoadCover();
            if (cancellationToken.IsCancellationRequested)
            {
                return;//Stop Loading ImgCover for Albums below if current token was canceled
            }
        }
    }
    
    [RelayCommand]
    private void BuyMusic()
    {
        if (SelectedAlbum != null)
        {
            WeakReferenceMessenger.Default.Send(new MusicStoreCloseMessage(SelectedAlbum));
        }
    }
}