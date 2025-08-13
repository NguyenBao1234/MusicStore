using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
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
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;
        IsBusy = true;
        SearchResults.Clear();

        var albums = await Album.SearchAsync(term);// Get from Service

        foreach (var album in albums)
        {
            var vm = new AlbumViewModel(album);
            SearchResults.Add(vm);
        }

        IsBusy = false;
        if (!cancellationToken.IsCancellationRequested)
        {
            LoadCovers(cancellationToken);
        }
    }
    partial void OnSearchTextChanged(string? value)
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
                return;
            }
        }
    }
}