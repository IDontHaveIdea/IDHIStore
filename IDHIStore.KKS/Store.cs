//
// Debug testing
//

using KKAPI.MainGame;
using KKAPI.Utilities;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        public const int StoreItemId = 4194304; // 2^22
        internal const int _stock = 3;
        internal const int _cost = 300;

        internal class Store
        {
            static internal void Init()
            {
                var icon = ResourceUtils.GetEmbeddedResource(
                    "hlevel_item.png", 
                    typeof(IDHIStoreItems).Assembly).LoadTexture();
                var helvelCategoryId = StoreApi.RegisterShopItemCategory(icon);

                StoreApi.RegisterShopItem(
                    itemId: StoreItemId, 
                    itemName: "H Experience Levels", 
                    explaination:"Gives access to H poses independent of heroine experience there are " 
                        + "three levels.  Firs level for experience 50 second for experience " 
                        + "100 and third (sorry no more space)",
#if DEBUG
                    shopType: StoreApi.ShopType.Normal,
#else
                    shopType: StoreApi.ShopType.NightOnly,
#endif
                    itemBackground: StoreApi.ShopBackground.Pink,
                    itemCategory: helvelCategoryId,
                    stock: _stock, 
                    resetsDaily: true, 
                    cost: _cost,
                    numText: "{0} available upgrades");
            }

            static public int GetHLevel()
            {
                return StoreApi.GetItemAmountBought(StoreItemId);
            }
        }
    }
}
