namespace ColorConquest.Core.Services;

public interface IPreferences
{
    string? Get(string key, string? defaultValue = null);
    void Set(string key, string value);
    void Remove(string key);
    void Clear();
}

public class InMemoryPreferences : IPreferences
{
    private readonly Dictionary<string, string> _store = new();
    public string? Get(string key, string? defaultValue = null) => _store.TryGetValue(key, out var v) ? v : defaultValue;
    public void Set(string key, string value) => _store[key] = value;
    public void Remove(string key) => _store.Remove(key);
    public void Clear() => _store.Clear();
}
