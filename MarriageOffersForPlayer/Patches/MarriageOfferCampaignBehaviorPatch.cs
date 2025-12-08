using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace MarriageOffersForPlayer
{
    [HarmonyPatch(typeof(MarriageOfferCampaignBehavior))]
    class MarriageOfferCampaignBehaviorPatch
    {
        private static readonly bool PlayerReceivesMarriageOffers = Configs.Instance.PlayerReceivesMarriageOffers;

        private static MethodInfo CanOfferMarriageForClanMethod = AccessTools.Method(typeof(MarriageOfferCampaignBehavior), "CanOfferMarriageForClan");
        private static MethodInfo ConsiderMarriageForPlayerClanMemberMethod = AccessTools.Method(typeof(MarriageOfferCampaignBehavior), "ConsiderMarriageForPlayerClanMember");

        [HarmonyPrefix]
        [HarmonyPatch("DailyTickClan")]
        public static bool DailyTickClan(Clan consideringClan, MarriageOfferCampaignBehavior __instance, Dictionary<Hero, Hero> ____acceptedMarriageOffersThatWaitingForAvailability)
        {
            bool CanOfferMarriageForClan = (bool)CanOfferMarriageForClanMethod.Invoke(__instance, new object[] { consideringClan });

            if (CanOfferMarriageForClan)
            {
                MobileParty.NavigationType navigationType = consideringClan.HasNavalNavigationCapability ? MobileParty.NavigationType.All : MobileParty.NavigationType.Default;

                float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(Clan.PlayerClan.FactionMidSettlement, consideringClan.FactionMidSettlement, false, false, navigationType);

                if (MBRandom.RandomFloat >= distance / Campaign.Current.Models.MapDistanceModel.GetMaximumDistanceBetweenTwoConnectedSettlements(navigationType) - 0.5f)
                {
                    foreach (Hero hero in Clan.PlayerClan.Heroes)
                    {
                        bool ConsiderMarriageForPlayerClanMember = (bool)ConsiderMarriageForPlayerClanMemberMethod.Invoke(__instance, new object[] { hero, consideringClan });

                        if (PlayerReceivesMarriageOffers)
                        {
                            if (hero.CanMarry() && !____acceptedMarriageOffersThatWaitingForAvailability.ContainsKey(hero) && !ConsiderMarriageForPlayerClanMember)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (hero != Hero.MainHero && hero.CanMarry() && !____acceptedMarriageOffersThatWaitingForAvailability.ContainsKey(hero) && !ConsiderMarriageForPlayerClanMember)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}