using System.Reflection;

using IDHIPlugins;
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


namespace IDHIPlugins
{
    public partial class IDHIStoreItems
    {
        public const string GUID = "com.ihavenoidea.idhistore";
        public const string Version = "1.0.1.0";
#if DEBUG
        public const string PluginDisplayName = "IDHI Store Items (Debug)";
#else
        public const string PluginDisplayName = "IDHI Store Items";
#endif
        public const string PluginName = "IDHIStore";
    }
}
