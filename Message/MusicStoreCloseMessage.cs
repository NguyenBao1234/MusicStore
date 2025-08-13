using MusicStore.ViewModels;

namespace MusicStore.Message;

public class MusicStoreCloseMessage
{
    public AlbumViewModel SelectedAlbum { get; }

    public MusicStoreCloseMessage(AlbumViewModel selectedAlbum)
    {
        SelectedAlbum = selectedAlbum;
    }
}