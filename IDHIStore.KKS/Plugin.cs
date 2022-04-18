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
    [BepInDependency("essuhauled.animationloader", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(GUID, PluginDisplayName, Version)]
    [BepInProcess(KoikatuAPI.GameProcessName)]
    [BepInProcess(KoikatuAPI.VRProcessName)]
    public partial class IDHIStoreItems : BaseUnityPlugin
    {
        internal static Logg _Log = new();

        #region Unity methods
        private void Awake()
        {
            _Log.SetLogSource(base.Logger);
            ConfigEntries();
            _Log.Enabled = DebugInfo.Value;
#if DEBUG
            _Log.Level(LogLevel.Info, $"0010: Logging set to {_Log.Enabled}");
#endif
            Store.Init();
        }

        private void Start()
        {
            Hooks.Init();
        }
        #endregion
    }
}
