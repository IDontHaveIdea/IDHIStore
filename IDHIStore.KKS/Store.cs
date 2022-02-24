//
// Debug testing
//

using KKAPI.MainGame;


namespace IDHIPlugins
{
    public partial class IDHIStore
    {
        public const int StoreItemId = 4194304; // 2^22

        internal class Store
        {
            static internal void Init()
            {
                StoreApi.RegisterShopItem(StoreItemId, "H Levels", "Gives access to H poses" +
                    " independent of heroine experience there are three levels.  Firs level for" +
                    " experience 50 second for 100 experience and third (sorry no more space)",
                    StoreApi.ShopType.Normal, StoreApi.ShopBackground.Yellow, 3, 3, false, 200,
                        numText: "{0} available upgrades");
            }

            static public int GetHLevel()
            {
                return StoreApi.GetItemAmountBought(StoreItemId);
            }
        }
    }
}
