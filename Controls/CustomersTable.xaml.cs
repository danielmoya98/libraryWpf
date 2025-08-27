using System.Windows;
using System.Windows.Controls;

namespace library.Controls
{
    public partial class CustomersTable : UserControl
    {
        public CustomersTable()
        {
            InitializeComponent();
        }

        public bool IsCardView
        {
            get => (bool)GetValue(IsCardViewProperty);
            set => SetValue(IsCardViewProperty, value);
        }

        public static readonly DependencyProperty IsCardViewProperty =
            DependencyProperty.Register(
                nameof(IsCardView),
                typeof(bool),
                typeof(CustomersTable),
                new PropertyMetadata(false));
    }
}