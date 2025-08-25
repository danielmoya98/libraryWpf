using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace library.Models
{
    /// <summary>
    /// Modelo de reserva (Orders/Reservations).
    /// Usa Status como string para encajar con el ViewModel actual.
    /// </summary>
    public partial class Reservation : ObservableObject
    {
        // Identificador legible (ej: R-250801-001)
        [ObservableProperty] private string? code;

        // Persona que reserva
        [ObservableProperty] private string? memberName;

        // Libro reservado
        [ObservableProperty] private string? bookTitle;

        // Fechas clave
        [ObservableProperty] private DateTime startDate;
        [ObservableProperty] private DateTime dueDate;
        [ObservableProperty] private DateTime? returnedDate;

        /// <summary>
        /// Estado textual: usa constantes en <see cref="ReservationStatuses"/>.
        /// Valores esperados: "Pending" | "Active" | "Returned" | "Overdue".
        /// </summary>
        [ObservableProperty] private string status = ReservationStatuses.Pending;

        /// <summary>
        /// Multa acumulada (si aplica).
        /// </summary>
        [ObservableProperty] private decimal fine;

        // ----------------- Propiedades derivadas (solo lectura) -----------------

        /// <summary>
        /// True si la reserva está vencida según las fechas o el estado.
        /// </summary>
        public bool IsOverdue =>
            string.Equals(Status, ReservationStatuses.Overdue, StringComparison.OrdinalIgnoreCase) ||
            (ReturnedDate is null && DateTime.Today > DueDate.Date);

        /// <summary>
        /// Días restantes hasta la fecha de entrega (negativo si ya venció).
        /// </summary>
        public int DaysRemaining => (DueDate.Date - DateTime.Today).Days;

        /// <summary>
        /// True si ya fue devuelto.
        /// </summary>
        public bool IsReturned =>
            string.Equals(Status, ReservationStatuses.Returned, StringComparison.OrdinalIgnoreCase) ||
            ReturnedDate is not null;

        public override string ToString()
            => $"{Code} · {MemberName} → {BookTitle} · {Status}";
    }

    /// <summary>
    /// Constantes de estado para evitar literales mágicos.
    /// </summary>
    public static class ReservationStatuses
    {
        public const string Pending  = "Pending";
        public const string Active   = "Active";
        public const string Returned = "Returned";
        public const string Overdue  = "Overdue";
    }
}
