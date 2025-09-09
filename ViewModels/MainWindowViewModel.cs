using CommunityToolkit.Mvvm.ComponentModel;
using library.Models;
using library.Services;
using library.ViewModels;
using System;

namespace library.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IUserSessionService _userSessionService;

        [ObservableProperty] private object? currentViewModel;

        [ObservableProperty] private bool isLoggedIn;

        [ObservableProperty] private Usuario? currentUser;

        [ObservableProperty] private string welcomeMessage = "Bienvenido";

        // ViewModel para el login
        [ObservableProperty] private DatabaseLoginViewModel loginViewModel;

        // ViewModels para la aplicación principal
        [ObservableProperty] private TopBarViewModel topBar = new();

        public MainWindowViewModel(IUserSessionService userSessionService, DatabaseLoginViewModel loginViewModel)
        {
            _userSessionService = userSessionService;
            LoginViewModel = loginViewModel;

            // Suscribirse a eventos del servicio de sesión
            _userSessionService.UserLoggedIn += OnUserLoggedIn;
            _userSessionService.UserLoggedOut += OnUserLoggedOut;

            // Suscribirse al evento de login exitoso del LoginViewModel
            LoginViewModel.LoginSuccess += OnLoginSuccess;

            // Inicializar estado
            UpdateAuthenticationState();
        }

        private void OnLoginSuccess(Usuario usuario)
        {
            // El LoginViewModel ya manejó el login, solo actualizamos el estado
            _userSessionService.Login(usuario);
        }

        private void OnUserLoggedIn(Usuario usuario)
        {
            UpdateAuthenticationState();

            // Navegar al dashboard por defecto
            NavigateToDashboard();
        }

        private void OnUserLoggedOut()
        {
            UpdateAuthenticationState();
        }

        private void UpdateAuthenticationState()
        {
            IsLoggedIn = _userSessionService.IsLoggedIn;
            CurrentUser = _userSessionService.CurrentUser;

            if (CurrentUser != null)
            {
                WelcomeMessage = $"Bienvenido, {CurrentUser.DisplayName}";

                // Actualizar información en el TopBar
                TopBar.UpdateUserInfo(CurrentUser);
            }
            else
            {
                WelcomeMessage = "Inicia sesión para continuar";
                CurrentViewModel = null;
            }
        }

        // Métodos de navegación
        public void NavigateToDashboard()
        {
            if (!IsLoggedIn) return;
            CurrentViewModel = App.GetService<DashboardViewModel>();
        }

        public void NavigateToCustomers()
        {
            if (!IsLoggedIn) return;
            CurrentViewModel = App.GetService<CustomersViewModel>();
        }

        public void NavigateToBooks()
        {
            if (!IsLoggedIn) return;
            CurrentViewModel = App.GetService<BooksViewModel>();
        }

        public void NavigateToOrders()
        {
            if (!IsLoggedIn) return;
            CurrentViewModel = App.GetService<OrdersViewModel>();
        }

        public void Logout()
        {
            _userSessionService.Logout();
        }

        // Cleanup
        ~MainWindowViewModel()
        {
            _userSessionService.UserLoggedIn -= OnUserLoggedIn;
            _userSessionService.UserLoggedOut -= OnUserLoggedOut;
            if (LoginViewModel != null)
            {
                LoginViewModel.LoginSuccess -= OnLoginSuccess;
            }
        }
    }
}