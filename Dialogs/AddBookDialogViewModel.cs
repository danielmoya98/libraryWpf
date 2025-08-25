using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace library.Dialogs.AddBook
{
    public partial class AddBookDialogViewModel : ObservableObject
    {
        // ======= Paso actual del wizard (0=Buscar, 1=Detalles, 2=Distribución) =======
        [ObservableProperty] private int stepIndex = 0;

        public bool IsStepSearch        => StepIndex == 0;
        public bool IsStepDetails       => StepIndex == 1;
        public bool IsStepDistribution  => StepIndex == 2;

        partial void OnStepIndexChanged(int value)
        {
            OnPropertyChanged(nameof(IsStepSearch));
            OnPropertyChanged(nameof(IsStepDetails));
            OnPropertyChanged(nameof(IsStepDistribution));
            OnPropertyChanged(nameof(CanSave));
        }

        // ======= Resultado seleccionado =======
        [ObservableProperty] private string? title;
        [ObservableProperty] private string? author;
        [ObservableProperty] private string? isbn;
        [ObservableProperty] private string? category;
        [ObservableProperty] private int year;
        [ObservableProperty] private int stock = 1;
        [ObservableProperty] private double rating;
        [ObservableProperty] private bool available = true;
        [ObservableProperty] private string? coverPath;

        // ======= Campos adicionales =======
        [ObservableProperty] private string? coverUrl;
        [ObservableProperty] private string? publisher;
        [ObservableProperty] private string? synopsis;

        // Estado del libro (ComboBox)
        [ObservableProperty] private string conditionKey = "new"; // new|good|damaged|lost

        public IReadOnlyList<KeyValuePair<string, string>> ConditionItems { get; } = new[]
        {
            new KeyValuePair<string,string>("new",     "Nuevo"),
            new KeyValuePair<string,string>("good",    "Bueno"),
            new KeyValuePair<string,string>("damaged", "Dañado"),
            new KeyValuePair<string,string>("lost",    "Perdido"),
        };

        [ObservableProperty] private DateTime? publicationDate;

        // ======= Filtros paso 1 =======
        [ObservableProperty] private string searchText = string.Empty;
        [ObservableProperty] private string availability = "all";  // all|in|out
        [ObservableProperty] private string selectedGenre = "all";
        [ObservableProperty] private double minRating = 0;

        public IReadOnlyList<KeyValuePair<string, string>> AvailabilityItems { get; } = new[]
        {
            new KeyValuePair<string,string>("all","Todos"),
            new KeyValuePair<string,string>("in","Disponible"),
            new KeyValuePair<string,string>("out","No disponible"),
        };

        public IReadOnlyList<KeyValuePair<string, string>> GenreItems { get; } = new[]
        {
            new KeyValuePair<string,string>("all","Todos los géneros"),
            new KeyValuePair<string,string>("novel","Novel"),
            new KeyValuePair<string,string>("history","History"),
            new KeyValuePair<string,string>("technology","Technology"),
            new KeyValuePair<string,string>("fantasy","Fantasy"),
            new KeyValuePair<string,string>("science","Science"),
        };

        public IReadOnlyList<KeyValuePair<double, string>> RatingItems { get; } = new[]
        {
            new KeyValuePair<double,string>(0,   "Todas las calificaciones"),
            new KeyValuePair<double,string>(3.0, "3★ o más"),
            new KeyValuePair<double,string>(4.0, "4★ o más"),
            new KeyValuePair<double,string>(4.5, "4.5★ o más"),
        };

        // ======= Sugerencias =======
        public ObservableCollection<BookPick> Suggestions { get; }
        public ICollectionView SuggestionsView { get; }

        [ObservableProperty] private BookPick? selectedSuggestion;

        // ======= Distribución por estado =======
        [ObservableProperty] private int distNew;
        [ObservableProperty] private int distUsed;
        [ObservableProperty] private int distWorn;
        [ObservableProperty] private int distDamaged;
        [ObservableProperty] private int distRepaired;
        [ObservableProperty] private int distRestoring;

        public int DistTotal =>
            DistNew + DistUsed + DistWorn + DistDamaged + DistRepaired + DistRestoring;

        public int DistDiff => Stock - DistTotal;

        // Validaciones
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Title) &&
            !string.IsNullOrWhiteSpace(Author) &&
            !string.IsNullOrWhiteSpace(Isbn);

        // Guardar permitido:
        // - En paso 1 (Detalles) alcanza con IsValid
        // - En paso 2 (Distribución) además DistDiff debe ser 0
        public bool CanSave =>
            IsValid && (StepIndex == 2 ? DistDiff == 0 : true);

        public AddBookDialogViewModel()
        {
            Suggestions = new ObservableCollection<BookPick>(Demo());
            SuggestionsView = CollectionViewSource.GetDefaultView(Suggestions);
            SuggestionsView.Filter = Filter;
        }

        // ======= Filtro paso 1 =======
        private bool Filter(object obj)
        {
            if (obj is not BookPick s) return false;

            var q = (SearchText ?? "").Trim();
            if (q.Length > 0 &&
                !(Contains(s.Title, q) || Contains(s.Author, q) || Contains(s.ISBN, q)))
                return false;

            if (Availability == "in"  && !s.Available) return false;
            if (Availability == "out" &&  s.Available) return false;

            if (SelectedGenre != "all" &&
                !string.Equals(s.CategoryKey, SelectedGenre, StringComparison.OrdinalIgnoreCase))
                return false;

            if (s.Rating < MinRating) return false;

            return true;
        }

        private static bool Contains(string? src, string term) =>
            !string.IsNullOrWhiteSpace(src) &&
            src.Contains(term, StringComparison.OrdinalIgnoreCase);

        partial void OnSearchTextChanged(string value)     => SuggestionsView.Refresh();
        partial void OnAvailabilityChanged(string value)   => SuggestionsView.Refresh();
        partial void OnSelectedGenreChanged(string value)  => SuggestionsView.Refresh();
        partial void OnMinRatingChanged(double value)      => SuggestionsView.Refresh();

        // Recalcular totales cuando cambian cantidades o stock
        partial void OnStockChanged(int value)             => RaiseDistTotals();
        partial void OnDistNewChanged(int value)           => RaiseDistTotals();
        partial void OnDistUsedChanged(int value)          => RaiseDistTotals();
        partial void OnDistWornChanged(int value)          => RaiseDistTotals();
        partial void OnDistDamagedChanged(int value)       => RaiseDistTotals();
        partial void OnDistRepairedChanged(int value)      => RaiseDistTotals();
        partial void OnDistRestoringChanged(int value)     => RaiseDistTotals();

        private void RaiseDistTotals()
        {
            OnPropertyChanged(nameof(DistTotal));
            OnPropertyChanged(nameof(DistDiff));
            OnPropertyChanged(nameof(CanSave));
        }

        // Validación mínima
        partial void OnTitleChanged(string? value)  { OnPropertyChanged(nameof(IsValid)); OnPropertyChanged(nameof(CanSave)); }
        partial void OnAuthorChanged(string? value) { OnPropertyChanged(nameof(IsValid)); OnPropertyChanged(nameof(CanSave)); }
        partial void OnIsbnChanged(string? value)   { OnPropertyChanged(nameof(IsValid)); OnPropertyChanged(nameof(CanSave)); }

        partial void OnCoverUrlChanged(string? value)
        {
            // Sincroniza preview
            CoverPath = value;
        }

        // ======= Navegación / acciones =======
        [RelayCommand]
        private void SelectSuggestion(BookPick? s)
        {
            if (s is null) return;

            foreach (var it in Suggestions) it.IsSelected = false;
            s.IsSelected = true;
            SelectedSuggestion = s;

            // Copiar datos al formulario
            Title     = s.Title;
            Author    = s.Author;
            Isbn      = s.ISBN;
            Category  = s.Category;
            Year      = s.Year;
            Stock     = 1;
            Rating    = s.Rating;
            Available = s.Available;

            CoverPath = s.CoverPath;
            CoverUrl  = s.CoverPath;

            ConditionKey    = "new";
            PublicationDate ??= Year > 0 ? new DateTime(Year, 1, 1) : null;

            StepIndex = 1;
            OnPropertyChanged(nameof(CanSave));
        }

        [RelayCommand]
        private void OpenDistribution() => StepIndex = 2;

        [RelayCommand]
        private void Back()
        {
            if (StepIndex > 0) StepIndex--;
        }

        [RelayCommand]
        private void CreateAuthor()
        {
            if (string.IsNullOrWhiteSpace(Author))
                Author = "Nuevo autor";
            else
                Author = Author.Trim();

            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(CanSave));
        }

        // ====== Mock de sugerencias ======
        private static IEnumerable<BookPick> Demo() => new[]
        {
            new BookPick("El Quijote","Miguel de Cervantes","9788420471839","novel","Novel",1605,4.8,true,null,48,"Clásico de la literatura española."),
            new BookPick("Cien años de soledad","G. García Márquez","9780307474728","novel","Novel",1967,4.7,true,null,53,"Realismo mágico en Macondo."),
            new BookPick("Sapiens","Yuval Noah Harari","9780062316110","science","Science",2011,4.6,true,null,45,"Breve historia de la humanidad."),
            new BookPick("Clean Code","Robert C. Martin","9780132350884","technology","Technology",2008,4.5,true,null,40,"Buenas prácticas de código."),
            new BookPick("Harry Potter y la piedra filosofal","J.K. Rowling","9788478884452","fantasy","Fantasy",1997,4.7,true,null,32,"Inicio de la saga de Hogwarts."),
            new BookPick("Educated","Tara Westover","9780399590504","history","History",2018,4.4,false,null,29,"Memorias de superación."),
        };
    }

    // DTO para tarjetas
    public partial class BookPick : ObservableObject
    {
        public string Title { get; }
        public string Author { get; }
        public string ISBN { get; }
        public string CategoryKey { get; }
        public string Category { get; }
        public int Year { get; }
        public double Rating { get; }
        public bool Available { get; }
        public string? CoverPath { get; }
        public int Score { get; }
        public string Summary { get; }

        [ObservableProperty] private bool isSelected;

        public BookPick(string title, string author, string isbn,
                        string categoryKey, string category, int year,
                        double rating, bool available, string? coverPath,
                        int score, string summary)
        {
            Title = title; Author = author; ISBN = isbn;
            CategoryKey = categoryKey; Category = category; Year = year;
            Rating = rating; Available = available; CoverPath = coverPath;
            Score = score; Summary = summary;
        }
    }
}
