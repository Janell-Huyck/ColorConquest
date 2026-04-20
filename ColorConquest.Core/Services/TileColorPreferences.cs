namespace ColorConquest.Core.Services;

public sealed record TileColorOption(string Key, string Name, string Hex);

public static class TileColorPreferences
{
    private static readonly IReadOnlyList<TileColorOption> Colors = new List<TileColorOption>
    {
        new("blue", "Blue", "#2196F3"),
        new("red", "Red", "#F44336"),
        new("green", "Green", "#4CAF50"),
        new("yellow", "Yellow", "#FFEB3B"),
        new("purple", "Purple", "#9C27B0"),
        new("orange", "Orange", "#FF9800"),
        new("teal", "Teal", "#009688"),
        new("pink", "Pink", "#E91E63")
    };

    public static IReadOnlyList<TileColorOption> GetAvailableColors() => Colors;

    public static string GetPrimaryColorKey() => "blue";
    public static string GetSecondaryColorKey() => "red";

    public static void SetPrimaryColorKey(string key) { /* TODO: Implement persistent storage */ }
    public static void SetSecondaryColorKey(string key) { /* TODO: Implement persistent storage */ }

    public static TileColorOption GetPrimaryColor() => GetByKey(GetPrimaryColorKey());
    public static TileColorOption GetSecondaryColor() => GetByKey(GetSecondaryColorKey());

    private static TileColorOption GetByKey(string key) => Colors.First(c => c.Key == key);
}
