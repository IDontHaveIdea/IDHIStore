//
// IDHIStore
//
using BepInEx;

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
        static internal Logg _log = new();

        #region Unity methods
        private void Awake()
        {
            _log.SetLogSource(base.Logger);
#if DEBUG
            _log.Enabled = true;
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
