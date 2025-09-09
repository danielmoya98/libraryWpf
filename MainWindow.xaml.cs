using library.Models;
using System.Windows;

namespace library
{
    public partial class MainWindow : Window
    {
        private readonly ShellViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                _viewModel = App.GetService<ShellViewModel>();
                DataContext = _viewModel;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inicializando MainWindow: {ex.Message}");

                // Cerrar la aplicación si no se puede inicializar
                Application.Current.Shutdown();
            }
        }

        // Método público para navegación desde otros componentes
        public void NavigateToMainContent(Usuario usuario)
        {
            // Este método se puede llamar desde el LoginView o desde otros lugares
            _viewModel?.NavigateToDashboard();
        }

        // Métodos públicos para navegación
        public void NavigateToDashboard() => _viewModel?.NavigateToDashboard();
        public void NavigateToCustomers() => _viewModel?.NavigateToCustomers();
        public void NavigateToBooks() => _viewModel?.NavigateToBooks();
        public void NavigateToOrders() => _viewModel?.NavigateToOrders();
    }
}