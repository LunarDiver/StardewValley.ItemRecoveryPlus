using System.Linq;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

namespace ItemRecoveryPlus
{
    /// <summary>
    /// This patch is meant to replace the in-game method to answer a dialogue question in order to add our own options.
    /// </summary>
    [HarmonyPatch]
    internal static class DialoguePatch
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(ref bool __result, string questionAndAnswer)
        {
            if(questionAndAnswer is not "adventureGuild_RecoveryPlus")
                return;

            Game1.drawDialogue(Game1.getCharacterFromName("Marlon"), "You want me to recover your items? I can do that but you'll only have this one chance to pay the fee.");

            Game1.afterDialogues += () =>
            {
                Game1.activeClickableMenu = new ConfirmationDialog("Are you okay with that?", player =>
                {
                    Game1.activeClickableMenu = new ShopMenu(player.itemsLostLastDeath.Cast<ISalable>().ToList());

                    player.itemsLostLastDeath.Clear();
                });
            };

            __result = true;
        }
    }
}
