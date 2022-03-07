//
// Hooks for Store - Add special animations categories to useCategorys
//
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ActionGame;
using H;

using HarmonyLib;

using UnityEngine;



namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal partial class Hooks
        {
            /// <summary>
            /// Add animations for special action points like in Free-H
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.GetCloseCategory))]
            static private void AddToCategoryListPostfix(object __instance)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;
                var categorys = hsceneTraverse.Field<List<int>>("categorys").Value;

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

                if (_hLevel > 2)
                {
                    _Log.Debug($"0003: Level 3 enabled.");
                }
                else
                {
                    _Log.Debug($"0004: Level {_hLevel} to low.");
                    return;
                }

                #region get needed fields using reflection
                var lines = new StringBuilder();
                var lstInitCategory = hsceneTraverse
                    .Field<List<int>>("lstInitCategory").Value;
                var map = hsceneTraverse
                    .Field<ActionMap>("map").Value;
                var nowHpointData = hsceneTraverse
                    .Field<string>("nowHpointData").Value;
                var closeHpointData = hsceneTraverse
                    .Field<List<HPointData>>("closeHpointData").Value;
                var useCategorys = hsceneTraverse
                    .Field<List<int>>("useCategorys").Value;
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
                //var flagAdditionalPosesRange1 = lstInitCategory.Any(c => c == 12 || c >= 1000);
                //var flag3PRange2 = lstInitCategory.Any(c => c >= 3000 && c < 4000);
                var hPointDataArray = componentsInChildren;

                for (var index1 = 0; index1 < hPointDataArray.Length; ++index1)
                {
                    hPointDataArray[index1].BackUpPosition();
                }

                var msg = new StringBuilder();

                foreach (var hPointData in componentsInChildren.Where(
                    x => x.category.Any(c => (c == 12) || (c >= 1000 && c < 1999))))
                {
                    msg.Clear();
                    if (!component.list.Contains(hPointData.gameObject))
                    {
                        // Enable all animations like in FreeH for MainGame
                        //if (!hpointData.category.Any((int c)
                        //        => MathfEx.IsRange(2000, c, 2999, true))
                        //    && IsCategoryInAnimationList(hsceneTraverse, hpointData.category)
                        //    && IsExperience(hsceneTraverse, hpointData.Experience))
                        //{
                            /*msg.Append("GetCloseCategoryPostfix");
                            SetCategoryXX(
                                hsceneTraverse,
                                hpointData,
                                ref useCategorys,
                                false,
                                msg.ToString());*/
                            var category = hPointData.category[0];
                            if (!useCategorys.Contains(category))
                            {
                                //lines.Append($"Add to useCategorys {category}\n");
                                useCategorys.Add(category);
                            }
                            if (nowHpointData == null || !(hPointData.name == nowHpointData))
                            {
                                if (!closeHpointData.Contains(hPointData))
                                {
                                    closeHpointData.Add(hPointData);
                                }
                            }
                        //}
                    }
                }
            }

            /*static private void SetCategoryXX(Traverse hsceneTraverse,
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
                    var category = _data.category[i];
                    //if ((!flags.isFreeH || IsCategoryInAnimationList(hsceneTraverse, num))
                    if ((IsCategoryInAnimationList(hsceneTraverse, category))
                        && (!_isSpecial || ((category >= 1000 || category == 12)
                        && !((!flag3P) ? (!lstInitCategory.Contains(category))
                            : (category < 3000 || category >= 4000))))
                        && !useCategorys.Contains(category))
                    {
                        useCategorys.Add(category);
                        _Log.Debug($"0008: [{msg}] Add category {(PositionCategory)category}");
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
            }*/
        }
    }
}

