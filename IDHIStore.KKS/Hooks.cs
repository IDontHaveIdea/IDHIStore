//
// Debug testing
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

namespace IDHIPlugins
{
    public partial class IDHIStore
    {
        

        static internal Harmony _hookInstance;
        static internal Dictionary<int, Dictionary<int, int>> _dicExpAddTaii = new();

        internal partial class Hooks
        {
            static internal void Init()
            {
                _hookInstance = Harmony.CreateAndPatchAll(typeof(Hooks), nameof(Hooks));

                _hookInstance.Patch(
                    AccessTools.Method(
                        Type.GetType("HSceneProc, Assembly-CSharp"),
                            nameof(HSceneProc.GetCloseCategory)),
                        postfix: new HarmonyMethod(typeof(Hooks),
                            nameof(GetCloseCategoryPostfix)));

                _hookInstance.Patch(
                    AccessTools.Method(
                        Type.GetType("HSceneProc, Assembly-CSharp"),
                            nameof(HSceneProc.LoadAddTaii),
                            new Type[] { typeof(List<AddTaiiData.Param>) }),
                        postfix: new HarmonyMethod(typeof(Hooks),
                            nameof(LoadAddTaiiPostfix)));

            }

            static private void GetCloseCategoryPostfix(object __instance)
            {
                if (KoikatuAPI.GetCurrentGameMode() != GameMode.MainGame)
                {
                    return;
                }

                var _hLevel = Store.GetHLevel();

                if (_hLevel > 2)
                {
                    Log.Warning($"XXXX: [GetCloseCategoryPostfix] Enabled level 3.");
                }
                else
                {
                    Log.Warning($"XXXX: [GetCloseCategoryPostfix] Level {_hLevel} continue anyway");
                }

                #region get needed fields using reflection
                var hsceneTraverse = Traverse.Create(__instance);
                var lines = new StringBuilder();
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;
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

                Log.Warning($"XXXX: [GetCloseCategoryPostfix] start useCategorys={Utilities.CategoryList(useCategorys, true, false)}");

                StringBuilder stringBuilder = new("HPoint_");

                if (categorys.Any(c => c >= 1010 && c < 1200))
                {
                    Log.Warning($"XXXX: [GetCloseCategoryPostfix] one stringBuilder.Append(Add_)");
                }

                if (categorys.Any(c => c >= 1010 && c < 1100)
                    || categorys.Any(c => c >= 1100 && c < 1200))
                {
                    stringBuilder.Append("Add_");
                    Log.Warning($"XXXX: [GetCloseCategoryPostfix] two stringBuilder.Append(Add_)");
                }
                else if (categorys.Any(
                    c => c >= 3000 && c < 4000))
                {
                    stringBuilder.Append("3P_");
                    Log.Warning($"XXXX: [GetCloseCategoryPostfix] two stringBuilder.Append(3P_)");
                }

                Log.Warning($"XXXX: [GetCloseCategoryPostfix] reading: h/common/{stringBuilder}"
                    + $"{map.no}");

                //List<GameObject> gameObjectList =
                var gameObjectList =
                    GlobalMethod.LoadAllFolder<GameObject>("h/common/",
                        stringBuilder.ToString() + map.no.ToString());

                if (gameObjectList == null || gameObjectList.Count == 0)
                {
                    return;
                }

                //HPointData[] componentsInChildren = gameObjectList[gameObjectList.Count - 1]
                var componentsInChildren = gameObjectList[gameObjectList.Count - 1]
                    .GetComponentsInChildren<HPointData>(true);
                //HPointOmitObject component = gameObjectList[gameObjectList.Count - 1]
                var component = gameObjectList[gameObjectList.Count - 1]
                    .GetComponent<HPointOmitObject>();
                // bool flagAdditionalPosesRange1 = lstInitCategory.Any(c => c == 12 || c >= 1000);
                var flagAdditionalPosesRange1 = lstInitCategory.Any(c => c == 12 || c >= 1000);
                // bool flag3PRange2 = lstInitCategory.Any(c => c >= 3000 && c < 4000);
                var flag3PRange2 = lstInitCategory.Any(c => c >= 3000 && c < 4000);
                //float num = flags.HpointSearch * flags.HpointSearch;
                var num = flags.HpointSearch * flags.HpointSearch;
                // HPointData[] hpointDataArray = componentsInChildren;
                var hpointDataArray = componentsInChildren;

                for (var index1 = 0; index1 < hpointDataArray.Length; ++index1)
                {
                    hpointDataArray[index1].BackUpPosition();
                }

                var msg = new StringBuilder();

                //foreach (HPointData hpointData in componentsInChildren)
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

                Log.Warning($"XXXX: categorys={Utilities.CategoryList(categorys, true, false)}\n"
                    + $"useCategorys={Utilities.CategoryList(useCategorys, true, false)}\n");
            }


