using System.Linq;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;

namespace ItemRecoveryPlus
{
    internal class ModEntry : Mod
    {
        /// <inheritdoc />
        public override void Entry(IModHelper helper)
        {
            helper.Events.Display.MenuChanged += AddDialogueOption;

            //Patches the game with the new dialogue action
            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.Patch(
                AccessTools.Method(
                    typeof(GameLocation), 
                    nameof(GameLocation.answerDialogueAction)),
                postfix: new HarmonyMethod(
                    typeof(DialoguePatch), 
                    nameof(DialoguePatch.Postfix)));
        }

        /// <summary>
        /// Adds the new recovery option to Marlons dialogue, replacing the vanilla option in the process.
        /// </summary>
        private void AddDialogueOption(object sender, MenuChangedEventArgs e)
        {
            //Checks if player is in adventure guild
            if(Game1.currentLocation is not AdventureGuild)
                return;
            
            //Checks if the current menu is a dialogue box
            if(e.NewMenu is not DialogueBox dialogue)
                return;
            
            const string shopKey = "Shop";
            const string recovKey = "Recovery";
            const string leaveKey = "Leave";

            //Checks if the current dialogue provides the options usually presented by Marlon
            string[] responseKeys = dialogue.responses.Select(res => res.responseKey).ToArray();
            if(!responseKeys.Contains(shopKey) || !responseKeys.Contains(recovKey) || !responseKeys.Contains(leaveKey))
                return;

            //Recovery dialogue option
            Response recoveryResponse = dialogue.responses.First(res => res.responseKey.Equals(recovKey));

            //Replaces the recovery option with a custom option
            dialogue.responses[dialogue.responses.IndexOf(recoveryResponse)] = new Response("RecoveryPlus", recoveryResponse.responseText);
        }
    }
}
