using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using library.Models;

namespace library.Dialogs;

public partial class EditBookDialogViewModel : ObservableObject
{
    // Campos editables
    [ObservableProperty] private string? title;
    [ObservableProperty] private string? author;              // uno o varios autores
    [ObservableProperty] private string? isbn;
    [ObservableProperty] private string? category;           // texto visible (género)
    [ObservableProperty] private string  selectedGenre = "all"; // clave del género (Combo)
    [ObservableProperty] private string? publisher;
    [ObservableProperty] private string  conditionKey = "new";   // new|good|damaged|lost
    [ObservableProperty] private int     year;
    [ObservableProperty] private int     stock = 1;
    [ObservableProperty] private double  rating;
    [ObservableProperty] private bool    available = true;
    [ObservableProperty] private string? coverPath;           // preview/archivo local
    [ObservableProperty] private string? coverUrl;            // url (sincroniza preview)
    [ObservableProperty] private string? synopsis;

    // Catálogos
    public IReadOnlyList<KeyValuePair<string, string>> GenreItems { get; } = new[]
    {
        new KeyValuePair<string,string>("all","(Sin cambio)"),
        new KeyValuePair<string,string>("novel","Novel"),
        new KeyValuePair<string,string>("history","History"),
        new KeyValuePair<string,string>("technology","Technology"),
        new KeyValuePair<string,string>("fantasy","Fantasy"),
        new KeyValuePair<string,string>("science","Science"),
    };

    public IReadOnlyList<KeyValuePair<string, string>> ConditionItems { get; } = new[]
    {
        new KeyValuePair<string,string>("new",     "Nuevo"),
        new KeyValuePair<string,string>("good",    "Bueno"),
        new KeyValuePair<string,string>("damaged", "Dañado"),
        new KeyValuePair<string,string>("lost",    "Perdido"),
    };

    // Validación mínima
    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Title) &&
        !string.IsNullOrWhiteSpace(Author) &&
        !string.IsNullOrWhiteSpace(Isbn);

    partial void OnTitleChanged(string? v)  => OnPropertyChanged(nameof(IsValid));
    partial void OnAuthorChanged(string? v) => OnPropertyChanged(nameof(IsValid));
    partial void OnIsbnChanged(string? v)   => OnPropertyChanged(nameof(IsValid));
    partial void OnCoverUrlChanged(string? v) { CoverPath = v; }

    // Acciones
    [RelayCommand]
    private void NewAuthor()
    {
        if (string.IsNullOrWhiteSpace(Author))
            Author = "Nuevo autor";
        else
            Author = Author.Trim();
        OnPropertyChanged(nameof(IsValid));
    }

    // Carga desde el modelo
    public static EditBookDialogViewModel FromBook(Book b) => new()
    {
        Title        = b.Title,
        Author       = b.Author,
        Isbn         = b.ISBN,
        Publisher    = b.Publisher, 
        Year         = b.Year,
        Stock        = b.Stock,
        CoverPath    = b.CoverPath,
        CoverUrl     = b.CoverPath
    };

    private static string GuessGenreKey(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "all";
        var t = text.Trim().ToLowerInvariant();
        return t switch
        {
            "novel"      => "novel",
            "history"    => "history",
            "technology" => "technology",
            "fantasy"    => "fantasy",
            "science"    => "science",
            _            => "all"
        };
    }
}
