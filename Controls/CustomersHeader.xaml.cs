using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace library.Controls
{
    public partial class CustomersHeader : UserControl
    {
        public CustomersHeader() => InitializeComponent();

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(CustomersHeader), new PropertyMetadata("Clientes"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(CustomersHeader), new PropertyMetadata(null));

        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        public static readonly DependencyProperty ReportCommandProperty =
            DependencyProperty.Register(nameof(ReportCommand), typeof(ICommand), typeof(CustomersHeader), new PropertyMetadata(null));

        public ICommand ReportCommand
        {
            get => (ICommand)GetValue(ReportCommandProperty);
            set => SetValue(ReportCommandProperty, value);
        }

        public static readonly DependencyProperty IsCardViewProperty =
            DependencyProperty.Register(nameof(IsCardView), typeof(bool), typeof(CustomersHeader), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsCardView
        {
            get => (bool)GetValue(IsCardViewProperty);
            set => SetValue(IsCardViewProperty, value);
        }
    }
}