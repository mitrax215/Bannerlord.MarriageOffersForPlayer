using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace MarriageOffersForPlayer
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            _ = Configs.Instance;

            Harmony harmony = new Harmony("com.MarriageOffersForPlayer");

            harmony.PatchAll();
        }
    }
}