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
    [BepInDependency(IDHIUtils.Info.GUID, IDHIUtils.Info.Version)]
    [BepInDependency("essuhauled.animationloader",
        BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(GUID, PluginDisplayName, Version)]
    [BepInProcess(KoikatuAPI.GameProcessName)]
    [BepInProcess(KoikatuAPI.VRProcessName)]
    public partial class IDHIStoreItems : BaseUnityPlugin
    {
        internal static Logg _Log = new();
        internal static Utilities.PInfo _animatinLoaderPInfo = new();
        internal static bool _animationLoaderOK = false;

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
            _animatinLoaderPInfo.GUID = "essuhauled.animationloader";
            if (_animatinLoaderPInfo.Instance != null && _animatinLoaderPInfo.VersionAtLeast("1.1.1.3"))
            {
                _animationLoaderOK = true;
            }
            Hooks.Init();
        }
        #endregion
    }
}
