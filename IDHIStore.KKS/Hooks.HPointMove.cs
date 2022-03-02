using System.Collections.Generic;

using HarmonyLib;

namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal partial class Hooks
        {
            /// <summary>
            /// Trick system to think it is in Free-H so HPoint move menu will include special
            /// animations like Free-H.
            /// TODO: Make Bench BlowJob also appear.
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="__state"></param>
            //[HarmonyPrefix]
            //[HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.GotoPointMoveScene))]
            static private void GotoPointMoveScenePrefix(object __instance, ref bool __state)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;
                var categorys = hsceneTraverse.Field<List<int>>("categorys").Value;
                
                var _hLevel = Store.GetHLevel();

                // actually Free-H
                if (flags.isFreeH)
                {
                    return;
                }

                if ((categorys[0] == 12) || (categorys[0] >= 1000))
                {
                    return;
                }

                if (_hLevel < 3)
                {
                    return;
                }

                __state = flags.isFreeH;
                flags.isFreeH = true;
            }

            //[HarmonyPostfix]
            //[HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.GotoPointMoveScene))]
            static private void GotoPointMoveScenePostfix(HSceneProc __instance, ref bool __state)
            {
                var hsceneTraverse = Traverse.Create(__instance);
                var flags = hsceneTraverse
                    .Field<HFlag>("flags").Value;
                var categorys = hsceneTraverse.Field<List<int>>("categorys").Value;

                var _hLevel = Store.GetHLevel();

                if (flags.isFreeH)
                {
                    return;
                }

                // Special point animation
                if ((categorys[0] == 12) || (categorys[0] >= 1000))
                {
                    return;
                }

                if (_hLevel < 3)
                {
                    return;
                }

                __instance.flags.isFreeH = __state;
            }

            /*
            private void GotoPointMoveSceneOriginal()
            {
                if (this.hand.IsAction() 
                    || this.flags.voiceWait 
                    || this.categorys.Any<int>((Func<int, bool>)(c => MathfEx.IsRange<int>(2000, c, 2999, true))) 
                    || this.flags.mode == HFlag.EMode.masturbation && !this.flags.isFreeH)
                    return;

                this.lstOldFemaleVisible.Clear();

                for (int index = 0; index < this.lstFemale.Count; ++index)
                {
                    this.lstOldFemaleVisible.Add(this.lstFemale[index].visibleAll);
                    this.lstFemale[index].visibleAll = false;
                }

                this.lstOldMaleVisible.Clear();
                this.lstOldMaleVisible.Add(this.male.visibleAll);
                this.male.visibleAll = false;

                if ((bool)(UnityEngine.Object)this.male1)
                {
                    this.lstOldMaleVisible.Add(this.male1.visibleAll);
                    this.male1.visibleAll = false;
                }
                this.item.SetVisible(false);
                this.hand.SceneChangeItemEnable(false);
                GameObject commonSpace = Manager.Scene.commonSpace;

                if ((UnityEngine.Object)commonSpace != (UnityEngine.Object)null)
                {
                    DeliveryHPointData deliveryHpointData1 = commonSpace.AddComponent<DeliveryHPointData>();
                    deliveryHpointData1.actionSelect = new Action<HPointData, int>(this.ChangeCategory);
                    deliveryHpointData1.actionBack = new Action(this.CancelForHPointMove);
                    deliveryHpointData1.IDMap = this.map.no;
                    deliveryHpointData1.cam = this.flags.ctrlCamera;
                    deliveryHpointData1.lstCategory = this.useCategorys;
                    deliveryHpointData1.initPos = this.initPos;
                    deliveryHpointData1.initRot = this.initRot;
                    deliveryHpointData1.isFreeH = this.flags.isFreeH;
                    deliveryHpointData1.status = this.flags.lstHeroine[0].HExperience;
                    deliveryHpointData1.lstAnimInfo = this.lstAnimInfo;
                    deliveryHpointData1.isDebug = false;
                    deliveryHpointData1.flags = this.flags;
                    deliveryHpointData1.lstInitCategory = this.lstInitCategory;
                    deliveryHpointData1.usePointInit = this.lstInitCategory.Any<int>((Func<int, bool>)(c => c == 0 || c == 1 || c == 8 || c == 1302));
                    deliveryHpointData1.initOffsetPos = this.initKindOffsetPos;
                    deliveryHpointData1.initOffsetRot = this.initKindOffsetRot;
                    ActionScene instance = SingletonInitializer<ActionScene>.instance;
                    bool flag = (UnityEngine.Object)instance != (UnityEngine.Object)null && instance.Cycle.withHeroine != null && (instance.Cycle.withHeroineSituation == 0 || instance.Cycle.withHeroineSituation == 1);
                    DeliveryHPointData deliveryHpointData2 = deliveryHpointData1;
                    deliveryHpointData2.usePointInit = ((deliveryHpointData2.usePointInit ? 1 : 0) & (this.appointAction == 0 || this.appointAction == 8 || this.appointAction == 9 ? 0 : (!flag ? 1 : 0))) != 0;
                    if ((UnityEngine.Object)instance != (UnityEngine.Object)null && instance.isPenetration && this.isMasturbationInitPoint)
                        deliveryHpointData1.usePointInit = false;
                    deliveryHpointData1.femalePos = this.initPos;
                }

                this.sprite.gameObject.SetActive(false);
                Singleton<GameCursor>.Instance.SetCursorTexture(-1);
                this.UndoMapObject();
                this.raycaster.enabled = false;
                this.guideObject.gameObject.SetActive(false);
                this.ctrlObi.Clear();
                this.AllDoorOpenClose(true);
                this.flags.ctrlCamera.GetComponent<CameraEffectorConfig>().useDOF = false;
                this.flags.ctrlCamera.GetComponent<CameraEffector>().dof.enabled = false;
                this.crossfade.FadeStart();

                Manager.Scene.LoadReserve(new Manager.Scene.Data()
                {
                    levelName = "HPointMove",
                    isAdd = true,
                    isFade = false
                }, false);
            }*/
        }
    }
}
