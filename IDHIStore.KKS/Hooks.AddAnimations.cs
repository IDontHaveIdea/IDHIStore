//
// Hooks for Store - Add animations changing it's context to Category list
//
// Ignore Spelling: categorys

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
            /// <summary>
            /// Add animations that work on specific H points
            /// 12 - Bench Blowjob animation added to some of the available working
            /// H points
            /// </summary>
            internal static Dictionary<int, Dictionary<int, List<string>>> MapHPoints
                = new()
            {
                // Training Center Outside
                {  2, new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string>
                            {
                                "nagaisu_00",
                                "nagaisu_00 (1)"
                            }
                        }
                    }
                },
                // Harbor
                {  3, new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string>
                            {
                                "nagaisu_00 (1)"
                            }
                        }
                    }
                },
                // Nature Park
                {  5, new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string>
                            {
                                "senasi_nagaisu_00",
                                "senasi_nagaisu_00 (1)",
                                "senasi_nagaisu_00 (3)"
                            }
                        }
                    }
                },
                // Aquarium Outside
                {  7, new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string>
                            {
                                "nagaisu_00 (5)",
                                "nagaisu_00 (6)",
                                "nagaisu_00 (7)"
                            }
                        }
                    }
                },
                // Stone Wall Pathway
                {  8, new Dictionary<int, List<string>>
                    {
                        // Box Doggy
                        { 1009, new List<string>
                            {
                                "senasi_naga_isu_00"
                            }
                        }
                    }
                },
                // Hotel Lobby
                { 11, new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string>
                            {
                                "nagaisu_00 (1)",
                                "nagaisu_00 (4)"
                            }
                        }
                    }
                },
                // Hotel Changing Room
                { 17, new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string>
                            {
                                "nagaisu_00",
                                "nagaisu_00 (1)"
                            }
                        }
                    }
                },
                // Training Center
                { 23, new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string> 
                            {
                                "nagaisu_00",
                                "nagaisu_00 (1)"
                            }
                        }
                    }
                },
                // Clothing Shop
                // Is in map 27 the Fitting Room
                { 32, new Dictionary<int, List<string>>
                    {
                        // Reverse Cowgirl Against Wall
                        { 1302, new List<string>
                            {
                                "HPointBase"
                            }
                        }
                    }
                },
                // Souvenir Shop
                { 35,
                    new Dictionary<int, List<string>>
                    {
                        // Bench Blowjob
                        { 12, new List<string>
                            {
                                "nagaisu_00"
                            }
                        }
                    }

                }
            };

            /// <summary>
            /// Add some special animations to other maps adjusting categories
            /// Some names were change because of context
            /// TODO: how to set fingers straight for some animations
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            [HarmonyPatch(typeof(HSceneProc), nameof(HSceneProc.CreateAllAnimationList))]
            public static void AddAnimationsPostfix(object __instance)
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
                            => (c.category == 12)
                                || (c.category >= 1000 && c.category < 1999))))
                    {
                        if (anim.lstCategory.Any(c => useCategorys.Contains(c.category)))
                        {
                            continue;
                        }

                        var category = anim.lstCategory[0].category;
                        switch (category)
                        {
                            case 12:
                            // Bench Blowjob
                            case 1302:
                            // Reverse Cowgirl Against Wall
                            case 1009:
                            // Box Doggy

                                // Add new category to an HPoint
                                if (MapHPoints.TryGetValue(map.no, out var hPointCategory)
                                    && hPointCategory.TryGetValue(category, out var validHPoints))
                                {
                                    foreach (var hPointData in closeHpointData)
                                    {
                                        if (validHPoints.Contains(hPointData.name))
                                        {
                                            /*
                                                hPointData.category
                                                    .Concat(new int[] { category })
                                                    .ToArray();
                                             */
                                            hPointData.category =
                                                [.. hPointData.category, .. new int[] { category }];
                                            if (!useCategorys.Contains(category))
                                            {
                                                useCategorys.Add(category);
                                            }
                                        }
                                    }
                                }
                                /*if (MapHPoints.ContainsKey(map.no)
                                    && MapHPoints[map.no].ContainsKey(category))
                                {
                                    var vHPoints = MapHPoints[map.no][category];

                                    foreach (var hPointData in closeHpointData)
                                    {
                                        if (vHPoints.Contains(hPointData.name))
                                        {
                                            hPointData.category =
                                                hPointData.category
                                                    .Concat(new int[] { category })
                                                    .ToArray();
                                            if (!useCategorys.Contains(category))
                                            {
                                                useCategorys.Add(category);
                                            }
                                        }
                                    }
                                }*/
                                break;
                            case 1002:
                                // Bookshelf Caress - Wall Mischievous Caress
#if DEBUG
                                anim.nameAnimation = "壁いたずら愛撫";
#else
                                anim.nameAnimation = "Wall Mischievous Caress";
#endif
                                anim.lstCategory.Add(new HSceneProc.Category {
                                    category = (int)PositionCategory.Wall,
                                });
                                break;
                            case 1006:
                                if (anim.id == 21)
                                {
                                    // Fence Doggy - Wall Doggy 2
#if DEBUG
                                    anim.nameAnimation = "壁バック2";
#else
                                    anim.nameAnimation = "Wall Doggy 2";
#endif
                                }
                                if (anim.id == 22)
                                {
                                    // Fence Lifting - Wall Lifting
#if DEBUG
                                    anim.nameAnimation = "壁掴まり駅弁";
#else
                                    anim.nameAnimation = "Wall Lifting";
#endif
                                }
                                anim.lstCategory.Add(new HSceneProc.Category {
                                    category = (int)PositionCategory.Wall,
                                });
                                break;
#if DEBUG
                            case 1008:
                                // Piledriver Missionary
                                anim.lstCategory.Add(new HSceneProc.Category {
                                    category = (int)PositionCategory.SitChair,
                                });
                                anim.lstCategory.Add(new HSceneProc.Category {
                                    category = (int)PositionCategory.SofaBench,
                                });
                                break;
                            case 1200:
                                // Straddle Bench Blowjob
                                // Position has to be rotated for it to work need
                                // more research
                                anim.lstCategory.Add(new HSceneProc.Category {
                                    category = (int)PositionCategory.BacklessBench,
                                });
                                break;
                            case 1201:
                                // Sofa Cowgirl
                                if (MapHPoints.TryGetValue(map.no, out var hCategory))
                                {
                                    anim.lstCategory.Add(new HSceneProc.Category {
                                        category = (int)PositionCategory.SofaBench,
                                    });
                                }
                                break;
#endif
                            case 1300:
                                if (mode == 2)
                                {
                                    // Volleyball Net Doggystyle - Wall Doggy 3
#if DEBUG
                                    anim.nameAnimation = "壁バック3";
#else
                                    anim.nameAnimation = "Wall Doggy 3";
#endif
                                    anim.lstCategory.Add(new HSceneProc.Category
                                    {
                                        category = (int)PositionCategory.Wall,
                                    });
                                }
                                break;
                            case 1304:
                                // Pressed From Behind
                                anim.lstCategory.Add(new HSceneProc.Category {
                                    category = (int)PositionCategory.Wall,
                                });
                                break;
                            case 1307:
                                // Beach Ball Missionary
                                // Float Doggystyle
                                if (mode == 2)
                                {
                                    if (useCategorys.Contains(9))
                                    {
                                        anim.lstCategory.Add(new HSceneProc.Category {
                                            category = (int)PositionCategory.StandPool,
                                        });
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
