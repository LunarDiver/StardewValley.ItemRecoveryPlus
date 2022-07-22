using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            helper.Events.Display.MenuChanged += MenuChangedEvent;

            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.Patch(
                AccessTools.Method(
                    typeof(GameLocation), 
                    nameof(GameLocation.answerDialogueAction)),
                postfix: new HarmonyMethod(
                    typeof(DialoguePatch), 
                    nameof(DialoguePatch.Postfix)));
        }

        private void MenuChangedEvent(object sender, MenuChangedEventArgs e)
        {
            if(Game1.currentLocation is not AdventureGuild)
                return;
            
            if(e.NewMenu is not DialogueBox dialogue)
                return;
            
            const string shopKey = "Shop";
            const string recovKey = "Recovery";
            const string leaveKey = "Leave";

            string[] responseKeys = dialogue.responses.Select(res => res.responseKey).ToArray();
            if(!responseKeys.Contains(shopKey) || !responseKeys.Contains(recovKey) || !responseKeys.Contains(leaveKey))
                return;

            Response recoveryResponse = dialogue.responses.First(res => res.responseKey.Equals(recovKey));

            dialogue.responses[dialogue.responses.IndexOf(recoveryResponse)] = new Response("RecoveryPlus", recoveryResponse.responseText);
            return;

            
        }
    }
}
