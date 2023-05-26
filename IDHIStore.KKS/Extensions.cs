using System;
using System.Collections.Generic;

using IDHIUtils;

namespace IDHIPlugIns
{
    public static class Extensions
    {
        /// <summary>
        /// Get _alDicExpAddTaii through reflection
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<int, Dictionary<string, int>>>
            GetExpAddTaii(this Utilities.PInfo pInfo)
        {
            var _alDicExpAddTaii = pInfo.Traverse
                    .Field<Dictionary<string,
                        Dictionary<int, Dictionary<string, int>>>>("_alDicExpAddTaii")
                            .Value;
            return _alDicExpAddTaii;
        }

        /// <summary>
        /// Get string constructed for the Animation key through reflection
        /// </summary>
        /// <param name="pInfo"></param>
        /// <param name="anim"></param>
        /// <returns></returns>
        public static string GetAnimationKey(
            this Utilities.PInfo pInfo, HSceneProc.AnimationListInfo anim)
        {
            var tmp = "";
            try
            {
                var animationInfo = pInfo.Traverse
                        .Method("GetAnimationKey",
                            new Type[] {
                                typeof(HSceneProc.AnimationListInfo), typeof(bool) });
                if(animationInfo != null)
                {
                    tmp = animationInfo?.GetValue<string>(anim);
                }
            }
            catch(Exception ex)
            {
                IDHIStoreItems._Log.Error($"Error: Extension GetAnimationKey " +
                    $"{ex.Message}");
            }

            return tmp;
        }
    }
}