            static private void LoadAddTaiiPostfix(object __instance, List<AddTaiiData.Param> param)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var dicExpAddTaii = hsceneTraverse
                    .Field<Dictionary<int, Dictionary<int, int>>>("dicExpAddTaii").Value;

                var _hLevel = Store.GetHLevel();

                if (_hLevel > 1)
                {
                    Log.Warning($"XXXX: [GetCloseCategoryPostfix] Enabled level 2.");
                }
                else
                {
                    Log.Warning($"XXXX: [GetCloseCategoryPostfix] Level {_hLevel} continue anyway");
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

                    if (_hLevel > 0)
                    {
                        switch (_hLevel)
                        {
                            case 1:
                                for (var i = 0; i < item.Value.Count; i++)
                                {
                                    if (item.Value[i] == 50)
                                    {
                                        item.Value[i] = 0;
                                    }
                                }
                                break;
                            default:
                                item.Value.Clear();
                                break;
                        }
                    }
                    else
                    {
                        item.Value.Clear();
                    }
                }
            }

            static private void LoadAddTaiiReference(object __instance, List<AddTaiiData.Param> param)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var dicExpAddTaii = hsceneTraverse
                    .Field<Dictionary<int, Dictionary<int, int>>>("dicExpAddTaii").Value;

                foreach (var obj in param)
                {
                    var hExp = obj.hExp;
                    foreach (var info in obj.info)
                    {
                        if (!dicExpAddTaii.ContainsKey(info.taii))
                        {
                            dicExpAddTaii.Add(info.taii, new Dictionary<int, int>());
                        }
                        foreach (var id in info.Ids)
                        {
                            if (!dicExpAddTaii[info.taii].ContainsKey(id))
                            {
                                dicExpAddTaii[info.taii].Add(id, hExp);
                            }
                            else
                            {
                                dicExpAddTaii[info.taii][id] = hExp;
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
                var useCategorysGA = hsceneTraverse
                    .Field<List<int>>("useCategorys").Value;

                // bool flag3P = lstInitCategory.Any((int c) => c >= 3000 && c < 4000);
                var flag3P = lstInitCategory.Any((int c) => c >= 3000 && c < 4000);

                // for (int i = 0; i < _data.category.Length; i++)
                for (var i = 0; i < _data.category.Length; i++)
                {
                    // int num = _data.category[i];
                    var num = _data.category[i];
                    if ((!flags.isFreeH || IsCategoryInAnimationList(hsceneTraverse, num))
                        && (!_isSpecial || ((num >= 1000 || num == 12)
                        && !((!flag3P) ? (!lstInitCategory.Contains(num))
                            : (num < 3000 || num >= 4000))))
                        && !useCategorys.Contains(num))
                    {
                        useCategorys.Add(num);
                        Log.Warning($"XXXX: [SetCategoryXX] [{msg}] Add category {(PositionCategory)num}");
                        if (!useCategorysGA.Contains(num))
                        {
                            Log.Warning($"XXXX: [SetCategoryXX] [{msg}] Add category to GA {(PositionCategory)num}");
                        }
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

                // SaveData.Heroine.HExperienceKind hexperience = flags.lstHeroine[0].HExperience;
                var hexperience = flags.lstHeroine[0].HExperience;
                return _experience == 0
                    || (_experience == 1 && hexperience > SaveData.Heroine.HExperienceKind.不慣れ);
            }
        }
    }
}

/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDHIStore.KKS
{
    public class Class1
    {
    }
}
*/