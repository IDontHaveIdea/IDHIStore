//
// IDHIStore
//
using BepInEx;
#if DEBUG
using BepInEx.Logging;
#endif

using KKAPI;

using IDHIUtils;


namespace IDHIPlugins
{
/// <summary>
/// KKS Store plug-in that will make available the animations regardless of heroine experience
/// </summary>
    [BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
    [BepInDependency(Utilities.GUID, Utilities.Version)]
    [BepInPlugin(GUID, PluginDisplayName, Version)]
    [BepInProcess(KoikatuAPI.GameProcessName)]
    public partial class IDHIStoreItems : BaseUnityPlugin
    {
        static internal Logg _Log = new();

        #region Unity methods
        private void Awake()
        {
            _Log.SetLogSource(base.Logger);
            ConfigEntries();
            _Log.Enabled = DebugInfo.Value;
#if DEBUG
            _Log.Level(LogLevel.Info, $"0009: Logging set to {_Log.Enabled}");
#endif
        }

        private void Start()
        {
            Hooks.Init();
            Store.Init();
        }
        #endregion
    }
}
