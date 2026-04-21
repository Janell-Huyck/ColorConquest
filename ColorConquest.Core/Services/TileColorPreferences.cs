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
    protected virtual IReadOnlyList<TileColorOption> Colors { get; } = new List<TileColorOption>
    {
        new("blue", "Blue", "#1976D2"),
        new("red", "Red", "#D32F2F"),
        new("yellow", "Yellow", "#FFC107"),
        new("purple", "Purple", "#8E24AA"),
        new("orange", "Orange", "#F57C00"),
        new("lime", "Lime", "#C6FF00"),
        new("pink", "Pink", "#EC407A"),
        new("brown", "Brown", "#795548")
    };

    private readonly IPreferences _preferences;

    public TileColorPreferences(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public virtual IReadOnlyList<TileColorOption> GetAvailableColors() => Colors;

    public string GetPrimaryColorKey()
    {
        if (Colors.Count == 0)
            return string.Empty;
        var saved = _preferences.Get(PrimaryColorKey, Colors[0].Key);
        return Colors.Any(c => c.Key == saved) ? saved : Colors[0].Key;
    }
    public string GetSecondaryColorKey()
    {
        if (Colors.Count < 2)
            return Colors.Count == 1 ? Colors[0].Key : string.Empty;
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
    public TileColorOption GetPrimaryColor()
    {
        if (Colors.Count == 0)
            return new TileColorOption("", "", "#000000");
        return GetByKeySafe(GetPrimaryColorKey(), Colors[0]);
    }

    public TileColorOption GetSecondaryColor()
    {
        if (Colors.Count == 0)
            return new TileColorOption("", "", "#000000");
        if (Colors.Count == 1)
            return Colors[0];
        return GetByKeySafe(GetSecondaryColorKey(), Colors[1]);
    }

    private TileColorOption GetByKeySafe(string key, TileColorOption fallback)
        => Colors.FirstOrDefault(c => c.Key == key) ?? fallback;
}
