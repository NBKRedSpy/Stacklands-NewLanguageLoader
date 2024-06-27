using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace Stacklands_NewLanguageLoader.Patches
{
    [HarmonyPatch(typeof(SokLoc), nameof(SokLoc.SetLanguage))]
    internal class SokLocSetLanguage_Patch
    {
        public static bool Prefix(string language)
        {

            //languageDef will be the custom language or null.
            LanguageInfoLoader.LoadedLanguages.TryGetValue(language, out LanguageDefinition languageDefinition);

            //True if the Select Language Screen is currently open.
            bool optionScreenIsOpen = GameCanvas.instance?.CurrentScreen?.GetType() == typeof(SelectLanguageScreen);

            //DEV NOTE:  This code should be on the option screen's buttons, but the buttons are created on init in a loop with an event Action,
            //  so it would be a bit of work to intercept the events.  There might be an easy way to push an event to the top, but since it's 
            //  not an EventHandler, it's a bit of a pain.  At least as far as I'm aware.

            //If the option screen is open and a custom language is being set, set the language and reboot.
            //  Otherwise fonts may not load and the custom language's translation table may not be present.
            if (optionScreenIsOpen && languageDefinition != null && SokLoc.instance.CurrentLanguage != language)
            {

                SokLoc.instance.CurrentLanguage = languageDefinition.ColumnLanguageName;
                OptionsScreen.SaveSettings();
                WorldManager.RebootGame();
                return false;
            }


            GetFont_Patch.SetLanguage(languageDefinition);
            return true;
        }
    }
}
