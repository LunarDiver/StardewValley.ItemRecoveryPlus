using System.Linq;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

namespace ItemRecoveryPlus
{
    [HarmonyPatch]
    internal static class DialoguePatch
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(ref bool __result, string questionAndAnswer)
        {
            if(questionAndAnswer is not "adventureGuild_RecoveryPlus")
                return;

            Game1.activeClickableMenu = new ShopMenu(Game1.player.itemsLostLastDeath.Cast<ISalable>().ToList(), who: "Marlon_Recovery");

            Game1.player.itemsLostLastDeath.Clear();

            __result = true;
        }
    }
}
