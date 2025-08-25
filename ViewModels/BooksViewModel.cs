using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using library.Abstractions;
using library.Controls;
using library.Dialogs;
using library.Dialogs.AddBook;
using library.Models;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;

namespace library.ViewModels;

public partial class BooksViewModel : ObservableObject, ISearchable
{
    public ObservableCollection<Book> Books { get; } = new();

    public ICollectionView BooksView { get; }

    // Comandos (puedes cambiar a diálogos como en Clientes cuando quieras)
    public IAsyncRelayCommand AddBookAsyncCommand    { get; }
    public IAsyncRelayCommand<Book?> EditBookCommand { get; }
    public IAsyncRelayCommand<Book?> DeleteBookCommand { get; }
    
    
    
    [ObservableProperty] private AvailabilityFilter availability = AvailabilityFilter.All;
    [ObservableProperty] private BookCategory? selectedGenre = null;
    [ObservableProperty] private int? minRating = null;

    private string _search = string.Empty;

    public ObservableCollection<DashStat> Stats { get; }
    public BooksViewModel()
    {
        // Demo data
        Books.Add(new Book { Title="El Quijote", Author="Miguel de Cervantes", ISBN="9788420471839", Year=1605, Category=BookCategory.Novel, Stock=6 });
        Books.Add(new Book { Title="Cien años de soledad", Author="G. García Márquez", ISBN="9780307474728", Year=1967, Category=BookCategory.Novel, Stock=4 });
        Books.Add(new Book { Title="Sapiens", Author="Yuval Noah Harari", ISBN="9780062316110", Year=2011, Category=BookCategory.History, Stock=5 });
        Books.Add(new Book { Title="Clean Code", Author="Robert C. Martin", ISBN="9780132350884", Year=2008, Category=BookCategory.Technology, Stock=3 });
        Books.Add(new Book { Title="Harry Potter y la piedra filosofal", Author="J.K. Rowling", ISBN="9788478884452", Year=1997, Category=BookCategory.Fantasy, Stock=8 });

        BooksView = CollectionViewSource.GetDefaultView(Books);
        BooksView.Filter = Filter;

        AddBookAsyncCommand    = new AsyncRelayCommand(AddBookAsync);
        EditBookCommand        = new AsyncRelayCommand<Book?>(EditBookAsync);
        DeleteBookCommand      = new AsyncRelayCommand<Book?>(DeleteBookAsync);
        
        Stats = new ObservableCollection<DashStat>
        {
            new DashStat("Libros Totales",     4210,  35, PackIconMaterialKind.BookMultipleOutline),
            new DashStat("Libros Prestados",    186,  12, PackIconMaterialKind.BookOpenVariant),
            new DashStat("Libros Disponibles", 4024, -10, PackIconMaterialKind.BookCheckOutline),
        };

    }

    public IRelayCommand GenerateBooksReportCommand => new RelayCommand(() =>
    {
        // TODO: generar reporte
    });
    
    private bool Filter(object obj)
    {
        if (string.IsNullOrWhiteSpace(_search)) return true;
        if (obj is not Book b) return false;

        var t = _search.Trim();
        return Contains(b.Title, t) ||
               Contains(b.Author, t) ||
               Contains(b.ISBN, t) ||
               (b.Publisher?.Contains(t, StringComparison.OrdinalIgnoreCase) ?? false) ||
               b.Year.ToString().Contains(t, StringComparison.OrdinalIgnoreCase) ||
               b.Category.ToString().Contains(t, StringComparison.OrdinalIgnoreCase);
    }

    private static bool Contains(string? s, string term)
        => !string.IsNullOrEmpty(s) && s.Contains(term, StringComparison.OrdinalIgnoreCase);

    // ISearchable
    public void SetSearch(string? text)
    {
        _search = text ?? string.Empty;
        BooksView.Refresh();
    }

    // Puedes reemplazar estos MessageBox por diálogos Material más adelante
    [RelayCommand]
    private async Task AddBookAsync()
    {
        var vm   = new AddBookDialogViewModel();
        var view = new AddBookDialog { DataContext = vm };

        var result = await DialogHost.Show(view, "RootDialog");
        if (result is AddBookDialogViewModel m)
        {
            // Mapea a tu modelo Book (ajusta nombres si difieren)
            Books.Add(new Book
            {
                Title     = m.Title ?? "",
                Author    = m.Author ?? "",
                ISBN      = m.Isbn ?? "",
                Year      = m.Year,
                Stock     = Math.Max(1, m.Stock),
                CoverPath = m.CoverPath
            });
        }
    }

    private async Task EditBookAsync(Book? b)
    {
        if (b is null) return;

        var vm   = EditBookDialogViewModel.FromBook(b);
        var view = new EditBookDialog { DataContext = vm };

        var result = await DialogHost.Show(view, "RootDialog");
        switch (result)
        {
            // Guardar: el diálogo devuelve su VM
            case EditBookDialogViewModel m:
            {
                b.Title      = m.Title ?? b.Title;
                b.Author     = m.Author ?? b.Author;
                b.ISBN       = m.Isbn ?? b.ISBN;
                b.Year       = m.Year;
                b.Stock      = m.Stock;
                b.CoverPath  = m.CoverPath ?? b.CoverPath;

                // opcionales si existen en tu modelo
                b.Publisher    = m.Publisher ?? b.Publisher;
                CollectionViewSource.GetDefaultView(Books).Refresh();
                break;
            }

            // Eliminar: el botón devuelve el token "__DELETE__"
            case string s when s == "__DELETE__":
            {
                Books.Remove(b);
                CollectionViewSource.GetDefaultView(Books).Refresh();
                break;
            }
        }
    }

    private async Task DeleteBookAsync(Book? b)
    {
        if (b is null) return;

        var view = new library.Dialogs.ConfirmDeleteBookDialog
        {
            DataContext = b   // el diálogo bindea directamente al Book
        };

        var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
        if (result is bool ok && ok)
        {
            Books.Remove(b);
            CollectionViewSource.GetDefaultView(Books).Refresh();
        }
    }
}
