//
// Configuration entries IDHIStore
//

using BepInEx.Configuration;
using BepInEx.Logging;

using KKAPI.Utilities;

namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal static ConfigEntry<bool> DebugInfo { get; set; }
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
                    tags: new ConfigurationManagerAttributes { Order = 40 }));
            DebugInfo.SettingChanged += (_sender, _args) =>
            {
                _Log.Enabled = DebugInfo.Value;
#if DEBUG
                _Log.Level(LogLevel.Info, $"0010: Log.Enabled set to {_Log.Enabled}");
#endif
            };
        }
    }
}
