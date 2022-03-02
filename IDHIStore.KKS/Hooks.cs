//
// Hooks for Store - Initialization
//
using System.Collections.Generic;

using HarmonyLib;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        static internal Dictionary<int, Dictionary<int, int>> _dicExpAddTaii = new();

        internal partial class Hooks
        {
            static internal void Init()
            {
                _ = Harmony.CreateAndPatchAll(typeof(Hooks));
            }
        }
    }
}
