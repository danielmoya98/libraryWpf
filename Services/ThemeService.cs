using System;
using System.Linq;
using System.Windows;

namespace library.Services
{
    public static class ThemeService
    {
        static readonly Uri LightUri = new Uri("pack://application:,,,/library;component/Resources/Themes/Theme.Light.xaml", UriKind.Absolute);
        static readonly Uri DarkUri  = new Uri("pack://application:,,,/library;component/Resources/Themes/Theme.Dark.xaml",  UriKind.Absolute);

        public static bool IsDark { get; private set; }

        public static void Apply(bool useDark)
        {
            var app = Application.Current;
            if (app is null) return;

            var merged = app.Resources.MergedDictionaries;

            // Quita cualquier diccionario Theme.* previo
            for (int i = merged.Count - 1; i >= 0; i--)
            {
                var src = merged[i].Source;
                if (src != null && src.OriginalString.Contains("Resources/Themes/Theme.", StringComparison.OrdinalIgnoreCase))
                    merged.RemoveAt(i);
            }

            // Inserta el nuevo tema **después** de Palette.xaml y **antes** de tus Styles
            // Lo más fácil: añadirlo en la posición 1 (asumiendo que Palette.xaml está en 0)
            var themeDict = new ResourceDictionary { Source = useDark ? DarkUri : LightUri };
            int insertIndex = Math.Min(1, merged.Count);
            merged.Insert(insertIndex, themeDict);

            IsDark = useDark;
        }

        public static void Toggle() => Apply(!IsDark);
    }
}