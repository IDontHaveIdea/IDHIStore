//
// Register item in store
//
using KKAPI.MainGame;
using KKAPI.Utilities;


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        public const int AnimationLevelItemId = Constants.StoreItemId; // 2^22
        internal const int _stock = 3;
        internal const int _cost = 200;

        internal class Store
        {
            /// <summary>
            /// Register H Level item in store
            /// </summary>
            internal static void Init()
            {
                var icon = ResourceUtils.GetEmbeddedResource(
                    "hlevel_item.png", 
                    typeof(IDHIStoreItems).Assembly).LoadTexture();
                var hLevelCategoryId = StoreApi.RegisterShopItemCategory(icon);

                StoreApi.RegisterShopItem(
                    itemId: AnimationLevelItemId, 
                    itemName: "H Experience Levels", 
                    explaination:"Gives access to H animations independent of heroine"
                        + " experience there are three levels.  Firs level for "
                        + "experience 50 second for experience 100 and third "
                        + "(sorry no more space)",
                    shopType: StoreApi.ShopType.NightOnly,
                    itemBackground: StoreApi.ShopBackground.Pink,
                    itemCategory: hLevelCategoryId,
                    stock: _stock, 
                    resetsDaily: false, 
                    cost: _cost,
                    numText: "{0} available upgrades");
            }

            public static int GetHLevel()
            {
                return StoreApi.GetItemAmountBought(AnimationLevelItemId);
            }
        }
    }
}
