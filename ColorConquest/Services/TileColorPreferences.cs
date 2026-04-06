namespace ColorConquest.Services;

public sealed record TileColorOption(string Key, string Name, string Hex);

public static class TileColorPreferences
{
    private const string PrimaryColorKey = "tile_primary_color";
    private const string SecondaryColorKey = "tile_secondary_color";
    private const string DefaultPrimaryKey = "dark-indigo";
    private const string DefaultSecondaryKey = "light-gold";

    private static readonly IReadOnlyList<TileColorOption> Colors = new List<TileColorOption>
    {
        // Dark colors
        new("dark-navy", "Dark - Navy", "#1E3A8A"),
        new("dark-indigo", "Dark - Indigo", "#4338CA"),
        new("dark-purple", "Dark - Purple", "#6D28D9"),
        new("dark-teal", "Dark - Teal", "#0F766E"),
        new("dark-emerald", "Dark - Emerald", "#047857"),
        new("dark-rose", "Dark - Rose", "#BE123C"),
        // Light colors
        new("light-sky", "Light - Sky", "#93C5FD"),
        new("light-lavender", "Light - Lavender", "#C4B5FD"),
        new("light-mint", "Light - Mint", "#99F6E4"),
        new("light-peach", "Light - Peach", "#FDBA74"),
        new("light-gold", "Light - Gold", "#FDE68A"),
        new("light-pink", "Light - Pink", "#FDA4AF")
    };

    public static event EventHandler? ColorsChanged;

    public static IReadOnlyList<TileColorOption> GetAvailableColors() => Colors;

    public static string GetPrimaryColorKey()
    {
        var saved = Preferences.Default.Get(PrimaryColorKey, DefaultPrimaryKey);
        return Colors.Any(c => c.Key == saved) ? saved : DefaultPrimaryKey;
    }

    public static string GetSecondaryColorKey()
    {
        var saved = Preferences.Default.Get(SecondaryColorKey, DefaultSecondaryKey);
        return Colors.Any(c => c.Key == saved) ? saved : DefaultSecondaryKey;
    }

    public static TileColorOption GetPrimaryColor() => GetByKey(GetPrimaryColorKey());
    public static TileColorOption GetSecondaryColor() => GetByKey(GetSecondaryColorKey());

    public static void SetPrimaryColorKey(string key)
    {
        var normalized = Colors.Any(c => c.Key == key) ? key : DefaultPrimaryKey;
        Preferences.Default.Set(PrimaryColorKey, normalized);
        ColorsChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void SetSecondaryColorKey(string key)
    {
        var normalized = Colors.Any(c => c.Key == key) ? key : DefaultSecondaryKey;
        Preferences.Default.Set(SecondaryColorKey, normalized);
        ColorsChanged?.Invoke(null, EventArgs.Empty);
    }

    private static TileColorOption GetByKey(string key) => Colors.First(c => c.Key == key);
}
