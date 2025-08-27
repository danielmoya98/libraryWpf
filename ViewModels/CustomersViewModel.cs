using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using library.Abstractions; // <- IMPORTANTE
using library.Dialogs;
using library.Models;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;

namespace library.ViewModels;

public partial class CustomersViewModel : ObservableObject, ISearchable
{
    // Comandos EXPLÍCITOS
    public IAsyncRelayCommand AddCustomerAsyncCommand { get; }
    public IAsyncRelayCommand<Customer?> EditCustomerCommand { get; }
    public IRelayCommand<Customer?> DeleteCustomerCommand { get; }

    public bool IsCardView { get; set; }
    
    public ObservableCollection<DashStat> Stats { get; }
    public ObservableCollection<Customer> Customers { get; }

    public ICollectionView CustomersView { get; }

    private string _search = string.Empty;

    public CustomersViewModel()
    {
        AddCustomerAsyncCommand = new AsyncRelayCommand(AddCustomerAsync);
        EditCustomerCommand = new AsyncRelayCommand<Customer?>(EditCustomerAsync);
        DeleteCustomerCommand = new AsyncRelayCommand<Customer?>(DeleteCustomerAsync);

        Customers = new ObservableCollection<Customer>
        {
            new Customer
            {
                FirstName = "Juan", LastName = "Quispe", CI = "LP-12.345.678", Email = "juan.quispe@mail.com",
                Phone = "+591 712-345678", Address = "Av. Mariscal Santa Cruz 120, La Paz", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "María", LastName = "Mamani", CI = "SCZ-23.456.789", Email = "maria.mamani@mail.com",
                Phone = "+591 725-998877", Address = "Av. Cristo Redentor 3450, Santa Cruz de la Sierra",
                Gender = Gender.Female, PhotoPath = null
            },
            new Customer
            {
                FirstName = "José", LastName = "Choque", CI = "CBBA-34.567.890", Email = "jose.choque@mail.com",
                Phone = "+591 739-112233", Address = "Av. Blanco Galindo 550, Cochabamba", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Ana", LastName = "Flores", CI = "EA-45.678.901", Email = "ana.flores@mail.com",
                Phone = "+591 764-556677", Address = "Av. 6 de Marzo 1001, El Alto", Gender = Gender.Female,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Carlos", LastName = "Rojas", CI = "SRE-56.789.012", Email = "carlos.rojas@mail.com",
                Phone = "+591 711-889900", Address = "Calle España 210, Sucre", Gender = Gender.Male, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Gabriela", LastName = "Gutierrez", CI = "TJ-67.890.123",
                Email = "gabriela.gutierrez@mail.com", Phone = "+591 721-334455", Address = "Av. La Madrid 420, Tarija",
                Gender = Gender.Female, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Fernando", LastName = "Vargas", CI = "OR-78.901.234", Email = "fernando.vargas@mail.com",
                Phone = "+591 770-246810", Address = "Av. 6 de Octubre 50, Oruro", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Daniela", LastName = "Alvarez", CI = "PT-89.012.345", Email = "daniela.alvarez@mail.com",
                Phone = "+591 753-667788", Address = "Calle Bolivar 320, Potosí", Gender = Gender.Female,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Rodrigo", LastName = "Fernandez", CI = "BEN-90.123.456",
                Email = "rodrigo.fernandez@mail.com", Phone = "+591 776-908070", Address = "Av. Bolívar 600, Trinidad",
                Gender = Gender.Male, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Andrea", LastName = "Aguilar", CI = "PAN-01.234.567", Email = "andrea.aguilar@mail.com",
                Phone = "+591 715-443322", Address = "Av. 9 de Febrero 45, Cobija", Gender = Gender.Female,
                PhotoPath = null
            },

            new Customer
            {
                FirstName = "Miguel", LastName = "Arce", CI = "LP-13.579.246", Email = "miguel.arce@mail.com",
                Phone = "+591 741-112244", Address = "Zona Sopocachi, Calle 20 N° 85, La Paz", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Paola", LastName = "Salazar", CI = "SCZ-24.680.357", Email = "paola.salazar@mail.com",
                Phone = "+591 763-221144", Address = "Av. Beni 1234, Santa Cruz de la Sierra", Gender = Gender.Female,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Jorge", LastName = "Mendoza", CI = "CBBA-35.791.468", Email = "jorge.mendoza@mail.com",
                Phone = "+591 701-778899", Address = "Av. América 890, Cochabamba", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Veronica", LastName = "Romero", CI = "EA-46.802.579", Email = "veronica.romero@mail.com",
                Phone = "+591 717-909090", Address = "Villa Adela, Calle 3 N° 120, El Alto", Gender = Gender.Female,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Diego", LastName = "Gonzales", CI = "SRE-57.913.680", Email = "diego.gonzales@mail.com",
                Phone = "+591 744-665544", Address = "Av. Hernando Siles 77, Sucre", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Patricia", LastName = "Perez", CI = "TJ-68.024.791", Email = "patricia.perez@mail.com",
                Phone = "+591 723-556677", Address = "Barrio San Roque, Tarija", Gender = Gender.Female,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Alvaro", LastName = "Cruz", CI = "OR-79.135.802", Email = "alvaro.cruz@mail.com",
                Phone = "+591 735-223344", Address = "Calle Adolfo Mier 130, Oruro", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Carla", LastName = "Cardenas", CI = "PT-80.246.913", Email = "carla.cardenas@mail.com",
                Phone = "+591 746-889900", Address = "Av. Colón 410, Potosí", Gender = Gender.Female, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Sergio", LastName = "Paredes", CI = "BEN-91.357.024", Email = "sergio.paredes@mail.com",
                Phone = "+591 730-111222", Address = "Zona Las Palmas, Trinidad", Gender = Gender.Male, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Monica", LastName = "Camacho", CI = "PAN-02.468.135", Email = "monica.camacho@mail.com",
                Phone = "+591 769-314159", Address = "Av. Pando 25, Cobija", Gender = Gender.Female, PhotoPath = null
            },

            new Customer
            {
                FirstName = "Hugo", LastName = "Vaca", CI = "SCZ-26.813.579", Email = "hugo.vaca@mail.com",
                Phone = "+591 762-808080", Address = "Av. Banzer, Km 8, Warnes", Gender = Gender.Male, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Alejandra", LastName = "Nava", CI = "CBBA-37.924.680", Email = "alejandra.nava@mail.com",
                Phone = "+591 716-224466", Address = "Plaza Bolívar, Quillacollo", Gender = Gender.Female,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Marco", LastName = "Arteaga", CI = "LP-14.035.791", Email = "marco.arteaga@mail.com",
                Phone = "+591 754-667788", Address = "Av. 6 de Agosto 300, Viacha", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Sofia", LastName = "Velasco", CI = "SRE-58.146.802", Email = "sofia.velasco@mail.com",
                Phone = "+591 709-556644", Address = "Barrio Central, Sucre", Gender = Gender.Female, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Cristian", LastName = "Rivero", CI = "OR-70.257.913", Email = "cristian.rivero@mail.com",
                Phone = "+591 741-909876", Address = "Zona Este, Oruro", Gender = Gender.Male, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Valeria", LastName = "Mercado", CI = "TJ-69.368.024", Email = "valeria.mercado@mail.com",
                Phone = "+591 731-222333", Address = "Av. Libertadores 145, Yacuiba", Gender = Gender.Female,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Oscar", LastName = "Salinas", CI = "PT-81.479.135", Email = "oscar.salinas@mail.com",
                Phone = "+591 777-640320", Address = "Av. Potosí 50, Uyuni", Gender = Gender.Male, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Ivana", LastName = "Rocha", CI = "BEN-92.580.246", Email = "ivana.rocha@mail.com",
                Phone = "+591 706-888777", Address = "Av. Beni 780, Riberalta", Gender = Gender.Female, PhotoPath = null
            },
            new Customer
            {
                FirstName = "Edwin", LastName = "Torrico", CI = "SCZ-27.691.358", Email = "edwin.torrico@mail.com",
                Phone = "+591 713-505050", Address = "Av. Circunvalación, Montero", Gender = Gender.Male,
                PhotoPath = null
            },
            new Customer
            {
                FirstName = "Luz", LastName = "Quiroga", CI = "CBBA-38.702.469", Email = "luz.quiroga@mail.com",
                Phone = "+591 756-112233", Address = "Av. Villazón 900, Sacaba", Gender = Gender.Female,
                PhotoPath = null
            },
        };


        Stats = new ObservableCollection<DashStat>
        {
            new DashStat("Clientes Totales", 12873, 120, PackIconMaterialKind.AccountGroup),
            new DashStat("Clientes Nuevos", 342, 18, PackIconMaterialKind.AccountPlus),
            new DashStat("Clientes Activos", 97, -5, PackIconMaterialKind.AccountCheck),
        };

        CustomersView = CollectionViewSource.GetDefaultView(Customers);
        CustomersView.Filter = FilterCustomer;
    }

