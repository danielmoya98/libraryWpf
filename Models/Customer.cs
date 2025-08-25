using CommunityToolkit.Mvvm.ComponentModel;

namespace library.Models;

public enum Gender { Male, Female, Other }

public partial class Customer : ObservableObject
{
    public string? PhotoPath { get; set; }        // ruta/URI de la foto (opcional)
    public string FirstName { get; set; } = "";
    public string LastName  { get; set; } = "";
    public string CI        { get; set; } = "";   // documento
    public string Email     { get; set; } = "";
    public string Phone     { get; set; } = "";
    public string Address   { get; set; } = "";
    public Gender Gender    { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}