//
// IDHIStore
//
using BepInEx;

using KKAPI;

using IDHIUtils;

namespace IDHIPlugins
{
    /// <summary>
    /// </summary>
    /// <remarks>
    ///
    ///
    /// </remarks>
    [BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
    [BepInDependency(IDHIUtils.PInfo.GUID, IDHIUtils.PInfo.Version)]
    [BepInPlugin(PInfo.GUID, PInfo.PluginDisplayName, PInfo.Version)]
    [BepInProcess(KoikatuAPI.GameProcessName)]
    public partial class IDHIStore : BaseUnityPlugin
    {
        #region Unity methods
        private void Awake()
        {
            Log.SetLogSource(base.Logger);
            Log.Enabled = true;
        }

        private void Start()
        {
            Log.Warning($"XXXX: [IDHIStore] Start Called.");

            if (KoikatuAPI.GetCurrentGameMode() != GameMode.MainGame)
            {
                return;
            }

            Hooks.Init();
            Store.Init();
        }
        #endregion
    }
}
