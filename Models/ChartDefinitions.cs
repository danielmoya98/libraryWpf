using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace library.Charts;

// Base
public abstract class ChartDef
{
    public string Title { get; init; } = "";
}

// Cartesian (barras/líneas/dispersion)
public sealed class CartesianChartDef : ChartDef
{
    public ISeries[] Series { get; init; } = [];
    public Axis[] XAxes { get; init; } = [];
    public Axis[] YAxes { get; init; } = [];
}

// Pie/Donut
public sealed class PieChartDef : ChartDef
{
    public ISeries[] Series { get; init; } = [];
}