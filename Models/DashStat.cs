using MahApps.Metro.IconPacks;

namespace library.Models;

public sealed class DashStat
{
    public string Title { get; }
    public int Value { get; }
    public int ThisMonth { get; }
    public PackIconMaterialKind Icon { get; }

    public DashStat(string title, int value, int thisMonth, PackIconMaterialKind icon)
    {
        Title = title;
        Value = value;
        ThisMonth = thisMonth;
        Icon = icon;
    }
}