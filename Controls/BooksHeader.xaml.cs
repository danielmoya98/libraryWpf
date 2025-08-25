using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using library.Models; // BookCategory enum (ajusta el namespace si está en otro lado)

namespace library.Controls
{
    public enum AvailabilityFilter { All, Available, Unavailable }

    public partial class BooksHeader : UserControl
    {
         public double MediumBreakpoint { get; set; } = 1100;
            public double NarrowBreakpoint { get; set; } = 800;
        public BooksHeader()
        {
            InitializeComponent();

            // Disponibilidad
            var availability = new List<KeyValuePair<AvailabilityFilter, string>>();
            availability.Add(new KeyValuePair<AvailabilityFilter, string>(AvailabilityFilter.All,        "Todos"));
            availability.Add(new KeyValuePair<AvailabilityFilter, string>(AvailabilityFilter.Available,  "Disponible"));
            availability.Add(new KeyValuePair<AvailabilityFilter, string>(AvailabilityFilter.Unavailable,"No disponible"));
            AvailabilityItems = availability;

            // Géneros
            var genres = new List<KeyValuePair<BookCategory?, string>>();
            genres.Add(new KeyValuePair<BookCategory?, string>(null, "Todos los géneros"));
            foreach (var cat in Enum.GetValues(typeof(BookCategory)).Cast<BookCategory>())
                genres.Add(new KeyValuePair<BookCategory?, string>(cat, cat.ToString()));
            GenreItems = genres;

            // Calificación mínima
            var ratings = new List<KeyValuePair<int?, string>>();
            ratings.Add(new KeyValuePair<int?, string>(null, "Todas las calificaciones"));
            ratings.Add(new KeyValuePair<int?, string>(1, "★ 1+"));
            ratings.Add(new KeyValuePair<int?, string>(2, "★ 2+"));
            ratings.Add(new KeyValuePair<int?, string>(3, "★ 3+"));
            ratings.Add(new KeyValuePair<int?, string>(4, "★ 4+"));
            ratings.Add(new KeyValuePair<int?, string>(5, "★ 5"));
            RatingItems = ratings;
        }

        private void OnHeaderLoaded(object sender, RoutedEventArgs e)
        {
            ApplyResponsive();
            // Nos suscribimos también al SizeChanged del contenedor padre por si el UserControl no cambia su ActualWidth
            if (Parent is FrameworkElement feParent)
                feParent.SizeChanged += (_, __) => ApplyResponsive();
        }

        private void OnHeaderSizeChanged(object sender, SizeChangedEventArgs e) => ApplyResponsive();

        private void ApplyResponsive()
        {
            // Ancho disponible del propio control (si estás dentro de un Grid, se estira)
            double w = ActualWidth;
            if (w <= 0 && Parent is FrameworkElement p) w = p.ActualWidth;

            if (w < NarrowBreakpoint)
            {
                // Móvil: sólo disponibilidad + iconos
                cmbGenre.Visibility   = Visibility.Collapsed;
                cmbRating.Visibility  = Visibility.Collapsed;
                cmbAvailability.Visibility = Visibility.Collapsed;
            }
            else if (w < MediumBreakpoint)
            {
                // Tablet: ocultar calificación; botones con texto
                cmbGenre.Visibility   = Visibility.Visible;
                cmbAvailability.Visibility   = Visibility.Collapsed;
                cmbRating.Visibility  = Visibility.Collapsed;
                txtAdd.Visibility     = Visibility.Visible;
                txtReport.Visibility  = Visibility.Visible;
            }
            else
            {
                // Desktop: todo visible
                cmbGenre.Visibility   = Visibility.Visible;
                cmbRating.Visibility  = Visibility.Visible;
                cmbAvailability.Visibility = Visibility.Visible;
                txtAdd.Visibility     = Visibility.Visible;
                txtReport.Visibility  = Visibility.Visible;
            }
        }
        // ---------- Collections para los ComboBox ----------
        public IReadOnlyList<KeyValuePair<AvailabilityFilter, string>> AvailabilityItems { get; }
        public IReadOnlyList<KeyValuePair<BookCategory?, string>>      GenreItems        { get; }
        public IReadOnlyList<KeyValuePair<int?, string>>               RatingItems       { get; }

        // ---------- Commands ----------
        public static readonly DependencyProperty AddBookCommandProperty =
            DependencyProperty.Register(nameof(AddBookCommand), typeof(ICommand), typeof(BooksHeader));

        public ICommand? AddBookCommand
        {
            get => (ICommand?)GetValue(AddBookCommandProperty);
            set => SetValue(AddBookCommandProperty, value);
        }

        public static readonly DependencyProperty GenerateReportCommandProperty =
            DependencyProperty.Register(nameof(GenerateReportCommand), typeof(ICommand), typeof(BooksHeader));

        public ICommand? GenerateReportCommand
        {
            get => (ICommand?)GetValue(GenerateReportCommandProperty);
            set => SetValue(GenerateReportCommandProperty, value);
        }

        // ---------- Filtros ----------
        public static readonly DependencyProperty AvailabilityProperty =
            DependencyProperty.Register(nameof(Availability), typeof(AvailabilityFilter), typeof(BooksHeader),
                new FrameworkPropertyMetadata(AvailabilityFilter.All, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public AvailabilityFilter Availability
        {
            get => (AvailabilityFilter)GetValue(AvailabilityProperty);
            set => SetValue(AvailabilityProperty, value);
        }

        public static readonly DependencyProperty SelectedGenreProperty =
            DependencyProperty.Register(nameof(SelectedGenre), typeof(BookCategory?), typeof(BooksHeader),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public BookCategory? SelectedGenre
        {
            get => (BookCategory?)GetValue(SelectedGenreProperty);
            set => SetValue(SelectedGenreProperty, value);
        }

        public static readonly DependencyProperty MinRatingProperty =
            DependencyProperty.Register(nameof(MinRating), typeof(int?), typeof(BooksHeader),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int? MinRating
        {
            get => (int?)GetValue(MinRatingProperty);
            set => SetValue(MinRatingProperty, value);
        }
    }
}
