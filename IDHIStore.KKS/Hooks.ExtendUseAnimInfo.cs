//
// Hooks for Store - Add special animations to lstUseAnimInfo
//
using System.Collections.Generic;
using System.Linq;

using HarmonyLib;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal partial class Hooks
        {
            /// <summary>
            /// Add animations in the range 1010-1099 and 1100-1199 to lstUseAnimInfo
            /// </summary>
            /// <param name="__instance">HSceneProc instance</param>
            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.CreateListAnimationFileName))]
            static public void ExtendUseAnimInfoPostfix(
                object __instance)
            {
                var hsceneTraverse = Traverse.Create(__instance);

                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;

                if (flags.isFreeH)
                {
                    return;
                }

                var _hLevel = Store.GetHLevel();

                if (_hLevel < 3)
                {
                    return;
                }

                var lstUseAnimInfo = Traverse
                    .Create(__instance)
                    .Field<List<HSceneProc.AnimationListInfo>[]>("lstUseAnimInfo").Value;
                var lstAnimInfo = hsceneTraverse
                    .Field<List<HSceneProc.AnimationListInfo>[]>("lstAnimInfo").Value;
                var categorys = hsceneTraverse
                    .Field<List<int>>("categorys").Value;
                var useCategorys = hsceneTraverse
                    .Field<List<int>>("useCategorys").Value;

                // Test for range 1010-1099 and 1100-1199
                var flagCategoryRange1 = categorys.Any(c =>
                    MathfEx.IsRange(1010, c, 1099, true)
                    || MathfEx.IsRange(1100, c, 1199, true));

                // Test for range 3000-3099
                var flagCategoryRange2 = categorys.Any(c =>
                    MathfEx.IsRange(3000, c, 3099, true));

                for (var mode = 0; mode < lstAnimInfo.Length; ++mode)
                {
                    for (var id = 0; id < lstAnimInfo[mode].Count; ++id)
                    {
                        // check we have a correct category
                        if (flagCategoryRange1)
                        {
                            if (!lstAnimInfo[mode][id].lstCategory.Any(c => categorys.Contains(c.category)))
                            {
                                continue;
                            }
                        }
                        else if (!lstAnimInfo[mode][id].lstCategory.Any(c => useCategorys.Contains(c.category)))
                        {
                            continue;
                        }

                        // correct category paranoia for 3P
                        if (!flagCategoryRange2)
                        {
                            if (!lstUseAnimInfo[mode].Contains(lstAnimInfo[mode][id]))
                            {
                                lstUseAnimInfo[mode].Add(lstAnimInfo[mode][id]);
                            }
                        }
                    }
                }
            }
        }
    }
}
