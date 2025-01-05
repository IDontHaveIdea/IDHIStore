//
// Hooks for Store - Add special animations categories to useCategorys
//

using System.Linq;
using System.Text;

using UnityEngine;
using H;

using HarmonyLib;

using IDHIUtils;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal partial class IDHIStoreItemsHooks
        {
            /// <summary>
            /// Add H points special animations action points like in Free-H
            /// TODO: Consider distance
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.GetCloseCategory))]
            private static void AddToCategoryListPostfix(object __instance)
            {
                var hSceneTraverse = new HSceneProcTraverse(__instance);
                var flags = hSceneTraverse.flags;
                var categorys = hSceneTraverse.categorys;

                if (flags.isFreeH)
                {
                    _Log.Debug($"0001: Disabling in Free-H");
                    return;
                }
                if ((categorys[0] == 12) || (categorys[0] >= 1000))
                {
                    _Log.Debug($"0002: Disabling is a special H point.");
                    return;
                }
                var _hLevel = Store.GetHLevel();
#if DEBUG
                // Bypass Store
                if (!SpecialAnimations.Value)
                {
                    _Log.Debug($"[AddToCategoryListPostfix] Special animations disabled.");
                    return;
                }
                if (!StoreLogic.Value)
                {
                    _Log.Debug($"[AddToCategoryListPostfix] Special animations enabled.");
                    _hLevel = 3;
                }
#endif
                if (_hLevel > 2)
                {
                    _Log.Debug($"[AddToCategoryListPostfix] Level 3 enabled.");
                }
                else
                {
                    _Log.Debug($"0004: Level {_hLevel} to low.");
                    return;
                }

                #region get needed fields using reflection
                var lines = new StringBuilder();
                var lstInitCategory = hSceneTraverse.lstInitCategory;
                var map = hSceneTraverse.map;
                var nowHpointData = hSceneTraverse.nowHpointData;
                var closeHpointData = hSceneTraverse.closeHpointData;
                var useCategorys = hSceneTraverse.useCategorys;
                #endregion

                StringBuilder sbTmp = new("HPoint_");

                if (categorys.Any(c => c >= 1010 && c < 1100)
                    || categorys.Any(c => c >= 1100 && c < 1200))
                {
                    sbTmp.Append("Add_");
                }
                else if (categorys.Any(
                    c => c >= 3000 && c < 4000))
                {
                    sbTmp.Append("3P_");
                }

                var gameObjectList =
                    GlobalMethod.LoadAllFolder<GameObject>("h/common/",
                        sbTmp.ToString() + map.no.ToString());

                if (gameObjectList == null || gameObjectList.Count == 0)
                {
                    return;
                }

                var componentsInChildren = gameObjectList[gameObjectList.Count - 1]
                    .GetComponentsInChildren<HPointData>(true);
                var component = gameObjectList[gameObjectList.Count - 1]
                    .GetComponent<HPointOmitObject>();
                var hPointDataArray = componentsInChildren;

                for (var index1 = 0; index1 < hPointDataArray.Length; ++index1)
                {
                    hPointDataArray[index1].BackUpPosition();
                }

                // Loop through special animations and add any missing
                // special category to the used category list
                foreach (var hPointData in componentsInChildren.Where(
                    x => x.category.Any(c => (c == 12) || (c >= 1000 && c < 1999))))
                {
                    if (!component.list.Contains(hPointData.gameObject))
                    {
                        var category = hPointData.category[0];
                        if (!useCategorys.Contains(category))
                        {
                            useCategorys.Add(category);
                        }
                        if (nowHpointData == null || !(hPointData.name == nowHpointData))
                        {
                            if (!closeHpointData.Contains(hPointData))
                            {
                                // Shot to see if added category appears in category move list
                                // hSceneTraverse.SetCategory(hPointData);
#if DEBUG
                                _Log.Debug($"[AddToCategoryListPostfix] HPoint={hPointData.name} category={category}");
#endif
                                closeHpointData.Add(hPointData);
                            }
                        }
                    }
                }
            }
        }
    }
}

