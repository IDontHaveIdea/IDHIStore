//
// Hooks for Store - Clear experience access according to number of items sold in store
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
    public partial class IDHIStoreItems
    {
        internal partial class Hooks
        {
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
                var categorys = hsceneTraverse.Field<List<int>>("categorys").Value;

                if (flags.isFreeH)
                {
                    return;
                }

                if ((categorys[0] == 12) || (categorys[0] >= 1000))
                {
                    return;
                }

                var dicExpAddTaii = hsceneTraverse
                    .Field<Dictionary<int, Dictionary<int, int>>>("dicExpAddTaii").Value;

                var _hLevel = Store.GetHLevel();

                if (_hLevel <= 0)
                {
                    _Log.Debug("0005: No levels bought yet.");
                    return;
                }

                if (_hLevel < 3)
                {
                    _Log.Debug($"0006: Level {_hLevel} enabled.");
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
        }
    }
}
