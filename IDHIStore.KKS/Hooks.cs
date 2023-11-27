//
// Hooks for Store - Initialization
//
using System.Collections.Generic;

using HarmonyLib;


namespace IDHIPlugIns
{
    public partial class IDHIStoreItems
    {
        internal static Dictionary<int, Dictionary<int, int>> _dicExpAddTaii = [];

        internal partial class Hooks
        {
            internal static void Init()
            {
                _ = Harmony.CreateAndPatchAll(typeof(Hooks));
            }
        }
    }
}
