using CommunityToolkit.Mvvm.Messaging.Messages;
using MusicStore.ViewModels;

namespace MusicStore.Message;

public class PurchaseAlbumMessage : AsyncRequestMessage<AlbumViewModel?>;