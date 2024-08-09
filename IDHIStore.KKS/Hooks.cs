//
// Hooks for Store - Initialization
//
using System.Collections.Generic;

using HarmonyLib;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal static Dictionary<int, Dictionary<int, int>> _dicExpAddTaii = [];

        internal partial class IDHIStoreItemsHooks
        {
            internal static void Init()
            {
                _ = Harmony.CreateAndPatchAll(typeof(IDHIStoreItemsHooks));
            }
        }
    }
}
