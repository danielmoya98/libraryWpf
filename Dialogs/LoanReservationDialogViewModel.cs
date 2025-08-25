using System;
using CommunityToolkit.Mvvm.ComponentModel;
using library.Models;

namespace library.Dialogs
{
    public partial class LoanReservationDialogViewModel : ObservableObject
    {
        public Reservation Reservation { get; }

        [ObservableProperty] private DateTime loanStart;
        [ObservableProperty] private DateTime dueDate;
        [ObservableProperty] private string? notes;
        [ObservableProperty] private bool resetFine = true;

        public bool IsValid => DueDate.Date >= LoanStart.Date;

        public LoanReservationDialogViewModel(Reservation r)
        {
            Reservation = r;
            LoanStart   = DateTime.Today;
            // Sugerimos devolver en 7 días o mantenemos el dueDate mayor
            var minDue  = LoanStart.AddDays(7).Date;
            DueDate     = r.DueDate > minDue ? r.DueDate.Date : minDue;
        }
    }
}