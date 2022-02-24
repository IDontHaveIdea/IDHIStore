//
// Debug testing
//

using KKAPI.MainGame;
using KKAPI.Utilities;


namespace IDHIPlugins
{
    public partial class IDHIStore
    {
        public const int StoreItemId = 4194304; // 2^22

        internal class Store
        {
            static internal void Init()
            {
                var icon = ResourceUtils.GetEmbeddedResource(
                    "hlevel_item.png", 
                    typeof(IDHIStore).Assembly).LoadTexture();
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
                    stock: 3, 
                    resetsDaily: false, 
                    cost: 200,
                    numText: "{0} available upgrades");
            }

            static public int GetHLevel()
            {
                return StoreApi.GetItemAmountBought(StoreItemId);
            }
        }
    }
}
