using CommunityToolkit.Mvvm.ComponentModel;
using library.Models;

namespace library.Dialogs
{
    public partial class CancelReservationDialogViewModel : ObservableObject
    {
        public Reservation Reservation { get; }
        [ObservableProperty] private string? reason;

        public CancelReservationDialogViewModel(Reservation r)
        {
            Reservation = r;
        }
    }
}