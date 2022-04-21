using System;
using System.Collections.Generic;
using System.Linq;

using BepInEx.Logging;

using HarmonyLib;

namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal partial class Hooks
        {
            internal enum State { On = 0, Shift = 1, Hang = 2, Off = 3 }

            // Clothes to take off in pool
            internal static readonly List<ChaFileDefine.ClothesKind> Clothes = new() {
                ChaFileDefine.ClothesKind.top,
                ChaFileDefine.ClothesKind.bot,
                ChaFileDefine.ClothesKind.gloves,
                ChaFileDefine.ClothesKind.panst,
                ChaFileDefine.ClothesKind.socks,
                ChaFileDefine.ClothesKind.shoes_inner
            };

            // Footjob related animations
            // TODO: Maybe external configuarion file
            internal static readonly List<string> _FootJob = new() {
                "kpluganim-houshi-khh_f_82-023",
                "kpluganim-houshi-khh_f_63-004",
                "kpluganim-houshi-khh_f_61-002",
                "com.illusion-houshi-khh_f_54-054",
                "kpluganim-houshi-khh_f_91-069",
                "kpluganim-houshi-khh_f_81-022",
                "com.illusion-houshi-khh_f_55-055",
                "kpluganim-houshi-khh_f_62-003",
                "com.illusion-houshi-khh_f_59-059",
                "kpluganim-houshi-khh_f_60-001",
                "kpluganim-houshi-khh_f_76-017",
                "kpluganim-houshi-khh_f_83-024",
                "kpluganim-houshi-khh_f_77-018",
                "kpluganim-houshi-khh_f_80-021",
                "kpluganim-houshi-khh_f_84-025",
                "kpluganim-houshi-khh_f_75-016"
            };

            /// <summary>
            /// When in pool (swin) or bathtub (bathing) change coordinate as appropriate current
            /// clothes state is not modified (On, Half, Off, ..)
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="_nextAinmInfo"></param>
            [HarmonyPrefix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.ChangeAnimator))]
            private static void PoolClothesPrefix(
                object __instance,
                HSceneProc.AnimationListInfo _nextAinmInfo)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var flags = hsceneTraverse
                        .Field<HFlag>("flags").Value;
                var categorys = hsceneTraverse.Field<List<int>>("categorys").Value;

                if(flags.isFreeH)
                {
                    return;
                }

                try
                {
                    var female = flags.lstHeroine[0].chaCtrl;
                    var male = flags.player.chaCtrl;
                    var animationKey = "";

                    if(_animationLoaderOK)
                    {
                        animationKey = _animatinLoaderPInfo.GetAnimationKey(_nextAinmInfo);
                    }

                    // Pool
                    if (categorys.Contains(1307) || categorys.Contains(1004))
                    {
                        female.ChangeCoordinateTypeAndReload(ChaFileDefine.CoordinateType.Swim);
                        male.ChangeCoordinateTypeAndReload(ChaFileDefine.CoordinateType.Swim);

                        foreach(var cloth in Clothes)
                        {
                            female.SetClothesState((int)cloth, (byte)State.Off);
                        }
                        _Log.Debug("0007: Changing to swimsuit");
                    }
                    // Bathtub VIP room
                    else if (categorys.Contains(1306))
                    {
                        female.ChangeCoordinateTypeAndReload(ChaFileDefine.CoordinateType.Bathing);
                        male.ChangeCoordinateTypeAndReload(ChaFileDefine.CoordinateType.Bathing);
                        _Log.Debug("0008: Changing to bathing");
                    }
                    // Footjob animations
                    else if (_nextAinmInfo.mode == HFlag.EMode.houshi)
                    {
                        if (_animationLoaderOK && _FootJob.Contains(animationKey))
                        {
                            female.SetClothesState((int)ChaFileDefine.ClothesKind.shoes_inner, (byte)State.Off);
                            _Log.Debug("0011: Taking of shoes");
                        }
                    }
                }
                catch(Exception e)
                {
                    _Log.Level(LogLevel.Debug, $"0009: [ClothesState] Error - {e.Message}");
                }
            }
        }
    }
}

