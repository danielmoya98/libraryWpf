using System;
using System.Collections.ObjectModel;
using System.ComponentModel; // ICollectionView
using System.Linq;
using System.Threading.Tasks;
using System.Windows; // MessageBox (confirmaciones simples)
using System.Windows.Data; // CollectionViewSource
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using library.Abstractions; // ISearchable
using library.Models; // Reservation
using MaterialDesignThemes.Wpf; // DialogHost
using library.Dialogs;
using MahApps.Metro.IconPacks; // LoanReservationDialog, CancelReservationDialog VMs

namespace library.ViewModels
{
    public partial class OrdersViewModel : ObservableObject, ISearchable
    {
        // ===== Colección y vista filtrable =====
        public ObservableCollection<Reservation> Orders { get; }
        public ICollectionView OrdersView { get; }

        // ===== Filtros (vinculados al OrdersHeader) =====
        [ObservableProperty] private string? searchText;
        [ObservableProperty] private string status = "all"; // all|Pending|Active|Returned|Overdue
        [ObservableProperty] private DateTime? dateFrom;
        [ObservableProperty] private DateTime? dateTo;

        public ObservableCollection<DashStat> Stats { get; }


        // ===== Comandos existentes =====
        public IAsyncRelayCommand AddReservationAsyncCommand { get; }
        public IRelayCommand<Reservation?> EditReservationCommand { get; }
        public IRelayCommand<Reservation?> DeleteReservationCommand { get; }
        public IRelayCommand GenerateReservationsReportCommand { get; }

        // ===== NUEVOS comandos (tabla actualizada) =====
        public IAsyncRelayCommand<Reservation?> StartLoanCommand { get; }
        public IAsyncRelayCommand<Reservation?> CancelReservationCommand { get; }

        public OrdersViewModel()
        {
            Stats = new ObservableCollection<DashStat>
            {
                new DashStat("Prestamos Activos", 73, 120, PackIconMaterialKind.BookMultiple),
                new DashStat("Reservas activas", 97, -5, PackIconMaterialKind.Abacus),
                new DashStat("Reservas Finalizadas", 7, -5, PackIconMaterialKind.Abacus)
            };

            // Demo data
            Orders = new ObservableCollection<Reservation>(Seed());
            NormalizeStatuses();

            OrdersView = CollectionViewSource.GetDefaultView(Orders);
            OrdersView.Filter = FilterOrder;

            // Comandos existentes
            AddReservationAsyncCommand = new AsyncRelayCommand(AddReservationAsync);
            EditReservationCommand = new RelayCommand<Reservation?>(EditReservation);
            DeleteReservationCommand = new RelayCommand<Reservation?>(DeleteReservation);
            GenerateReservationsReportCommand = new RelayCommand(GenerateReport);

            // NUEVOS
            StartLoanCommand = new AsyncRelayCommand<Reservation?>(StartLoanAsync);
            CancelReservationCommand = new AsyncRelayCommand<Reservation?>(CancelReservationAsync);
        }

        // ====== Filtro principal ======
        private bool FilterOrder(object obj)
        {
            if (obj is not Reservation r) return false;

            // texto: busca en código, miembro, libro
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var q = SearchText.Trim();
                bool hit =
                    Contains(r.Code, q) ||
                    Contains(r.MemberName, q) ||
                    Contains(r.BookTitle, q);
                if (!hit) return false;
            }

