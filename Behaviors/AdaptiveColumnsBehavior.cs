using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace library.Behaviors
{
    /// <summary>
    /// Oculta/muestra columnas de un DataGrid según breakpoints.
    /// - Asigna una clave a cada columna con: beh:AdaptiveColumnsBehavior.Key="Email"
    /// - Define reglas en el DataGrid:
    ///   beh:AdaptiveColumnsBehavior.Breakpoints="1180:Address;1080:Phone;980:Email;900:Gender;840:CI;780:LastName"
    /// </summary>
    public static class AdaptiveColumnsBehavior
    {
        // ======== Breakpoints (en el DataGrid) ========
        public static string? GetBreakpoints(DependencyObject obj)
            => (string?)obj.GetValue(BreakpointsProperty);

        public static void SetBreakpoints(DependencyObject obj, string? value)
            => obj.SetValue(BreakpointsProperty, value);

        public static readonly DependencyProperty BreakpointsProperty =
            DependencyProperty.RegisterAttached(
                "Breakpoints",
                typeof(string),
                typeof(AdaptiveColumnsBehavior),
                new PropertyMetadata(null, OnBreakpointsChanged));

        private static void OnBreakpointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DataGrid dg) return;

            dg.Loaded -= DgOnLoaded;
            dg.SizeChanged -= DgOnSizeChanged;

            if (e.NewValue is string)
            {
                dg.Loaded += DgOnLoaded;
                dg.SizeChanged += DgOnSizeChanged;
            }
        }

        private static void DgOnLoaded(object? sender, RoutedEventArgs e) => Apply(sender as DataGrid);
        private static void DgOnSizeChanged(object? sender, SizeChangedEventArgs e) => Apply(sender as DataGrid);

        // ======== Clave (en cada columna) ========
        public static string? GetKey(DependencyObject obj)
            => (string?)obj.GetValue(KeyProperty);

        public static void SetKey(DependencyObject obj, string? value)
            => obj.SetValue(KeyProperty, value);

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.RegisterAttached(
                "Key",
                typeof(string),
                typeof(AdaptiveColumnsBehavior),
                new PropertyMetadata(null));

        // ======== Lógica ========
        private sealed class Rule
        {
            public double Threshold { get; set; }
            public string[] Keys { get; set; } = Array.Empty<string>();
        }

        private static void Apply(DataGrid? dg)
        {
            if (dg == null) return;

            var spec = GetBreakpoints(dg);
            if (string.IsNullOrWhiteSpace(spec)) return;

            var rules = ParseRules(spec);
            if (rules.Count == 0) return;

            // Todo visible por defecto
            foreach (var c in dg.Columns)
                c.Visibility = Visibility.Visible;

            var actualWidth = dg.ActualWidth;

            foreach (var rule in rules)
            {
                if (actualWidth < rule.Threshold)
                {
                    foreach (var key in rule.Keys)
                    {
                        var col = dg.Columns.FirstOrDefault(c =>
                            string.Equals(GetKey(c) ?? c.Header?.ToString(), key, StringComparison.OrdinalIgnoreCase));

                        if (col != null) col.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private static List<Rule> ParseRules(string spec)
        {
            var list = new List<Rule>();
            var parts = spec.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var kv = part.Split(':');
                if (kv.Length != 2) continue;

                if (double.TryParse(kv[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var threshold))
                {
                    var keys = kv[1].Split(',')
                                    .Select(s => s.Trim())
                                    .Where(s => s.Length > 0)
                                    .ToArray();
                    if (keys.Length > 0)
                        list.Add(new Rule { Threshold = threshold, Keys = keys });
                }
            }

            // Ordenar por ancho ascendente
            list.Sort((a, b) => a.Threshold.CompareTo(b.Threshold));
            return list;
        }
    }
}
