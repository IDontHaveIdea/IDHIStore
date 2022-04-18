//
// Hooks for Store - Initialization
//
using System.Collections.Generic;

using HarmonyLib;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal static Dictionary<int, Dictionary<int, int>> _dicExpAddTaii = new();

        internal partial class Hooks
        {
            internal static void Init()
            {
                _ = Harmony.CreateAndPatchAll(typeof(Hooks));
            }
        }
    }
}
