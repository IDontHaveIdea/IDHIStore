//
// Configuration entries IDHIStore
//

using ADV.Commands.Base;

using BepInEx.Configuration;
using BepInEx.Logging;

using KKAPI.Utilities;

namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal static ConfigEntry<bool> DebugInfo { get; set; }
        internal static ConfigEntry<bool> DebugToConsole { get; set; }
        internal const string DebugSection = "Debug";

        internal void ConfigEntries()
        {
            DebugInfo = Config.Bind(
                section: DebugSection,
                key: "Debug Information",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "Show debug information",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 2,
                        IsAdvanced = true
                    }));
            DebugInfo.SettingChanged += (_sender, _args) =>
            {
                _Log.Enabled = DebugInfo.Value;
#if DEBUG
                _Log.Level(LogLevel.Info, $"[ConfigEntries] Log.Enabled set to {_Log.Enabled}");
#endif
            };

            DebugToConsole = Config.Bind(
                section: DebugSection,
                key: "Debug information to Console",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "Show debug information in Console",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 1,
                        IsAdvanced = true
                    }));
            DebugToConsole.SettingChanged += (_sender, _args) =>
            {
                _Log.DebugToConsole = DebugToConsole.Value;
#if DEBUG
                _Log.Level(LogLevel.Info, $"[ConfigEntries] Log.DebugToConsole set to " +
                    $"{_Log.DebugToConsole}");
#endif

            };
        }
    }
}