            // estado
            if (!string.Equals(Status, "all", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals(r.Status, Status, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            // rango de fechas (inicio / límite)
            if (DateFrom is DateTime from && r.StartDate.Date < from.Date) return false;
            if (DateTo is DateTime to && r.DueDate.Date > to.Date) return false;

            return true;
        }

        private static bool Contains(string? s, string term) =>
            !string.IsNullOrWhiteSpace(s) &&
            s.Contains(term, StringComparison.OrdinalIgnoreCase);

        // ===== Notificar refresh al cambiar filtros =====
        partial void OnSearchTextChanged(string? value) => OrdersView.Refresh();
        partial void OnStatusChanged(string value) => OrdersView.Refresh();
        partial void OnDateFromChanged(DateTime? value) => OrdersView.Refresh();
        partial void OnDateToChanged(DateTime? value) => OrdersView.Refresh();

        // ===== Integración con buscador global =====
        public void SetSearch(string? text)
        {
            SearchText = text ?? string.Empty;
            // el Refresh lo dispara el setter via OnSearchTextChanged
        }

        // ===== Comandos existentes =====
        private async Task AddReservationAsync()
        {
            // Placeholder: aquí luego abres tu AddReservationDialog si deseas
            await Task.Yield();

            var today = DateTime.Today;
            var demo = new Reservation
            {
                Code = $"R-{today:yyMMdd}-{Orders.Count + 1:000}",
                MemberName = "Nuevo miembro",
                BookTitle = "Libro sin título",
                StartDate = today,
                DueDate = today.AddDays(7),
                Status = "Pending",
                Fine = 0m
            };
            Orders.Add(demo);
            OrdersView.Refresh();
        }

        private void EditReservation(Reservation? r)
        {
            if (r is null) return;

            // Placeholder: implementar EditReservationDialog si lo necesitas
            MessageBox.Show($"Editar reserva {r.Code} (pendiente implementar diálogo).",
                "Editar reserva", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteReservation(Reservation? r)
        {
            if (r is null) return;
            var ok = MessageBox.Show($"¿Eliminar la reserva {r.Code} de {r.MemberName}?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ok == MessageBoxResult.Yes)
            {
                Orders.Remove(r);
                OrdersView.Refresh();
            }
        }

        private void GenerateReport()
        {
            var total = Orders.Count;
            var act = Orders.Count(o => o.Status == "Active");
            var pend = Orders.Count(o => o.Status == "Pending");
            var ret = Orders.Count(o => o.Status == "Returned");
            var late = Orders.Count(o => o.Status == "Overdue");

            MessageBox.Show(
                $"Reservas totales: {total}\n" +
                $"Activas: {act}\nPendientes: {pend}\nDevueltas: {ret}\nAtrasadas: {late}",
                "Resumen de reservas",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ===== NUEVOS flujos =====
        private async Task StartLoanAsync(Reservation? r)
        {
            if (r is null) return;

            var vm = new LoanReservationDialogViewModel(r);
            var view = new LoanReservationDialog { DataContext = vm };

            var result = await DialogHost.Show(view, "RootDialog");
            if (result is LoanReservationDialogViewModel m)
            {
                // Confirmado: pasa a préstamo activo
                r.Status = "Active";
                r.StartDate = m.LoanStart.Date;
                r.DueDate = m.DueDate.Date;
                if (m.ResetFine) r.Fine = 0m;

                OrdersView.Refresh();
            }
        }

        private async Task CancelReservationAsync(Reservation? r)
        {
            if (r is null) return;

            var vm = new CancelReservationDialogViewModel(r);
            var view = new CancelReservationDialog { DataContext = vm };

            var result = await DialogHost.Show(view, "RootDialog");
            if (result is CancelReservationDialogViewModel)
            {
                // Política: cancelar = eliminar de la lista
                Orders.Remove(r);
                OrdersView.Refresh();
            }
        }

        // ===== Helpers =====
        private void NormalizeStatuses()
        {
            var today = DateTime.Today;
            foreach (var r in Orders)
            {
                if ((r.Status == "Active" || r.Status == "Pending")
                    && r.DueDate.Date < today)
                {
                    r.Status = "Overdue";
                    if (r.Fine <= 0) r.Fine = 2.50m; // demo
                }
            }
        }

        private static Reservation[] Seed()
        {
            var t = DateTime.Today;
            return new[]
            {
                new Reservation
                {
                    Code = "R-250801-001", MemberName = "Ana García", BookTitle = "El Quijote",
                    StartDate = t.AddDays(-10), DueDate = t.AddDays(-3), Status = "Overdue", Fine = 3.75m
                },

                new Reservation
                {
                    Code = "R-250804-002", MemberName = "Luis Pérez", BookTitle = "Clean Code",
                    StartDate = t.AddDays(-6), DueDate = t.AddDays(+1), Status = "Active", Fine = 0m
                },

                new Reservation
                {
                    Code = "R-250806-003", MemberName = "María Torres", BookTitle = "Sapiens",
                    StartDate = t.AddDays(-4), DueDate = t.AddDays(+4), Status = "Active", Fine = 0m
                },

                new Reservation
                {
                    Code = "R-250807-004", MemberName = "Andrés Rojas", BookTitle = "Cien años de soledad",
                    StartDate = t.AddDays(-3), DueDate = t.AddDays(+5), Status = "Pending", Fine = 0m
                },

                new Reservation
                {
                    Code = "R-250730-005", MemberName = "Valentina López", BookTitle = "Harry Potter I",
                    StartDate = t.AddDays(-20), DueDate = t.AddDays(-12), Status = "Returned", Fine = 0m
                },

                new Reservation
                {
                    Code = "R-250802-006", MemberName = "Alex Mendoza", BookTitle = "Clean Architecture",
                    StartDate = t.AddDays(-8), DueDate = t.AddDays(-1), Status = "Overdue", Fine = 5.00m
                },

                new Reservation
                {
                    Code = "R-250805-007", MemberName = "Carla Zapata", BookTitle = "The Pragmatic Programmer",
                    StartDate = t.AddDays(-5), DueDate = t.AddDays(+2), Status = "Active", Fine = 0m
                },

                new Reservation
                {
                    Code = "R-250803-008", MemberName = "Daniel Ortega", BookTitle = "Educated",
                    StartDate = t.AddDays(-7), DueDate = t.AddDays(+3), Status = "Active", Fine = 0m
                },

                new Reservation
                {
                    Code = "R-250729-009", MemberName = "Jorge Rivas", BookTitle = "Design Patterns",
                    StartDate = t.AddDays(-21), DueDate = t.AddDays(-14), Status = "Returned", Fine = 0m
                },

                new Reservation
                {
                    Code = "R-250808-010", MemberName = "Paula Núñez", BookTitle = "Refactoring",
                    StartDate = t.AddDays(-2), DueDate = t.AddDays(+6), Status = "Pending", Fine = 0m
                },
            };
        }
    }
}