    private bool FilterCustomer(object obj)
    {
        if (string.IsNullOrWhiteSpace(_search)) return true;
        if (obj is not Customer c) return false;

        var t = _search.Trim();

        return Contains(c.FirstName, t)
               || Contains(c.LastName, t)
               || Contains(c.Email, t)
               || Contains(c.CI, t)
               || Contains(c.Phone, t)
               || Contains(c.Address, t)
               || Fold(c.Gender.ToString()).Contains(Fold(t), StringComparison.OrdinalIgnoreCase);
    }

    // Comparación insensible a mayúsculas y TILDES
    private static bool Contains(string? source, string term)
        => !string.IsNullOrEmpty(source)
           && Fold(source).Contains(Fold(term), StringComparison.OrdinalIgnoreCase);

    private static string Fold(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        // Quita diacríticos (tildes) para comparar "Andres" == "Andrés"
        var norm = value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(norm.Length);
        foreach (var ch in norm)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (cat != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    // ---------- ISearchable ----------
    public void SetSearch(string? text)
    {
        _search = text ?? string.Empty;
        CustomersView.Refresh();
    }

    // ---------- Crear ----------
    private async Task AddCustomerAsync()
    {
        var vm = new AddCustomerDialogViewModel();
        var view = new AddCustomerDialog { DataContext = vm };

        var result = await DialogHost.Show(view, "RootDialog");
        if (result is AddCustomerDialogViewModel m)
        {
            Customers.Add(new Customer
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                CI = m.Ci,
                Email = m.Email,
                Phone = m.Phone,
                Address = m.Address,
                Gender = m.Gender,
                PhotoPath = m.PhotoPath
            });
        }
    }

    // ---------- Editar ----------
    private async Task EditCustomerAsync(Customer? c)
    {
        if (c is null) return;

        var vm = EditCustomerDialogViewModel.FromCustomer(c);
        var view = new EditCustomerDialog { DataContext = vm };

        var result = await DialogHost.Show(view, "RootDialog");
        if (result is EditCustomerDialogViewModel m)
        {
            c.FirstName = m.FirstName;
            c.LastName = m.LastName;
            c.CI = m.Ci;
            c.Email = m.Email;
            c.Phone = m.Phone;
            c.Address = m.Address;
            c.Gender = m.Gender;
            c.PhotoPath = m.PhotoPath;

            CollectionViewSource.GetDefaultView(Customers).Refresh();
        }
    }

    // ---------- Eliminar ----------
    private async Task DeleteCustomerAsync(Customer? c)
    {
        if (c is null) return;

        var view = new ConfirmDeleteCustomerDialog { DataContext = c };
        var result = await DialogHost.Show(view, "RootDialog");
        if (result is bool ok && ok)
        {
            Customers.Remove(c);
            CollectionViewSource.GetDefaultView(Customers).Refresh();
        }
    }
}