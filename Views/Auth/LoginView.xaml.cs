using System.Windows.Controls;
using library.ViewModels;

namespace library.Views.Auth
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            try
            {
                // Asignar el ViewModel desde el contenedor de dependencias
                var viewModel = App.GetService<DatabaseLoginViewModel>();
                DataContext = viewModel;

                // Suscribirse a eventos
                viewModel.LoginSuccess += OnLoginSuccess;

                // Configurar binding para PasswordBox
                PasswordBox.PasswordChanged += (s, e) =>
                {
                    if (DataContext is DatabaseLoginViewModel vm && !vm.IsPasswordVisible)
                    {
                        vm.Password = PasswordBox.Password;
                    }
                };
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inicializando LoginView: {ex.Message}");

                // Fallback: crear sin dependencias (solo para debugging)
                // DataContext = new DatabaseLoginViewModel(null);
            }
        }

        private void OnLoginSuccess(Models.Usuario usuario)
        {
            // Aquí puedes manejar el login exitoso
            System.Diagnostics.Debug.WriteLine(
                $"Usuario logueado: {usuario.NombreCompleto} - Rol: {usuario.RolDisplay}");

            // Limpiar la contraseña por seguridad
            PasswordBox.Password = string.Empty;

            // Notificar a la ventana principal
            var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
           
        }
    }
}