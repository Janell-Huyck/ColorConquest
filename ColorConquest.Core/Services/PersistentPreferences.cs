using System;
using Microsoft.Maui.Storage;

namespace ColorConquest.Core.Services
{
    public class PersistentPreferences : IPreferences
    {
        public string? Get(string key, string? defaultValue = null)
        {
            return Preferences.Default.Get(key, defaultValue);
        }

        public void Set(string key, string value)
        {
            Preferences.Default.Set(key, value);
        }

        public void Remove(string key)
        {
            Preferences.Default.Remove(key);
        }

        public void Clear()
        {
            Preferences.Default.Clear();
        }
    }
}
