using System.Reflection;


using IDHIPlugIns;

#region Assembly attributes

/*
 * These attributes define various meta-information of the generated DLL.
 * In general, you don't need to touch these. Instead, edit the values in Info.
 */
[assembly: AssemblyTitle(IDHIUtils.Constants.Prefix + "_" + IDHIStoreItems.PlugInName 
    + " (" + IDHIStoreItems.GUID + ")")]
[assembly: AssemblyProduct(IDHIUtils.Constants.Prefix + "_" + IDHIStoreItems.PlugInName)]
[assembly: AssemblyVersion(IDHIStoreItems.Version)]
[assembly: AssemblyFileVersion(IDHIStoreItems.Version)]

#endregion Assembly attributes

//
// Login ID: 0012
//


namespace IDHIPlugIns
{
    public partial class IDHIStoreItems
    {
        public const string GUID = "com.ihavenoidea.idhistore";
        public const string Version = "1.0.2.0";
        public const string AnimationLoaderVersion = "1.1.2.2";
#if DEBUG
        public const string PlugInDisplayName = "IDHI Store Items (Debug)";
#else
        public const string PluginDisplayName = "IDHI Store Items";
#endif
        public const string PlugInName = "IDHIStore";
    }

    public static class Constants
    {
        public const int StoreItemId = 4194304;
    }
}
