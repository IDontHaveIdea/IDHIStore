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
        internal static ConfigEntry<bool> DebugToConsole { get; set; }
        internal static ConfigEntry<bool> NoExperience { get; set; }
        internal static ConfigEntry<bool> AdditionalCategories { get; set; }
        internal static ConfigEntry<bool> SpecialAnimations { get; set; }
        internal static ConfigEntry<bool> StoreLogic { get; set; }

        internal void ConfigEntries()
        {
            var section = "Debug";

            DebugInfo = Config.Bind(
                section: section,
                key: "Debug Information",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "Show debug information",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 50,
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
                section: section,
                key: "Debug information to Console",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "Show debug information in Console",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 49,
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
            section = "Options";
#if DEBUG
            NoExperience = Config.Bind(
                section: section,
                key: "No Experience to Unlock Animations",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "There is no experience required to unlock the animations.",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 30,
                    }));
            NoExperience.SettingChanged += (_sender, _args) =>
            {
#if DEBUG
                _Log.Level(LogLevel.Info, $"[ConfigEntries] NoExperience set to " +
                    $"{NoExperience.Value}");
#endif
            };
#endif
#if DEBUG
            AdditionalCategories = Config.Bind(
                section: section,
                key: "Special Animations to Regular Categories",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "Add special animations to regular categories in some of the maps.",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 29
                    }));
            AdditionalCategories.SettingChanged += (_sender, _args) =>
            {
#if DEBUG
                _Log.Level(LogLevel.Info, $"[ConfigEntries] AdditionalCategories set to " +
                    $"{AdditionalCategories.Value}");
#endif
            };
#endif
#if DEBUG
            SpecialAnimations = Config.Bind(
                section: section,
                key: "Special Animations",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "Add special animations to the menu on the maps " +
                        "that have them.",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 28
                    }));
            SpecialAnimations.SettingChanged += (_sender, _args) =>
            {
#if DEBUG
                _Log.Level(LogLevel.Info, $"[ConfigEntries] OnlySpecialAnimations set to " +
                    $"{SpecialAnimations.Value}");
#endif
            };
#endif
#if DEBUG
            StoreLogic = Config.Bind(
                section: section,
                key: "Store Logic",
                defaultValue: false,
                configDescription: new ConfigDescription(
                    description: "Work like the store in Release version.",
                    acceptableValues: null,
                    tags: new ConfigurationManagerAttributes {
                        Order = 27
                    }));
            StoreLogic.SettingChanged += (_sender, _args) =>
            {
#if DEBUG
                _Log.Level(LogLevel.Info, $"[ConfigEntries] StoreLogic set to " +
                    $"{StoreLogic.Value}");
#endif
            };
#endif
        }
    }
}
