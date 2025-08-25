using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.IconPacks;

namespace library.Models;

public enum NavSection { Primary, Secondary }

public partial class NavigationItem : ObservableObject
{
    public NavigationItem(
        string title,
        PackIconMaterialKind icon,
        NavSection section = NavSection.Primary,
        bool active = false,
        bool isToggle = false,
        object? targetVm = null)
    {
        Title = title;
        Icon = icon;
        Section = section;
        IsActive = active;
        IsToggle = isToggle;
        TargetViewModel = targetVm;
    }

    public string Title { get; }
    public PackIconMaterialKind Icon { get; }
    public NavSection Section { get; }

    [ObservableProperty] private bool isActive;
    public bool IsToggle { get; }

    /// VM destino para navegación (puede ser nulo para acciones especiales como “Cerrar sesión”)
    public object? TargetViewModel { get; }
}