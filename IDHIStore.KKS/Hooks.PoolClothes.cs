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
            public enum State { On = 0, Shift = 1, Hang = 2, Off = 3 }
            private static readonly List<ChaFileDefine.ClothesKind> Clothes = new() {
                ChaFileDefine.ClothesKind.top,
                ChaFileDefine.ClothesKind.bot,
                ChaFileDefine.ClothesKind.gloves,
                ChaFileDefine.ClothesKind.panst,
                ChaFileDefine.ClothesKind.socks,
                ChaFileDefine.ClothesKind.shoes_inner
            };

            /// <summary>
            /// When in pool or bathtub change coordinate as appropriate
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

                    // Pool
                    if(categorys.Contains(1307) || categorys.Contains(1004))
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
                    else if(categorys.Contains(1306))
                    {
                        female.ChangeCoordinateTypeAndReload(ChaFileDefine.CoordinateType.Bathing);
                        male.ChangeCoordinateTypeAndReload(ChaFileDefine.CoordinateType.Bathing);
                        _Log.Debug("0008: Changing to bathing");
                    }
                }
                catch(Exception e)
                {
                    _Log.Level(LogLevel.Debug, $"0009: [PoolClothes] Error - {e.Message}");
                }
            }
        }
    }
}

