namespace ColorConquest.Services;

public sealed record TileColorOption(string Key, string Name, string Hex);

public class TileColorPreferences
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

    public event EventHandler? ColorsChanged;

    private readonly ColorConquest.Core.Services.IPreferences _preferences;

    public TileColorPreferences(ColorConquest.Core.Services.IPreferences preferences)
    {
        _preferences = preferences;
    }

    public IReadOnlyList<TileColorOption> GetAvailableColors() => Colors;

    public string GetPrimaryColorKey()
    {
        var saved = _preferences.Get(PrimaryColorKey, DefaultPrimaryKey);
        return Colors.Any(c => c.Key == saved) ? saved : DefaultPrimaryKey;
    }

    public string GetSecondaryColorKey()
    {
        var saved = _preferences.Get(SecondaryColorKey, DefaultSecondaryKey);
        return Colors.Any(c => c.Key == saved) ? saved : DefaultSecondaryKey;
    }

    public TileColorOption GetPrimaryColor() => GetByKey(GetPrimaryColorKey());
    public TileColorOption GetSecondaryColor() => GetByKey(GetSecondaryColorKey());

    public void SetPrimaryColorKey(string key)
    {
        var normalized = Colors.Any(c => c.Key == key) ? key : DefaultPrimaryKey;
        _preferences.Set(PrimaryColorKey, normalized);
        ColorsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSecondaryColorKey(string key)
    {
        var normalized = Colors.Any(c => c.Key == key) ? key : DefaultSecondaryKey;
        _preferences.Set(SecondaryColorKey, normalized);
        ColorsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Reset()
    {
        _preferences.Set(PrimaryColorKey, Colors[0].Key);
        _preferences.Set(SecondaryColorKey, Colors[1].Key);
        ColorsChanged?.Invoke(this, EventArgs.Empty);
    }

    private static TileColorOption GetByKey(string key) => Colors.First(c => c.Key == key);
}
