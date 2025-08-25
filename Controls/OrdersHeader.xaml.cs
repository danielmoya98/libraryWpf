using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace library.Controls
{
    public partial class OrdersHeader : UserControl
    {
        public OrdersHeader() => InitializeComponent();

        // SearchText
        public string? SearchText
        {
            get => (string?)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(OrdersHeader), new PropertyMetadata(default(string)));

        // Status (all|Pending|Active|Returned|Overdue)
        public string? Status
        {
            get => (string?)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(nameof(Status), typeof(string), typeof(OrdersHeader), new PropertyMetadata("all"));

        // DateFrom
        public DateTime? DateFrom
        {
            get => (DateTime?)GetValue(DateFromProperty);
            set => SetValue(DateFromProperty, value);
        }
        public static readonly DependencyProperty DateFromProperty =
            DependencyProperty.Register(nameof(DateFrom), typeof(DateTime?), typeof(OrdersHeader), new PropertyMetadata(null));

        // DateTo
        public DateTime? DateTo
        {
            get => (DateTime?)GetValue(DateToProperty);
            set => SetValue(DateToProperty, value);
        }
        public static readonly DependencyProperty DateToProperty =
            DependencyProperty.Register(nameof(DateTo), typeof(DateTime?), typeof(OrdersHeader), new PropertyMetadata(null));

        // Commands
        public ICommand? AddReservationCommand
        {
            get => (ICommand?)GetValue(AddReservationCommandProperty);
            set => SetValue(AddReservationCommandProperty, value);
        }
        public static readonly DependencyProperty AddReservationCommandProperty =
            DependencyProperty.Register(nameof(AddReservationCommand), typeof(ICommand), typeof(OrdersHeader), new PropertyMetadata(null));

        public ICommand? GenerateReportCommand
        {
            get => (ICommand?)GetValue(GenerateReportCommandProperty);
            set => SetValue(GenerateReportCommandProperty, value);
        }
        public static readonly DependencyProperty GenerateReportCommandProperty =
            DependencyProperty.Register(nameof(GenerateReportCommand), typeof(ICommand), typeof(OrdersHeader), new PropertyMetadata(null));
    }
}
