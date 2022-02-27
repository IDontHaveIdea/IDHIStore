//
// Hooks for Store
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using H;
using ActionGame;

using HarmonyLib;

using KKAPI;

using IDHIUtils;
//using static ActionGame.EnvLineArea3D;

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

            /// <summary>
            /// Add animations for special action points like in Free-H
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.GetCloseCategory))]
            static private void GetCloseCategoryPostfix(object __instance)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;

                if (flags.isFreeH)
                {
                    _log.Debug($"0001: Disabling in Free-H");
                    return;
                }

                var _hLevel = Store.GetHLevel();

                if (_hLevel > 2)
                {
                    _log.Debug($"0002: Level 3 enabled.");
                }
                else
                {
                    _log.Debug($"0003: Level {_hLevel} to low.");
                    return;
                }

                // Level greater than 2 enable all categories like in Free-H
                // TODO: enable animations in lower categories maybe modifying lstCategory

                #region get needed fields using reflection
                var lines = new StringBuilder();
                var lstInitCategory = hsceneTraverse
                    .Field<List<int>>("lstInitCategory").Value;
                var categorys = hsceneTraverse
                    .Field<List<int>>("categorys").Value;
                var map = hsceneTraverse
                    .Field<ActionMap>("map").Value;
                var nowHpointData = hsceneTraverse
                    .Field<string>("nowHpointData").Value;
                var closeHpointData = hsceneTraverse
                    .Field<List<HPointData>>("closeHpointData").Value;
                var useCategorys = hsceneTraverse
                    .Field<List<int>>("useCategorys").Value;
                #endregion

                StringBuilder stringBuilder = new("HPoint_");

                if (categorys.Any(c => c >= 1010 && c < 1100)
                    || categorys.Any(c => c >= 1100 && c < 1200))
                {
                    stringBuilder.Append("Add_");
                }
                else if (categorys.Any(
                    c => c >= 3000 && c < 4000))
                {
                    stringBuilder.Append("3P_");
                }

                var gameObjectList =
                    GlobalMethod.LoadAllFolder<GameObject>("h/common/",
                        stringBuilder.ToString() + map.no.ToString());

                if (gameObjectList == null || gameObjectList.Count == 0)
                {
                    return;
                }

                var componentsInChildren = gameObjectList[gameObjectList.Count - 1]
                    .GetComponentsInChildren<HPointData>(true);
                var component = gameObjectList[gameObjectList.Count - 1]
                    .GetComponent<HPointOmitObject>();
                var flagAdditionalPosesRange1 = lstInitCategory.Any(c => c == 12 || c >= 1000);
                var flag3PRange2 = lstInitCategory.Any(c => c >= 3000 && c < 4000);
                var num = flags.HpointSearch * flags.HpointSearch;
                var hpointDataArray = componentsInChildren;

                for (var index1 = 0; index1 < hpointDataArray.Length; ++index1)
                {
                    hpointDataArray[index1].BackUpPosition();
                }

                var msg = new StringBuilder();

                foreach (var hpointData in componentsInChildren)
                {
                    msg.Clear();
                    if (!component.list.Contains(hpointData.gameObject))
                    {
                        // Enable all animations like in FreeH for MainGame
                        if (!hpointData.category.Any((int c)
                                => MathfEx.IsRange(2000, c, 2999, true))
                            && IsCategoryInAnimationList(hsceneTraverse, hpointData.category)
                            && IsExperience(hsceneTraverse, hpointData.Experience))
                        {
                            msg.Append("GetCloseCategoryPostfix");
                            SetCategoryXX(
                                hsceneTraverse,
                                hpointData,
                                ref useCategorys,
                                false,
                                msg.ToString());
                            if (nowHpointData == null || !(hpointData.name == nowHpointData))
                            {
                                if (!closeHpointData.Contains(hpointData))
                                {
                                    closeHpointData.Add(hpointData);
                                }
                            }
                        }
                    }
                }
                _log.Debug($"0004: categorys={Utilities.CategoryList(categorys, true, false)}\n"
                    + $"useCategorys={Utilities.CategoryList(useCategorys, true, false)}\n");
            }

            /// <summary>
            /// Systematically clearing items in dicExpAddTaii makes animations available 
            /// disregarding heroine experience
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="param"></param>
            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.LoadAddTaii), new Type[] { typeof(List<AddTaiiData.Param>) })]
            static private void LoadAddTaiiPostfix(object __instance, List<AddTaiiData.Param> param)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;

                if (flags.isFreeH)
                {
                    return;
                }

                var dicExpAddTaii = hsceneTraverse
                    .Field<Dictionary<int, Dictionary<int, int>>>("dicExpAddTaii").Value;

                var _hLevel = Store.GetHLevel();

                if (_hLevel <= 0)
                {
                    _log.Debug("0005: No levels bought yet.");
                    return;
                }

                if (_hLevel < 3)
                {
                    _log.Debug($"0006: Level {_hLevel} enabled.");
                }

                foreach (var item in dicExpAddTaii)
                {
                    if (_dicExpAddTaii != null)
                    {
                        if (!_dicExpAddTaii.ContainsKey(item.Key))
                        {
                            // Save the original dictionary
                            _dicExpAddTaii.Add(item.Key, new Dictionary<int, int>(item.Value));
                        }
                    }
                    switch (_hLevel)
                    {
                        case 1:
                            // For first Level clear all 50 sysTaii setting to 0
                            for (var i = 0; i < item.Value.Count; i++)
                            {
                                if (item.Value[i] == 50)
                                {
                                    item.Value[i] = 0;
                                }
                            }
                            break;
                        default:
                            // For level greater than 2 set the entire dictionary to empty
                            item.Value.Clear();
                            break;
                    }
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.CreateListAnimationFileName))]
            static public void CreateListAnimationFileNamePostfix(
                object __instance, bool _isAnimListCreate = true)
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

            static private void SetCategoryXX(Traverse hsceneTraverse,
                HPointData _data,
                ref List<int> useCategorys,
                bool _isSpecial = false,
                string msg = "")
            {
                var lstInitCategory = hsceneTraverse
                    .Field<List<int>>("lstInitCategory").Value;
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;

                var flag3P = lstInitCategory.Any((int c) => c >= 3000 && c < 4000);

                for (var i = 0; i < _data.category.Length; i++)
                {
                    var num = _data.category[i];
                    if ((!flags.isFreeH || IsCategoryInAnimationList(hsceneTraverse, num))
                        && (!_isSpecial || ((num >= 1000 || num == 12)
                        && !((!flag3P) ? (!lstInitCategory.Contains(num))
                            : (num < 3000 || num >= 4000))))
                        && !useCategorys.Contains(num))
                    {
                        useCategorys.Add(num);
                        _log.Debug($"0007: [{msg}] Add category {(PositionCategory)num}");
                    }
                }
            }

            static private bool IsCategoryInAnimationList(Traverse hsceneTraverse, int[] _categorys)
            {
                var categorys = hsceneTraverse
                    .Field<List<int>>("categorys").Value;

                if (categorys.Any((int c) => c >= 1010 && c < 1100))
                {
                    return _categorys.Any((int c) => MathfEx.IsRange(1010, c, 1099, true));
                }
                if (categorys.Any((int c) => c >= 1100 && c < 1200))
                {
                    return _categorys.Any((int c) => MathfEx.IsRange(1100, c, 1199, true));
                }
                return true;
            }

            static private bool IsCategoryInAnimationList(Traverse hsceneTraverse, int _category)
            {
                var categorys = hsceneTraverse
                    .Field<List<int>>("categorys").Value;

                if (categorys.Any((int c) => c >= 1010 && c < 1100))
                {
                    return MathfEx.IsRange(1010, _category, 1099, true);
                }
                return !categorys.Any((int c) => c >= 1100 && c < 1200)
                    || MathfEx.IsRange(1100, _category, 1199, true);
            }

            static private bool IsExperience(Traverse hsceneTraverse, int _experience)
            {
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;

                var hexperience = flags.lstHeroine[0].HExperience;
                return _experience == 0
                    || (_experience == 1 && hexperience > SaveData.Heroine.HExperienceKind.不慣れ);
            }
        }
    }
}
