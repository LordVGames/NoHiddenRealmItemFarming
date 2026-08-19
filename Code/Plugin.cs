using BepInEx;
using MonoDetour;
namespace NoHiddenRealmItemFarming;


[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    public void Awake()
    {
        ConfigOptions.BindConfigOptions(Config);
        Log.Init(Logger);
        MonoDetourManager.InvokeHookInitializers(typeof(Plugin).Assembly);
    }
}