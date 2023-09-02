using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace Stacklands_NewLanguageLoader.Patches
{
    [HarmonyPatch(typeof(SokLoc), nameof(SokLoc.SetLanguage))]
    internal class SokLocSetLanguage_Patch
    {
        public static void Prefix(string language)
        {

            //languageDef will be the custom language or null.
            LanguageInfoLoader.LoadedLanguages.TryGetValue(language, out LanguageDefinition languageDefinition);

            GetFont_Patch.SetLanguage(languageDefinition);
        }
    }
}
