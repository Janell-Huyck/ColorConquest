namespace ColorConquest.Core.Services;

using Microsoft.Maui.Graphics;

public sealed record TileColorOption(string Key, string Name, string Hex)
{
    public Color ColorValue => Color.FromArgb(Hex);
}

public class TileColorPreferences
{
    private const string PrimaryColorKey = "tile_primary_color";
    private const string SecondaryColorKey = "tile_secondary_color";
    public event EventHandler? ColorsChanged;
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

    private readonly IPreferences _preferences;

    public TileColorPreferences(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public IReadOnlyList<TileColorOption> GetAvailableColors() => Colors;

    public string GetPrimaryColorKey()
    {
        var saved = _preferences.Get(PrimaryColorKey, Colors[0].Key);
        return Colors.Any(c => c.Key == saved) ? saved : Colors[0].Key;
    }
    public string GetSecondaryColorKey()
    {
        var saved = _preferences.Get(SecondaryColorKey, Colors[1].Key);
        return Colors.Any(c => c.Key == saved) ? saved : Colors[1].Key;
    }

    public void SetPrimaryColorKey(string key)
    {
        var normalized = Colors.Any(c => c.Key == key) ? key : Colors[0].Key;
        _preferences.Set(PrimaryColorKey, normalized);
        ColorsChanged?.Invoke(this, EventArgs.Empty);
    }
    public void SetSecondaryColorKey(string key)
    {
        var normalized = Colors.Any(c => c.Key == key) ? key : Colors[1].Key;
        _preferences.Set(SecondaryColorKey, normalized);
        ColorsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Reset()
    {
        _preferences.Set(PrimaryColorKey, Colors[0].Key); // blue
        _preferences.Set(SecondaryColorKey, Colors[1].Key); // red
        ColorsChanged?.Invoke(this, EventArgs.Empty);
    }
    public TileColorOption GetPrimaryColor() => GetByKeySafe(GetPrimaryColorKey(), Colors[0]);
    public TileColorOption GetSecondaryColor() => GetByKeySafe(GetSecondaryColorKey(), Colors[1]);

    private static TileColorOption GetByKeySafe(string key, TileColorOption fallback)
        => Colors.FirstOrDefault(c => c.Key == key) ?? fallback;
}
