//
// Hooks for Store - Add animations changing it's context to Category list
//
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ActionGame;

using H;

using HarmonyLib;

using IDHIUtils;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        internal partial class Hooks
        {
            static internal Dictionary<int, Dictionary<int, List<string>>> MapHPoints = new()
            {
                {  2, new Dictionary<int, List<string>>
                    {
                        { 12, new List<string>
                            {
                                "nagaisu_00",
                                "nagaisu_00 (1)"
                            }
                        }
                    }
                },
                {  3, new Dictionary<int, List<string>>
                    {
                        { 12, new List<string>
                            {
                                "nagaisu_00 (1)"
                            }
                        }
                    }
                },
                {  5, new Dictionary<int, List<string>>
                    {
                        { 12, new List<string>
                            {
                                "senasi_nagaisu_00",
                                "senasi_nagaisu_00 (1)",
                                "senasi_nagaisu_00 (3)"
                            }
                        }
                    }
                },
                {  7, new Dictionary<int, List<string>>
                    {
                        { 12, new List<string>
                            {
                                "nagaisu_00 (5)",
                                "nagaisu_00 (6)",
                                "nagaisu_00 (7)"
                            }
                        }
                    }
                },
                {
                    17, new Dictionary<int, List<string>>
                    {
                        { 12, new List<string>
                            {
                                "nagaisu_00",
                                "nagaisu_00 (1)"
                            }
                        }
                    }
                },
                {
                    23, new Dictionary<int, List<string>>
                    {
                        { 12, new List<string> 
                            {
                                "nagaisu_00",
                                "nagaisu_00 (1)" 
                            }
                        }
                    }
                }
            };

            /// <summary>
            /// Add some special animations to other maps adjusting categories
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.CreateAllAnimationList))]
            static public void AddAnimationsPostfix(object __instance)
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

                var _hLevel = Store.GetHLevel();

                if (_hLevel < 3)
                {
                    return;
                }

                var addedAnimations = new StringBuilder();
                var closeHpointData = hsceneTraverse
                    .Field<List<HPointData>>("closeHpointData").Value;
                var lstAnimInfo = hsceneTraverse
                    .Field<List<HSceneProc.AnimationListInfo>[]>("lstAnimInfo").Value;
                var map = hsceneTraverse.Field<ActionMap>("map").Value;
                var useCategorys = hsceneTraverse.Field<List<int>>("useCategorys").Value;

                // Loop through aibu, houshi and sonyu
                for (var mode = 0; mode < 3; mode++)
                {
                    foreach (var anim in lstAnimInfo[mode].Where(x
                        => x.lstCategory.Any(c
                            => (c.category == 12) || (c.category >= 1000 && c.category < 1999))))
                    {
                        if (anim.lstCategory.Any(c => useCategorys.Contains(c.category)))
                        {
                            continue;
                        }

                        var category = anim.lstCategory[0].category;
                        switch (category)
                        {
                            case 12:
                                if (MapHPoints.ContainsKey(map.no)
                                    && MapHPoints[map.no].ContainsKey(category))
                                {
                                    var validHPoints = MapHPoints[map.no][category];

                                    foreach (var hPointData in closeHpointData)
                                    {
                                        if (validHPoints.Contains(hPointData.name))
                                        {
                                            hPointData.category = 
                                                hPointData.category
                                                    .Concat(new int[] { category }).ToArray();
                                        }
                                        if (!useCategorys.Contains(category))
                                        {
                                            useCategorys.Add(category);
                                        }
                                    }
                                }
                                break;
                            case 1002:
                                // Bookshelf Caress
                                anim.nameAnimation = "壁いたずら愛撫";
                                anim.lstCategory.Add(new HSceneProc.Category
                                {
                                    category = (int)PositionCategory.Wall,
                                });
                                break;
                            case 1304:
                                // Pressed From Behind
                                anim.lstCategory.Add(new HSceneProc.Category
                                {
                                    category = (int)PositionCategory.Wall,
                                });
                                break;
                            case 1006:
                                if (anim.id == 21)
                                {
                                    // Fence Doggy
                                    anim.nameAnimation = "壁バック 2";
                                }
                                if (anim.id == 22)
                                {
                                    // Fence Lifting
                                    anim.nameAnimation = "壁掴まり駅弁";
                                }
                                anim.lstCategory.Add(new HSceneProc.Category
                                {
                                    category = (int)PositionCategory.Wall,
                                });
                                break;
                            case 1008:
                                // Piledriver Missionary
                                anim.lstCategory.Add(new HSceneProc.Category
                                {
                                    category = (int)PositionCategory.SitChair,
                                });
                                break;
                            case 1300:
                                if (mode == 2)
                                {
                                    // Volleyball Net Doggystyle
                                    anim.nameAnimation = "壁バック 3";
                                    anim.lstCategory.Add(new HSceneProc.Category
                                    {
                                        category = (int)PositionCategory.Wall,
                                    });
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
