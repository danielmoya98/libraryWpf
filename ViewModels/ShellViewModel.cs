using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using library;
using library.Abstractions;
using library.Models;
using library.ViewModels;
using MahApps.Metro.IconPacks;

public partial class ShellViewModel : ObservableObject
{
    [ObservableProperty] private object? currentViewModel;
    [ObservableProperty] private bool isSidebarExpanded = true;
    [ObservableProperty] private string? globalSearch;

    [ObservableProperty] private bool isDarkTheme;
    
    // Instancias únicas (inyectables si usas DI)
    
    public DashboardViewModel Dashboard { get; } = new();
    public CustomersViewModel Customers { get; } = new();
    public BooksViewModel     Books     { get; } = new();
    public MembersViewModel   Members   { get; } = new();
    public OrdersViewModel    Orders    { get; } = new();
    public SettingsViewModel  Settings  { get; } = new();

    
    partial void OnGlobalSearchChanged(string? value)
        => (CurrentViewModel as ISearchable)?.SetSearch(value);
    
    partial void OnCurrentViewModelChanged(object? value)
        => (value as ISearchable)?.SetSearch(GlobalSearch);
    public ObservableCollection<NavigationItem> NavItems { get; } = new();

    public ShellViewModel()
    {
        // Menú
        NavItems.Add(new NavigationItem("Expandir / Ocultar", PackIconMaterialKind.ChevronLeft, NavSection.Primary, isToggle: true));
        NavItems.Add(new NavigationItem("Dashboard", PackIconMaterialKind.ViewDashboardOutline, NavSection.Primary, active: true, targetVm: Dashboard));
        NavItems.Add(new NavigationItem("Clientes",  PackIconMaterialKind.AccountGroupOutline,  NavSection.Primary, targetVm: Customers));
        NavItems.Add(new NavigationItem("Libros",    PackIconMaterialKind.BookMultipleOutline,  NavSection.Primary, targetVm: Books));
        NavItems.Add(new NavigationItem("Reservas",  PackIconMaterialKind.CalendarMonthOutline, NavSection.Primary, targetVm: Orders));
        NavItems.Add(new NavigationItem("Ajustes",   PackIconMaterialKind.CogOutline,           NavSection.Secondary, targetVm: Settings));
        NavItems.Add(new NavigationItem("Cerrar sesión", PackIconMaterialKind.LogoutVariant,    NavSection.Secondary));

        CurrentViewModel = Dashboard; // vista inicial
    }

    [RelayCommand] public void ToggleSidebar() => IsSidebarExpanded = !IsSidebarExpanded;

    // para compatibilidad si tu SideBar llama SelectCommand
    [RelayCommand] public void Select(NavigationItem item) => ExecuteNav(item);

    [RelayCommand]
    private void ExecuteNav(NavigationItem item)
    {
        if (item.IsToggle) { ToggleSidebar(); return; }

        foreach (var it in NavItems) it.IsActive = false;
        item.IsActive = true;

        if (item.TargetViewModel is not null)
        {
            CurrentViewModel = item.TargetViewModel;
            return;
        }

        // Acciones especiales (logout, etc.)
        if (item.Title.Contains("cerrar", StringComparison.OrdinalIgnoreCase))
        {
            // TODO: confirmar y cerrar sesión
            return;
        }
    }

    // Navegación directa desde botones internos
    [RelayCommand] public void Navigate(object vm) => CurrentViewModel = vm;
    

}
