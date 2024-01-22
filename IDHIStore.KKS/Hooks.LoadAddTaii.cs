//
// Hooks for Store - Clear experience access according to number of items sold in store
//
using System;
using System.Collections.Generic;

using HarmonyLib;


namespace IDHIPlugIns
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
            [HarmonyPatch(
                typeof(HSceneProc),
                nameof(HSceneProc.LoadAddTaii),
                [typeof(List<AddTaiiData.Param>)])]
            private static void LoadAddTaiiPostfix(
                object __instance, List<AddTaiiData.Param> param)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;
                var categorys = hsceneTraverse.Field<List<int>>("categorys").Value;
                var dicExpAddTaii = hsceneTraverse
                    .Field<Dictionary<int, Dictionary<int, int>>>("dicExpAddTaii").Value;

                if (flags.isFreeH)
                {
                    return;
                }

                if ((categorys[0] == 12) || (categorys[0] >= 1000))
                {
                    return;
                }

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

                var modes = new List<int>(dicExpAddTaii.Keys);
                foreach (var mode in modes)
                {
                    if (_dicExpAddTaii != null)
                    {
                        if (!_dicExpAddTaii.ContainsKey(mode))
                        {
                            // Save the original dictionary
                            _dicExpAddTaii.Add(
                                mode, new Dictionary<int, int>(dicExpAddTaii[mode]));
                        }
                    }
                    switch (_hLevel)
                    {
                        case 1:
                            // For first Level clear all 50 sysTaii setting them to 0
                            var ids = new List<int>(dicExpAddTaii[mode].Keys);
                            foreach (var id in ids)
                            {
                                if (dicExpAddTaii[mode][id] == 50)
                                {
                                    dicExpAddTaii[mode][id] = 0;
                                }
                            }
                            break;
                        default:
                            // For level > 2 set the entire dictionary to empty
                            dicExpAddTaii[mode].Clear();
                            break;
                    }
                }

                if (_animationLoaderOK)
                {
                    var _alDicExpAddTaii = _animationLoader.GetExpAddTaii();

                    var guids = new List<string>(_alDicExpAddTaii.Keys);
                    foreach(var guid in guids)
                    {
                        modes = new List<int>(_alDicExpAddTaii[guid].Keys);
                        foreach(var mode in modes)
                        {
                            switch(_hLevel)
                            {
                                case 1:
                                    // For first Level clear all 50 sysTaii
                                    // setting them to 0
                                    var ids = new List<string>
                                        (_alDicExpAddTaii[guid][mode].Keys);
                                    foreach(var id in ids)
                                    {
                                        if(_alDicExpAddTaii[guid][mode][id] <= 50)
                                        {
                                            _alDicExpAddTaii[guid][mode][id] = 0;
                                        }
                                    }
                                    break;
                                default:
                                    // For level greater than 2 set the entire
                                    // dictionary to empty
                                    _alDicExpAddTaii[guid][mode].Clear();
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
