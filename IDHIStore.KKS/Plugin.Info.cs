using System.Reflection;

using IDHIPlugIns;
using IDHIUtils;

#region Assembly attributes

/*
 * These attributes define various meta-information of the generated DLL.
 * In general, you don't need to touch these. Instead, edit the values in Info.
 */
[assembly: AssemblyTitle(Constants.Prefix + "_" + IDHIStoreItems.PluginName 
    + " (" + IDHIStoreItems.GUID + ")")]
[assembly: AssemblyProduct(Constants.Prefix + "_" + IDHIStoreItems.PluginName)]
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
        public const string PluginDisplayName = "IDHI Store Items (Debug)";
#else
        public const string PluginDisplayName = "IDHI Store Items";
#endif
        public const string PluginName = "IDHIStore";
    }
}
