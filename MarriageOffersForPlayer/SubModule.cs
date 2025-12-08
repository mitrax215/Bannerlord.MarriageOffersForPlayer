using HarmonyLib;
using TaleWorlds.MountAndBlade;


namespace MarriageOffersForPlayer
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Harmony harmony = new Harmony("com.MarriageOffersForPlayer");

            harmony.PatchAll();
        }
    }
}